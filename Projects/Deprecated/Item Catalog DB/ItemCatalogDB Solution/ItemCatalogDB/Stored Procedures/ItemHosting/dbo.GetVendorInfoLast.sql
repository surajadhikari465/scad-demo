
IF EXISTS (
       SELECT *
       FROM   dbo.sysobjects
       WHERE  id = OBJECT_ID(N'[dbo].[GetVendorInfoLast]')
              AND OBJECTPROPERTY(id, N'IsProcedure') = 1
   )
    DROP PROCEDURE [dbo].[GetVendorInfoLast]
GO


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

