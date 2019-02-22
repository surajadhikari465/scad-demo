CREATE PROCEDURE [dbo].[AuditItemLocale]
  @action VARCHAR(25),
  @region VARCHAR(2),
  @groupSize INT = 250000,
  @maxRows INT = 0,
  @groupId INT = 0
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @msg varchar(Max) = null,
          @cols NVARCHAR(Max) = null,
          @query NVARCHAR(Max) = null;

  IF(Not Exists(SELECT 1 FROM Regions WHERE Region = @region))
  BEGIN
    SET @msg = 'Invalid region specified ' + @region + '.';
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
