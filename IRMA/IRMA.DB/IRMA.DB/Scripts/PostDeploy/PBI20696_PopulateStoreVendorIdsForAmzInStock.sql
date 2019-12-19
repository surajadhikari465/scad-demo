DECLARE @vendorIds varchar(350), @vendorIdSingle varchar(6)

DECLARE vendor_cursor CURSOR
  FOR SELECT Vendor_ID FROM Vendor v
        JOIN Store s on v.Store_No = s.Store_No
          WHERE s.WFM_Store = 1
         AND s.Internal = 1
         AND s.Store_No not in (SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('LabAndClosedStoreNo', 'IRMA CLIENT'), '|')) 

OPEN vendor_cursor
FETCH NEXT FROM vendor_cursor INTO @vendorIdSingle

WHILE @@FETCH_STATUS = 0  
BEGIN  
       PRINT CAST(@vendorIdSingle AS VARCHAR(6)) + '|'
       IF LEN(@vendorIds) > 0
             SET @vendorIds = @vendorIds + CAST(@vendorIdSingle AS VARCHAR(6)) + '|'
       ELSE
             SET @vendorIds = CAST(@vendorIdSingle AS VARCHAR(6)) + '|'

       FETCH NEXT FROM vendor_cursor INTO @vendorIdSingle
END   
CLOSE vendor_cursor;  
DEALLOCATE vendor_cursor; 

select @vendorIds = '''' + LEFT(@vendorIds, LEN(@vendorIds) - 1) + ''''

UPDATE V
       SET V.Value = @vendorIds
FROM AppConfigValue V
INNER JOIN AppConfigApp A on V.ApplicationID = A.ApplicationID
INNER JOIN AppConfigKey K on V.KeyID = K.KeyID
WHERE K.Name = 'AmazonInStockEnabledStoreVendorId'
AND A.Name = 'IRMA CLIENT'
