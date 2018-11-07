SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertItemOrigin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemOrigin]
GO


CREATE PROCEDURE dbo.InsertItemOrigin
@Origin_Name varchar(50)
AS 

INSERT INTO ItemOrigin (Origin_Name)
VALUES (@Origin_Name)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

