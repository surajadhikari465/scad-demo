CREATE PROCEDURE dbo.ItemMasterReport
    @Store_No varchar(5),
    @Vendor_No varchar(10),
    @SubTeam_No varchar(10),
    @SubDept_No varchar(10),
    @Class_No varchar(10),
    @SubClass_No varchar(10),
    @SKU_Status varchar(5),
    @Merch_Group varchar(5),
    @Supplier_Type varchar(5),
    @Plu_Type varchar(15),
    @From_PLU varchar(50),
    @To_PLU varchar(50)
WITH RECOMPILE
AS

   -- **************************************************************************
   -- Procedure: ItemMasterReport()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from multiple RDL files and generates reports consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 01/11/2013  BAS	Update i.Discontinue_Item filter in WHERE clause to
   --					account for schema change.
   -- 07/30/2013  BAS	Added (nolock) to 'FROM Item' (line 86) statement. This was fixed via
   --					ECC 50903 directly, but adding this change to Source Control
  -- **************************************************************************

BEGIN
    SET NOCOUNT ON
    DECLARE @SQL varchar(8000)
   
        CREATE TABLE #ReportItemKey (Item_Key int,Store_No int,Store_Name varchar(50),VendorNo int,
                                     VendorPartNumber varchar(20),Dept int,SubDept varchar(35),Class varchar(50),
                                     SubClass varchar(50),Status char(1),MGrp varchar(25),SupType varchar(50),
                                     Description varchar(60),Cost varchar(50),Price varchar(50),
                                     BuyUOM varchar(50),SellUOM varchar(50),InPk varchar(50),
                                     CasePack varchar(50),SelWt varchar(50),SelDesc varchar(50),
                                     ScaleDescription varchar(75),Shelflife varchar(75),Extratext varchar(50),
                                     Nutrifact varchar(50),ScaleTare varchar(50),AltScaleTare varchar(50),Ingredients varchar(4200))

		SELECT @SQL = 'INSERT INTO #ReportItemKey
			SELECT DISTINCT
				   I.Item_Key As Item_Key,
				   P.Store_No As Store_No,
				   S.Store_Name As Store_Name,
         		   IV.Vendor_ID As VendorNo,
				   IV.Item_ID As VendorPartNumber,
				   I.SubTeam_No As Dept,
				   IC.Category_Name As SubDept,
				   PH3.Description As Class,
				   PH4.Description As SubClass,
				   CASE WHEN II.Deleted_Identifier=1 THEN ''P'' WHEN dbo.fn_GetDiscontinueStatus(I.Item_Key, NULL, NULL) = 1 THEN ''D'' ELSE ''A'' END As Status,
				   IB.Brand_Name As MGrp,
				   IM.Value As SupType,
				   I.Item_Description As Description,
				   dbo.fn_GetCurrentNetCost(I.Item_Key,P.Store_No) As Cost,
				   P.Price As Price,
				   VCHE.Case_Size As BuyUOM,
				   IU.Unit_Abbreviation As SellUOM,
				   I.Package_Desc1 As InPk,
				   VDH.CaseQty As CasePack,
				   IU2.Unit_Abbreviation SelWt,
				   I.Package_Desc2 As SelDesc,			   
				   ISC.Scale_Description1 As ScaleDescription,
				   ISC.ShelfLife_Length As ShelfLife,
				   SE.Description As ExtraText,
				   NF.Description As Nutrifact,
				   ST.Description As ScaleTare,
				   ST2.Description As AltScaleTare,
				   SE.ExtraText As Ingredients				   
			FROM Item I (nolock)
			JOIN 
				ItemCategory IC (nolock)
				ON IC.Category_ID = I.Category_ID
			JOIN 
				ProdHierarchyLevel4 PH4 (nolock)
				ON PH4.ProdHierarchyLevel4_ID = I.ProdHierarchyLevel4_ID
			JOIN 
				ProdHierarchyLevel3 PH3 (nolock)
				ON PH3.ProdHierarchyLevel3_ID = PH4.ProdHierarchyLevel3_ID
			JOIN 
				ItemIdentifier II (nolock)
				ON II.Item_Key = I.Item_Key
			JOIN 
				ItemUnit IU (nolock)
				ON IU.Unit_ID = I.Retail_Unit_ID
			JOIN 
				ItemUnit IU2 (nolock)
				ON IU2.Unit_ID = I.Package_Unit_ID			
			JOIN 
				ItemVendor IV (nolock)
				ON IV.Item_Key = I.Item_Key
			JOIN 
				ItemBrand IB (nolock)
				ON IB.Brand_ID = I.Brand_ID
			JOIN 
				Price P (nolock)
				ON P.Item_Key = I.Item_Key
			JOIN 
				StoreItemVendor SIV (nolock)
				ON SIV.Item_Key = I.Item_Key
			JOIN Store S (nolock)
				ON S.Store_No = P.Store_No
			LEFT JOIN 
				ItemManager IM (nolock)
				ON IM.Manager_ID = I.Manager_ID
			LEFT JOIN 
				ItemScale ISC (nolock)
				ON ISC.Item_Key = I.Item_Key
			LEFT JOIN 
				Scale_ExtraText SE (nolock)
				ON SE.Scale_ExtraText_ID = ISC.Scale_ExtraText_ID
			LEFT JOIN 
				Nutrifacts NF (nolock)
				ON NF.NutrifactsID = ISC.Nutrifact_ID
			LEFT JOIN 
				Scale_Tare ST (nolock)
				ON ST.Scale_Tare_ID = ISC.Scale_Tare_ID
			LEFT JOIN 
				Scale_Tare ST2 (nolock)
				ON ST2.Scale_Tare_ID = ISC.Scale_Alternate_Tare_ID
			LEFT JOIN 
				VendorCostHistoryExceptions VCHE (nolock)
				ON VCHE.UPC = II.Identifier
			LEFT JOIN 
				VendorDealHistory VDH (nolock)
				ON VDH.StoreItemVendorId = SIV.StoreItemVendorId
			WHERE 1=1 '	
			
			IF @Store_No IS NOT NULL
					SELECT @SQL = @SQL + '
					AND P.Store_No = ' + @Store_No + ' AND SIV.Store_No = ' + @Store_No + ' '
					
			IF @Vendor_No IS NOT NULL
					SELECT @SQL = @SQL + '
					AND IV.Vendor_ID = ' + @Vendor_No + ' '
					
			IF @SubTeam_No IS NOT NULL
					SELECT @SQL = @SQL + '
					AND I.SubTeam_No = ' + @SubTeam_No + ' '
					
			IF @SubDept_No IS NOT NULL
					SELECT @SQL = @SQL + '
					AND I.Category_ID = ' + @SubDept_No + ' '
					
			IF @Class_No IS NOT NULL
					SELECT @SQL = @SQL + '
					AND PH3.ProdHierarchyLevel3_ID = ' + @Class_No + ' '				
					
			IF @SubClass_No IS NOT NULL
					SELECT @SQL = @SQL + '
					AND I.ProdHierarchyLevel4_ID =  ' + @SubClass_No + ' '										

			IF @SKU_Status IS NOT NULL
					IF @SKU_Status = 'A'
						SELECT @SQL = @SQL + '
						AND dbo.fn_GetDiscontinueStatus(I.Item_Key, NULL, NULL) = 0 '				
					ELSE IF @SKU_Status = 'D'
						SELECT @SQL = @SQL + '
						AND dbo.fn_GetDiscontinueStatus(I.Item_Key, NULL, NULL) = 1 '				    
					ELSE IF @SKU_Status = 'P'
						SELECT @SQL = @SQL + '
						AND II.Deleted_Identifier= 1 '				    						
						
			IF @Merch_Group IS NOT NULL
					SELECT @SQL = @SQL + '
					AND IB.Brand_Name LIKE %' + @Merch_Group + '% '													

			IF @Supplier_Type IS NOT NULL
					SELECT @SQL = @SQL + '
					AND IM.Manager_ID =' + @Supplier_Type + ' '																										
					
			IF @From_PLU IS NOT NULL
					SELECT @SQL = @SQL + '
					AND II.Identifier >= ''' + @From_PLU + ''' '																	

			IF @To_PLU IS NOT NULL
					SELECT @SQL = @SQL + '
					AND II.Identifier <= ''' + @To_PLU + ''''		

		EXEC(@SQL)
		
		SELECT @SQL = 'SELECT DISTINCT
		       RIK.*,
		       II.Identifier As Identifier,
		       II.IdentifierType As ID_Type,
		       CASE WHEN II.Default_Identifier=1 THEN ''T'' ELSE ''F'' END As Default_Identifier,
		       II.National_Identifier As NatUPC
		FROM #ReportItemKey RIK
		JOIN 
			ItemIdentifier II (nolock)
			ON II.Item_Key = RIK.Item_Key'
		
		IF @Plu_Type = 'Toledo' 
				SELECT @SQL = @SQL + '
				WHERE II.IdentifierType = ''O'' AND II.Identifier LIKE ''20%00000'''
		ELSE IF @Plu_Type = 'FrontEnd'
				SELECT @SQL = @SQL + '
				WHERE II.IdentifierType = ''O'' AND II.Identifier LIKE ''000000000%'''
			
		EXEC(@SQL)
		
		DROP TABLE #ReportItemKey
														
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemMasterReport] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemMasterReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemMasterReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemMasterReport] TO [IRMAReportsRole]
    AS [dbo];

