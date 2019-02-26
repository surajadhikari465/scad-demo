CREATE PROCEDURE [dbo].[AuditItemLocale]
  @action VARCHAR(25),
  @region VARCHAR(2),
  @groupSize INT = 250000,
  @maxRows INT = 0,
  @groupId INT = 0
AS
BEGIN
  SET NOCOUNT ON;

  IF(Not Exists(SELECT 1 FROM Regions WHERE Region = @region))
  BEGIN
    DECLARE @msg varchar(255) = 'Invalid region specified ' + @region + '.';
    RAISERROR (@msg, 16, 1);
    RETURN;
  END

  IF @action = 'Initilize'
  BEGIN
    EXEC stage.ItemLocaleExport @Region = @region, @GroupSize = @groupSize, @maxRows = @maxRows;
    return;
  END
  
  IF @action = 'Get'
  BEGIN
    SELECT A.* FROM stage.ItemLocaleExportStaging A WHERE A.GroupId = IsNull(@groupId, A.GroupId);
    RETURN;
  END

  SET @msg = 'Unsupported action (' + @action + ').';
  RAISERROR (@msg, 16, 1);
  RETURN;

  SET NOCOUNT OFF;
END
GO

GRANT EXECUTE ON [dbo].AddOrUpdateItems TO [MammothRole]
GO