CREATE PROCEDURE dbo.ValidateIfTableExists
	@Schema VARCHAR(50),
	@Table VARCHAR(64)
AS

BEGIN
	SELECT CASE WHEN (EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' and table_name =@Table AND table_schema =  @Schema))
              THEN 1
              ELSE 0 END
              AS IsExists
END

GO

GRANT EXECUTE ON OBJECT::dbo.ValidateIfTableExists TO [IRSUser] AS [dbo];
GO
GRANT EXECUTE ON OBJECT::dbo.ValidateIfTableExists TO [IRMAClientRole] AS [dbo];
GO