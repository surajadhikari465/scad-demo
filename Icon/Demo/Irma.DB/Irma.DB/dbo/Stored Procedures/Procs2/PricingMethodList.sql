-- this is only being used in EPromo currently.  

CREATE PROCEDURE dbo.PricingMethodList
AS 

SELECT PricingMethod_ID, PricingMethod_Name
FROM PricingMethod
ORDER By PricingMethod_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PricingMethodList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PricingMethodList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PricingMethodList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PricingMethodList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PricingMethodList] TO [IRMAReportsRole]
    AS [dbo];

