CREATE PROCEDURE dbo.GetAllInvoiceTolerances

AS
BEGIN

         SET NOCOUNT ON

/*

   -- Description:
   -- This procedure returns three record sets with all Invoice Tolerances
   --
   -- Modification History:
   -- Date        Init Comment
   -- 11/11/2009  AZ  Creation Date

*/



SELECT     [Vendor_Tolerance], [Vendor_Tolerance_Amount], [User_ID], [UpdateDate]
FROM         InvoiceMatchingTolerance (nolock)



SELECT     I.Store_No, rtrim(S.Store_Name) as Store_Name, I.Vendor_Tolerance, I.Vendor_Tolerance_Amount, I.User_ID,
                      I.UpdateDate
FROM         InvoiceMatchingTolerance_StoreOverride I (nolock) INNER JOIN
                      Store S (nolock) ON I.Store_No = S.Store_No




SELECT     I.Vendor_ID, rtrim(V.CompanyName) as Vendor_Name, I.Vendor_Tolerance,
                      I.Vendor_Tolerance_Amount, I.User_ID,
                      I.UpdateDate
FROM         InvoiceMatchingTolerance_VendorOverride I (nolock) INNER JOIN
                      Vendor V (nolock) ON I.Vendor_ID = V.Vendor_ID


        SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllInvoiceTolerances] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllInvoiceTolerances] TO [IRMAClientRole]
    AS [dbo];

