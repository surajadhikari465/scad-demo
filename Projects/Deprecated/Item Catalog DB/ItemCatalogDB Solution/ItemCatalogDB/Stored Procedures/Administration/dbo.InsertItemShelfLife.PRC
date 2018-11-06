SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertItemShelfLife]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemShelfLife]
GO


CREATE PROCEDURE dbo.InsertItemShelfLife
@ShelfLife_Name varchar(50)
AS 

INSERT INTO ItemShelfLife (ShelfLife_Name)
VALUES (@ShelfLife_Name)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

