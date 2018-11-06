IF NOT EXISTS (	SELECT * FROM dbo.sysobjects WHERE OBJECTPROPERTY(id, N'IsProcedure') = 1 AND
	id = OBJECT_ID(N'[dbo].[InsertItemIdentifier]') 
)
BEGIN
	EXEC ('CREATE PROCEDURE dbo.InsertItemIdentifier AS SELECT 1')
END
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

ALTER PROCEDURE [dbo].[InsertItemIdentifier]
    @Item_Key					int,
    @IdentifierType				char(1),
    @Identifier					varchar(13),
    @CheckDigit					char(1),
    @National_Identifier		tinyint,
    @NumPluDigitsSentToScale	int, 
    @Scale_Identifier			bit
AS 

-- ****************************************************************************************************************
-- Procedure: [InsertItemIdentifier]
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012-04-21	KM		14989	For now, don't add new item events to the queue for PLUs.   
-- 2012-06-16   MZ      15157   Checked the two app config keys (EnableUPCIRMAToIConFlow and EnablePLUIRMAIConFlow). 
--                              Depending on the config keys' seeting, insert new item events into iConItemChangeQueue 
--								only for UPCs or PLUs or both.        
-- 2015-08-24   MZ      16385   Also generates new item event for Icon for non-retail items in specified ranges.
-- ****************************************************************************************************************

BEGIN
    SET NOCOUNT ON

    -- Make sure the input Identifier is an integer
    DECLARE @Error_No int, @X bigint, @newItemChgTypeID tinyint, @Retail_Sale bit
    SELECT @X = CAST(@Identifier as bigint)
	SELECT @newItemChgTypeID = itemchgtypeid FROM itemchgtype WHERE itemchgtypedesc like 'new'
	SELECT @Retail_Sale = Retail_Sale FROM item WHERE Item_Key = @Item_Key

	DECLARE @EnableUPCIRMAToIConFlow bit
	SELECT  @EnableUPCIRMAToIConFlow = acv.Value
			FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
			ON acv.EnvironmentID = ace.EnvironmentID 
			INNER JOIN AppConfigApp aca
			ON acv.ApplicationID = aca.ApplicationID 
			INNER JOIN AppConfigKey ack
			ON acv.KeyID = ack.KeyID 
			WHERE aca.Name = 'IRMA Client' AND
			ack.Name = 'EnableUPCIRMAToIConFlow' and
			SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
	
	DECLARE @EnablePLUIRMAIConFlow bit
	SELECT @EnablePLUIRMAIConFlow = acv.Value
			FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
			ON acv.EnvironmentID = ace.EnvironmentID 
			INNER JOIN AppConfigApp aca
			ON acv.ApplicationID = aca.ApplicationID 
			INNER JOIN AppConfigKey ack
			ON acv.KeyID = ack.KeyID 
			WHERE aca.Name = 'IRMA Client' AND
			ack.Name = 'EnablePLUIRMAIConFlow' and
			SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
		BEGIN
			SET NOCOUNT OFF
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
			RAISERROR ('InsertItemIdentifier failed with @@ERROR: %d', @Severity, 1, @Error_No)
			RETURN
		END
   
    INSERT INTO ItemIdentifier 
	(
		Item_Key, 
		Identifier, 
		Add_Identifier, 
		Default_Identifier, 
		IdentifierType, 
		CheckDigit, 
		National_Identifier, 
		NumPluDigitsSentToScale, 
		Scale_Identifier
	)
    SELECT 
		@Item_Key, 
		@Identifier, 
		[Add_Identifier] =	-- TFS 3585; don't set Add_Identifier for secondary identifiers if non-processed PBD rows exist for the new, primary identifier
			CASE 
				WHEN EXISTS (SELECT *
							FROM PriceBatchDetail PBD (NOLOCK)
								LEFT JOIN PriceBatchHeader PBH (NOLOCK) ON PBH.PriceBatchHeaderID = PBD.PriceBatchHeaderID
							WHERE ISNULL(PBH.PriceBatchStatusID, 0) < 6 -- (6 = Processed)
								AND PBD.ItemChgTypeID = @newItemChgTypeID -- (1 = New)
								AND PBD.Item_Key = @Item_Key
								AND ISNULL(PBD.Expired, 0) = 0)
				THEN 0
				ELSE 1
			END,
		[Default_Identifier] = 0, 
		@IdentifierType, 
		CASE WHEN RTRIM(ISNULL(@CheckDigit, '')) = '' THEN NULL ELSE @CheckDigit END,
		@National_Identifier,
		@NumPluDigitsSentToScale,
		@Scale_Identifier

    SELECT @Error_No = @@ERROR

	-- [iCon] Add identifier-add to event queue only if the item is marked as retail sale item.
	-- Additionally, only add an event to the queue if the identifier is NOT a PLU.
	IF @Error_No = 0 AND @Retail_Sale = 1 
	BEGIN
		IF	(@EnableUPCIRMAToIConFlow = 1 AND @EnablePLUIRMAIConFlow = 1) OR
			(@EnableUPCIRMAToIConFlow = 1 AND NOT (LEN(@Identifier) < 7 OR @Identifier LIKE '2%00000')) OR
			(@EnablePLUIRMAIConFlow = 1 AND (LEN(@Identifier) < 7 OR @Identifier LIKE '2%00000'))
		BEGIN
			INSERT INTO iConItemChangeQueue 
			(
				Item_Key, 
				Identifier, 
				ItemChgTypeID
			)
			SELECT
				Item_Key = @Item_Key,
				Identifier = @Identifier,
				ItemChgTypeID = @newItemChgTypeID
		END
	END
	
	IF @Error_No = 0 AND @Retail_Sale = 0
		BEGIN
			IF ((CONVERT(FLOAT, @Identifier) >= 46000000000 And CONVERT(FLOAT, @Identifier)  <= 46999999999) OR 
				(CONVERT(FLOAT, @Identifier) >= 48000000000 And CONVERT(FLOAT, @Identifier) <= 48999999999))
			BEGIN
			INSERT INTO iConItemChangeQueue 
			(
				Item_Key, 
				Identifier, 
				ItemChgTypeID
			)
			SELECT
				Item_Key = @Item_Key,
				Identifier = @Identifier,
				ItemChgTypeID = @newItemChgTypeID
			END
		END

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO
