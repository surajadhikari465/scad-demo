CREATE PROCEDURE [dbo].[Reporting_DASHandlingCharge]
   	@StoreList					varchar(max),
	@ListDelimiter				char(1) = '|',    
	@Distribution_SubTeam_No	int,
	@StartRecvDate				datetime,
	@EndRecvDate				datetime,
	@Identifier					varchar(13)
AS
   -- **************************************************************************
   -- Procedure: Reporting_DASHandlingCharge()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 10/10/2008  BBB	Performed the following actions in response to bug 7877
   --                   Modified AvgCost to pull OI.LadnedCost, DelCost = MarkupCost
   --					Handling Cost = MarkupCost - LandedCost, ExtAvgCost = Landed*Qty,
   --					ExtDelCost = Markup*Qty
   -- 11/24/2008  BBB	Modified HandlingCharge to pull straight from OrderItem table
   --					and modified Freight to be HandlingCharge + LandedCost
   -- 12/30/2008  BBB	New columns being returned as part of a copy of the SO region
   --					report that this is to emulate. Additional parameters in WHERE
   --					clause. As well as taking into account return orders.
   -- 02/23/2010  AlexZ Removed the union for the credit orders and recalculated the Margin.
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON

	----------------------------------------------
	-- Use TRY...CATCH for error handling
	----------------------------------------------
	BEGIN TRY
		-- local variables
		DECLARE @Identifier2 varchar(15)

		----------------------------------------------
		-- Add wildcards for searching if none exist within these parameters
		----------------------------------------------
		IF CHARINDEX('%', @Identifier) = 0
			SELECT @Identifier2 = '%' + @Identifier + '%'
		ELSE
			SELECT @Identifier2 = @Identifier
			
		----------------------------------------------
		-- Verify the UPC (if specified)
		----------------------------------------------
		IF @Identifier2 IS NOT NULL
			IF NOT EXISTS (SELECT * FROM dbo.ItemIdentifier WHERE Identifier LIKE @Identifier2)
			  BEGIN
				RAISERROR('No items found matching the UPC ''%s''!', 16, 1, @Identifier)
			  END

		----------------------------------------------
		-- Verify stores were selected to use
		----------------------------------------------
		IF NOT EXISTS (SELECT *
						FROM dbo.Store S (NOLOCK)
							INNER JOIN dbo.fn_Parse_List(@StoreList, @ListDelimiter) L ON L.Key_Value = S.Store_No)
		  BEGIN
			RAISERROR('No valid stores were specified for the report!', 16, 1)
		  END

		--**************************************************************************
		--Create temp table to hold information from receipts and return aggregation
		--**************************************************************************
		DECLARE @DASOrders TABLE
				(
				SubTeam				varchar(50),
				OrderHeaderID		int, 
				Identifier			varchar(13),
				ItemDescription		varchar(65), 
				CasesShipped		int, 
				AvgCaseMarkup		money, 
				DCCaseCost			money, 
				StoreCaseCost		money, 
				Sales				money, 
				CostOfGoods			money, 
				Margin				money,
				QuantityUOM			varchar(15),
				PackageQty			int,
				PackageUOM			varchar(15)
				)

		--**************************************************************************
		--Select Order receipt and return totals into temp table
		--**************************************************************************
		INSERT INTO @DASOrders
			SELECT 
				[SubTeam]				= RTRIM(dst.SubTeam_Name),
				[OrderHeaderID]			= oh.OrderHeader_ID,
				[Identifier]			= ii.Identifier,
				[ItemDescription]		= RTRIM(i.Item_Description),
				[CasesShipped]			= oi.QuantityReceived,
				[AvgCaseMarkup]			= oi.HandlingCharge, 
				[DCCaseCost]			= oi.LandedCost,
				[StoreCaseCost]			= oi.MarkupCost,
				[Sales]					= oi.ReceivedItemCost,
				[CostOfGoods]			= (oi.QuantityReceived * oi.LandedCost),
				[Margin]				= (oi.ReceivedItemCost - (oi.QuantityReceived * oi.LandedCost))/(oi.ReceivedItemCost) * 100,
				[QuantityUOM]			= RTRIM(qu.Unit_Abbreviation), 
				[PackageQty]			= oi.Package_Desc1,
				[PackageUOM]			= RTRIM(pu.Unit_Abbreviation)
			FROM 
				OrderHeader					(nolock) oh
				INNER JOIN Vendor			(nolock) dv		ON	dv.Vendor_ID			= oh.Vendor_ID
				INNER JOIN Store			(nolock) ds		ON	ds.Store_No				= dv.Store_No
				INNER JOIN SubTeam			(nolock) dst	ON	dst.SubTeam_No			= oh.Transfer_SubTeam
				INNER JOIN Vendor			(nolock) pv		ON	pv.Vendor_ID			= oh.PurchaseLocation_ID
				INNER JOIN Store			(nolock) ps		ON	ps.Store_No				= pv.Store_No
				INNER JOIN SubTeam			(nolock) pst	ON	pst.SubTeam_No			= oh.Transfer_To_SubTeam
				INNER JOIN dbo.fn_Parse_List(@StoreList, @ListDelimiter)
													 sl		ON	sl.Key_Value			= ps.Store_No
				INNER JOIN ZoneSupply		(nolock) zs		ON	zs.FromZone_ID			= ds.Zone_ID 
															AND zs.ToZone_ID			= ps.Zone_ID
															AND zs.SubTeam_No			= oh.Transfer_SubTeam
				INNER JOIN OrderItem		(nolock) oi		ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
				INNER JOIN Item				(nolock) i		ON	i.Item_Key				= oi.Item_Key
				INNER JOIN ItemIdentifier	(nolock) ii		ON	ii.Item_Key				= i.Item_Key 
															AND ii.Default_Identifier	= 1
				LEFT JOIN ItemUnit			(nolock) qu		ON	qu.Unit_ID				= oi.QuantityUnit
				LEFT JOIN ItemUnit			(nolock) pu		ON	pu.Unit_ID				= oi.Package_Unit_ID
			WHERE 
				ds.Distribution_Center									=		1
				AND oi.QuantityReceived									>		0 
				AND oh.Return_Order										=		0
				AND ISNULL(@Distribution_SubTeam_No, dst.SubTeam_No)	=		dst.SubTeam_No
				AND ii.Identifier LIKE    ISNULL(@Identifier2, ii.Identifier)	 
				AND CONVERT(VARCHAR(10), oh.CloseDate, 101)				BETWEEN CONVERT(VARCHAR(10), @StartRecvDate, 101)
																		AND		CONVERT(VARCHAR(10), @EndRecvDate, 101)


		--**************************************************************************
		--Select grouped by totals to return final sums
		--**************************************************************************
		SELECT 
			[SubTeam]			= SubTeam,
			[OrderHeaderID]		= OrderHeaderID,
			[Identifier]		= Identifier,
			[ItemDescription]	= ItemDescription,
			[CasesShipped]		= SUM(CasesShipped),
			[AvgCaseMarkup]		= SUM(AvgCaseMarkup), 
			[DCCaseCost]		= SUM(DCCaseCost),
			[StoreCaseCost]		= SUM(StoreCaseCost),
			[Sales]				= SUM(Sales),
			[CostOfGoods]		= SUM(CostOfGoods),
			[Margin]			= SUM(Margin),
			[QuantityUOM]		= QuantityUOM,
			[PackageQty]		= PackageQty,
			[PackageUOM]		= PackageUOM
		FROM
			@DASOrders
		GROUP BY
			SubTeam,
			OrderHeaderID,
			Identifier,
			ItemDescription,
			QuantityUOM,
			PackageQty,
			PackageUOM

	END TRY
	----------------------------------------------
	BEGIN CATCH
		DECLARE @ParameterInfo nvarchar(1000)

		SELECT @ParameterInfo = N'@StoreList = ''{0}'', @Distribution_SubTeam_No = {1}, @StartRecvDate = ''{2}'', @EndRecvDate = ''{3}'', @Identifier = {4}'

		SELECT @ParameterInfo = REPLACE(@ParameterInfo, '{0}', SUBSTRING(@StoreList, 1, 900))
		SELECT @ParameterInfo = REPLACE(@ParameterInfo, '{1}', ISNULL(CONVERT(varchar, @Distribution_SubTeam_No), 'NULL'))
		SELECT @ParameterInfo = REPLACE(@ParameterInfo, '{2}', CONVERT(varchar, @StartRecvDate, 121))
		SELECT @ParameterInfo = REPLACE(@ParameterInfo, '{3}', CONVERT(varchar, @EndRecvDate, 121))
		SELECT @ParameterInfo = REPLACE(@ParameterInfo, '{4}', ISNULL('''' + RTRIM(@Identifier) + '''', 'NULL'))

		EXEC [dbo].[TryCatch_GetErrorInfo]
			@WriteToLog = 1,
			@AdditionalInfo = @ParameterInfo
	END CATCH
	----------------------------------------------

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_DASHandlingCharge] TO [IRMAReportsRole]
    AS [dbo];

