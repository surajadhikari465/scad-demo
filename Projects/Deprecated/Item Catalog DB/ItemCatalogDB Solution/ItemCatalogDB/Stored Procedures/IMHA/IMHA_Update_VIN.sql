--****************************************************************************
-- Script file: create_irma_stored_procedures_v3.sql
--      Author: Ron Savage
--        Date: 08/14/2007
--
-- Description:
-- This file creates the stored procedures needed to be run in the IRMA
-- ItemCatalog database for the SO region.  The primary difference is the
-- use of Vendor_ID rather than Vendor_Key to identify records.
--
-- Change History:
-- Date        Init. Description
-- 04/18/2008  RS    Copied SO Cost Update procedure and modified for V3 to avoid
--                   explicitly setting the Cost UOM to 'Case'
-- 03/20/2008  RS    Added condition to the reference table query to eliminate
--                   logically deleted StoreItemVendor records in IMHA_Update_Costs().
-- 03/19/2008  RS    Added global temp reference table to IMHA_Update_Costs() for
--                   a large performance improvement.
-- 09/27/2007  RS    Added Case Uom name as an argument to IMHA_Update_Costs().
-- 08/14/2007  RS    Copied and modified to use Vendor_ID.
--****************************************************************************
IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'IMHA_Update_VIN') DROP PROCEDURE dbo.IMHA_Update_VIN
go

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
go
 