SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateCategoryInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateCategoryInfo]
GO


CREATE PROCEDURE dbo.UpdateCategoryInfo
@Category_ID int,
@Category_Name varchar(50),
@SubTeam_No int
AS 

UPDATE ItemCategory
SET User_ID = NULL,
    Category_Name = @Category_Name,
    SubTeam_No = @SubTeam_No
WHERE Category_ID = @Category_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


