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

--****************************************************************************
-- Create the imha_user login for the system
--****************************************************************************
if not exists (select name from master.dbo.syslogins where name in ('imha_user', 'IMHA_User'))
   begin
   print 'Creating new imha_user login ...'
   CREATE LOGIN imha_user WITH PASSWORD = 'imha_user', DEFAULT_DATABASE = imha, CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
   end
else
   print 'System login imha_user is already created.'
go

--****************************************************************************
-- Create the imha_user for this database, associated to the imha_user login
--****************************************************************************
if not exists (select name from sysusers where name = 'imha_user')
  begin
  create user imha_user for login imha_user with default_schema=dbo
  end
else
   print 'Database user imha_user is already created.'
go

--****************************************************************************
-- Create the IMHARole role, and associate the imha_user user. :-)
--****************************************************************************
if not exists (select name from sysusers where name = 'IMHARole')
   begin
   print 'Creating IMHARole and assigning rights ...'
   create role IMHARole   authorization dbo
   exec sp_addrolemember 'IMHARole',         'imha_user'
   exec sp_addrolemember 'db_owner',         'IMHARole'
   exec sp_addrolemember 'db_ddladmin',      'IMHARole'
   exec sp_addrolemember 'db_datawriter',    'IMHARole'
   exec sp_addrolemember 'db_datareader',    'IMHARole'
   end
else
   print 'IMHARole role is already created.'
go

--stored procedure
grant exec on dbo.IMHA_Update_Costs to IMHARole
grant exec on dbo.IMHA_Update_VIN to IMHARole
grant exec on dbo.fn_CostConversion to IMHARole

--tables
grant select on dbo.Item to IMHARole
grant select on dbo.ItemIdentifier to IMHARole
grant select on dbo.ItemBrand to IMHARole
grant select on dbo.ItemUnit to IMHARole
grant select on dbo.Price to IMHARole
grant select on dbo.Store to IMHARole
grant select on dbo.SubTeam to IMHARole
grant select on dbo.VendorDealType to IMHARole
grant select on dbo.VendorCostHistory to IMHARole
grant select on dbo.StoreItemVendor to IMHARole
grant select on dbo.Vendor to IMHARole
grant select on dbo.ItemVendor to IMHARole
grant select on dbo.Zone to IMHARole
grant select on dbo.NatItemFamily to IMHARole
grant select on dbo.NatItemCat to IMHARole
grant select on dbo.NatItemClass to IMHARole
grant select on PriceBatchDetail to IMHARole
grant select on PriceChgType to IMHARole
grant select on Sales_SumByItem to IMHARole

--functions
grant select on dbo.fn_VendorCostAll to IMHARole
grant select on dbo.fn_VendorCost to IMHARole
go
 