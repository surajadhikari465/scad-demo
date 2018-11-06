SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertItemBrand]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemBrand]
GO


CREATE PROCEDURE dbo.InsertItemBrand
@Brand_Name varchar(50)
AS 

INSERT INTO ItemBrand (Brand_Name)
VALUES (@Brand_Name)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


