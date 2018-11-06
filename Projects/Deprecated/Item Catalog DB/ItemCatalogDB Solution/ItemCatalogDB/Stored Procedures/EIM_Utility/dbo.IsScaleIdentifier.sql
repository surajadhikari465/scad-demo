if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IsScaleIdentifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[IsScaleIdentifier]

GO

Create PROCEDURE [dbo].[IsScaleIdentifier] 
	@Identifier varchar(13)
	,
	@IsScaleIdentifier bit OUTPUT
AS 
BEGIN
    
    SELECT @IsScaleIdentifier = dbo.fn_IsScaleIdentifier(@Identifier)

END

GO