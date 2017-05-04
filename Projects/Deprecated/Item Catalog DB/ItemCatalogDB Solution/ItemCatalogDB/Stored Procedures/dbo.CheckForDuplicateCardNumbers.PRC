SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateCardNumbers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateCardNumbers]
GO


CREATE PROCEDURE dbo.CheckForDuplicateCardNumbers
@Begin_Number int,
@End_Number int 
AS 

SELECT COUNT(*) AS CustomerCount
FROM FSCustomer
WHERE Customer_Code >= @Begin_Number AND Customer_Code <= @End_Number 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


