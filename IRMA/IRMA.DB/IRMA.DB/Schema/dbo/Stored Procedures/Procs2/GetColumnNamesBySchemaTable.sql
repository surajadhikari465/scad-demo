CREATE PROCEDURE dbo.GetColumnNamesBySchemaTable
  @Schema VARCHAR(50),
  @Table VARCHAR(64)
AS
BEGIN
		SELECT COLUMN_NAME
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME= @Table and TABLE_SCHEMA = @Schema and DATA_TYPE IN('date', 'datetime', 'datetime2', 'smalldatetime')
    ORDER BY COLUMN_NAME
END
GO

GRANT EXECUTE ON OBJECT::dbo.GetColumnNamesBySchemaTable TO [IRSUser];
GO
GRANT EXECUTE ON OBJECT::dbo.GetColumnNamesBySchemaTable TO [IRMAClientRole];
GO