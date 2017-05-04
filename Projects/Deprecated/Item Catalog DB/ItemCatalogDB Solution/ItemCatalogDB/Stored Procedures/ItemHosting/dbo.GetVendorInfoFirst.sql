
IF EXISTS (
       SELECT *
       FROM   dbo.sysobjects
       WHERE  id = OBJECT_ID(N'[dbo].[GetVendorInfoFirst]')
              AND OBJECTPROPERTY(id, N'IsProcedure') = 1
   )
    DROP PROCEDURE [dbo].[GetVendorInfoFirst]
GO


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

