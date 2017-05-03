SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertItemUnit]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemUnit]
GO


CREATE PROCEDURE dbo.InsertItemUnit
@Unit_Name varchar(50)
AS 

INSERT INTO ItemUnit (Unit_Name)
VALUES (@Unit_Name)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

