CREATE PROCEDURE dbo.GetVendorInfoLast
AS
   -- **************************************************************************
   -- Procedure: GetVendorInfoLast
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date			Init	Comment
   -- 11/10/2009	BBB		update existing SP to retrieve BusinessUnit_ID from Vendor table 
   -- 04/06/2010	BBB		Removed BusinessUnit_ID from output
   -- **************************************************************************
begin
		declare @maxVID int
		select @maxVID = MAX(Vendor_ID) FROM Vendor
		exec dbo.GetVendorInfo @maxVID
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorInfoLast] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorInfoLast] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorInfoLast] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorInfoLast] TO [IRMAReportsRole]
    AS [dbo];

