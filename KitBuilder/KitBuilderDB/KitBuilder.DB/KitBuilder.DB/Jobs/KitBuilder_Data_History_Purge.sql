USE [msdb]
GO

begin transaction;
  declare @jobID binary(16) = null,
          @returnCode int = 0,
          @dbName sysname = 'KitBuilder',
          @jobName sysname = 'KitBuilder_Data_History_Purge',
          @categoryName nvarchar(100) = '[Uncategorized (Local)]'

  if not exists(select name from msdb.dbo.syscategories where name = @categoryName and category_class = 1)
  begin
    exec @returnCode = msdb.dbo.sp_add_category @class = 'JOB', @type = 'LOCAL', @name = @categoryName

    if(@@error <> 0 or @returnCode <> 0) rollback transaction;
    return;
  end

  -- Remove existing job if exists.
  if exists(select job_id from msdb.dbo.sysjobs_view where name = @jobName)
	  exec msdb.dbo.sp_delete_job @job_name = @jobName, @delete_unused_schedule = 1;

  exec @returnCode = msdb.dbo.sp_add_job @job_name = @jobName, 
		                                     @enabled = 1, 
		                                     @notify_level_eventlog = 0, 
		                                     @notify_level_email = 2, 
		                                     @notify_level_netsend = 0, 
		                                     @notify_level_page = 0, 
		                                     @delete_level = 0, 
		                                     @description = 'KitBuilder daily data history purge', 
		                                     @category_name = @categoryName, 
		                                     @owner_login_name ='sa', 
		                                     --@notify_email_operator_name = 'SQL Server DBAs',
                                         @job_id = @jobID output;
 
  --Check Maintenance Mode
  if(@@error = 0 and @returnCode = 0)
    exec @returnCode = msdb.dbo.sp_add_jobstep @job_id = @jobID, @step_name = 'Verify DB access', 
	  	                                         @step_id = 1, 
	  	                                         @cmdexec_success_code = 0, 
	  	                                         @on_success_action = 3, 
	  	                                         @on_success_step_id = 2, 
	  	                                         @on_fail_action = 1, -- 1 => Quit with success; 2 => Quit with fuilure; 3 => Go to next step; 4 => Go to step on_fail_step_id 
	  	                                         @on_fail_step_id = 0,
	  	                                         @retry_attempts = 0, 
	  	                                         @retry_interval = 0, 
	  	                                         @os_run_priority = 0,
                                               @subsystem = 'TSQL', 
                                               @command = 'if exists(select 1 from  sys.databases where name = ''KitBuilder'' and (IsNull(is_read_only, 1) <> 0 or IsNull(state, 1) <> 0 or IsNull(user_access, 1) <> 0))
                                                             begin
                                                               declare @appName nvarchar(128) = (select LTrim(RTrim(program_name)) from sys.sysprocesses where spid = @@spid),
                                                                       @dbState nvarchar(128) = (select FormatMessage(''%s, IsReadOnly = %s, UserAccess = %s, State = %s'', name, cast(is_read_only as varchar), user_access_desc, state_desc) from sys.databases where name = db_name());
                                                               declare @msg nvarchar(512) = FormatMessage(''DB is offline for maintenance or in restricted mode. AppName = %s, DbName = %s'', @appName, @dbState);
                                                               throw 50000, @msg, 1;
                                                             end',
	  	                                         @database_name = @dbName, 
	  	                                         @flags = 0;

  --Purge Data
  if(@@error = 0 and @returnCode = 0)
    exec @returnCode = msdb.dbo.sp_add_jobstep @job_id = @jobID,
                                               @step_name = 'Purge Data', 
    		                                       @step_id = 2, 
    		                                       @cmdexec_success_code = 0, 
    		                                       @on_success_action = 4, 
    		                                       @on_success_step_id = 3, 
    		                                       @on_fail_action = 4, 
    		                                       @on_fail_step_id = 4, 
    		                                       @retry_attempts = 0, 
    		                                       @retry_interval = 0, 
    		                                       @os_run_priority = 0,
                                               @subsystem = 'TSQL', 
    		                                       @command= 'exec dbo.PurgeData',
    		                                       @database_name = @dbName, 
    		                                       @flags = 0;

  --Log success
  if(@@error = 0 and @returnCode = 0)
    exec @returnCode = msdb.dbo.sp_add_jobstep @job_id = @jobID,
                                               @step_name = 'Log success', 
	  	                                         @step_id = 3, 
	  	                                         @cmdexec_success_code = 0, 
	  	                                         @on_success_action = 1, 
	  	                                         @on_success_step_id = 0, 
	  	                                         @on_fail_action = 2, 
	  	                                         @on_fail_step_id = 0, 
	  	                                         @retry_attempts = 0, 
	  	                                         @retry_interval = 0, 
	  	                                         @os_run_priority = 0,
                                               @subsystem= 'TSQL', 
	  	                                         @command = 'declare @user nvarchar(255) = Current_User,
                                                                   @host nvarchar(255) = Host_Name();
                                                           exec AddLogEvent @AppName  = ''KitBuilder Data Purge'',
	  					                                                              @UserName = @user,
	  					                                                              @Level = ''Info'',
	  					                                                              @Source = ''KitBuilder Data Purge'',
	  					                                                              @Message = ''Data Purge was successful.'',
	  					                                                              @MachineName = @host;',
	  	                                         @database_name = @dbName, 
	  	                                         @flags = 0;

  --Log failure
  if(@@error = 0 and @returnCode = 0)
    exec @returnCode = msdb.dbo.sp_add_jobstep @job_id = @jobID,
                                               @step_name = 'Log failure', 
    		                                       @step_id = 4, 
    		                                       @cmdexec_success_code = 0, 
    		                                       @on_success_action = 2, 
    		                                       @on_success_step_id = 0, 
    		                                       @on_fail_action = 2, 
    		                                       @on_fail_step_id = 0, 
    		                                       @retry_attempts = 0, 
    		                                       @retry_interval = 0, 
    		                                       @os_run_priority = 0,
                                               @subsystem = 'TSQL', 
    		                                       @command = 'declare @user nvarchar(255) = Current_User,
                                                                   @host nvarchar(255) = Host_Name();
                                                           exec app.AddLogEvent @AppName = ''KitBuilder Data Purge'',
    		                                       				                          @UserName = @user,
    		                                       				                          @Level = ''Error'',
    		                                       				                          @Source = ''KitBuilder Data Purge'',
    		                                       				                          @Message = ''KitBuilder Data Purge was unsuccessful. See job history for details.'',
    		                                       				                          @MachineName = @host;', 
    		                                       @database_name = @dbName, 
    		                                       @flags = 0;

  if(@@error = 0 and @returnCode = 0)
    exec @returnCode = msdb.dbo.sp_update_job @job_id = @jobID, @start_step_id = 1;

  if(@@error = 0 and @returnCode = 0)
    exec @returnCode = msdb.dbo.sp_add_jobschedule @job_id = @jobID,
                                                   @name = 'Daily after 2100', 
	  	                                             @enabled = 1, 
	  	                                             @freq_type = 4, 
	  	                                             @freq_interval = 1, 
	  	                                             @freq_subday_type = 4, 
	  	                                             @freq_subday_interval = 10, 
	  	                                             @freq_relative_interval = 0, 
	  	                                             @freq_recurrence_factor = 0, 
	  	                                             @active_start_date = 20180901, 
	  	                                             @active_end_date = 99991231, 
	  	                                             @active_start_time = 0,--210000, 
	  	                                             @active_end_time = 235959;
    
  if(@@error = 0 and @returnCode = 0)
    exec @returnCode = msdb.dbo.sp_add_jobserver @job_id = @jobID, @server_name = '(local)';

  if(@@error = 0 and @returnCode = 0)
    commit transaction
  else
    begin
      if(@@TranCount > 0) rollback transaction;
    end