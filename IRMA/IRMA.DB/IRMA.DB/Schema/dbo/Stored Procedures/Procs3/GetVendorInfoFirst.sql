CREATE PROCEDURE dbo.GetVendorInfoFirst
AS
   -- **************************************************************************
   -- Procedure: GetVendorInfoFirst
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
		declare @minVID int
		select @minVID = MIN(Vendor_ID) FROM Vendor
		exec dbo.GetVendorInfo @minVID
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

