
/****** Object:  StoredProcedure [dbo].[EPromotions_ValidateGroupName]    Script Date: 06/16/2006 13:05:05 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_ValidateGroupName]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure  [dbo].[EPromotions_ValidateGroupName]
	END

GO

CREATE PROCEDURE [dbo].[EPromotions_ValidateGroupName]
	@GroupName varchar(255),
	@DeletedGroupIds varchar(255)
	
AS 

BEGIN
    SET NOCOUNT ON

	-- Return False if GroupName already exists. Return True if it does not.
	declare @sql varchar(1000)
	
	set @sql = 'SELECT CASE WHEN EXISTS (SELECT GroupName FROM ItemGroup WHERE GroupName = ''' + @GroupName + ''' and Group_Id not in (' + @DeletedGroupIds + ')) THEN ''false'' ELSE ''true'' END AS isValid '
	exec(@sql)


END

GO