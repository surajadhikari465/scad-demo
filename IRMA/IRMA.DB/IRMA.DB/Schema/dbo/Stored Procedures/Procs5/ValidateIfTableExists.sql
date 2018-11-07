
CREATE PROCEDURE dbo.ValidateIfTableExists
(
	@Schema VARCHAR(50),
	@Table VARCHAR(64),
	@IsTableValid bit output
)
AS

BEGIN

	IF EXISTS(SELECT table_name , table_schema
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_TYPE = 'BASE TABLE' and table_name =@Table AND table_schema =  @Schema
	)

	BEGIN
	   
	    SET @IsTableValid=1

	END

	ELSE

	BEGIN
		   SET @IsTableValid=0
	END
	RETURN  @IsTableValid
END
