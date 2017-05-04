SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUsersLastOrderHeaderID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUsersLastOrderHeaderID]
GO


CREATE PROCEDURE dbo.GetUsersLastOrderHeaderID
@CreatedBy int
AS 

SELECT MAX(OrderHeader_ID) AS OrderHeader_ID
FROM OrderHeader 
WHERE CreatedBy = @CreatedBy

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


