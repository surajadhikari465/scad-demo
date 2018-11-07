SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderItemQueueView]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderItemQueueView]
GO

CREATE PROCEDURE [dbo].[GetOrderItemQueueView]
	@PurchasingVendor_ID int,
	@ToSubTeam_No int,  
	@Item_Description varchar(60),
	@Identifier varchar(13), 
    @Transfer bit, 
    @Credit bit, 
    @Vendor_ID int, 
    @User_ID int 
AS 
BEGIN

/*********************************************************************************************
CHANGE LOG
DEV				DATE		TASK		Description
----------------------------------------------------------------------------------------------
DBS				20110211	13748		Add Discontinue Item for grid
BAS				20130104	8755		Edited Discontinue field with scalar function 
										due to schema change. Updated file extension to .sql
***********************************************************************************************/

	SET NOCOUNT ON

	DECLARE @SQL varchar(2000)

	DECLARE @Store_No int
	IF @PurchasingVendor_ID > 0 BEGIN SELECT @Store_No = Store_No FROM Vendor WHERE Vendor_ID = @PurchasingVendor_ID END

	IF (@Identifier <> '') 
		SELECT @SQL = 'SELECT  DISTINCT TOP 1001 
			  Q.OrderItemQueue_ID
			, Item.Item_Key 
			, Item_Description
			, Identifier 
			, Q.Quantity 
			, Q.Unit_ID 
			, IU.Unit_Name AS QuantityUnitName 
			, U.UserName 
			, CONVERT(varchar(255), Q.Insert_Date, 121) As Insert_Date 
			, ISNULL(V.CompanyName, '''') as PrimaryVendor 
			, dbo.fn_GetDiscontinueStatus(Item.Item_Key,' + CONVERT(varchar(10),@Store_No) + ', NULL) AS Discontinue_Item '
	ELSE
		SELECT @SQL = 'SELECT DISTINCT TOP 1001 
			  Q.OrderItemQueue_ID
			, Item.Item_Key 
			, Item_Description
			, Identifier = 
				(SELECT TOP 1 Identifier 
		            	FROM ItemIdentifier (nolock) 
		              	WHERE ItemIdentifier.Item_Key = Item.Item_Key 
		              	ORDER BY Default_Identifier DESC) 
			, Q.Quantity 
			, Q.Unit_ID 
			, IU.Unit_Name AS QuantityUnitName 
			, U.UserName 
			, CONVERT(varchar(255), Q.Insert_Date, 121) As Insert_Date 
			, ISNULL(V.CompanyName, '''') as PrimaryVendor
			, dbo.fn_GetDiscontinueStatus(Item.Item_Key,' + CONVERT(varchar(10),@Store_No) + ', NULL) AS Discontinue_Item '

	SELECT @SQL = @SQL + 'FROM Item (nolock) 
		INNER JOIN OrderItemQueue Q (nolock) 
		    ON Item.Item_Key = Q.Item_Key 
		    AND Q.Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ' 
		    AND Q.TransferToSubTeam_No = ' + CONVERT(VARCHAR(10), @ToSubTeam_No) + ' 
			AND Q.Transfer = ' + CAST(@Transfer as char(1)) + ' 
			AND Q.Credit = ' + CAST(@Credit as char(1)) + ' 
		INNER JOIN ItemUnit IU (nolock)
		    ON Q.Unit_ID = IU.Unit_ID 
		INNER JOIN Users U (nolock)
		    ON Q.User_ID = U.User_ID '

    IF (@Vendor_ID > 0) 
        -- User wants only items for this vendor (thus inner join)
        SELECT @SQL = @SQL + 'INNER JOIN 
                            (StoreItemVendor SIV (nolock) INNER JOIN Vendor V ON SIV.Vendor_ID = V.Vendor_ID)
                            ON Q.Item_Key = SIV.Item_Key 
                            AND Q.Store_No = SIV.Store_No 
                            AND SIV.Vendor_ID = ' + CONVERT(VARCHAR(10), @Vendor_ID) + '
                            AND (SIV.DeleteDate IS NULL OR SIV.DeleteDate > GETDATE()) '
    ELSE 
        -- Only join on SIV to get Primary Vendor info (thus left join)
		SELECT @SQL = @SQL + 'LEFT OUTER JOIN 
            (StoreItemVendor SIV INNER JOIN Vendor V ON SIV.Vendor_ID = V.Vendor_ID)
    		    ON Q.Store_No = SIV.Store_No 
    		    AND Q.Item_Key = SIV.Item_Key 
    		    AND SIV.PrimaryVendor = 1 '

	IF (@Identifier <> '') SELECT @SQL = @SQL + 'INNER JOIN ItemIdentifier (nolock) 
	                                                ON Item.Item_Key = ItemIdentifier.Item_Key '
	SELECT @SQL = @SQL + 'WHERE 1=1 ' 

	IF (@Item_Description <> '') SELECT @SQL = @SQL + 'AND Item.Item_Description LIKE ''%' + @Item_Description + '%'' '
	IF (@Identifier <> '') SELECT @SQL = @SQL + 'AND Identifier LIKE ''' + @Identifier + '%'' '
    IF (@User_ID > 0) SELECT @SQL = @SQL + 'AND U.User_ID = ' + CONVERT(VARCHAR(10), @User_ID)  

	SELECT @SQL = @SQL + 'ORDER BY Item_Description '

--Used only for testing----------------------------------------------------------------------
--	Insert into MySQLTester (Vendor_ID, Vendor_Name, Store_No, From_SubTeam, SQLText)
--	Values(0, '', @Store_No, @ToSubTeam_No, ISNULL(@SQL, 'SQL text was null'))
------------------------------------------------------------------------------------------

	EXECUTE(@SQL)

	SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

