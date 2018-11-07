
create view [dbo].[zz_deprecated_CIX_Store] as
select
   --s.storeid                        Store_No,
   s.store                        Store_No,
   s.name1                          Store_Name,
   null                               Phone_Number,
   0                                Mega_Store,
   0                                Distribution_Center,
   0                                Manufacturer,
   1                                WFM_Store,
   1                                Internal,
   --null                               FTPUser,
   --null                               FTPPassword,
   null                               TelnetUser,
   null                               TelnetPassword,
   0                                BatchID,
   0                                BatchRecords,
   --s.hostid                         --IP_Address,
  -- '.'                              --HD_Directory,
   s.storeid                        BusinessUnit_ID,
   z.zone_id                      Zone_ID,
   null                               UNFI_Store,
   null                               LastRecvLogDate,
   null                               LastRecvLog_No,
   null                               RecvLogUser_ID,
   null                               EXEWarehouse,
   0                                Regional,
   null                               LastSalesUpdateDate,
   s.name2                               StoreAbbr,
   s.store                          PLUMStoreNo,
   t.TaxJurisdictionID              TaxJurisdictionID,
   null								possystemid
from
   [dbo].cxbstorr s,
   [dbo].Zone z,
   [dbo].TaxJurisdiction t
where
   z.zone_id  = s.mzone
