SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DailyReceiving]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DailyReceiving]
GO



CREATE PROCEDURE [dbo].[DailyReceiving] 
@DateReceived SMALLDATETIME 
--@DateReceived varchar(20)  
AS 

-- Changed @DateReceived parameter to SMALLDATETIME data type from varchar(20)	- 10/30/2007 (Hussain Hashim)
-- Converted Where Clause to accept small date field

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT Vendor.CompanyName, SubTeam_Name, Identifier, Item_Description,
       QuantityReceived, Unit_Name, Total_Weight, DateReceived, OrderHeader.Return_Order
FROM ItemUnit (NOLOCK) INNER JOIN (
       Vendor (NOLOCK) INNER JOIN (
         SubTeam (NOLOCK) INNER JOIN (
           ItemIdentifier (NOLOCK) INNER JOIN (
             Item (NOLOCK) INNER JOIN (
               OrderHeader (NOLOCK) INNER JOIN OrderItem (NOLOCK) ON (OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID)
             ) ON (Item.Item_Key = OrderItem.Item_Key)
           ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
         ) ON (SubTeam.SubTeam_No = (CASE WHEN Transfer_SubTeam IS NOT NULL THEN Item.SubTeam_No ELSE ISNULL(Transfer_To_SubTeam, Item.SubTeam_No) END))
       ) ON (Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID)
     ) ON (ItemUnit.Unit_ID = OrderItem.QuantityUnit)
WHERE (CONVERT(Varchar(10), dbo.OrderItem.DateReceived, 101) = CONVERT(Varchar(10), @DateReceived, 101)) AND OrderHeader.Return_Order = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
