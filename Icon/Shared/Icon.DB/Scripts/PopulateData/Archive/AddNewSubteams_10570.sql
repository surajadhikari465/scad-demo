SET NOCOUNT ON;

DECLARE @scriptKey VARCHAR(128) = 'AddNewSubteams_10570';

IF (NOT EXISTS (SELECT *FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	DECLARE @Error VARCHAR(2500) = NULL
	  ,@SubteamId INT
		,@HierarchyClassId INT
		,@HierarchyClassName NVARCHAR(255)
		,@PdnTraitValue NVARCHAR(255)
		,@NumTraitValue NVARCHAR(255)
		,@NamTraitValue NVARCHAR(255)
		,@FinTraitValue NVARCHAR(255);
	DECLARE @hierarchyId INT = (
			SELECT [HierarchyId]
			FROM Hierarchy
			WHERE HierarchyName = 'Financial'
			);--5
	DECLARE @pdnTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'PDN'
			);--PosDepartmentNumber: 62
	DECLARE @NumTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'NUM'
			);--TeamNumber: 63
	DECLARE @NamTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'NAM'
			) --TeamName: 64
	DECLARE @FinTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'FIN'
			);--Financial Hierarchy Code: 53

	DECLARE @financialHierarchyClassLevel INT = (
		SELECT hierarchyLevel
		FROM HierarchyPrototype
		WHERE HIERARCHYID = @hierarchyId
		);
	DECLARE @HierarchyMessageTypeId INT = (
			SELECT MessageTypeId
			FROM app.MessageType
			WHERE MessageTypeName = 'Hierarchy'
			);
	DECLARE @ReadyStatusId INT = (
			SELECT MessageStatusId
			FROM app.MessageStatus
			WHERE MessageStatusName = 'Ready'
			);
	DECLARE @ActionId INT = (
			SELECT MessageActionId
			FROM app.MessageAction
			WHERE MessageActionName = 'AddOrUpdate'
			);

	IF (OBJECT_ID('tempdb..#Subteams') IS NOT NULL)	DROP TABLE #Subteams;

	CREATE TABLE #Subteams (
		SubteamId INT NOT NULL IDENTITY(1, 1)
		,HierarchyClassId INT
		,HierarchyClassName NVARCHAR(255)
		,PdnTraitValue NVARCHAR(255)
		,NumTraitValue NVARCHAR(255)
		,NamTraitValue NVARCHAR(255)
		,FinTraitValue NVARCHAR(255));

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000006
		,'Candy, Snacks, Bev, Breakfast (1500)'
		,'296'
		,'100'
		,'Grocery'
		,'1500');

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000012
		,'Accoutrements (2450)'
		,'283'
		,'140'
		,'Specialty'
		,'2450');

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000007
		,'Commodity Cheese, Charcuterie (2460)'
		,'284'
		,'140'
		,'Specialty'
		,'2460');

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000013
		,'Value Add (1750)'
		,'285'
		,'120'
		,'Produce'
		,'1750');

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000014
		,'Bakery Production (4250)'
		,'286'
		,'220'
		,'Bakery'
		,'4250');

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000015
		,'Prep Foods Production (4850)'
		,'287'
		,'240'
		,'Prepared Foods'
		,'4850');

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000016
		,'Service Venue (4950)'
		,'289'
		,'240'
		,'Prepared Foods'
		,'4950');

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000008
		,'Allegro Coffee Bar (6270)'
		,'290'
		,'260'
		,'Third Party Vendors'
		,'6270');

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000009
		,'Amazon Initiative TBD 1 (5380)'
		,'291'
		,'550'
		,'Amazon Initiatives'
		,'5380');

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000010
		,'Amazon Initiative TBD 2 (5390)'
		,'292'
		,'550'
		,'Amazon Initiatives'
		,'5390');

	INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue)
	VALUES (
		1000011
		,'Amazon Initiative TBD 3 (5410)'
		,'293'
		,'550'
		,'Amazon Initiatives'
		,'5410');

	DECLARE cur CURSOR
	FOR
	SELECT SubteamId
		,HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue
	FROM #Subteams
	ORDER BY SubteamId;

	OPEN cur;

	FETCH cur
	INTO @SubteamId
		,@HierarchyClassId
		,@HierarchyClassName
		,@PdnTraitValue
		,@NumTraitValue
		,@NamTraitValue
		,@FinTraitValue;

	WHILE @@Fetch_Status = 0
	BEGIN
		IF NOT EXISTS (
				SELECT 1
				FROM dbo.HierarchyClass
				WHERE hierarchyClassName = @HierarchyClassName AND hierarchyID = @hierarchyId
				)
		BEGIN
			BEGIN TRANSACTION;

			BEGIN TRY
				SET IDENTITY_INSERT dbo.HierarchyClass ON

				--HierarchyClass
				INSERT INTO dbo.HierarchyClass (
					hierarchyClassID
					,[hierarchyLevel]
					,[hierarchyID]
					,[hierarchyParentClassID]
					,[hierarchyClassName]
					)
				VALUES (
					@HierarchyClassId
					,1
					,@HierarchyId
					,NULL
					,@HierarchyClassName
					)

				SET IDENTITY_INSERT dbo.HierarchyClass OFF;

				INSERT INTO dbo.HierarchyClassTrait (
					traitID
					,hierarchyClassID
					,uomID
					,traitValue
					)
				VALUES (
					@PdnTraitId
					,@HierarchyClassId
					,NULL
					,@PdnTraitValue
					);

				INSERT INTO dbo.HierarchyClassTrait (
					traitID
					,hierarchyClassID
					,uomID
					,traitValue
					)
				VALUES (
					@NumTraitId
					,@HierarchyClassId
					,NULL
					,@NumTraitValue
					);

				INSERT INTO [dbo].[HierarchyClassTrait] (
					traitID
					,hierarchyClassID
					,uomID
					,traitValue
					)
				VALUES (
					@FinTraitId
					,@HierarchyClassId
					,NULL
					,@FinTraitValue
					);

				INSERT INTO dbo.HierarchyClassTrait (
					traitID
					,hierarchyClassID
					,uomID
					,traitValue
					)
				VALUES (
					@NamTraitId
					,@HierarchyClassId
					,NULL
					,@NamTraitValue
					);

				INSERT INTO app.MessageQueueHierarchy (
					MessageTypeId
					,MessageStatusId
					,MessageHistoryId
					,MessageActionId
					,InsertDate
					,[HierarchyId]
					,HierarchyName
					,HierarchyLevelName
					,ItemsAttached
					,HierarchyClassId
					,HierarchyClassName
					,HierarchyLevel
					,HierarchyParentClassId
					,InProcessBy
					,ProcessedDate
					,NationalClassCode
					)
				SELECT @HierarchyMessageTypeId
					,@ReadyStatusId
					,NULL
					,@ActionId
					,GetDate()
					,h.HIERARCHYID
					,h.hierarchyName
					,hp.hierarchyLevelName
					,hp.itemsAttached
					,substring(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4)
					,hc.hierarchyClassName
					,hc.hierarchyLevel
					,hc.hierarchyParentClassID
					,NULL
					,NULL
					,NULL
				FROM Hierarchy h
				JOIN HierarchyClass hc ON h.HIERARCHYID = hc.HIERARCHYID
				JOIN HierarchyPrototype hp ON hc.HIERARCHYID = hp.HIERARCHYID
					AND hc.hierarchyLevel = hp.hierarchyLevel
				WHERE h.HIERARCHYID = @hierarchyId
					AND hc.hierarchyLevel = @financialHierarchyClassLevel
					AND hc.hierarchyClassName = @HierarchyClassName;

				COMMIT TRANSACTION;

				PRINT 'Hierarchy Class: ' + @HierarchyClassName + ' added.'
			END TRY

			BEGIN CATCH
				IF @@TRANCOUNT > 0
				BEGIN
					PRINT 'Error:';
					SET @Error = ERROR_MESSAGE();

					SELECT ERROR_NUMBER() AS ErrorNumber
						,ERROR_SEVERITY() AS ErrorSeverity
						,ERROR_STATE() AS ErrorState
						,ERROR_PROCEDURE() AS ErrorProcedure
						,ERROR_LINE() AS ErrorLine
						,@Error AS ErrorMessage;

					ROLLBACK TRANSACTION;
				END
			END CATCH;
		END

		FETCH cur
		INTO @SubteamId
			,@HierarchyClassId
			,@HierarchyClassName
			,@PdnTraitValue
			,@NumTraitValue
			,@NamTraitValue
			,@FinTraitValue;
	END

	CLOSE cur;

	DEALLOCATE cur;

	IF(@Error IS NULL)
	BEGIN
		INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime)
			VALUES (@scriptKey , GETDATE());

		PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Completed: ' + @scriptKey;
	END
	ELSE
	BEGIN
		PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Failed: ' + @scriptKey + ' Error = ' + @Error;
	END
	
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey;
END

SET NOCOUNT OFF;
GO