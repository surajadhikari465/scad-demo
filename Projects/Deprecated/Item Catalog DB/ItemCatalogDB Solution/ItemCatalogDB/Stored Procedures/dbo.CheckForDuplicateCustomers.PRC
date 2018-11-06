SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateCustomers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateCustomers]
GO


CREATE PROCEDURE dbo.CheckForDuplicateCustomers 
@Customer_ID int,
@CustomerName varchar(25) 
AS 

SELECT COUNT(*) AS CustomerCount 
FROM FSCustomer
WHERE CustomerName = @CustomerName AND Customer_Id <> @Customer_ID 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

