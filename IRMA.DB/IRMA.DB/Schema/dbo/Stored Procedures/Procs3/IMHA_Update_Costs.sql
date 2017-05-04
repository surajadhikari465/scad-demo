
CREATE PROCEDURE [dbo].[IMHA_Update_Costs]
   @UpcNo varchar(20),
   @Vendor_ID varchar(20),
   @Warehouse varchar(20),
   @Promotional bit,
   @UnitFreight decimal(12,4),
   @CaseCost decimal(12,4),
   @CaseSize decimal(12,4),
   @StartDate datetime,
   @EndDate varchar(40),
   @MSRP decimal(12,4),
   @FromVendor int,
   @VendorDealCode varchar(3),
   @CostPromoCode int,
   @InsertDate datetime,
   @PromoRecCount int,
   @CaseUomName varchar(20),
   @Debug int = 0
AS
--**************************************************************************
-- Procedure: IMHA_Update_Costs()
--    Author: Greg Bowles
--      Date: 10/16/2006
--
-- Description:
-- This procedure updates the IRMA VendorCostHistory or VendorDealHistory tables.
-- For IRMA versions 2.2 and below (no VendorDealHistory update).
--
-- Change History:
-- Date        Init. Description
-- 06/11/2013  ??    Updated to use Discontinue Item flag on SIV table rather than Item (flag moved).
-- 11/28/2012  VA    Bug 8859. Keep the existing UnitFreight value.
-- 07/28/2010  RS    Fixed bug introduced when Billy viciously reformatted my code
--                   to make it less readable (and ugly!). :-)
--                   Also restored the format of the aforementioned code to it's original
--                   timeless elegance.
-- 07/07/2010  BBB   Removed delete promo clause
-- 02/03/2009  RS    Added a CASE statement to multiply up and modify the casepack and cost
--                   in the original variables in order to catch Promo entries as well
--                   as costs.
-- 04/18/2008  RS    Modified to use the previous settings for CostUnit_Id and FreightUnit_Id
--                   rather than the ID for 'CASE', to avoid changing the UOM for Costed by weight items.
-- 03/31/2008  RS    Modified the reference table to use only the StoreItemVendor
--                   table for reference, so missing VCH cost records would automatically
--                   be inserted.
-- 03/20/2008  RS    Added condition to the reference table query to eliminate
--                   logically deleted StoreItemVendor records.
-- 03/19/2008  RS    Added a global temp reference table holding all the peripheral
--                   info needed for a cost update, so it only has to do the
--                   query against the joined IRMA tables once per session.  All
--                   subsequent calls use the indexed temp table for the data,
--                   resulting in a large performance improvement.
-- 03/14/2008  RS    Modified the CostedByWeight case again based on discussion
--                   with Gary and David.
-- 03/03/2008  RS    Updated to remove the conversion function with a simple
--                   multiply for costed by weight items.
-- 02/22/2008  RS    Added a global temp reference table for the VendorCostHistory
--                   table join to speed up subsequent item updates.
-- 10/12/2007  RS    Re-added code to "multiply up" CostedByWeight costs up to
--                   case costs in SO IRMA.
-- 08/20/2007  RS    Added use of fn_CostConversion() function to convert the
--                   VIP (case) cost to IRS cost unit.
-- 08/14/2007  RS    Modified to use Vendor_ID.
-- 07/13/2007  RS    Modified to add additional procedure arguments to match
--                   the IRMA 2.3+ procedure, even though they don't get used.
-- 10/16/2006  GB    Created.
-- 01/08/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with siv.DiscontinueItem
--**************************************************************************
   BEGIN
      SET NOCOUNT ON;

      --**************************************************************************
      -- Create a temp table with references to all the latest records in
      -- the VendorCostHistory table by StoreItemVendorId.
      --**************************************************************************
      if not exists (select * from tempdb.dbo.sysobjects where  name = '##imha_tmp_ref')
      begin
         print 'Building reference ...';
         --**************************************************************************
         -- Get pointers to the current cost records
         --**************************************************************************
         if exists (select * from tempdb.dbo.sysobjects where  name like '#cur_cost%') drop table #cur_cost;

         select
            StoreItemVendorId,
            max(VendorCostHistoryId) VendorCostHistoryId
         into
            #cur_cost
         from
            VendorCostHistory
         where
            getdate() between startdate and enddate
         group by
            StoreItemVendorId;

         create index cc_ndx on #cur_cost ( StoreItemVendorId, VendorCostHistoryId);

         --**************************************************************************
         -- Create a global temp table with all the reference info we need for all
         -- IRMA items that will live between calls to the procedure
         --**************************************************************************
         SElECT
            siv.StoreItemVendorID,
            ii.Identifier,
            v.Vendor_Key,
            iv.Item_Id as Warehouse,
            vch.CostUnit_Id,
            vch.FreightUnit_Id,
            vch.Package_Desc1,
            i.Package_Desc2,
            i.Package_Unit_Id,
            i.CostedByWeight,
            vch.UnitFreight
         into
            ##imha_tmp_ref
         FROM
            StoreItemVendor siv (nolock)

            JOIN ItemIdentifier II (nolock)
               on ( ii.Item_Key = siv.Item_Key )

            JOIN Vendor V (nolock)
               on ( v.Vendor_Id = siv.Vendor_Id )

            JOIN ItemVendor iv (nolock)
               on ( iv.Item_Key = siv.Item_Key
                    and iv.Vendor_Id = siv.Vendor_Id )

            JOIN Item i (nolock)
               on ( i.Item_Key = siv.Item_Key )

            JOIN #cur_cost cc (nolock)
               on ( cc.StoreItemVendorId = siv.StoreItemVendorId )

            JOIN VendorCostHistory vch (nolock)
               on ( vch.VendorCostHistoryId = cc.VendorCostHistoryId )
         where
            ( siv.DeleteDate is null or siv.DeleteDate > getdate() )
            and i.deleted_item = 0
            and i.remove_item = 0
            and siv.DiscontinueItem = 0
            and ii.remove_identifier = 0
            and ii.deleted_identifier = 0
               ;

            create clustered index #imhaiindx on ##imha_tmp_ref (Identifier, Vendor_Key );
         end;

      if @EndDate = ''  set @EndDate = null;

   --**************************************************************************
   -- Multiply CostedByWeight costs and casepacks up to the IRMA casepack
   --**************************************************************************
   SElECT
      @Casecost = case
                     when r.CostedByWeight = 1 and r.Package_Desc1 > 1 and @CaseSize = 1
                        then r.Package_Desc1 * @CaseCost
                     else @CaseCost
                  end,
      @CaseSize = case
                     when r.CostedByWeight = 1 and r.Package_Desc1 > 1 and @CaseSize = 1
                        then r.Package_Desc1
                     else @CaseSize
                  end
   FROM
      ##imha_tmp_ref r
    WHERE
      r.Identifier = @UpcNo
      and r.Warehouse = @Warehouse
      and r.Vendor_Key = @Vendor_ID;

   --**************************************************************************
   -- If this is a promotional cost record, pass it to IMHA_Update_Promo
   --**************************************************************************
   IF @Promotional = 1
      BEGIN
         if ( @Debug = 1 ) print 'IMHA_Update_Costs: Calling IMHA_Update_Promo for [' + @Vendor_ID + '], [' + @Warehouse + '], [' + @UpcNo + '] ...'

         DECLARE @CaseQty int;
         SET @CaseQty = 0;

         EXEC IMHA_Update_Promo @UpcNo, @Vendor_ID, @Warehouse, @CaseQty, @CaseSize, @CaseCost, @StartDate, @EndDate, @VendorDealCode, @FromVendor, @CostPromoCode, @InsertDate, @PromoRecCount, @Debug
      END
   ELSE
      BEGIN
      --**************************************************************************
      -- Insert the new cost record
      --**************************************************************************
      Insert Into VendorCostHistory (StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate, MSRP, FromVendor, CostUnit_Id, FreightUnit_Id)
      SElECT
         r.StoreItemVendorID,
         @Promotional,
         @CaseCost as UnitCost,
         r.UnitFreight,
         @CaseSize as CaseSize,
         ISNULL(@StartDate, CONVERT(datetime,CONVERT(varchar(255),GETDATE(),101))) as StartDate,
           ISNULL(cast(@EndDate as datetime), '2079-06-06') as EndDate,
         @MSRP,
         @FromVendor,
         r.CostUnit_Id,
         r.FreightUnit_Id
      FROM
         ##imha_tmp_ref r
       WHERE
         r.Identifier = @UpcNo
         and r.Warehouse = @Warehouse
         and r.Vendor_Key = @Vendor_ID;

      --**************************************************************************
      -- If a non-null MSRP is sent, also update the price table for any items
      -- that use this Vendor as a primary vendor.
      --**************************************************************************
      update p
         set MSRPPrice = @MSRP
      FROM
         ItemIdentifier AS II

         JOIN StoreItemVendor AS SI
            ON ( SI.Item_Key = II.Item_Key )

         JOIN ItemVendor AS IV
            ON ( IV.Vendor_ID = SI.Vendor_ID AND
                 IV.Item_Key = si.Item_Key )

         JOIN Vendor as v
            ON ( v.Vendor_ID = iv.Vendor_ID )

         JOIN Price as p
            ON ( p.Item_Key = si.Item_Key
                 and p.Store_No = si.Store_No )
      where
         II.Identifier = @UpcNo
         and v.Vendor_key = @Vendor_ID
         and iv.Item_ID = @Warehouse
         and si.PrimaryVendor = 1
      END
   END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IMHA_Update_Costs] TO [IMHARole]
    AS [dbo];

