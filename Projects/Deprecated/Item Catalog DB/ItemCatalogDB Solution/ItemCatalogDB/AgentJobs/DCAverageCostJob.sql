USE [msdb]
GO
/****** Object:  Job [StoreOps - Archive Load IRMA Purchase Orders Output File]    Script Date: 09/16/2008 09:12:49 ******/
IF EXISTS 
	(SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'DC Average Cost Import') 
	EXEC msdb.dbo.sp_delete_job @job_name=N'DC Average Cost Import', @delete_unused_schedule=1 
GO
/****** Object:  Job [DC Average Cost Import]    Script Date: 07/06/2009 15:07:19 ******/
DECLARE @SQL varchar(MAX)
DECLARE @Environment varchar(10)
DECLARE @ReturnCode INT
DECLARE @jobId BINARY(16)

IF EXISTS (SELECT * FROM master.dbo.sysdatabases WHERE Name = 'ItemCatalog_Test')
	SELECT @Environment = Environment FROM ItemCatalog_Test.dbo.Version WHERE ApplicationName = 'SYSTEM'
ELSE
	SELECT @Environment = Environment FROM ItemCatalog.dbo.Version WHERE ApplicationName = 'SYSTEM'

IF @Environment = 'TEST'
	BEGIN
		BEGIN TRANSACTION
		SELECT @ReturnCode = 0
		/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 3/23/2017 4:34:56 AM ******/
		IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
		BEGIN
			EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
			IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		END

		EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'DC Average Cost Import', 
				@enabled=0, 
				@notify_level_eventlog=0, 
				@notify_level_email=2, 
				@notify_level_netsend=0, 
				@notify_level_page=0, 
				@delete_level=0, 
				@description=N'No description available.', 
				@category_name=N'[Uncategorized (Local)]', 
				@owner_login_name=N'sa', 
				@job_id = @jobId OUTPUT
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		/****** Object:  Step [Check Maintenance Mode]    Script Date: 3/23/2017 4:34:56 AM ******/
		EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Check Maintenance Mode', 
				@step_id=1, 
				@cmdexec_success_code=0, 
				@on_success_action=3, 
				@on_success_step_id=0, 
				@on_fail_action=4, 
				@on_fail_step_id=3, 
				@retry_attempts=0, 
				@retry_interval=0, 
				@os_run_priority=0, @subsystem=N'TSQL', 
				@command=N'IF (SELECT StatusFlagValue FROM dbo.DbStatus where FlagName = ''IsOfflineForMaintenance'') = 1
								RAISERROR(''Database is in maintenance mode.'', 16, 1)',
				@database_name=N'ItemCatalog_Test', 
				@flags=0
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		/****** Object:  Step [Update DC Average Cost]    Script Date: 3/23/2017 4:34:56 AM ******/
		EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Update DC Average Cost', 
				@step_id=2, 
				@cmdexec_success_code=0, 
				@on_success_action=1, 
				@on_success_step_id=0, 
				@on_fail_action=2, 
				@on_fail_step_id=0, 
				@retry_attempts=0, 
				@retry_interval=0, 
				@os_run_priority=0, @subsystem=N'TSQL', 
				@command=N'
				DECLARE @DCVendor TABLE (Vendor_Id int)
				DECLARE @CurrentCost TABLE (Item_Key int, Store_No int, Vendor_Id int, UnitCost smallmoney, UnitFreight smallmoney, Package_Desc1 decimal(9,4), StartDate datetime, EndDate datetime, Promotional bit, MSRP smallmoney, FromVendor bit, CostUnit_ID int, FreightUnit_ID int, StoreItemVendorID int)
				DECLARE @CurrentAvgCost TABLE (Item_Key int, AvgCost smallmoney)
				DECLARE @EmailRecipients varchar(255)
				DECLARE @RecordCount int
				DECLARE @Body varchar(255)
				DECLARE @Subject varchar(255)
				DECLARE @ErrorMsg varchar(255)

				BEGIN TRY
					INSERT INTO @DCVendor
						SELECT v.Vendor_Id
						FROM Vendor v
						JOIN Store s ON s.Store_No = v.Store_No
						WHERE s.Distribution_Center = 1 AND WFM_Store = 0 AND Manufacturer = 0

					INSERT INTO @CurrentCost
						SELECT
							i.Item_Key, siv.Store_No, siv.Vendor_Id, vch.unitcost,vch.unitfreight, vch.package_desc1, CONVERT(varchar(255),GETDATE(),101) As StartDate, 
							vch.EndDate, 0, vch.MSRP, 0, vch.CostUnit_ID, vch.FreightUnit_ID, siv.StoreItemVendorID
						FROM 
							VendorCostHistory vch (nolock)
							JOIN StoreItemVendor siv (nolock) ON siv.StoreItemVendorId = vch.StoreItemVendorId
							JOIN Item i ON i.item_key = siv.Item_Key
						WHERE
							VendorCostHistoryid = (
													SELECT TOP 1 vch.VendorCostHistoryId
													FROM 
														VendorCostHistory vch (nolock)
													WHERE 
														vch.StoreItemVendorId = dbo.fn_GetStoreItemVendorID(i.Item_Key, siv.Store_No)
														AND StartDate <= GETDATE()
														AND EndDate >= GETDATE()
													ORDER BY
														VendorCostHistoryID DESC)
							AND i.EXEDistributed = 1 AND i.Deleted_Item = 0 AND siv.DiscontinueItem = 0 AND siv.PrimaryVendor = 1 AND siv.Vendor_Id IN (SELECT Vendor_Id FROM @DCVendor)

					INSERT INTO @CurrentAvgCost
						SELECT a.Item_Key, ROUND(a.AvgCost,2)
						FROM AvgCostHistory a
						WHERE a.Effective_Date = (SELECT MAX(Effective_Date) FROM AvgCostHistory b WHERE b.Item_Key = a.Item_Key)
						ORDER BY a.item_key

					INSERT INTO VendorCostHistory (StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate, MSRP, FromVendor, CostUnit_ID, FreightUnit_ID, IsFromJDASync)
						SELECT c.StoreItemVendorID, c.Promotional, CAST(((a.AvgCost * c.Package_Desc1) + dbo.fn_GetCurrentHandlingCharge(a.Item_Key, c.Vendor_ID)) AS smallmoney), 
							   c.UnitFreight, c.Package_Desc1, c.StartDate, c.EndDate, c.MSRP, c.FromVendor, c.CostUnit_ID, c.FreightUnit_ID, 0
						FROM @CurrentCost c
						JOIN @CurrentAvgCost a ON a.Item_Key = c.Item_Key
						WHERE c.UnitCost <> ((a.AvgCost * c.Package_Desc1) + dbo.fn_GetCurrentHandlingCharge(a.Item_Key, c.Vendor_ID))

					SELECT @RecordCount = COUNT(DISTINCT c.Item_Key)
					FROM @CurrentCost c
					JOIN @CurrentAvgCost a ON a.Item_Key = c.Item_Key
					WHERE c.UnitCost <> ((a.AvgCost * c.Package_Desc1) + dbo.fn_GetCurrentHandlingCharge(a.Item_Key, c.Vendor_ID))
				END TRY

				BEGIN CATCH
					SELECT @ErrorMsg = ERROR_MESSAGE()
				END CATCH

				IF ISNULL(@ErrorMsg,'''') = ''''
					BEGIN
						SELECT @Subject = ''DEV DC Average Cost Import Successful''
				
						IF @RecordCount > 1
							SELECT @Body = CAST(@RecordCount AS Varchar(10)) + '' items had their cost updated''
						ELSE
							SELECT @Body = CAST(@RecordCount AS Varchar(10)) + '' item had its cost updated''
					END
				ELSE
					BEGIN
						SELECT @Subject = ''DC Average Cost Import Failed''
						SELECT @Body = ''The DC Average Cost Import failed with the following error: <BR><BR> '' + @ErrorMsg
					END

				SET @EmailRecipients = (SELECT av.vALUE 
										FROM AppConfigValue av
										INNER JOIN AppConfigEnv ae ON av.EnvironmentId = ae.EnvironmentId
										INNER JOIN AppConfigApp aa ON av.ApplicationId = aa.ApplicationId
										INNER JOIN AppConfigKey ak ON av.KeyId = ak.KeyId
										WHERE ae.ShortName = ''TST'' AND aa.Name = ''IRMA CLIENT'' AND ak.Name = ''DCAvgCostJob_Email'')

				IF @EmailRecipients <> ''''
					BEGIN
						EXEC msdb.dbo.sp_send_dbmail 
							@profile_name = ''TEST DC Average Cost Import'',
							@recipients = @EmailRecipients, 
							@body = @Body, 
							@subject = @Subject,
							@Body_format = ''HTML''
					END', 
				@database_name=N'ItemCatalog_Test', 
				@flags=0
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		/****** Object:  Step [Report Maintenance Mode]    Script Date: 3/23/2017 4:34:56 AM ******/
		EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Report Maintenance Mode', 
				@step_id=3, 
				@cmdexec_success_code=0, 
				@on_success_action=1, 
				@on_success_step_id=0, 
				@on_fail_action=2, 
				@on_fail_step_id=0, 
				@retry_attempts=0, 
				@retry_interval=0, 
				@os_run_priority=0, @subsystem=N'TSQL', 
				@command=N'IF EXISTS (
								SELECT *
								FROM dbo.DbStatus
								WHERE StatusFlagName = ''IsOfflineForMaintenance''
									AND StatusFlagValue = 1
							)
							BEGIN
								DECLARE @statusMsg NVARCHAR(256)
									,@appName NVARCHAR(128)
									,@dbState VARCHAR(128)

								SELECT @appName = ltrim(rtrim(program_name))
								FROM sys.sysprocesses
								WHERE spid = @@spid

								SELECT @dbState = ''DbName='' + NAME + '', IsReadOnly='' + cast(is_read_only AS VARCHAR) + '', UserAccess='' + user_access_desc + '', State='' + state_desc
								FROM sys.databases
								WHERE NAME LIKE db_name()

								SELECT @statusMsg = ''** DB Offline For Maintenance ** --> '' + ''AppName='' + @appName + '', '' + @dbstate
								
								RAISERROR (@statusMs, 16, 0)
							END',
				@database_name=N'ItemCatalog_Test', 
				@flags=0
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'DC Average Cost Import', 
				@enabled=1, 
				@freq_type=4, 
				@freq_interval=1, 
				@freq_subday_type=1, 
				@freq_subday_interval=0, 
				@freq_relative_interval=0, 
				@freq_recurrence_factor=0, 
				@active_start_date=20090706, 
				@active_end_date=99991231, 
				@active_start_time=50000, 
				@active_end_time=235959
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		COMMIT TRANSACTION
	END
ELSE IF @Environment = 'QA'
	BEGIN
		BEGIN TRANSACTION
		SELECT @ReturnCode = 0
		/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 3/23/2017 4:34:56 AM ******/
		IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
		BEGIN
		EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

		END

		EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'DC Average Cost Import', 
				@enabled=0, 
				@notify_level_eventlog=0, 
				@notify_level_email=2, 
				@notify_level_netsend=0, 
				@notify_level_page=0, 
				@delete_level=0, 
				@description=N'No description available.', 
				@category_name=N'[Uncategorized (Local)]', 
				@owner_login_name=N'sa', 
				@job_id = @jobId OUTPUT
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		/****** Object:  Step [Check Maintenance Mode]    Script Date: 3/23/2017 4:34:56 AM ******/
		EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Check Maintenance Mode', 
				@step_id=1, 
				@cmdexec_success_code=0, 
				@on_success_action=3, 
				@on_success_step_id=0, 
				@on_fail_action=4, 
				@on_fail_step_id=3, 
				@retry_attempts=0, 
				@retry_interval=0, 
				@os_run_priority=0, @subsystem=N'TSQL', 
				@command=N'IF (SELECT StatusFlagValue FROM dbo.DbStatus where FlagName = ''IsOfflineForMaintenance'') = 1
							RAISERROR(''Database is in maintenance mode.'', 16, 1)',
				@database_name=N'ItemCatalog', 
				@flags=0
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		/****** Object:  Step [Update DC Average Cost]    Script Date: 3/23/2017 4:34:56 AM ******/
		EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Update DC Average Cost', 
				@step_id=2, 
				@cmdexec_success_code=0, 
				@on_success_action=1, 
				@on_success_step_id=0, 
				@on_fail_action=2, 
				@on_fail_step_id=0, 
				@retry_attempts=0, 
				@retry_interval=0, 
				@os_run_priority=0, @subsystem=N'TSQL', 
				@command=N'
				DECLARE @DCVendor TABLE (Vendor_Id int)
				DECLARE @CurrentCost TABLE (Item_Key int, Store_No int, Vendor_Id int, UnitCost smallmoney, UnitFreight smallmoney, Package_Desc1 decimal(9,4), StartDate datetime, EndDate datetime, Promotional bit, MSRP smallmoney, FromVendor bit, CostUnit_ID int, FreightUnit_ID int, StoreItemVendorID int)
				DECLARE @CurrentAvgCost TABLE (Item_Key int, AvgCost smallmoney)
				DECLARE @EmailRecipients varchar(255)
				DECLARE @RecordCount int
				DECLARE @Body varchar(255)
				DECLARE @Subject varchar(255)
				DECLARE @ErrorMsg varchar(255)

				BEGIN TRY
					INSERT INTO @DCVendor
						SELECT v.Vendor_Id
						FROM Vendor v
						JOIN Store s ON s.Store_No = v.Store_No
						WHERE s.Distribution_Center = 1 AND WFM_Store = 0 AND Manufacturer = 0

					INSERT INTO @CurrentCost
						SELECT
							i.Item_Key, siv.Store_No, siv.Vendor_Id, vch.unitcost,vch.unitfreight, vch.package_desc1, CONVERT(varchar(255),GETDATE(),101) As StartDate, 
							vch.EndDate, 0, vch.MSRP, 0, vch.CostUnit_ID, vch.FreightUnit_ID, siv.StoreItemVendorID
						FROM 
							VendorCostHistory vch (nolock)
							JOIN StoreItemVendor siv (nolock) ON siv.StoreItemVendorId = vch.StoreItemVendorId
							JOIN Item i ON i.item_key = siv.Item_Key
						WHERE
							VendorCostHistoryid = (
													SELECT TOP 1 vch.VendorCostHistoryId
													FROM 
														VendorCostHistory vch (nolock)
													WHERE 
														vch.StoreItemVendorId = dbo.fn_GetStoreItemVendorID(i.Item_Key, siv.Store_No)
														AND StartDate <= GETDATE()
														AND EndDate >= GETDATE()
													ORDER BY
														VendorCostHistoryID DESC)
							AND i.EXEDistributed = 1 AND i.Deleted_Item = 0 AND siv.DiscontinueItem = 0 AND siv.PrimaryVendor = 1 AND siv.Vendor_Id IN (SELECT Vendor_Id FROM @DCVendor)

					INSERT INTO @CurrentAvgCost
						SELECT a.Item_Key, ROUND(a.AvgCost,2)
						FROM AvgCostHistory a
						WHERE a.Effective_Date = (SELECT MAX(Effective_Date) FROM AvgCostHistory b WHERE b.Item_Key = a.Item_Key)
						ORDER BY a.item_key

					INSERT INTO VendorCostHistory (StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate, MSRP, FromVendor, CostUnit_ID, FreightUnit_ID, IsFromJDASync)
						SELECT c.StoreItemVendorID, c.Promotional, CAST(((a.AvgCost * c.Package_Desc1) + dbo.fn_GetCurrentHandlingCharge(a.Item_Key, c.Vendor_ID)) AS smallmoney), 
							   c.UnitFreight, c.Package_Desc1, c.StartDate, c.EndDate, c.MSRP, c.FromVendor, c.CostUnit_ID, c.FreightUnit_ID, 0
						FROM @CurrentCost c
						JOIN @CurrentAvgCost a ON a.Item_Key = c.Item_Key
						WHERE c.UnitCost <> ((a.AvgCost * c.Package_Desc1) + dbo.fn_GetCurrentHandlingCharge(a.Item_Key, c.Vendor_ID))

					SELECT @RecordCount = COUNT(DISTINCT c.Item_Key)
					FROM @CurrentCost c
					JOIN @CurrentAvgCost a ON a.Item_Key = c.Item_Key
					WHERE c.UnitCost <> ((a.AvgCost * c.Package_Desc1) + dbo.fn_GetCurrentHandlingCharge(a.Item_Key, c.Vendor_ID))
				END TRY

				BEGIN CATCH
					SELECT @ErrorMsg = ERROR_MESSAGE()
				END CATCH

				IF ISNULL(@ErrorMsg,'''') = ''''
					BEGIN
						SELECT @Subject = ''DC Average Cost Import Successful''
				
						IF @RecordCount > 1
							SELECT @Body = CAST(@RecordCount AS Varchar(10)) + '' items had their cost updated''
						ELSE
							SELECT @Body = CAST(@RecordCount AS Varchar(10)) + '' item had its cost updated''
					END
				ELSE
					BEGIN
						SELECT @Subject = ''QA DC Average Cost Import Failed''
						SELECT @Body = ''The DC Average Cost Import failed with the following error: <BR><BR> '' + @ErrorMsg
					END

				SET @EmailRecipients = (SELECT av.vALUE 
										FROM AppConfigValue av
										INNER JOIN AppConfigEnv ae ON av.EnvironmentId = ae.EnvironmentId
										INNER JOIN AppConfigApp aa ON av.ApplicationId = aa.ApplicationId
										INNER JOIN AppConfigKey ak ON av.KeyId = ak.KeyId
										WHERE ae.ShortName = ''QA'' AND aa.Name = ''IRMA CLIENT'' AND ak.Name = ''DCAvgCostJob_Email'')

				IF @EmailRecipients <> ''''
					BEGIN
						EXEC msdb.dbo.sp_send_dbmail 
							@profile_name = ''QA DC Average Cost Import'',
							@recipients = @EmailRecipients, 
							@body = @Body, 
							@subject = @Subject,
							@Body_format = ''HTML''
					END', 
				@database_name=N'ItemCatalog', 
				@flags=0
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		/****** Object:  Step [Report Maintenance Mode]    Script Date: 3/23/2017 4:34:56 AM ******/
		EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Report Maintenance Mode', 
				@step_id=3, 
				@cmdexec_success_code=0, 
				@on_success_action=1, 
				@on_success_step_id=0, 
				@on_fail_action=2, 
				@on_fail_step_id=0, 
				@retry_attempts=0, 
				@retry_interval=0, 
				@os_run_priority=0, @subsystem=N'TSQL', 
				@command=N'IF EXISTS (
							SELECT *
							FROM dbo.DbStatus
							WHERE StatusFlagName = ''IsOfflineForMaintenance''
								AND StatusFlagValue = 1
						)
						BEGIN
							DECLARE @statusMsg NVARCHAR(256)
								,@appName NVARCHAR(128)
								,@dbState VARCHAR(128)

							SELECT @appName = ltrim(rtrim(program_name))
							FROM sys.sysprocesses
							WHERE spid = @@spid

							SELECT @dbState = ''DbName='' + NAME + '', IsReadOnly='' + cast(is_read_only AS VARCHAR) + '', UserAccess='' + user_access_desc + '', State='' + state_desc
							FROM sys.databases
							WHERE NAME LIKE db_name()

							SELECT @statusMsg = ''** DB Offline For Maintenance ** --> '' + ''AppName='' + @appName + '', '' + @dbstate
							
							RAISERROR (@statusMs, 16, 0)
						END',
				@database_name=N'ItemCatalog', 
				@flags=0
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'DC Average Cost Import', 
				@enabled=1, 
				@freq_type=4, 
				@freq_interval=1, 
				@freq_subday_type=1, 
				@freq_subday_interval=0, 
				@freq_relative_interval=0, 
				@freq_recurrence_factor=0, 
				@active_start_date=20090706, 
				@active_end_date=99991231, 
				@active_start_time=50000, 
				@active_end_time=235959
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		BEGIN TRANSACTION
		SELECT @ReturnCode = 0
		/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 3/23/2017 4:34:56 AM ******/
		IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
		BEGIN
			EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
			IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		END

		EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'DC Average Cost Import', 
				@enabled=0, 
				@notify_level_eventlog=0, 
				@notify_level_email=2, 
				@notify_level_netsend=0, 
				@notify_level_page=0, 
				@delete_level=0, 
				@description=N'No description available.', 
				@category_name=N'[Uncategorized (Local)]', 
				@owner_login_name=N'sa', 
				@job_id = @jobId OUTPUT
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		/****** Object:  Step [Check Maintenance Mode]    Script Date: 3/23/2017 4:34:56 AM ******/
		EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Check Maintenance Mode', 
				@step_id=1, 
				@cmdexec_success_code=0, 
				@on_success_action=3, 
				@on_success_step_id=0, 
				@on_fail_action=4, 
				@on_fail_step_id=3, 
				@retry_attempts=0, 
				@retry_interval=0, 
				@os_run_priority=0, @subsystem=N'TSQL', 
				@command=N'IF (SELECT StatusFlagValue FROM dbo.DbStatus where FlagName = ''IsOfflineForMaintenance'') = 1
								RAISERROR(''Database is in maintenance mode.'', 16, 1)',
				@database_name=N'ItemCatalog', 
				@flags=0
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		/****** Object:  Step [Update DC Average Cost]    Script Date: 3/23/2017 4:34:56 AM ******/
		EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Update DC Average Cost', 
				@step_id=2, 
				@cmdexec_success_code=0, 
				@on_success_action=1, 
				@on_success_step_id=0, 
				@on_fail_action=2, 
				@on_fail_step_id=0, 
				@retry_attempts=0, 
				@retry_interval=0, 
				@os_run_priority=0, @subsystem=N'TSQL', 
				@command=N'
				DECLARE @DCVendor TABLE (Vendor_Id int)
				DECLARE @CurrentCost TABLE (Item_Key int, Store_No int, Vendor_Id int, UnitCost smallmoney, UnitFreight smallmoney, Package_Desc1 decimal(9,4), StartDate datetime, EndDate datetime, Promotional bit, MSRP smallmoney, FromVendor bit, CostUnit_ID int, FreightUnit_ID int, StoreItemVendorID int)
				DECLARE @CurrentAvgCost TABLE (Item_Key int, AvgCost smallmoney)
				DECLARE @EmailRecipients varchar(255)
				DECLARE @RecordCount int
				DECLARE @Body varchar(255)
				DECLARE @Subject varchar(255)
				DECLARE @ErrorMsg varchar(255)

				BEGIN TRY
					INSERT INTO @DCVendor
						SELECT v.Vendor_Id
						FROM Vendor v
						JOIN Store s ON s.Store_No = v.Store_No
						WHERE s.Distribution_Center = 1 AND WFM_Store = 0 AND Manufacturer = 0

					INSERT INTO @CurrentCost
						SELECT
							i.Item_Key, siv.Store_No, siv.Vendor_Id, vch.unitcost,vch.unitfreight, vch.package_desc1, CONVERT(varchar(255),GETDATE(),101) As StartDate, 
							vch.EndDate, 0, vch.MSRP, 0, vch.CostUnit_ID, vch.FreightUnit_ID, siv.StoreItemVendorID
						FROM 
							VendorCostHistory vch (nolock)
							JOIN StoreItemVendor siv (nolock) ON siv.StoreItemVendorId = vch.StoreItemVendorId
							JOIN Item i ON i.item_key = siv.Item_Key
						WHERE
							VendorCostHistoryid = (
													SELECT TOP 1 vch.VendorCostHistoryId
													FROM 
														VendorCostHistory vch (nolock)
													WHERE 
														vch.StoreItemVendorId = dbo.fn_GetStoreItemVendorID(i.Item_Key, siv.Store_No)
														AND StartDate <= GETDATE()
														AND EndDate >= GETDATE()
													ORDER BY
														VendorCostHistoryID DESC)
							AND i.EXEDistributed = 1 AND i.Deleted_Item = 0 AND siv.DiscontinueItem = 0 AND siv.PrimaryVendor = 1 AND siv.Vendor_Id IN (SELECT Vendor_Id FROM @DCVendor)

					INSERT INTO @CurrentAvgCost
						SELECT a.Item_Key, ROUND(a.AvgCost,2)
						FROM AvgCostHistory a
						WHERE a.Effective_Date = (SELECT MAX(Effective_Date) FROM AvgCostHistory b WHERE b.Item_Key = a.Item_Key)
						ORDER BY a.item_key

					INSERT INTO VendorCostHistory (StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate, MSRP, FromVendor, CostUnit_ID, FreightUnit_ID, IsFromJDASync)
						SELECT c.StoreItemVendorID, c.Promotional, CAST(((a.AvgCost * c.Package_Desc1) + dbo.fn_GetCurrentHandlingCharge(a.Item_Key, c.Vendor_ID)) AS smallmoney), 
							   c.UnitFreight, c.Package_Desc1, c.StartDate, c.EndDate, c.MSRP, c.FromVendor, c.CostUnit_ID, c.FreightUnit_ID, 0
						FROM @CurrentCost c
						JOIN @CurrentAvgCost a ON a.Item_Key = c.Item_Key
						WHERE c.UnitCost <> ((a.AvgCost * c.Package_Desc1) + dbo.fn_GetCurrentHandlingCharge(a.Item_Key, c.Vendor_ID))

					SELECT @RecordCount = COUNT(DISTINCT c.Item_Key)
					FROM @CurrentCost c
					JOIN @CurrentAvgCost a ON a.Item_Key = c.Item_Key
					WHERE c.UnitCost <> ((a.AvgCost * c.Package_Desc1) + dbo.fn_GetCurrentHandlingCharge(a.Item_Key, c.Vendor_ID))
				END TRY

				BEGIN CATCH
					SELECT @ErrorMsg = ERROR_MESSAGE()
				END CATCH

				IF ISNULL(@ErrorMsg,'''') = ''''
					BEGIN
						SELECT @Subject = ''PROD DC Average Cost Import Successful''
				
						IF @RecordCount > 1
							SELECT @Body = CAST(@RecordCount AS Varchar(10)) + '' items had their cost updated''
						ELSE
							SELECT @Body = CAST(@RecordCount AS Varchar(10)) + '' item had its cost updated''
					END
				ELSE
					BEGIN
						SELECT @Subject = ''DC Average Cost Import Failed''
						SELECT @Body = ''The DC Average Cost Import failed with the following error: <BR><BR> '' + @ErrorMsg
					END

				SET @EmailRecipients = (SELECT av.vALUE 
										FROM AppConfigValue av
										INNER JOIN AppConfigEnv ae ON av.EnvironmentId = ae.EnvironmentId
										INNER JOIN AppConfigApp aa ON av.ApplicationId = aa.ApplicationId
										INNER JOIN AppConfigKey ak ON av.KeyId = ak.KeyId
										WHERE ae.ShortName = ''PRD'' AND aa.Name = ''IRMA CLIENT'' AND ak.Name = ''DCAvgCostJob_Email'')

				IF @EmailRecipients <> ''''
					BEGIN
						EXEC msdb.dbo.sp_send_dbmail 
							@profile_name = ''PROD DC Average Cost Import'',
							@recipients = @EmailRecipients, 
							@body = @Body, 
							@subject = @Subject,
							@Body_format = ''HTML''
					END', 
				@database_name=N'ItemCatalog', 
				@flags=0
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		/****** Object:  Step [Report Maintenance Mode]    Script Date: 3/23/2017 4:34:56 AM ******/
		EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Report Maintenance Mode', 
				@step_id=3, 
				@cmdexec_success_code=0, 
				@on_success_action=1, 
				@on_success_step_id=0, 
				@on_fail_action=2, 
				@on_fail_step_id=0, 
				@retry_attempts=0, 
				@retry_interval=0, 
				@os_run_priority=0, @subsystem=N'TSQL', 
				@command=N'IF EXISTS (
								SELECT *
								FROM dbo.DbStatus
								WHERE StatusFlagName = ''IsOfflineForMaintenance''
									AND StatusFlagValue = 1
							)
							BEGIN
								DECLARE @statusMsg NVARCHAR(256)
									,@appName NVARCHAR(128)
									,@dbState VARCHAR(128)

								SELECT @appName = ltrim(rtrim(program_name))
								FROM sys.sysprocesses
								WHERE spid = @@spid

								SELECT @dbState = ''DbName='' + NAME + '', IsReadOnly='' + cast(is_read_only AS VARCHAR) + '', UserAccess='' + user_access_desc + '', State='' + state_desc
								FROM sys.databases
								WHERE NAME LIKE db_name()

								SELECT @statusMsg = ''** DB Offline For Maintenance ** --> '' + ''AppName='' + @appName + '', '' + @dbstate
								
								RAISERROR (@statusMs, 16, 0))
							END',
				@database_name=N'ItemCatalog', 
				@flags=0
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'DC Average Cost Import', 
				@enabled=1, 
				@freq_type=4, 
				@freq_interval=1, 
				@freq_subday_type=1, 
				@freq_subday_interval=0, 
				@freq_relative_interval=0, 
				@freq_recurrence_factor=0, 
				@active_start_date=20090706, 
				@active_end_date=99991231, 
				@active_start_time=50000, 
				@active_end_time=235959
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
		IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
		COMMIT TRANSACTION
	END

GOTO EndSave
QuitWithRollback:
	IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO