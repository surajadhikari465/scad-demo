SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertItemCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemCategory]
GO


CREATE PROCEDURE dbo.InsertItemCategory
@Category_Name varchar(50),
@SubTeam_No int
AS 

INSERT INTO ItemCategory (Category_Name, SubTeam_No)
VALUES (@Category_Name, @SubTeam_No)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


