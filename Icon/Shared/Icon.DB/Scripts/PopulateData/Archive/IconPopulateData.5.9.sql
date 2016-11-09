/*
All pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.
*/

go

--Insert Frozen values
IF NOT EXISTS (SELECT * FROM SeafoodFreshOrFrozen where Description = 'Frozen')
BEGIN
	SET IDENTITY_INSERT SeafoodFreshOrFrozen ON

	insert into SeafoodFreshOrFrozen(SeafoodFreshOrFrozenId, Description)
	values(3, 'Frozen')

	SET IDENTITY_INSERT SeafoodFreshOrFrozen OFF
END


--NOTES 
--> This script must be run by a user with BULK INSERT permissions.
-- Update file path based on file location


BEGIN

	DECLARE @hierarchyFile nvarchar(128) = '\\cen106129\c$\Users\kasthuri.kona\Desktop\VIM\VIM StoreData.txt'
	DECLARE @file_exists int
	DECLARE @businessUnitTraitID int, @irmaStoreIdTraitID int, @postypeTraitID int, @faxTraitID int;
	SELECT @file_exists = 0

	EXEC master.dbo.xp_fileexist
		@hierarchyFile,
		@file_exists OUTPUT

	IF @file_exists = 1
	BEGIN
		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'VIM StoreData input-file found.'
	END
	ELSE
	BEGIN
		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + '[[Error -- Cannot Continue]] VIM StoreData  file not found.  Please verify file and try again.'
		set noexec on
	END

	SET @businessUnitTraitID = (select traitID from Trait where traitCode = 'BU')
	SET @irmaStoreIdTraitID  = (select traitID from Trait where traitCode = 'ISI')
	SET @postypeTraitID      = (select traitID from Trait where traitCode = 'SPT')
	SET @faxTraitID			 = (select traitID from Trait where traitCode = 'FAX')


	CREATE TABLE #tmpVIMStoreDataAll
			(
				PS_BU				[varchar](255) NOT NULL,
				REG_STORE_NUM		[varchar](255),	
				REGION				[varchar](255),	
				STORE_NAME			[varchar](255),
				STORE_ABBR			[varchar](255),
				POSTYPE				[varchar](255),
				ADDR1				[varchar](255),
				ADDR2				[varchar](255),
				CITY				[varchar](255),
				STATE_PROVINCE		[varchar](255),
				POSTAL_CODE			[varchar](255),
				COUNTRY				[varchar](255),
				PHONE				[varchar](255),
				FAX					[varchar](255),
				SERVERNAME			[varchar](255),
				LASTUSER			[varchar](255),
				TIMESTAMP			[varchar](255)
			
		) ON [PRIMARY]
		
	-- Import VIM store data.
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Importing VIm Store data text file...'
	BULK INSERT #tmpVIMStoreDataAll FROM '\\cen106129\c$\Users\kasthuri.kona\Desktop\VIM\VIM StoreData.txt'
	WITH (FIRSTROW = 2, FIELDTERMINATOR = ',', rowterminator = '\n') 


	SELECT DISTINCT l.localeID, ltt.traitValue  AS BU
	INTO #IconLocaleData
	FROM Locale l
	JOIN LocaleType lt on l.localeTypeID = lt.localeTypeID and lt.localeTypeCode = 'ST'
	JOIN LocaleTrait ltt on l.localeID = ltt.localeID and ltt.traitID = @businessUnitTraitID


	--Insert locale irma store ids
	MERGE
		LocaleTrait WITH (updlock, rowlock) it
	USING
		#IconLocaleData ild
		JOIN  #tmpVIMStoreDataAll vsd ON ild.BU = vsd.PS_BU --and LOWER(vsd.REG_STORE_NUM) <> 'xnew' and vsd.REG_STORE_NUM is not null and vsd.REG_STORE_NUM <> 'null' 
	ON
		it.localeID = ild.localeID and it.traitID = @irmaStoreIdTraitID
	WHEN matched and LOWER(vsd.REG_STORE_NUM) <> 'xnew' and vsd.REG_STORE_NUM is not null and vsd.REG_STORE_NUM <> 'null'  THEN
		update set it.[traitValue] = vsd.REG_STORE_NUM
	WHEN matched and (LOWER(vsd.REG_STORE_NUM) = 'xnew' or vsd.REG_STORE_NUM is null or vsd.REG_STORE_NUM = 'null'  ) THEN
		DELETE
	WHEN not matched and LOWER(vsd.REG_STORE_NUM) <> 'xnew' and vsd.REG_STORE_NUM is not null and vsd.REG_STORE_NUM <> 'null' THEN	
		INSERT ([localeID], [traitID], [uomID], [traitValue])
		VALUES (ild.localeID, @irmaStoreIdTraitID, null, vsd.REG_STORE_NUM);
	

	--insert locale POS types
	MERGE
		LocaleTrait WITH (updlock, rowlock) it
	USING
		#IconLocaleData ild
		JOIN  #tmpVIMStoreDataAll vsd ON ild.BU = vsd.PS_BU
	ON
		it.localeID = ild.localeID and it.traitID = @postypeTraitID --and vsd.POSTYPE is not null and vsd.POSTYPE <> 'null' 
	WHEN matched and vsd.POSTYPE is not null and vsd.POSTYPE <> 'null'  THEN
		UPDATE SET it.[traitValue] = CASE WHEN lower(vsd.POSTYPE) = 'new' THEN 'xNEW'
										  WHEN lower(vsd.POSTYPE) = 'closed' THEN 'CLOSED'
										  ELSE SUBSTRING (vsd.POSTYPE, 0, len(vsd.POSTYPE) - 4)
									END
	WHEN matched and (vsd.POSTYPE is null or vsd.POSTYPE = 'null' ) THEN
		DELETE
	WHEN not matched and vsd.POSTYPE is not null and vsd.POSTYPE <> 'null'   THEN	 
		INSERT ([localeID], [traitID], [uomID], [traitValue])
		VALUES (
					ild.localeID,
					@postypeTraitID,
					null,
					CASE WHEN lower(vsd.POSTYPE) = 'new' THEN 'xNEW'
						 WHEN lower(vsd.POSTYPE) = 'closed' THEN 'CLOSED'
						 ELSE SUBSTRING (vsd.POSTYPE, 0, len(vsd.POSTYPE) - 4)
					END
				);


	--insert locale FAX numbers
	MERGE
		LocaleTrait WITH (updlock, rowlock) it
	USING
		#IconLocaleData ild
		JOIN  #tmpVIMStoreDataAll vsd ON ild.BU = vsd.PS_BU-- and vsd.FAX is not null and vsd.FAX <> 'null' and vsd.FAX <> 'n/a'
	ON
		it.localeID = ild.localeID and it.traitID = @faxTraitID
	WHEN matched and vsd.FAX is not null and vsd.FAX <> 'null' and vsd.FAX <> 'n/a' THEN
		UPDATE SET it.[traitValue] = vsd.FAX
	WHEN matched and ( vsd.FAX is null or vsd.FAX = 'null' or vsd.FAX = 'n/a' ) THEN
		DELETE
	WHEN not matched and vsd.FAX is not null and vsd.FAX <> 'null' and vsd.FAX <> 'n/a' THEN	 
		INSERT ([localeID], [traitID], [uomID], [traitValue])
		VALUES (
					ild.localeID,
					@faxTraitID,
					null,
					vsd.FAX
				);

	DROP TABLE #IconLocaleData
	DROP TABLE  #tmpVIMStoreDataAll
	
	set noexec off
END

go
-- TFS_11918_Clear_ItemMovementErrorQueue
print N'TFS 11918: Clearing ItemMovementErrorQueue...'
DELETE FROM app.ItemMovementErrorQueue
go

