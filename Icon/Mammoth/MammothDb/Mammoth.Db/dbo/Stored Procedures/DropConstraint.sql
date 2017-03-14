CREATE PROCEDURE dbo.DropConstraint 
	@Constraint NVARCHAR(128) AS BEGIN

	DECLARE @sqlText NVARCHAR(MAX)
	DECLARE @TableSchema NVARCHAR(128)
	DECLARE @TableName NVARCHAR(128)

	SELECT @TableSchema = NULL, @TableName = NULL

	SELECT @TableSchema = TABLE_SCHEMA, @TableName = TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_NAME = @Constraint 

	IF @TableName IS NOT NULL	BEGIN

		SET @sqlText = N'
			IF EXISTS (	SELECT * 
						FROM sys.foreign_keys 
						WHERE object_id = OBJECT_ID(' + QUOTENAME(@Constraint, '''') + ') AND parent_object_id = OBJECT_ID(' + QUOTENAME(@TableName, '''') + '))
					ALTER TABLE ' + QUOTENAME(@TableSchema) + '.' + QUOTENAME(@TableName) + ' DROP CONSTRAINT ' + QUOTENAME(@Constraint);
		PRINT @sqlText;
		EXECUTE sp_executesql @sqlText;

	END

END