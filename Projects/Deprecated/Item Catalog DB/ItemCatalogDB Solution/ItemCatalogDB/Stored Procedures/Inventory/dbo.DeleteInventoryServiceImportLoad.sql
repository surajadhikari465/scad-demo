IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'DeleteInventoryServiceImportLoad')
	DROP  PROCEDURE  dbo.DeleteInventoryServiceImportLoad
GO

CREATE PROCEDURE dbo.DeleteInventoryServiceImportLoad
    @BusinessUnit_ID int
AS
    --Delete all data for the input Business Unit for the current processing period
    DECLARE @EOP smalldatetime, @Period int, @Year int
    
    SELECT @EOP = dbo.fn_LoadExternalCycleCountEOP(NULL)

    SELECT @Period = Period, @Year = [Year] FROM [Date] (nolock) WHERE Date_Key = CONVERT(smalldatetime, CONVERT(varchar(255), @EOP, 101))

    DELETE InventoryServiceImportLoad
    WHERE Period = @Period AND [Year] = @Year AND PS_BU = @BusinessUnit_ID

GO