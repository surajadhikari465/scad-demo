SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOriginInfoLast]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOriginInfoLast]
GO


CREATE PROCEDURE dbo.GetOriginInfoLast
AS 

SELECT Origin_Name, Origin_ID, User_ID 
FROM ItemOrigin
WHERE Origin_ID = (SELECT MAX(Origin_ID) FROM ItemOrigin)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



