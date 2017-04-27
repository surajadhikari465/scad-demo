SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemTypeList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemTypeList]
GO


CREATE PROCEDURE dbo.ItemTypeList
AS 
SELECT ItemType_ID, ItemType_Name
FROM ItemType
ORDER By ItemType_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


