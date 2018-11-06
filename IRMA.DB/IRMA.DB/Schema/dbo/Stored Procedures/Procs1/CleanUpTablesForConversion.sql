CREATE PROCEDURE dbo.[CleanUpTablesForConversion]
AS

BEGIN
--clean up tables
delete from iriskeytoirmakey;
delete from vendorCostHistory;
truncate table vendorDealHistory;
delete from storeItemVendor;
delete from itemVendor;
delete from price;
delete from itemIdentifier;
delete from pricebatchdetail;
delete from pricebatchheader; 
delete from itemChangeHistory;
delete from signqueue;
delete from Sales_SumByItem;
delete from taxoverride;
delete from itemUploadDetail;
delete from itemUploadHeader;
truncate table pricehistory;
delete from itemScale;
delete from Scale_ExtraTextChgQueue;
delete from Scale_ExtraText;
delete from UploadValue;
delete from UploadRow;
delete from item ;

--clean up temp data
DBCC CHECKIDENT ('dbo.Item', RESEED, 0);
DBCC CHECKIDENT ('dbo.pricebatchdetail', RESEED, 0);
DBCC CHECKIDENT ('dbo.pricebatchheader', RESEED, 0);
DBCC CHECKIDENT ('dbo.itemscale', RESEED, 0);
DBCC CHECKIDENT ('dbo.itemIdentifier', RESEED, 0);
DBCC CHECKIDENT ('dbo.storeItemVendor', RESEED, 0);
DBCC CHECKIDENT ('dbo.vendorCostHistory', RESEED, 0);
DBCC CHECKIDENT ('dbo.vendorDealHistory', RESEED, 0);
DBCC CHECKIDENT ('dbo.priceHistory', RESEED, 0);
DBCC CHECKIDENT ('dbo.Scale_ExtraText', RESEED, 0);
DBCC CHECKIDENT ('dbo.Scale_ExtraTextChgQueue', RESEED, 0);

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CleanUpTablesForConversion] TO [DataMigration]
    AS [dbo];

