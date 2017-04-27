SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertGLPushHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertGLPushHistory]
GO


CREATE PROCEDURE dbo.InsertGLPushHistory
@DateStamp DateTime,
@Store_No int, 
@Sales_Date DateTime, 
@Modified_By int, 
@Closed bit, 
@Credit money, 
@Debit money, 
@Account_ID varchar(50)
AS

INSERT INTO GLPushHistory (DateStamp, Store_No, Sales_Date, Modified_By, Closed, Credit, Debit, Account_ID)
VALUES (@DateStamp, @Store_No, @Sales_Date, @Modified_By, @Closed, @Credit, @Debit, @Account_ID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


