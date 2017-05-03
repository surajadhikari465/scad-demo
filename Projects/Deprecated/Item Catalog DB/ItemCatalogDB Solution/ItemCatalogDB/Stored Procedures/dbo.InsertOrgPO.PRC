SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertOrgPO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertOrgPO]
GO



CREATE PROCEDURE dbo.InsertOrgPO
@OrgOrderHeader_ID int,
@ReturnOrderHeader_ID int
AS 

BEGIN


DELETE ReturnOrderList WHERE ReturnOrderHeader_ID = @ReturnOrderHeader_ID

 INSERT INTO ReturnOrderList (OrderHeader_ID, ReturnOrderHeader_ID) VALUES (@OrgOrderHeader_ID, @ReturnOrderHeader_ID)
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


