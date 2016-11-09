/*
All pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.
*/

go

-- Insert updated values into the RetentionPolicy table (Production only!)
begin
	if (@@SERVERNAME = 'SQLSHARED3-PRD3\SHARED3P')
		begin
			USE [Icon]
		
			truncate table app.RetentionPolicy

			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'AppLog', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'IRMAPush', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageHistory', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueHierarchy', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueItemLocale', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueLocale', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueProduct', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueuePrice', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'MessageQueueProductSelectionGroup', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'R10MessageResponse', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'SQLSHARED3-PRD3\SHARED3P', N'Icon', N'app', N'EventQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-FL\FLP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-FL\FLP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-MA\MAP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-MA\MAP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-MW\MWP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-MW\MWP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NA\NAP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NA\NAP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-RM\RMP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-RM\RMP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SO\SOP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SO\SOP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NC\NCP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NC\NCP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NE\NEP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-NE\NEP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-PN\PNP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-PN\PNP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SP\SPP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SP\SPP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SW\SWP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-SW\SWP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-UK\UKP', N'ItemCatalog', N'dbo', N'IConPOSPushPublish', 10)
			INSERT [app].[RetentionPolicy] ([Server], [Database], [Schema], [Table], [DaysToKeep]) VALUES (N'IDP-UK\UKP', N'ItemCatalog', N'dbo', N'IconItemChangeQueue', 10)
		end
end
go



--NOTES 
--> This script must be run by a user with BULK INSERT permissions.
-- Update file path based on file location


BEGIN

	DECLARE @vimFile nvarchar(128) = '\\cewd6591\e$\VIM StoreData.txt'
	DECLARE @file_exists int
	DECLARE @businessUnitTraitID int, @phoneTraitID int
	SELECT @file_exists = 0

	EXEC master.dbo.xp_fileexist
		@vimFile,
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
	SET @phoneTraitID  = (select traitID from Trait where traitCode = 'PHN')
	


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
	BULK INSERT #tmpVIMStoreDataAll FROM '\\cewd6591\e$\VIM StoreData.txt'
	WITH (FIRSTROW = 2, FIELDTERMINATOR = ',', rowterminator = '\n') 


	SELECT DISTINCT l.localeID, ltt.traitValue  AS BU
	INTO #IconLocaleData
	FROM Locale l
	JOIN LocaleType lt on l.localeTypeID = lt.localeTypeID and lt.localeTypeCode = 'ST'
	JOIN LocaleTrait ltt on l.localeID = ltt.localeID and ltt.traitID = @businessUnitTraitID



	--insert locale Phone numbers
	MERGE
		LocaleTrait WITH (updlock, rowlock) it
	USING
		#IconLocaleData ild
		JOIN  #tmpVIMStoreDataAll vsd ON ild.BU = vsd.PS_BU and vsd.PHONE is not null and vsd.PHONE <> 'null' and vsd.PHONE <> 'n/a' and vsd.PHONE <> '0'
	ON
		it.localeID = ild.localeID and it.traitID = @phoneTraitID
	WHEN matched and  Ltrim(Rtrim(it.traitValue)) = '512-999-9999' THEN
		UPDATE SET it.[traitValue] = vsd.PHONE
	
	WHEN not matched THEN	 
		INSERT ([localeID], [traitID], [uomID], [traitValue])
		VALUES (
					ild.localeID,
					@phoneTraitID,
					null,
					vsd.PHONE
				);

	DROP TABLE #IconLocaleData
	DROP TABLE  #tmpVIMStoreDataAll
	
	set noexec off
END
