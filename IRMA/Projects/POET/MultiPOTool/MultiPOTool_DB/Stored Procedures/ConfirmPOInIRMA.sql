-- This script was created using WinSQL Professional
-- Timestamp: 12/5/2008 11:50:10 AM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: ConfirmPOInIRMA;1 - Script Date: 12/5/2008 11:50:10 AM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ConfirmPOInIRMA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ConfirmPOInIRMA]
GO
CREATE PROCEDURE dbo.ConfirmPOInIRMA
        
        (
        @RegionID int,
        @PONumber int, 
        @UploadSessionID int
        )
        
AS
         SET NOCOUNT ON 
        
BEGIN

        declare @IRMAServer varchar(6),
                        @IRMADatabase varchar(50),
                        @DBString varchar(max),
                        @PONumberQuote varchar(25),
                        @count  int,
                        @sql    nvarchar(4000),
                        @params nvarchar(4000)

        
        set @PONumberQuote = '%' + convert(varchar(25), @PONumber) + '%'

        select @IRMAServer = IRMAServer, @IRMADatabase = IRMADataBase from Regions where RegionID = @RegionID
        set @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'


                
                set @sql = N'if exists (select count(orderheader_ID) from ' + @DBString + '[OrderHeader] where  OrderHeaderDesc like ' 
                + quotename(@PONumberQuote, '''') + ') BEGIN update p set p.ConfirmedInIRMADate = ' 
                + quotename(getdate(), '''') + ' from POHeader p inner join PONumber pn on p.PONumberID = pn.PONumberID ' +
                ' where pn.PONumber = '
                + convert(varchar(25), @PONumber) + ' and p.RegionID = ' + convert(varchar(10), @RegionID) + ' and p.UploadSessionHistoryID = ' 
                + convert(varchar(25), @UploadSessionID) + ' ; select @cnt = @@Rowcount  END'
                
                set @params = N'@cnt      int      OUTPUT'
                
                
                --print (@sql)
                
                exec sp_executesql @sql, @params, @cnt = @count OUTPUT ;
                        
        
        END
        
        Return @count
        
        
        


GO