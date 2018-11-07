CREATE PROCEDURE [dbo].[Pricebook_Report]
	@Store_No varchar(MAX),
	@SubTeam_No varchar(MAX),
	@Vendor_Key varchar(MAX),
	@Brand_Name varchar(MAX),
	@Manufacturer_Number varchar(MAX),
	@Item_Description varchar(MAX),
	@Price_Type varchar(MAX),
	@Movement_Type varchar(10),
	@All_Stores smallint,
	@Zone varchar,
	@IncludeISS smallint
AS

DECLARE @DiscountId varchar(5)
DECLARE @AllowanceId varchar(5)
DECLARE @SQL varchar(MAX)
DECLARE @PeriodStartDate datetime
DECLARE @QuarterStartDate datetime
DECLARE @FiscalYearStartDate datetime
DECLARE @CurrentDate datetime


SELECT @CurrentDate = GETDATE()

exec GetBeginPeriodDate @CurrentDate,@PeriodStartDate output
exec GetBeginQuarterDate @CurrentDate,@QuarterStartDate output
exec GetBeginFiscalYearDate @CurrentDate,@FiscalYearStartDate output

SELECT @DiscountId = VendorDealTypeId
FROM	VendorDealType
WHERE	Code = 'D'

SELECT @AllowanceId = VendorDealTypeId
FROM	VendorDealType
WHERE	Code = 'A'

IF @All_Stores = 0
	BEGIN
		CREATE TABLE ##Movement (Item_Key int, Sales_Quantity int, Sales_Amount decimal(20,3))
		CREATE TABLE ##Movement_Totals (Item_Key int, Total_Qty int, Total_Amt decimal(20,3))

		SELECT @SQL = 'INSERT INTO ##Movement 
							SELECT SSBI.Item_Key, SUM(SSBI.Sales_Quantity) As Sales_Quantity, SUM(CAST(SSBI.Sales_Amount As decimal(20,3))) As Sales_Amount '
	END
ELSE
	BEGIN
		SELECT @SQL = 'SELECT
					   substring(I.Item_Description, 1, 30) Item_Description,
					   s.StoreAbbr,
					   SSBI.Item_Key,
					   SSBI.Sales_Quantity,
					   CAST(SSBI.Sales_Amount as decimal(20,3)) As Sales_Amount,
					   ''Total'' totcol'
	END
				   
	SELECT @SQL = 
		@SQL + '			   
				FROM 
					Sales_SumByItem SSBI
					JOIN
						Item I (nolock)
						ON I.Item_Key = SSBI.Item_Key
					JOIN
						ItemVendor IV (nolock)
						ON IV.Item_Key = SSBI.Item_Key
					JOIN
						Vendor V (nolock)
						ON V.Vendor_ID = IV.Vendor_ID
					JOIN
						ItemBrand IB (nolock)
 						ON IB.Brand_Id = I.Brand_Id
					JOIN
						dbo.fn_ParseStringList(''' + @Store_No + ''', ''|'') SN
						ON SN.Key_Value = SSBI.Store_No
					JOIN
						Price P (nolock)
						ON P.Item_Key = I.Item_Key AND P.Store_No = SSBI.Store_No
				    JOIN
						PriceChgType PCT (nolock)
						ON PCT.PriceChgTypeID = P.PriceChgTypeID
				    JOIN Store s
						on ( s.Store_No = ssbi.Store_No )
				WHERE 
					I.SubTeam_No IN (''' + REPLACE(@SubTeam_No,'|',''',''') + ''') '
				
					IF @Vendor_Key <> 'ALLVENDORS' 
						SELECT @SQL = @SQL + '
						AND V.Vendor_Key IN (''' + REPLACE(@Vendor_Key,'|',''',''') + ''')'
						
					IF @Movement_Type = 'PTD'
						SELECT @SQL = @SQL + '					
						AND	SSBI.Date_Key >= ''' + CONVERT(nvarchar(30), @PeriodStartDate, 101) + ''' AND SSBI.Date_Key <= ''' + CONVERT(nvarchar(30), GETDATE(), 101) + ''''
						
					IF @Movement_Type = 'QTD'
						SELECT @SQL = @SQL + '					
						AND	SSBI.Date_Key >= ''' + CONVERT(nvarchar(30), @QuarterStartDate, 101) + ''' AND SSBI.Date_Key <= ''' + CONVERT(nvarchar(30), GETDATE(), 101) + ''''
						
					IF @Movement_Type = 'YTD'
						SELECT @SQL = @SQL + '					
						AND	SSBI.Date_Key >= ''' + CONVERT(nvarchar(30), @FiscalYearStartDate, 101) + ''' AND SSBI.Date_Key <= ''' + CONVERT(nvarchar(30), GETDATE(), 101) + ''''
						
					IF @Movement_Type = '52WK'
						SELECT @SQL = @SQL + '					
						AND	SSBI.Date_Key >= ''' + CONVERT(nvarchar(30), DATEADD(wk,-52,GETDATE()), 101) + ''' AND SSBI.Date_Key <= ''' + CONVERT(nvarchar(30), GETDATE(), 101) + ''''												
												
					IF @Item_Description IS NOT NULL 
						SELECT @SQL = @SQL + '
						AND I.Item_Description LIKE ''%' + @Item_Description + '%'''

					IF @Manufacturer_Number IS NOT NULL 
						SELECT @SQL = @SQL + '
						AND SSBI.Item_Key IN (SELECT Item_Key FROM ItemIdentifier WHERE SUBSTRING(Identifier,2,5) = ''' + @Manufacturer_Number + ''')'
						
					IF @Brand_Name IS NOT NULL 
						SELECT @SQL = @SQL + '
						AND IB.Brand_Name LIKE ''%' + @Brand_Name + '%'''					
							
					IF @Zone > 0
						SELECT @SQL = @SQL + '
						AND S.Zone_ID IN (''' + @Zone + ''')'
						
					IF @IncludeISS = 0
						SELECT @SQL = @SQL + '
						AND PCT.PriceChgTypeDesc <> ''ISS'''
													
IF @All_Stores = 0
	BEGIN
		SELECT @SQL = @SQL + ' GROUP BY SSBI.Item_Key,P.Store_No'
		EXEC (@SQL)
	END
ELSE
	BEGIN
		exec dbo.pivot_query @SQL, 'Item_Key', 'StoreAbbr', 'sum(Sales_Quantity) Qty,sum(Sales_Amount) Amt','##Movement'
		exec dbo.pivot_query @SQL, 'Item_Key', 'totcol', 'sum(Sales_Quantity) Qty,sum(Sales_Amount) Amt','##Movement_Totals'
	END

						

SELECT @SQL = 'SELECT DISTINCT
					II.Identifier,
					ST.SubTeam_Name,'
					
			IF @All_Stores = 0
				SELECT @SQL = @SQL + 'P.Store_No, S.Store_Name,'
				
			SELECT @SQL = @SQL + '
					IB.Brand_Name,
					I.Item_Description,
					SUBSTRING(CAST(VCH.Package_Desc1 As varchar(MAX)),1,LEN(VCH.Package_Desc1)-1) As Vendor_Pack,
					SUBSTRING(CAST(I.Package_Desc1 As varchar(MAX)),1,LEN(I.Package_Desc1)-1) As Retail_Pack,
					SUBSTRING(CAST(I.Package_Desc2 As varchar(MAX)),1,LEN(I.Package_Desc2)-1) As Size,
					IU.Unit_Abbreviation,
					V.Vendor_Key,
							
					REPLACE(REPLACE(LTRIM(REPLACE(REPLACE(IV.Item_ID, '' '', ''-''),''0'','' '')),'' '',''0''), ''-'', '' '')  AS Item_ID,
					
					CASE WHEN VDH.VendorDealTypeId = ' + @DiscountId + ' THEN CAST(dbo.fn_GetCurrentSumDiscounts(I.Item_Key,P.Store_No) As Varchar(MAX)) + '' %'' ELSE '''' END As Discount,
					CASE WHEN VDH.VendorDealTypeId = ' + @DiscountId + ' THEN dbo.fn_GetDiscountAllowanceDateRange(I.Item_Key,P.Store_No,''D'') END As DiscountDate,
					CASE WHEN VDH.VendorDealTypeId = 1 THEN CAST(dbo.fn_GetCurrentSumAllowances(I.Item_Key,P.Store_No) As Varchar(MAX)) ELSE '''' END As Allowance,
					CASE WHEN VDH.VendorDealTypeId = ' + @AllowanceId + ' THEN dbo.fn_GetDiscountAllowanceDateRange(I.Item_Key,P.Store_No,''A'') END As AllowanceDate,

					CAST(VCH.UnitCost As money) As Base_Cost,
					CONVERT(nvarchar(30), VCH.StartDate ,101) As LastCost,
					CASE WHEN dbo.fn_IsCaseItemUnit(VCH.CostUnit_ID) = 0 THEN 
						CAST(dbo.fn_GetCurrentNetCost(I.Item_Key,P.Store_No) As money)
					ELSE 
						CAST((dbo.fn_GetCurrentNetCost(I.Item_Key,P.Store_No)/VCH.Package_Desc1) As money) 
					END As NetUnitCost,
					
					CAST(P.Price As money) As RegularRetail,
					CASE WHEN dbo.fn_OnSale(P.PriceChgTypeId) = 1 THEN 
						CAST(P.Sale_Price As money) 
					ELSE 
						CAST(P.Price As money) 
					END As CurrentRetail,
					
					CASE WHEN dbo.fn_IsCaseItemUnit(VCH.CostUnit_ID) = 0 THEN
						CASE WHEN dbo.fn_OnSale(P.PriceChgTypeId) = 1 THEN 
							dbo.fn_GetMargin(P.Sale_Price, P.Multiple, dbo.fn_GetCurrentNetCost(I.Item_Key,P.Store_No))
						ELSE
							dbo.fn_GetMargin(P.Price, P.Multiple, dbo.fn_GetCurrentNetCost(I.Item_Key,P.Store_No))
						END
					ELSE
						CASE WHEN dbo.fn_OnSale(P.PriceChgTypeId) = 1 THEN 
							dbo.fn_GetMargin(P.Sale_Price, P.Multiple, dbo.fn_GetCurrentNetCost(I.Item_Key,P.Store_No)/VCH.Package_Desc1)
						ELSE
							dbo.fn_GetMargin(P.Price, P.Multiple, dbo.fn_GetCurrentNetCost(I.Item_Key,P.Store_No)/VCH.Package_Desc1)
						END
					END
						As CurrentMargin,

					PCT.PriceChgTypeDesc As PType,
					CASE WHEN dbo.fn_IsCaseItemUnit(VCH.CostUnit_ID) = 0 THEN
						dbo.fn_GetMargin(P.Price, P.Multiple, dbo.fn_GetCurrentNetCost(I.Item_Key,P.Store_No))
					ELSE
						dbo.fn_GetMargin(P.Price, P.Multiple, dbo.fn_GetCurrentNetCost(I.Item_Key,P.Store_No)/VCH.Package_Desc1)
					END
						As RegularMargin,

					IA.Text_2 As TargetMargin,
					CAST(VCH.MSRP As money) As MSRPPrice,


					(SELECT TOP 1 Identifier FROM ItemIdentifier (nolock) WHERE ItemIdentifier.Item_Key = P.LinkedItem ORDER BY Default_Identifier DESC) As LinkedItem,
					IA.Text_3 As Plano_Set,
					I.Product_Code As Plano,
					CAST(NIF.NatFamilyID As Varchar(MAX)) + '' '' + NIF.NatFamilyName As NatFamilyID,
					CAST(NICat.NatCatID As varchar(MAX)) + '' '' + NICat.NatCatName As Category_Name,
					ICat.Category_Name As ClassID,
					IC.ItemChainDesc,
					IA.Text_1 As Seas,
					P.CompetitivePriceTypeID,
					CONVERT(nvarchar(30), I.Insert_Date, 101) As CreationDate,
					MV.*,
					MVT.*
				FROM
					Price P
					JOIN
						dbo.fn_ParseStringList(''' + @Store_No + ''', ''|'') SN
						ON SN.Key_Value = P.Store_No					
					JOIN
						Item I (nolock)
						ON I.Item_Key = P.Item_Key	
					JOIN
						SubTeam ST (nolock)
						ON ST.SubTeam_No = I.SubTeam_No
					JOIN
						ItemIdentifier II (nolock)
						ON II.Item_Key = P.Item_Key
					JOIN
						ItemBrand IB (nolock)
						ON IB.Brand_Id = I.Brand_Id
					JOIN
						ItemCategory ICat (nolock)
						ON ICat.Category_Id = I.Category_Id						
					JOIN
						Store S (nolock)
						ON S.Store_No = P.Store_No
					LEFT JOIN
						StoreItemVendor SIV (nolock)
						ON SIV.Item_Key = P.Item_Key AND
						   SIV.Store_No = P.Store_No AND
						   SIV.PrimaryVendor = 1
					JOIN
						ItemUnit IU (nolock)
						ON IU.Unit_Id = I.Package_Unit_Id
					JOIN 
						PriceChgType PCT (nolock)
						ON PCT.PriceChgTypeId = P.PriceChgTypeID
					LEFT JOIN
						VendorDealHistory VDH (nolock)
						ON VDH.StoreItemVendorId = SIV.StoreItemVendorId
					LEFT JOIN
						ItemAttribute IA (nolock)
						ON IA.Item_Key = P.Item_Key
					LEFT JOIN 
						ItemVendor IV (nolock)
						ON IV.Item_Key = P.Item_Key AND
							IV.Vendor_ID = SIV.Vendor_ID
					LEFT JOIN 
						StoreItem SI (nolock)
						ON SI.Item_Key = P.Item_Key AND 
							SI.Store_No = P.Store_No AND
							SI.Authorized = 1
					JOIN
						Vendor V (nolock)
						ON V.Vendor_Id = IV.Vendor_Id	
					LEFT JOIN
						NatItemClass NIClass (nolock)
						ON NIClass.ClassID = I.ClassID
					LEFT JOIN
						NatItemCat NICat (nolock)
						ON NICat.NatCatID = NIClass.NatCatID
					LEFT JOIN
						NatItemFamily NIF (nolock)
						ON NIF.NatFamilyID = NICat.NatFamilyID
					LEFT JOIN
						ItemChainItem ICI (nolock)
						ON ICI.Item_Key = I.Item_Key
					LEFT JOIN
						ItemChain IC (nolock)
						ON IC.ItemChainId = ICI.ItemChainId
					LEFT JOIN
						##Movement MV (nolock)
						ON MV.Item_Key = P.Item_Key			
					LEFT JOIN
						##Movement_Totals MVT (nolock)
						ON MVT.Item_Key = P.Item_Key
					CROSS APPLY
						dbo.fn_VendorCost(P.Item_Key, V.Vendor_ID, P.Store_No, CONVERT(smalldatetime, GetDate(), 101)) VCH										
				WHERE
					I.SubTeam_No IN (''' + REPLACE(@SubTeam_No,'|',''',''') + ''') '
				
				SELECT @SQL = @SQL + '
					AND SI.Authorized = 1 '
				
				IF @Vendor_Key <> 'ALLVENDORS' 
					SELECT @SQL = @SQL + '
					AND V.Vendor_Key IN (''' + REPLACE(@Vendor_Key,'|',''',''') + ''')'

				IF @Item_Description IS NOT NULL 
					SELECT @SQL = @SQL + '
					AND I.Item_Description LIKE ''%' + REPLACE(@Item_Description,'%20',' ') + '%'''
					
				IF @Brand_Name IS NOT NULL 
					SELECT @SQL = @SQL + '
					AND IB.Brand_Name LIKE ''%' + REPLACE(@Brand_Name,'%20',' ') + '%'''					
					
				IF @Manufacturer_Number IS NOT NULL 
					SELECT @SQL = @SQL + '
					AND SUBSTRING(II.Identifier,2,5) = (''' + @Manufacturer_Number + ''')'
				
				IF @Zone > 0
					SELECT @SQL = @SQL + '
					AND S.Zone_ID IN (''' + @Zone + ''')'
							
				IF @IncludeISS = 0
					SELECT @SQL = @SQL + '
					AND PCT.PriceChgTypeDesc <> ''ISS'''				
													
					
EXEC (@SQL)

DROP TABLE ##Movement
DROP TABLE ##Movement_Totals
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Pricebook_Report] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Pricebook_Report] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Pricebook_Report] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Pricebook_Report] TO [IRMAReportsRole]
    AS [dbo];

