CREATE PROCEDURE dbo.FiscalCompSales
@Store_No int,
@Zone_Id int,
@Period tinyint,
@Year smallint
AS

SELECT Date.Day_Of_Month, Date.Date_Key, Store.Store_No, SUM(Cash_Amount) + SUM(Credit_Amount) + SUM(Check_Amount) + SUM(Food_Stamp_Amount) + SUM(Vendor_Coupon_Amount) + SUM(Coupon_Amount)+ SUM(GC_In_Amount) + SUM(Employee_Dis_Amount) - SUM(Change_Amount) - SUM(Tax_Table1_Amount) - SUM(Tax_Table2_Amount) - SUM(Tax_Table3_Amount) - SUM(GC_Sales_Amount) AS TotalPrice 
FROM Store (NOLOCK) INNER JOIN (Buggy_SumByRegister (NOLOCK) RIGHT JOIN Date (NOLOCK) ON (Buggy_SumByRegister.Date_Key =  Date.Date_Key) 
	) ON (Store.Store_No = Buggy_SumByRegister.Store_No)
WHERE Period = @Period AND Year = @Year AND
	ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND 
	ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id
GROUP BY Date.Day_Of_Month, Date.Date_Key, Store.Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FiscalCompSales] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FiscalCompSales] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FiscalCompSales] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FiscalCompSales] TO [IRMAReportsRole]
    AS [dbo];

