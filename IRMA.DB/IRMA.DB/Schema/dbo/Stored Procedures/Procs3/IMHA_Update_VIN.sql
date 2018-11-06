
CREATE PROCEDURE dbo.IMHA_Update_VIN(@UpcNo varchar(20),@Vendor varchar(20),@OldVIN varchar(20),@NewVIN varchar(20)) as
--**************************************************************************
-- Procedure: IMHA_Update_VIN()
--    Author: Ron Savage
--      Date: 05/28/2007
--
-- Description:
-- This procedure updates the IRMA VIN numbers.
--
-- Change History:
-- Date        Init. Description
-- 04/06/2009  RS    Removed OldVIN matching criteria to handle NULL VINs, since
--                   the entries are unique by Vendor and UPC anyway.
-- 07/18/2008  RS    Fixed Vendor_Id to Vendor_Key reference in VIN UPdate procedure.
-- 05/23/2007  RS    Created, copied and modified Gregs original version for
--                  the IRMA view-tables.
--**************************************************************************
BEGIN
   SET NOCOUNT ON;

   update iv
      set Item_ID = @NewVIN
   FROM
      ItemVendor as iv

      JOIN ItemIdentifier as ii
         ON ( ii.Item_Key = iv.Item_Key )

      JOIN Vendor as v
         ON ( v.Vendor_ID = iv.Vendor_ID )
   where
      ii.Identifier = @UpcNo
      and v.Vendor_key = @Vendor
      -- and iv.Item_ID = @OldVIN
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IMHA_Update_VIN] TO [IMHARole]
    AS [dbo];

