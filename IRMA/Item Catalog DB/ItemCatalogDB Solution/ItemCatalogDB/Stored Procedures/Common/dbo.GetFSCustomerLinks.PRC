SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetFSCustomerLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetFSCustomerLinks]
GO

CREATE PROCEDURE dbo.GetFSCustomerLinks
@Customer_ID int 
AS 
SELECT Customer_ID AS Customer_ID FROM FSCustomer WHERE Customer_ID = @Customer_ID 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

