-- ******************* Add Alternate Jurisidiction Fields to ItemOverride ************************
-- ** PBI 12153 July 2017
-- ** Adding SignRomanceTextLong and SignRomanceTextShort columns to dbo.ItemOverride table
-- ** if they are not already there
-- ***********************************************************************************************

IF EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[ItemOverride]') 
      AND [name] = 'SignRomanceTextLong'
)
BEGIN
    --dropping alt jurisdiction columns from dbo.ItemOverride
    ALTER TABLE [dbo].[ItemOverride] DROP COLUMN [SignRomanceTextLong];
    ALTER TABLE [dbo].[ItemOverride] DROP COLUMN [SignRomanceTextShort];
END

GO