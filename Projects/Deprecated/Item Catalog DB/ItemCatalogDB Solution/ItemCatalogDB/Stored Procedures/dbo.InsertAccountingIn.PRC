SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertAccountingIn]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertAccountingIn]
GO


CREATE PROCEDURE dbo.InsertAccountingIn
@OrderHeader_ID  int,
@User_ID int
AS

UPDATE OrderHeader 
SET Accounting_In_DateStamp = GetDate(), Accounting_In_UserID = @User_ID 
WHERE OrderHeader_ID = @OrderHeader_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


