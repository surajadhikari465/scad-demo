SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRipeCustomerStoreNo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRipeCustomerStoreNo]
GO


CREATE PROCEDURE dbo.GetRipeCustomerStoreNo 
@CustomerID int

AS 
BEGIN
    SET NOCOUNT ON

    SELECT DISTINCT Recipe.dbo.Customer.CompanyName, Recipe.dbo.Customer.CustomerID, Recipe.dbo.Customer.StoreNo
    FROM Recipe.dbo.Customer 
    WHERE Recipe.dbo.Customer.CustomerID = @CustomerID
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


