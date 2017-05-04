if exists (select * from dbo.sysobjects where id = object_id(N'dbo.JDASync_ExceptionAudit') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.JDASync_ExceptionAudit
GO

SET ANSI_NULLS ON
GO

SET ANSI_WARNINGS ON
GO 

CREATE Procedure dbo.JDASync_ExceptionAudit
AS
	-- ======================================================
	-- Purpose: This procedure compares synchronized data from 
	-- IRMA to corresponding tables in JDA.
	-- It then creates an audit table detailing the 
	-- differences.
	--
	-- Tech. Contact: David Marine - dmarine@athensgroup.com
	-- Created: 05.09.07
	--
	-- Date			Name	TFS		Comment
	-- 01.13.2013	BS		8755	Updated Item.Discontinue_Item with dbo.fn_GetDiscontinueStatus
	--								in case this stored proc is still used.
	-- ======================================================


BEGIN

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN

		DECLARE @StartDateTime datetime,
				@EndDateTime datetime,
				@ErrorCount int,
				@Table_Name varchar(75),
				@Key_Value int,
				@Scan_Results varchar(4000),
				@ResultValue varchar(4000),
				@HeaderID int,
				@Column_Name varchar(75),
				@IRMA_Value varchar(150),
				@JDA_Value varchar(150)

		----------------------------------------------
		-- Begin the audit
		----------------------------------------------
		
		----------------------------------------------
		-- How the audit works:
		--
		-- 1. Values in the sync'ed tables are compared
		-- for instances (items, identifiers, vendors, etc)
		-- found in IRMA but not in JDA and those in JDA that differ
		-- in the values being sync'ed.
		--
		-- 2. These differences are temporarily (for the
		-- duration of the execution of the audit) stored in
		-- a scratch table. If an instance is in both systems
		-- but then differs in at least one of it's sync'ed
		-- values then a concatonated list of the column name and
		-- IRMA and JDA values is recorded for all sync'ed columns
		-- whether or not all columns have differing values.
		--
		-- 3. A header row is created for the audit run with
		-- the start datetime.
		--
		-- 4. The data in the scratch table is parsed into
		-- an audit detail table which contains one row for
		-- each column in each table where there is a value difference.
		-- Or, if the instance is not found in JDA, one detail row
		-- will be created with an 'Is_InJDA' flag set to false.
		--
		-- 5. Finally, the header's end datetime is set and
		-- a notification email is sent out with a link to a
		-- web-based report showing the results of the audit.
		----------------------------------------------

		-- clear the scratch table
		TRUNCATE TABLE JDASync_AuditScratch

		-- get the audit start time
		SELECT @StartDateTime = GETDATE()

		
		-- clear IRMA JDA_IRMAPC table
		TRUNCATE TABLE JDA_IRMAPC
				
		-- copy contents of IRMAPC from JDA to IRMA  - this improves the performance of queries against this table
		-- as opposed to querying IRMAPC over the linked server
		INSERT INTO JDA_IRMAPC
			SELECT * 
			FROM JDA_Sync.FRESH.MM4R3LIB.IRMAPC
		

		-- ---------------------------------
		-- 1. Audit Item data
		-- ---------------------------------		
		INSERT INTO JDASync_AuditScratch (Table_Name, Key_Value, Scan_Results) 
            SELECT 'Item' AS Table_Name, Item.Item_Key AS Key_Value,
                    IsNull(
                            'Desc|' +  SUBSTRING(Item.Item_Description, 1, 30) + '|' + INVMAST.IDESCR + '^' +
                            'JDA_Dept|' + CAST(jhm.JDA_Dept AS VARCHAR(200)) + '|' + CAST(INVMAST.IDEPT AS VARCHAR(200)) + '^'  + 
                            'JDA_SubDept|' + CAST(jhm.JDA_SubDept AS VARCHAR(200)) + '|' + CAST(INVMST.ISDEPT AS VARCHAR(200)) + '^'  +
                            'JDA_Class|' + CAST(jhm.JDA_Class AS VARCHAR(200)) + '|' + CAST(INVMST.ICLAS AS VARCHAR(200)) + '^'  + 
                            'JDA_SubClass|' + CAST(jhm.JDA_SubClass AS VARCHAR(200)) + '|' + CAST(INVMAST.ISCLAS AS VARCHAR(200)) + '^'  + 
                            'Package_Desc1|' + CAST(Item.Package_Desc1 AS VARCHAR(200)) + '|' + CAST(INVMAST.ISTDPK AS VARCHAR(200)) + '^'  +
                            'Package_Desc2|' + CAST(Item.Package_Desc2 AS VARCHAR(200)) + '|' + CAST(INVSKU.ISELUW AS VARCHAR(200)) + '^'  + 
                            'Pack_JDA_ID|' + CAST(jium_package.JDA_ID AS VARCHAR(200)) + '|' + CAST(INVSKU.ISELUD AS VARCHAR(200)) + '^'  + 
                            'Delete / Discontinue|' + CAST(Item.Deleted_Item AS VARCHAR(200)) + '|' + CAST(dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) AS VARCHAR(200)) + '^' + CAST(INVMAST.IDSCCD AS VARCHAR(200)) + ','  + 
                            'WFM_Item|' + CAST(ItemAttribute.Check_Box_1 AS VARCHAR(200)) + '|' + CAST(INVMAST.IATRB3 AS VARCHAR(200)) + '^'  + 
                            'Vendor_JDA_ID|' + CAST(jium_vendor.JDA_ID AS VARCHAR(200)) + '|' + CAST(INVMAST.IBYUM AS VARCHAR(200))  + '^'  +
                            'Manager_ID|' + CAST(im.Value AS VARCHAR(200)) + '|' + CAST(INVMAST.IATRB1 AS VARCHAR(200))
                            , 'NOT_IN_JDA') as SCAn_Results

            FROM Item (NOLOCK)
                    LEFT JOIN ItemAttribute (NOLOCK)
                            ON ItemAttribute.Item_Key = Item.Item_Key
                    LEFT JOIN  JDA_Sync.FRESH.MM4R3LIB.INVMST INVMAST (NOLOCK)
                            ON INVMAST.INUMBR = Item.Item_Key
                    LEFT JOIN  JDA_Sync.FRESH.MM4R3LIB.INVMST INVMST (NOLOCK)
                            ON INVMST.INUMBR = Item.Item_Key
                    LEFT JOIN  JDA_Sync.FRESH.MM4R3LIB.INVSKU INVSKU (NOLOCK)
                            ON INVSKU.INUMBR = Item.Item_Key
                    LEFT JOIN JDA_ItemBrandMapping jibm (NOLOCK)
                            ON jibm.Brand_ID = Item.Brand_ID
                    LEFT JOIN ItemManager im
                            ON im.Manager_ID = Item.Manager_ID
                    LEFT JOIN JDA_HierarchyMapping jhm (NOLOCK)
                            ON jhm.ProdHierarchyLevel4_ID = Item.ProdHierarchyLevel4_ID
                    LEFT JOIN JDA_ItemUnitMapping jium_package (NOLOCK)
                            ON jium_package.Unit_ID = Item.Package_Unit_Id
                    LEFT JOIN JDA_ItemUnitMapping jium_retail (NOLOCK)
                            ON jium_retail.Unit_ID = Item.Retail_Unit_ID
                    LEFT JOIN JDA_ItemUnitMapping jium_vendor (NOLOCK)
                            ON jium_vendor.Unit_ID = Item.Vendor_Unit_ID
            WHERE 	(INVMAST.INUMBR IS NULL)   
                    OR (INVMST.INUMBR IS NULL )      
                    OR (NOT SUBSTRING(Item.Item_Description, 1, 30) = INVMAST.IDESCR )
                    OR (NOT jhm.JDA_Dept = INVMAST.IDEPT)             
                    OR (NOT jhm.JDA_SubDept = INVMST.ISDEPT)  
                    OR (NOT jhm.JDA_Class = INVMST.ICLAS )       
                    OR (NOT jhm.JDA_SubClass = INVMST.ISCLAS )
                    OR (NOT CAST(Item.Package_Desc2 as decimal(9,3)) = CASE 
														WHEN ((INVMST.IDESCR LIKE '% 3%') OR (INVMST.IDESCR LIKE '% 4%') OR (INVMST.IDESCR LIKE '% 6%') OR (INVMST.IDESCR LIKE '% 8%') OR (INVMST.IDESCR LIKE '% 10%') OR (INVMST.IDESCR LIKE '% 12%'))
																  AND (INVMST.IDESCR LIKE '%PK%' OR INVMST.IDESCR LIKE '%PACK%' OR INVMST.IDESCR LIKE '%PCK%')
																  AND (INVMST.IDEPT = 103 OR (INVMST.IDEPT = 101 AND INVMST.ISDEPT = 3 AND INVMST.ICLAS = 5)) AND (INVSKU.ISELUD <> 'PK')
																  THEN CAST(12 as decimal(9,3))
														WHEN (INVMST.IDESCR LIKE '% 2%')
																  AND (INVMST.IDESCR LIKE '%PK%' OR INVMST.IDESCR LIKE '%PACK%' OR INVMST.IDESCR LIKE '%PCK%')
																  AND (INVMST.IDEPT = 103 OR (INVMST.IDEPT = 101 AND INVMST.ISDEPT = 3 AND INVMST.ICLAS = 5)) AND (INVSKU.ISELUD <> 'PK')
																  THEN CAST(22 as decimal(9,3))
														WHEn INVSKU.ISELUW = 0 THEN CAST(1 as decimal(9,3))
														WHEN INVMST.IDEPt = 117 THEN CAST(1 as decimal(9,3))
														ELSE CAST(INVSKU.ISELUW as decimal(9,3))
												END)
                    OR (NOT jium_package.JDA_ID = CASE
                                                        WHEN ISNULL(INVSKU.ISELUD,'') = '' THEN 'EA'
														WHEN INVMST.IDEPt = 117 THEN 'LB'
                                                        ELSE INVSKU.ISELUD
                                                END)
                  OR (NOT jium_retail.JDA_ID = CASE 						
													WHEN (LEFT(INVMAST.ISLUM,1) IN ('C', 'K')) AND ISNUMERIC(SUBSTRING(INVMAST.ISLUM,2,1)) = 1 THEN jium_retail.JDA_ID -- do not report variance
													WHEN INVMST.IDEPt = 117 THEN 'LB'
													ELSE INVMAST.ISLUM
											  END)
--                  --OR NOT (CASE WHEN Item.Deleted_Item = 1 THEN 'P' WHEN Item.Discontinue_Item = 1 THEN 'D' ELSE 'A' END) = INVMAST.IDSCCD
                  OR (NOT ISNULL(ItemAttribute.Check_Box_1, 0) = Case INVMAST.IATRB3 WHEN 'Y' THEN 1 ELSE 0 END)
                  OR (NOT CASE 
							WHEN (jium_vendor.JDA_ID = 'BOX') THEN 'CAS'
							ELSE jium_vendor.JDA_ID 
						 END 
						= CASE WHEN (LEFT(INVMAST.IBYUM, 1) = 'C' OR LEFT(INVMAST.IBYUM, 1) = 'K') THEN 'CAS' 
							   WHEN (ISNUMERIC(INVMST.IBYUM) = 1) THEN 'CAS'
								ELSE INVMAST.IBYUM 
						  END)
                  OR (NOT CASE Item.Manager_ID
                                          WHEn 1 THEN 1
                                          WHEN 2 THEN 20
                                          WHEN 3 THEn 30
                                  END  = CASE WHEN INVMAST.IATRB1  NOT IN (20,30) THEN 1 ELSE INVMAST.IATRB1  END)		
			
		-- ---------------------------------
		-- 2. Audit ItemIdentifier data
		-- ---------------------------------
		INSERT INTO JDASync_AuditScratch (Table_Name, Key_Value, Scan_Results) 
		SELECT 'ItemIdentifier' AS Table_Name, ItemIdent.Item_Key AS Key_Value, 
			IsNull(
			'Identifier|' + CAST(ItemIdent.Identifier AS VARCHAR(200))  + '|' +
			-- if National_Identifier is true in IRMA
			-- then use value in National Identifier table (INVUPCNA) in JDA	
			CASE WHEN ItemIdent.National_Identifier = 1 THEN CAST(INVUPCNA.NAUPC AS VARCHAR(200)) ELSE CAST(INVUPC.IUPC AS VARCHAR(200)) END + '^' +
			'National_Identifier|' + CAST(ItemIdent.National_Identifier AS VARCHAR(200))  + '|' + CASE WHEN INVUPCNA.NAUPC IS NULL THEN '0' ELSE '1' END + '^' +
			'ItemType_ID|' + CAST(Item.ItemType_ID AS VARCHAR(200)) + '|' + CAST(INVUPC.IUPCCD AS VARCHAR(200))
			, 'NOT_IN_JDA') AS Scan_Results
		FROM ItemIdentifier ItemIdent (NOLOCK)
			JOIN Item (NOLOCK)
				ON Item.Item_Key = ItemIdent.Item_Key
			LEFT JOIN  JDA_Sync.FRESH.MM4R3LIB.INVUPC INVUPC (NOLOCK)
				ON INVUPC.INUMBR = ItemIdent.Item_Key
			LEFT JOIN  JDA_Sync.FRESH.MM4R3LIB.INVUPCNA INVUPCNA (NOLOCK)
				ON INVUPCNA.NAITEM = ItemIdent.Item_Key
		WHERE NOT Item.ItemType_ID = INVUPC.IUPCCD
			AND (ItemIdent.National_Identifier = 0 AND INVUPCNA.NAUPC IS NULL
			OR (ItemIdent.National_Identifier = 1 AND NOT ItemIdent.National_Identifier = INVUPCNA.NAUPC))
			
			
		-- ---------------------------------
		-- 3. Audit Vendor data
		-- ---------------------------------
		INSERT INTO JDASync_AuditScratch (Table_Name, Key_Value, Scan_Results) 
			SELECT 'Vendor' AS Table_Name, Vendor.Vendor_ID AS Key_Value,  
				IsNull(
						'CompanyName|' + Vendor.CompanyName + '|' + APSUPP.ASNAME + '^'+
						'Address_Line_1|' + Vendor.Address_Line_1 + '|' + APADDR.AAADD1 + '^'+
						'Address_Line_2|' + Vendor.Address_Line_2 + '|' +  APADDR.AAADD2 + '^'+
						'City|' +  Vendor.City + '|' + APADDR.AAADD3  + '^'+
						'Vendor.State|' +  Vendor.State + '|' +  APADDR.AASTAT + '^'+
						'Zip_Code|' + CAST(Vendor.Zip_Code AS VARCHAR(200)) + '|' + CAST(APADDR.AAPSCD AS VARCHAR(200)) + '^'+
						'Country|' +  Vendor.Country + '|' +  CAST(APADDR.AAHOME AS VARCHAR(200)) + '^'+
						'Phone|' + CAST(Vendor.Phone AS VARCHAR(200))  + '|' + CAST(APADDR.AAPHON AS VARCHAR(200)) + '^'+
						'Fax|' +  CAST(Vendor.Fax AS VARCHAR(200)) + '|' +  CAST(APADDR.AAFAX# AS VARCHAR(200)) + '^'+
						'PS_Vendor_ID|' + CAST(Vendor.PS_Vendor_ID AS VARCHAR(200)) + '|' +  CAST(ISNULL(VIVXREF.VXPSVN,0) AS VARCHAR(200)) + '^'+
						-- map Vendor.Non_Product_Vendor = 1 to APSUPP.ASTYPE = 2
						'Non_Product_Vendor|' +  CASE WHEN Vendor.Non_Product_Vendor = 1 THEN '2' ELSE CAST(Vendor.Non_Product_Vendor AS VARCHAR(200)) END + '|' +  CAST(APSUPP.ASTYPE AS VARCHAR(200)) + '^'
						, 'NOT_IN_JDA') AS Scan_Results
					FROM Vendor Vendor (NOLOCK)
						LEFT JOIN  JDA_Sync.FRESH.MM4R3LIB.APSUPP APSUPP (NOLOCK)
							ON APSUPP.ASNUM = Vendor.Vendor_ID
						LEFT JOIN  JDA_Sync.FRESH.MM4R3LIB.APADDR APADDR (NOLOCK)
							ON APADDR.AANUM = Vendor.Vendor_ID
						LEFT JOIN  JDA_Sync.FRESH.MM4R3LIB.VIVXREF VIVXREF (NOLOCK)
							ON VIVXREF.VXVEND = Vendor.Vendor_ID
					WHERE ((APSUPP.ASNUM IS NULL) AND NOT (Vendor.Vendor_ID BETWEEN 40000 AND 40990)) -- do not return variances for store vendors
						OR ((APADDR.AANUM IS NULL) AND NOT (Vendor.Vendor_ID BETWEEN 40000 AND 40990)) -- do not return variances for store vendors
			--			OR VIVXREF.VXVEND IS NULL --removed as JDA does not necessarily have this
						OR NOT Vendor.CompanyName = APSUPP.ASNAME
						OR NOT Vendor.Address_Line_1 = APADDR.AAADD1
						OR NOT Vendor.Address_Line_2 = CASE LEN(RTRIM(APADDR.AAADD3)) WHEN 0 THEN Vendor.Address_Line_2 ELSE APADDR.AAADD2 END
						OR NOT Vendor.City = CASE LEN(RTRIM(APADDR.AAADD3)) WHEN 0 THEN APADDR.AAADD2 ELSE APADDR.AAADD3 END
						OR NOT Vendor.State = LEFT(APADDR.AASTAT,2)
						OR NOT Vendor.Zip_Code = APADDR.AAPSCD
						OR NOT Vendor.Country = CASE APADDR.AAHOME WHEN 'US' THEN 'USA' ELSE APADDR.AAHOME END
						OR NOT REPLACE(REPLACE(Vendor.Phone, '/', ''), '-', '') = REPLACE(CAST(APADDR.AAPHON AS VARCHAR(200)), '-', '')
						OR NOT REPLACE(REPLACE(Vendor.Fax, '/', ''), '-', '') = REPLACE(CAST(APADDR.AAFAX# AS VARCHAR(200)), '-', '')
						OR NOT Vendor.PS_Vendor_ID = ISNULL(VIVXREF.VXPSVN,Vendor.PS_Vendor_ID) -- If no VIVXREF record, do not return variance
						OR 
						(
							-- map Vendor.Non_Product_Vendor = 1 to APSUPP.ASTYPE IN (2,3)
							(Vendor.Non_Product_Vendor = 1 AND APSUPP.ASTYPE = 1)
							OR (Vendor.Non_Product_Vendor = 0 AND APSUPP.ASTYPE > 1)
						)
						OR NOT REPLACE(Vendor.Po_Note,' ','') = REPLACE(APSUPP.PONOT1 +  APSUPP.PONOT2  + APSUPP.PONOT3,' ','')
						OR NOT REPLACE(Vendor.Receiving_Authorization_Note,' ','') = REPLACE(APSUPP.RCNOT1 +  APSUPP.RCNOT2  + APSUPP.RCNOT3,' ','')
						OR NOT Vendor.Other_Name =  APSUPP.ASOTHN
		-- ---------------------------------
		-- 4. Audit ItemVendor data
		-- ---------------------------------
		INSERT INTO JDASync_AuditScratch (Table_Name, Key_Value, Scan_Results) 
		SELECT 'ItemVendor' AS Table_Name, ItemVendor.Item_Key AS Key_Value, 
			IsNull(
			'Vendor_ID_INVMST|' + CAST(ItemVendor.Vendor_ID AS VARCHAR(200)) + '|' + ISNULL(CAST(INVMST.ASNUM AS VARCHAR(200)),'No INVMST Vendor') + '^'+
			'Item_Id_INVMST|' + CAST(ItemVendor.Item_Id AS VARCHAR(200)) + '|' + ISNULL(CAST(INVMST.IVNDP# AS VARCHAR(200)),'No INVMST Vendor Part #') + '^'+
			'Vendor_ID_INVVEN|' + CAST(ItemVendor.Vendor_ID AS VARCHAR(200)) + '|' + ISNULL(CAST(INVVEN.IVVNUM AS VARCHAR(200)),'No INVVEN Vendor') + '^'+
			'Item_Id_INVVEN|' + CAST(ItemVendor.Item_Id AS VARCHAR(200)) + '|' + ISNULL(CAST(INVVEN.IVVND# AS VARCHAR(200)),'No INVVEN Vendor Part #')
			, 'NOT_IN_JDA') AS Scan_Results
		FROM ItemVendor ItemVendor (NOLOCK)
			LEFT JOIN  JDA_Sync.FRESH.MM4R3LIB.INVMST INVMST (NOLOCK)
				ON INVMST.INUMBR = ItemVendor.Item_Key AND INVMST.ASNUM = ItemVendor.Vendor_ID
			LEFT JOIN  JDA_Sync.FRESH.MM4R3LIB.INVVEN INVVEN (NOLOCK)
				ON INVVEN.INUMBR = ItemVendor.Item_Key AND INVVEN.IVVNUM = ItemVendor.Vendor_ID
		WHERE 
			((COALESCE(INVVEN.INUMBR,INVMST.INUMBR,NULL) IS NULL)
		OR (LTRIM(COALESCE(INVMST.IVNDP#,INVVEN.IVVND#,NULL)) <> ItemVendor.Item_ID))
		AND VENDOR_ID <> 40808		-- Exclude Warehouse items - there is no equivalnet in JDA
		AND Item_ID <> '000000'
		AND Item_ID <> CAST(INVMST.INUMBR as varchar(20))			
			
		-- ---------------------------------
		-- 5. Audit StoreItemVendor data
		-- ---------------------------------
		INSERT INTO JDASync_AuditScratch (Table_Name, Key_Value, Scan_Results) 
		SELECT 'StoreItemVendor' AS Table_Name, SIV.Store_No AS Key_Value,  
			IsNull(
			'Store_No|' + CAST(SIV.Store_No AS VARCHAR(200)) + '|' + CAST(I.Store_No AS VARCHAR(200)) + '^'+
			'Item_Key|' + CAST(SIV.Item_Key AS VARCHAR(200)) + '|' + CAST(I.INUMBR AS VARCHAR(200)) + '^'+
			'Vendor_ID|' + CAST(SIV.Vendor_ID AS VARCHAR(200)) + '|' + CAST(I.VendorNo AS VARCHAR(200)) + '^'+
			'PrimaryVendor|' + CAST(SIV.PrimaryVendor AS VARCHAR(200)) + '|' + CAST(I.PrimaryVendor AS VARCHAR(200))
			, 'NOT_IN_JDA') AS Scan_Results
		FROM StoreItemVendor SIV (NOLOCK)
			LEFT JOIN  (SELECT INVMST.INUMBR, s.Store_No as Store_No, ASNUM as VendorNo, CASE WHEN V.IVVNUM IS NULL THEN 1 ELSE CASE V.IVVNUM WHEN ASNUM THEN 1 ELSE 0 END END as PrimaryVendor
						FROM JDA_Sync.FRESH.MM4R3LIB.INVMST INVMST (NOLOCK)
						Cross Join Store s
						LEFT OUTER JOIN JDA_Sync.FRESH.MM4R3LIB.INVVEN  V(NOLOCK)
						ON INVMST.INUMBR = V.INUMBR AND S.Store_No = V.IVSTOR AND V.IVPRIM = 'P'
						UNION 
						SELECT V.INUMBR, V.IVSTOR as Store_No, V.IVVNUM as VendorNo, CASE WHEN V.IVPRIM = 'P' THEN 1 ELSE 0 END as PrimaryVendor
						FROM JDA_Sync.FRESH.MM4R3LIB.INVVEN V (NOLOCK)
						LEFT OUTER JOIN JDA_Sync.FRESH.MM4R3LIB.INVMST INVMST 
						ON INVMST.INUMBR = V.INUMBR
						WHERE ISNULL(INVMST.ASNUM,-1) <> V.IVVNUM
						) I
				ON  I.INUMBR = SIV.Item_Key AND I.VendorNo = SIV.Vendor_ID and i.Store_No = SIV.Store_No
		WHERE ((I.VendorNo IS NULL)
			OR (NOT SIV.PrimaryVendor = I.PrimaryVendor))
		AND Vendor_ID <> 40808
		AND SIV.Store_NO NOT IN (60,61,62,63,64,65,500,600)
			
-- ---------------------------------
		-- 6. Cost data
		-- ---------------------------------
		INSERT INTO JDASync_AuditScratch (Table_Name, Key_Value, Scan_Results) 

		SELECT 'Cost', (v.Item_Key * 100) + v.Store_No, 
		ISNULL('Net Cost|' + CAST(NetCost AS VARCHAR(200)) + '|' + CAST(BUYCST AS VARCHAR(200)) +  '^'+
		'Vendor|' + CAST(v.Vendor_ID AS VARCHAR(200)) + '|' + CAST(pc.CSTVND AS VARCHAR(200)) +  '^'+
		'Start Date| ' + CAST(v.StartDate as varchar(200))+ '|' + CAST(dbo.JDA_DATE(1,CAST(pc.O3SDT as int)) as varchar(200)) + '^'
		, 'NOT_IN_JDA')AS Scan_Results
		FROM JDA_IRMAPC pc 
		LEFT OUTER JOIN [dbo].[fn_VendorCostAll](GETDATE()) v
		ON v.Item_Key = pc.INUMBR AND v.Store_No = pc.STR#  AND CSTTYP = 'L'
		WHERE Store_No < 60
		AND ISNULL(v.NetCost,0) <> BUYCST
		AND ISNULL(v.vendor_Id,0)  = 40808		--warehouse; loaded cost
		UNION
		SELECT 'Cost', (v.Item_Key * 100) + v.Store_No, 
		IsNull('Net Cost|' + CAST(NetCost AS VARCHAR(200)) + '|' + CAST(BUYCST AS VARCHAR(200)) +  '^'+
		'Vendor|' + CAST(v.Vendor_ID AS VARCHAR(200)) + '|' + CAST(pc.CSTVND AS VARCHAR(200)) +  '^'+
		'Start Date| ' + CAST(v.StartDate as varchar(200))+ '|' + CAST(dbo.JDA_DATE(1,CAST(pc.O3SDT as int)) as varchar(200)) + '^'
		, 'NOT_IN_JDA')AS Scan_Results
		FROM JDA_IRMAPC pc
		LEFT OUTER JOIN [dbo].[fn_VendorCostAll](GETDATE()) v
		ON v.Item_Key = pc.INUMBR AND v.Store_No = pc.STR#  AND v.Vendor_ID = CSTVND
		WHERE Store_No < 60
		AND ISNULL(v.NetCost,0) <> BUYCST
		AND ISNULL(v.vendor_Id,0)  <> 40808		--non-loaded cost



		-- ---------------------------------
		-- 7. Price data
		-- ---------------------------------
		
		INSERT INTO JDASync_AuditScratch (Table_Name, Key_Value, Scan_Results) 
			SELECT  'Price', (Price.Item_Key * 100) + Price.Store_No, 
				IsNull(
				'JDA-Based PriceChgType|' + CAST(jpcm.JDA_Priority AS VARCHAR(200)) + '|' + CAST(IRMAPC.O3PPRE AS VARCHAR(200)) + '^' +
				'Reg Price|' + CAST(Price.POSPrice AS VARCHAR(200)) + '|' + CAST(IRMAPC.O3REGU AS VARCHAR(200)) + '^' +
				'Sale Price|' + CAST(Price.Sale_Price AS VARCHAR(200)) + '|' + CAST(IRMAPC.O3POS AS VARCHAR(200)) + '^'
				, 'NOT_IN_JDA') AS Scan_Results
			FROM Price (NOLOCK)
				LEFT JOIN dbo.JDA_PriceChgTypeMapping jpcm (NOLOCK)
					ON jpcm.PriceChgTypeID = Price.PriceChgTypeID
				LEFT JOIN  JDA_IRMAPC IRMAPC (NOLOCK)
					ON  IRMAPC.INUMBR = Price.Item_Key
					AND IRMAPC.[STR#] = Price.Store_No
					AND IRMAPC.[YMD#] = CAST(SUBSTRING(CONVERT(VARCHAR, GetDate(), 112), 3, 7) as int)
				WHERE ((IRMAPC.INUMBR IS NULL) 
						OR( NOT jpcm.JDA_Priority = CASE IRMAPC.O3POS
													WHEN 0 THEN jpcm.JDA_Priority  -- IF a zero price, ignore the priority as JDA sets some as 0
													ELSE IRMAPC.O3PPRE
												 END )
							OR (ABS(CASE WHEN Price.Multiple > 0 THEN ROUND(Price.Price / Price.Multiple,2) ELSE 0 END - IRMAPC.O3REGU) > 1)           -- Regular Price if Sale Multiple not met
							OR NOT (Price.Price = IRMAPC.O3REG)          
							OR NOT (Price.Multiple = CASE IRMAPC.O3POS
														WHEN 0 THEN Price.Multiple
														ELSE IRMAPC.O3RMLT 
													END )      -- Regular Multiple; do not report variance if JDA Price is 0
--							  -- Sale Price = POS Price; the IRMA field Sale_Price
--	--                        -- will be populated with the regular price if no sale is actually on
							OR (CASE 
										WHEN Price.Sale_Price > 0 THEN Price.Sale_Price
										ELSE Price.Price 
									END - IRMAPC.O3POS ) > .01
--	--                        -- Sale Multiple = POS Multiple; the IRMA field Sale_Multiple
--	--                        -- will be populated with the regular multiple if no sale is actually on
							OR NOT (Price.Sale_Multiple = CASE IRMAPC.O3POS
														WHEN 0 THEN Price.Sale_Multiple
														ELSE IRMAPC.O3PMLT 
													END ) 
					)
			AND price.Store_No < 60
			ORDER BY Item_key, Store_No

			
		-- ---------------------------------
		-- End of audit 
		-- ---------------------------------	
		SELECT @EndDateTime = GETDATE ()
		
		----------------------------------------------
		-- Save Results of audit
		----------------------------------------------

		-- Insert audit header row
		
		SELECT @EndDateTime = GETDATE ()
		
		INSERT INTO JDASync_AuditHeader
		(
			StartDateTime,
			EndDateTime
		)
		Values
		(
			@StartDateTime,
			@EndDateTime
		)
		
		
		Select @HeaderID = SCOPE_IDENTITY()
		
		DECLARE AuditScanResults_Cursor cursor for
			SELECT 
				[Table_Name]
				,[Key_Value]
				,[Scan_Results]
			FROM [JDASync_AuditScratch]
		
		OPEN AuditScanResults_Cursor
		
		FETCH NEXT FROM AuditScanResults_Cursor
			into @Table_Name, @Key_Value, @Scan_Results
			
		WHILE @@FETCH_STATUS = 0
		BEGIN -- loop through rows in the audit results temp table
		
			IF @Scan_Results = 'NOT_IN_JDA'
			BEGIN -- handle not in JDA condition

					INSERT INTO JDASync_AuditDetail
					(
						JDASync_AuditHeader_ID,
						Table_Name,
						Key_Value,
						Is_InJDA,
						ScanDateTime
					)
					VALUES
					(
						@HeaderID,
						@Table_Name,
						@Key_Value,
						0,
						@StartDateTime
					)
			
			END	-- handle not in JDA condition	
			ELSE
			BEGIN -- handle in JDA but different condition
		
				-- split an audit row into the concatonated string of
				-- audit properties for each column being audited
				DECLARE ColumnScanResults_Cursor CURSOR FOR
					SELECT Key_Value
					FROM
					dbo.fn_ParseStringList(@Scan_Results, '^')

				OPEN ColumnScanResults_Cursor

				FETCH NEXT FROM ColumnScanResults_Cursor 
					INTO @ResultValue
					
				WHILE @@FETCH_STATUS = 0
				BEGIN -- loop through each row in the audit results temp table
				
					-- split the concatonated string of
					-- audit properties for each column being audited
					-- into the separate properties
					DECLARE ColumnAuditResultsProperties_Cursor CURSOR FOR
					SELECT Key_Value
					FROM
					dbo.fn_ParseStringList(@ResultValue, '|')

					OPEN ColumnAuditResultsProperties_Cursor

					-- get the audited Column's name
					FETCH NEXT FROM ColumnAuditResultsProperties_Cursor 
						INTO @Column_Name
						
					-- get the audited Column's IRMA value
					FETCH NEXT FROM ColumnAuditResultsProperties_Cursor 
						INTO @IRMA_Value
						
					-- get the audited Column's JDA value
					FETCH NEXT FROM ColumnAuditResultsProperties_Cursor 
						INTO @JDA_Value

					-- close and clean up
					CLOSE ColumnAuditResultsProperties_Cursor		
					DEALLOCATE ColumnAuditResultsProperties_Cursor

					IF NOT @IRMA_Value = @JDA_Value
					BEGIN
						INSERT INTO JDASync_AuditDetail
						(
							JDASync_AuditHeader_ID,
							Table_Name,
							Key_Value,
							Is_InJDA,
							Column_Name,
							IRMA_Value,
							JDA_Value,
							Is_Queued,
							ScanDateTime
						)
						VALUES
						(
							@HeaderID,
							@Table_Name,
							@Key_Value,
							1,
							@Column_Name,
							@IRMA_Value,
							@JDA_Value,
							0,
							@StartDateTime
						)		
					END
					
					FETCH NEXT FROM ColumnScanResults_Cursor 
						INTO @ResultValue
							
				END -- loop through each row in the audit results temp table
								
				-- close and clean up
				CLOSE ColumnScanResults_Cursor		
				DEALLOCATE ColumnScanResults_Cursor

			END	 -- handle in JDA but different condition

			FETCH NEXT FROM AuditScanResults_Cursor
				into @Table_Name, @Key_Value, @Scan_Results

		END -- loop through each row in the audit results temp table

		-- close and clean up
		CLOSE AuditScanResults_Cursor
		DEALLOCATE AuditScanResults_Cursor

		-- set the audit completion time
		UPDATE JDASync_AuditHeader
		SET EndDateTime = GetDate()
		
		-- send out the audit complete notification
				
		EXEC dbo.JDASync_Notify
			@EventKey = 'SYNC_AUDIT_COMPLETE',
			@AdditionalBodyText = ''

	END -- if the SyncJDA flag is true
END
GO