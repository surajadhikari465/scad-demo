CREATE PROCEDURE [dbo].[CopyKitLocaleProperties]
	 @kitId INT
	,@fromLocaleId INT
	,@toLocaleIds dbo.CopyToLocalesType READONLY
AS
BEGIN
    
	SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

	DECLARE @statusId INT, @fromKitLocaleId INT
	DECLARE @KitLocaletable table (CopyFromKitLocaleId int, CopyToKitLocaleId int)
	DECLARE @KitLinkGroupLocaletable table (CopyFromKitLinkGroupLocaleId int, CopyToKitLinkGroupLocaleId int)
	
	SELECT @statusId = StatusID FROM Status WHERE StatusDescription = 'ReadyToPublish'
	
	SELECT @fromKitLocaleId = KitLocaleId 
	  FROM KitLocale 
	 WHERE KitId = @kitId
       AND LocaleId = @fromLocaleId

	BEGIN TRY
	BEGIN TRAN
	INSERT INTO [dbo].[KitLocale]
           ([KitId]
           ,[LocaleId]
           ,[MinimumCalories]
           ,[MaximumCalories]
           ,[Exclude]
           ,[StatusId]
           ,[InsertDateUtc]
           )
	OUTPUT @fromKitLocaleId,INSERTED.KitLocaleId
	  INTO @KitLocaletable(CopyFromKitLocaleId, CopyToKitLocaleId)
	SELECT  kl.[KitId]
           ,tl.[LocaleId]
           ,kl.[MinimumCalories]
           ,kl.[MaximumCalories]
           ,kl.[Exclude]
           ,@statusId
           ,GETUTCDATE()
	  FROM KitLocale kl
CROSS JOIN @toLocaleIds tl
     WHERE kl.KitId = @kitId
	   AND kl.LocaleId = @fromLocaleId

	INSERT INTO [dbo].[KitLinkGroupLocale]
           ([KitLinkGroupId]
           ,[KitLocaleId]
           ,[Properties]
           ,[DisplaySequence]
           ,[MinimumCalories]
           ,[MaximumCalories]
           ,[Exclude]
           ,[InsertDateUtc])
	OUTPUT INSERTED.KitLinkGroupLocaleId
	  INTO @KitLinkGroupLocaletable(CopyToKitLinkGroupLocaleId)
	SELECT klgl.[KitLinkGroupId]
		  ,kl.[CopyToKitLocaleId]
		  ,klgl.[Properties]
		  ,klgl.[DisplaySequence]
		  ,klgl.[MinimumCalories]
		  ,klgl.[MaximumCalories]
		  ,klgl.[Exclude]
		  ,GETUTCDATE()
	  FROM KitLinkGroupLocale klgl
	  JOIN @KitLocaletable kl on klgl.KitLocaleId = kl.CopyFromKitLocaleId 

	UPDATE @KitLinkGroupLocaletable
	   SET CopyFromKitLinkGroupLocaleId = klgf.KitLinkGroupLocaleId
	  FROM @KitLinkGroupLocaletable kllt
	  JOIN KitLinkGroupLocale klgt on kllt.CopyToKitLinkGroupLocaleId = klgt.KitLinkGroupLocaleId 
	  JOIN KitLinkGroupLocale klgf on klgt.KitLinkGroupId = klgf.KitLinkGroupId
	 WHERE klgf.KitLocaleId = @fromKitLocaleId

	INSERT INTO [dbo].[KitLinkGroupItemLocale]
           ([KitLinkGroupItemId]
           ,[KitLinkGroupLocaleId]
           ,[Properties]
           ,[DisplaySequence]
           ,[Exclude]
           ,[InsertDateUtc])
	 SELECT klgl.[KitLinkGroupItemId]
           ,klt.[CopyToKitLinkGroupLocaleId]
           ,klgl.[Properties]
           ,klgl.[DisplaySequence]
           ,klgl.[Exclude]
		   ,GETUTCDATE()
	   FROM [dbo].[KitLinkGroupItemLocale] klgl
	   JOIN @KitLinkGroupLocaletable klt on klgl.KitLinkGroupLocaleId = klt.CopyFromKitLinkGroupLocaleId
	
	SET NOCOUNT OFF

	COMMIT TRAN
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('CopyKitLocaleProperties failed with @@ERROR: %d', @Severity, 1, @error_no)
    END CATCH
END

