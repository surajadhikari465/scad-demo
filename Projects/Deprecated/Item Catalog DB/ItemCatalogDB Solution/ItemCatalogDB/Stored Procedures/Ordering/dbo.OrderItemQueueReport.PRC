SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderItemQueueReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[OrderItemQueueReport]
GO

/****************************************************************************
Title...: OrderItemQueueReport
Purpose.: This script generates a list of ordered items in queue
          based on the parameters passed to it from the IRMA client
Security:
GRANT EXEC ON [dbo].[OrderItemQueueReport] TO IRMAClientRole, IRMAReportsRole

History:
Date         Initials      Description
08/09/2010   MY            Modified.
09/13/2013   MZ            TFS #13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
****************************************************************************/

CREATE PROCEDURE dbo.OrderItemQueueReport
  	@PurchasingVendor_ID int, 
    @TransferToSubTeam_No int,
    @Item_Description varchar(60),
    @OrderType int,
    @Identifier varchar(13),
    @SortBy int, 
    @Vendor_ID int,
    @User_ID int
 
AS

BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @SQL varchar(MAX)
DECLARE @Store_No int
DECLARE @StoreName as varchar(50)
	
IF @PurchasingVendor_ID > 0 BEGIN SELECT @Store_No = Store_No, @StoreName = CompanyName FROM Vendor WHERE Vendor_ID = @PurchasingVendor_ID END

SELECT @SQL = 
'SELECT 
	S.Store_Name AS StoreName 
	, ST.Subteam_Name AS ToSubTeam
	, RIGHT(' + ''''+ '0000000000000' + ''''+ '+ CAST(II.Identifier AS varchar(20)),12)  AS BarCode
	, II.Identifier
	, SIV.Vendor_ID AS PrimaryVendor 
	, I.Item_Description AS ItemDescription
	, CONVERT(VARCHAR(10), I.Package_Desc1) + ''/'' + CONVERT(VARCHAR(10), CAST(ROUND(I.Package_Desc2, 2) as DECIMAL(8,2))) + '' '' + ISNULL(IU.Unit_Name, ''Unit'') AS PackageDescription
	, Q.Quantity 
	, U.Unit_Name AS Unit
	, US.UserName
FROM Item I (NOLOCK) 
	INNER JOIN OrderItemQueue Q (NOLOCK) 
		ON I.Item_Key = Q.Item_Key  
	INNER JOIN Store S (NOLOCK) 
		ON S.Store_No = Q.Store_No  
	INNER JOIN ItemUnit U (NOLOCK)
		ON Q.Unit_ID = U.Unit_ID 
	INNER JOIN ItemUnit IU (NOLOCK) 
		ON I.Package_Unit_ID = IU.Unit_ID 
	INNER JOIN SubTeam ST (NOLOCK) 
		ON Q.TransferToSubTeam_No = ST.SubTeam_No
	INNER JOIN ItemIdentifier II (NOLOCK) 
		ON I.Item_Key = II.Item_Key 
	INNER JOIN Users US (nolock)
		ON Q.User_ID = US.User_ID' 
   
IF (@Vendor_ID > 0) 
        -- User wants only items for this vendor 
        SELECT @SQL = @SQL + ' INNER JOIN 
			(StoreItemVendor SIV (NOLOCK) INNER JOIN Vendor V (NOLOCK) ON SIV.Vendor_ID = V.Vendor_ID)
			ON Q.Item_Key = SIV.Item_Key 
			AND Q.Store_No = SIV.Store_No 
			AND SIV.Vendor_ID = ' + CONVERT(VARCHAR(10), @Vendor_ID) + '
			AND (SIV.DeleteDate IS NULL OR SIV.DeleteDate > GETDATE()) '
ELSE 
        -- Only join on SIV to get Primary Vendor info 
		SELECT @SQL = @SQL + ' LEFT OUTER JOIN 
			(StoreItemVendor SIV (NOLOCK) INNER JOIN Vendor V (NOLOCK) ON SIV.Vendor_ID = V.Vendor_ID)
			ON Q.Store_No = SIV.Store_No 
			AND Q.Item_Key = SIV.Item_Key 
			AND SIV.PrimaryVendor = 1 '
       		   
SELECT @SQL = @SQL + 'WHERE Q.Store_No = ' + CAST(@Store_No AS varchar(20)) + 
                     ' AND Q.TransferToSubTeam_No = '+ CAST(@TransferToSubTeam_No AS varchar(10)) 
            
IF (@OrderType = 0)          SELECT @SQL = @SQL + ' AND (Q.Transfer = 0 AND Q.Credit = 0)'     -- Order
IF (@OrderType = 1)          SELECT @SQL = @SQL + ' AND Q.Transfer = 1'                        -- Transfer
IF (@OrderType = 2)          SELECT @SQL = @SQL + ' AND Q.Credit = 1'                          -- Credit

IF (@Item_Description <> '') SELECT @SQL = @SQL + ' AND I.Item_Description LIKE ''%' + @Item_Description + '%'' '
IF (@Identifier <> '')       SELECT @SQL = @SQL + ' AND Identifier LIKE ''' + @Identifier + '%'' '
IF (@User_ID > 0)            SELECT @SQL = @SQL + ' AND US.User_ID = ' + CONVERT(VARCHAR(10), @User_ID)  

IF @SortBy = 0               SELECT @SQL = @SQL + ' ORDER BY V.CompanyName ' 
IF @SortBy = 1               SELECT @SQL = @SQL + ' ORDER BY I.Item_Description ' 
IF @SortBy = 2               SELECT @SQL = @SQL + ' ORDER BY Identifier ' 

EXEC(@SQL)
 
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

