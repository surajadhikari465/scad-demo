/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DN		2013.01.03	8755	Updated Disco Ordering Logic

***********************************************************************************************/

CREATE VIEW [dbo].[DC_ITEM]
AS
        SELECT
          --ITEM VIEW
            i.Item_Key,
            --ItemIdentifier
            ii.Identifier,
            ii.Default_Identifier,
            ii.Deleted_Identifier,
            ii.Add_Identifier,
            ii.Remove_Identifier,
            ii.National_Identifier,
            ii.CheckDigit,
            CASE ii.IdentifierType
              WHEN 'B' THEN  'BARCODE'
              WHEN 'S' THEN  'SKU'
	      WHEN 'P' THEN  'PLU'
	      ELSE 'OTHER'
            END as IdentifierType,
            ii.NumPluDigitsSentToScale,
            ii.Scale_Identifier,
            Item_Description,
            Sign_Description,
            Ingredients,
            --i.SubTeam_No,
            Sales_Account,
            Package_Desc1 as [Retail Package Pack],
            Package_Desc2 as [Retail Pack Size],
            --Package_Unit_ID,
            ip.Unit_Name as [Retail Package UOM],
            Min_Temperature,
            Max_Temperature,
            Units_Per_Pallet,
            Average_Unit_Weight,
            Tie,
            High,
            Yield,
            ib.Brand_Name,
            --Category_ID,
            ic.Category_Name,
            --Origin_ID,
            io.Origin_Name,
            ShelfLife_Length,
            --ShelfLife_ID,
            isl.ShelfLife_Name,
            --Retail_Unit_ID,
            ir.Unit_Name as Retail_Unit_Name,
            --Vendor_Unit_ID,
            iv.Unit_Name as Vendor_Unit_Name,
            --i.Distribution_Unit_ID,
            id.Unit_Name as Distribution_Unit_Name,
            --Cost_Unit_ID,
            iu.Unit_Name as Cost_Unit_Name,
            iu.IsPackageUnit,
            --Freight_Unit_ID,
            ifr.Unit_Name as Freight_Unit_Name,
            Deleted_Item,
            dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, NULL) as Discontinue_Item,
            WFM_Item,
            Not_Available,
            Pre_Order,
            Remove_Item,
            NoDistMarkup,
            Organic,
            Refrigerated,
            Keep_Frozen,
            Shipper_Item,
            Full_Pallet_Only,
            --User_ID,
            POS_Description,
            Retail_Sale,
            Food_Stamps,
            Discountable,
            Price_Required,
            Quantity_Required,
            ItemType_ID,
            HFM_Item,
            ScaleDesc1,
            ScaleDesc2,
            Not_AvailableNote,
            --CountryProc_ID,
            ioc.Origin_Name as CountryProcName,
            Insert_Date,
            Manufacturing_Unit_ID,
            i.EXEDistributed,
            ClassID,
            --User_ID_Date,
            --DistSubTeam_No,
            ds.SubTeam_Name as DistSubteam,
            CostedByWeight,
            --TaxClassID,
            tc.TaxClassDesc,
            --LabelType_ID,
            lt.LabelTypeDesc,
            ScaleDesc3,
            ScaleDesc4,
            ScaleTare,
            ScaleUseBy,
            ScaleForcedTare,
            QtyProhibit,
            GroupList,
            --ProdHierarchyLevel4_ID,
            ph.description as ProdHierarchyLevel4Desc,
            Case_Discount,
            Coupon_Multiplier,
            Misc_Transaction_Sale,
            Misc_Transaction_Refund,
            Recall_Flag,
            --Manager_ID,
            Ice_Tare,
            LockAuth,
            PurchaseThresholdCouponAmount,
            PurchaseThresholdCouponSubTeam,
            Product_Code,
            Unit_Price_Category,
            --StoreJurisdictionID,
            sj.StoreJurisdictionDesc,
            CatchweightRequired,
            COOL,
            BIO,
            --Subteam
            s.SubTeam_No,
            s.Team_No,
            s.SubTeam_Name,
            s.SubTeam_Abbreviation,
            s.Dept_No,
            s.SubDept_No,
            s.Target_Margin,
            s.JDA,
            s.GLPurchaseAcct,
            s.GLDistributionAcct,
            s.GLTransferAcct,
            s.GLSalesAcct,
            s.Transfer_To_Markup,
            s.EXEWarehouseSent,
            s.ScaleDept,
            s.Retail,
            s.EXEDistributed as EXEDistributedSubteam,
            s.SubTeamType_ID,
            s.PurchaseThresholdCouponAvailable

          FROM Item       (nolock) i
             INNER JOIN ItemIdentifier (nolock) ii
                ON ii.Item_Key = i.Item_Key
              LEFT OUTER JOIN ItemBrand (nolock) ib
                ON ib.Brand_ID = i.Brand_ID
              LEFT OUTER JOIN Subteam (nolock) s
                ON s.subteam_No = i.subteam_No
              INNER JOIN ItemCategory (nolock) ic
                ON ic.Category_ID = i.Category_ID
              LEFT OUTER JOIN ItemUnit (nolock) iu
                ON iu.Unit_ID = i.Cost_Unit_ID
              LEFT OUTER JOIN ItemUnit (nolock) id
                ON id.Unit_ID = i.Distribution_Unit_ID
              LEFT OUTER JOIN ItemUnit (nolock) iv
                ON iv.Unit_ID = i.Vendor_Unit_ID
              LEFT OUTER JOIN ItemUnit (nolock) ir
                ON ir.Unit_ID = i.Retail_Unit_ID
              LEFT OUTER JOIN ItemUnit (nolock) ifr
                ON ifr.Unit_ID = i.Freight_Unit_ID
              LEFT OUTER JOIN ItemUnit (nolock) ip
                ON ip.Unit_ID = i.Package_Unit_ID
              LEFT OUTER JOIN ItemShelfLife (nolock) isl
                ON isl.ShelfLife_ID = i.ShelfLife_ID
              LEFT OUTER JOIN ItemOrigin (nolock) io
                ON io.Origin_ID = i.Origin_ID
              LEFT OUTER JOIN ItemOrigin (nolock) ioc
                ON ioc.Origin_ID = i.CountryProc_ID
              LEFT OUTER JOIN labeltype (nolock) lt
                ON lt.LabelType_ID = i.LabelType_ID
              LEFT OUTER JOIN ProdHierarchyLevel4 (nolock) ph
                ON ph.ProdHierarchyLevel4_ID = i.ProdHierarchyLevel4_ID
              LEFT OUTER JOIN StoreJurisdiction (nolock) sj
                ON sj.StoreJurisdictionID = i.StoreJurisdictionID
              LEFT OUTER JOIN Subteam (nolock) ds
                ON ds.subteam_No = i.DistSubTeam_No
              LEFT OUTER JOIN TaxClass (nolock) tc
                ON tc.TaxClassID = i.TaxClassID
GO
GRANT SELECT
    ON OBJECT::[dbo].[DC_ITEM] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DC_ITEM] TO [IRMADCAnalysisRole]
    AS [dbo];

