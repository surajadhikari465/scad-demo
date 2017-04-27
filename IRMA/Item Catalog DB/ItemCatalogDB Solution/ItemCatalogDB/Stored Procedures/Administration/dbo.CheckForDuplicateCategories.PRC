SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateCategories]
GO


CREATE PROCEDURE dbo.CheckForDuplicateCategories 
    @Category_ID int, 
    @Category_Name varchar(35), --Changed the size from 25 to 35 to fix Bug No:5390.
    @SubTeam_No int 
AS 

SELECT COUNT(*) AS CategoryCount 
FROM ItemCategory 
WHERE Category_Name = @Category_Name
AND SubTeam_No = @SubTeam_No
AND Category_ID <> @Category_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


