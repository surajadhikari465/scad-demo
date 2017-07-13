-- ******************* Add Alternate Jurisidiction Fields to ItemOverride ************************
-- ** PBI 12153 July 2017
-- ** Adding SignRomanceTextLong and SignRomanceTextShort columns to dbo.ItemOverride table
-- ** if they are not already there
-- ***********************************************************************************************

IF NOT EXISTS (
    SELECT * 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[ItemOverride]') 
      AND [name] = 'SignRomanceTextLong'
)
BEGIN
    --adding alt jurisdiction columns to dbo.ItemOverride
    ALTER TABLE [dbo].[ItemOverride]
         ADD [SignRomanceTextLong]           NVARCHAR(300)  NULL,
            [SignRomanceTextShort]          NVARCHAR(140)  NULL;
END

GO