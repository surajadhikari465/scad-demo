
IF EXISTS ( SELECT  *
            FROM    sysobjects
            WHERE   name = 'DC_ORDER'
                    AND xtype = 'V' ) 
    DROP VIEW [dbo].[DC_ORDER]
GO

CREATE VIEW [dbo].[DC_ORDER]
AS
    SELECT  OH.OrderHeader_ID AS PONumber ,
            OH.InvoiceNumber AS POInvoiceNumber ,
            OH.OrderHeaderDesc AS PONotes ,
            OH.Vendor_ID AS POVendor_ID ,
            V.CompanyName AS VendorName ,
            OH.PurchaseLocation_ID AS POPurchaseLocID ,
            PLOC.CompanyName AS PurchLocName ,
            OH.ReceiveLocation_ID AS POReceiveLocID ,
            RLOC.CompanyName AS RecvLocName ,
            OH.CreatedBy AS CreatedByUserID ,
            UCB.UserName AS CreatedByUserName ,
            UCB.FullName AS CreatedByFullName ,
            OH.OrderDate AS POOrderDate ,
            OH.Expected_Date AS POExpectedDate ,
            OH.Sent AS POIsSent ,
            OH.SentDate AS POSentDate ,
            OH.WarehouseSent AS POIsWarehouseSent ,
            OH.WarehouseSentDate AS POWarehouseSentDate ,
            OH.RecvLogDate AS POReceivedDate ,
            OH.RecvLogUser_ID AS POReceivedUserID ,
            OH.CloseDate AS POCloseDate ,
            OH.InvoiceDate AS POInvoiceDate ,
            OH.ApprovedDate AS POApprovedDate ,
            OH.ApprovedBy AS POApprovedByUserID ,
            UAB.UserName AS POApprovedByUserName ,
            UAB.FullName AS POApprovedByFullName ,
            OH.UploadedDate AS POUploadedDate ,
            OH.VendorDocDate AS POVendorDocDate ,
            OH.VendorDoc_ID AS POVendorDocId ,
            OH.SystemGenerated AS POIsSystemGenerated ,
            OH.Fax_Order AS POIsFaxOrder ,
            OH.SentToFaxDate AS POSentToFaxDate ,
            OH.Email_Order AS POIsEmailOrder ,
            OH.SentToEmailDate AS POSentToEmailDate ,
            OH.QuantityDiscount AS POQuantityDiscount ,
            OH.DiscountType AS PODiscountType ,
            OH.Transfer_SubTeam AS POTransferSubTeam ,
            TranST.SubTeam_Name AS POTransferSubTeamName ,
            TranST.GLPurchaseAcct AS POTransferSubTeamGLPurchAcct ,
            TranST.GLTransferAcct AS POTransferSubTeamGLTransAcct ,
            TranST.GLSalesAcct AS POTransferSubTeamGLSalesAcct ,
            TranST.EXEWarehouseSent AS POTransferSubTeamEXEWarehouseSent ,
            TranST.EXEDistributed AS POTransferSubTeamEXEDistributed ,
            OH.Transfer_To_SubTeam AS POTransferToSubTeam ,
            TranToST.SubTeam_Name AS POTransferToSubTeamName ,
            TranToST.GLPurchaseAcct AS POTransferToSubTeamGLPurchAcct ,
            TranToST.GLTransferAcct AS POTransferToSubTeamGLTransAcct ,
            TranToST.GLSalesAcct AS POTransferToSubTeamGLSalesAcct ,
            TranToST.EXEWarehouseSent AS POTransferToSubTeamEXEWarehouseSent ,
            TranToST.EXEDistributed AS POTransferToSubTeamEXEDistributed ,
            OH.Return_Order AS POIsReturnOrder ,
            OH.Temperature AS POTemperature ,
            OH.Accounting_In_DateStamp AS POAccountingInDate ,
            OH.Accounting_In_UserId AS POAccountingInUserId ,
            UAI.UserName AS POAccountingInUserName ,
            UAI.FullName AS POAccountingInFullName ,
            CASE OH.OrderType_ID
              WHEN 1 THEN 'Purchase Order'
              WHEN 2 THEN 'Distribution Order'
              WHEN 3 THEN 'Transfer Order'
              WHEN 4 THEN 'Flowthru Order'
            END AS POOrderType ,
            CASE OH.ProductType_ID
              WHEN 1 THEN 'Retail/Manufacturing'
              WHEN 2 THEN 'Packaging'
              WHEN 3 THEN 'Supplies'
            END AS POProductType ,
            OH.FromQueue AS POIsFromQueue ,
            OH.ClosedBy AS POClosedByUserId ,
            UCB2.UserName AS POClosedByUserName ,
            UCB2.FullName AS POClosedByFullName ,
            OH.MatchingDate AS POMatchingDate ,
            OH.MatchingValidationCode AS POMatchingValidationCode ,
            OH.MatchingUser_ID AS POMatchingUserId ,
            UMU.UserName AS POMatchingUserName ,
            UMU.FullName AS POMatchingUserFullName ,
            OH.Freight3Party_OrderCost AS POThirdPartyFreightOrderCost ,
            OH.DVOOrderID AS PODVOOrderId ,
            OH.eInvoice_Id AS POeInvoiceId ,
            OI.Item_Key AS POItemKey ,
            II.Identifier AS POItemIdentifier ,
            OI.ExpirationDate AS POItemExpirationDate ,
            OI.QuantityOrdered AS POItemQuantityOrdered ,
            OI.QuantityUnit AS POItemQuantityUnitId ,
            QU.Unit_Name AS POItemQuantityUnit ,
            OI.QuantityReceived AS POItemQuantityReceived ,
            OI.Total_Weight AS POItemTotalWeight ,
            OI.Units_per_Pallet AS POItemUnitsPerPallet ,
            OI.Cost AS POItemCost ,
            OI.UnitCost AS POItemUnitCost ,
            OI.UnitExtCost AS POItemUnitCostExt ,
            OI.CostUnit AS POItemCostUnitId ,
            CU.Unit_Name AS POItemCostUnit ,
            OI.QuantityDiscount AS POItemQuantityDiscount ,
            CASE OI.DiscountType
              WHEN 0 THEN 'No Discount'
              WHEN 1 THEN 'Cash Discount'
              WHEN 2 THEN 'Percent Discount'
              ELSE 'Free Items'
            END AS POItemDiscountType ,
            OI.AdjustedCost AS POItemAdjustedCost ,
            OI.Handling AS POItemHandling ,
            OI.HandlingUnit AS POItemHandlingUnitId ,
            HU.Unit_Name AS POItemHandlingUnit ,
            OI.Freight AS POItemFreight ,
            OI.FreightUnit AS POItemFreightUnitId ,
            FU.Unit_Name AS POItemFreightUnit ,
            OI.DateReceived AS POItemReceivedDate ,
            OI.OriginalDateReceived AS POItemOriginalReceiveDate ,
            OI.Comments AS POItemComments ,
            OI.LineItemCost AS POItemLineItemCost ,
            OI.LineItemHandling AS POItemLineItemHandling ,
            OI.LineItemFreight AS POItemLineItemFreight ,
            OI.ReceivedItemCost AS POItemReceivedItemCost ,
            OI.ReceivedItemHandling AS POItemReceivedItemHandling ,
            OI.ReceivedItemFreight AS POItemReceivedItemFreight ,
            OI.LandedCost AS POItemLandedCost ,
            OI.MarkupPercent AS POItemMarkupPercent ,
            OI.MarkupCost AS POItemMarkupCost ,
            OI.Package_Desc1 AS POItemVendorPack ,
            OI.Package_Desc2 AS POItemRetailPack ,
            OI.Package_Unit_ID AS POItemPackageUnitId ,
            PU.Unit_Name AS POItemPackageUnit ,
            OI.Retail_Unit_ID AS POItemRetailUnitId ,
            RU.Unit_Name AS POItemRetailUnit ,
            OI.Origin_ID AS POItemOriginId ,
            IOrg.Origin_Name AS POItemOrigin ,
            OI.ReceivedFreight AS POItemReceivedFreight ,
            OI.UnitsReceived AS POItemUnitsReceived ,
            OI.CreditReason_ID AS POItemCreditReasonID ,
            CR.CreditReason AS POItemCreditReason ,
            OI.QuantityAllocated AS POItemQuantityAllocated ,
            OI.CountryProc_ID AS POItemCountryProcId ,
            CP.Origin_Name AS POItemCountryProc ,
            OI.Lot_No AS POItemLotNo ,
            OI.NetVendorItemDiscount AS POItemNetVendorItemDiscount ,
            OI.CostAdjustmentReason_ID AS POItemCostAdjustmentReasonId ,
            CAR.Description AS POItemCostAdjustmentReason ,
            OI.Freight3Party AS POItemFreight3Party ,
            OI.LineItemFreight3Party AS POItemLineItemFreight3Party ,
            OI.HandlingCharge AS POItemHandlingCharge ,
            OI.eInvoiceQuantity AS POItemeInvoiceQuantity ,
            OI.SACCost AS POItemSACCost ,
            OI.OrderItemCOOL AS POItemIsCOOL ,
            OI.OrderItemBIO AS POItemIsBIO ,
            OI.Carrier AS POItemCarrier
    FROM    OrderHeader OH
            LEFT JOIN OrderItem OI ON OI.OrderHeader_ID = OH.OrderHeader_ID
            LEFT JOIN ItemIdentifier II ON II.Item_Key = OI.Item_Key
            LEFT JOIN Vendor V ON V.Vendor_ID = OH.Vendor_ID
            LEFT JOIN Vendor PLOC ON PLOC.Vendor_ID = OH.PurchaseLocation_ID
            LEFT JOIN Vendor RLOC ON RLOC.Vendor_ID = OH.ReceiveLocation_ID
            LEFT JOIN Users UCB ON UCB.User_ID = OH.CreatedBy
            LEFT JOIN Users UAB ON UAB.User_ID = OH.ApprovedBy
            LEFT JOIN Users UAI ON UAI.User_ID = OH.Accounting_In_UserId
            LEFT JOIN Users UMU ON UMU.User_ID = OH.MatchingUser_ID
            LEFT JOIN Users UCB2 ON UCB2.User_ID = OH.ClosedBy
            LEFT JOIN SubTeam TranST ON TranST.SubTeam_No = OH.Transfer_SubTeam
            LEFT JOIN SubTeam TranToST ON TranToST.SubTeam_No = OH.Transfer_To_SubTeam
            LEFT JOIN ItemUnit QU ON QU.Unit_ID = OI.QuantityUnit
            LEFT JOIN ItemUnit CU ON CU.Unit_ID = OI.CostUnit
            LEFT JOIN ItemUnit HU ON HU.Unit_ID = OI.HandlingUnit
            LEFT JOIN ItemUnit FU ON FU.Unit_ID = OI.FreightUnit
            LEFT JOIN ItemUnit PU ON PU.Unit_ID = OI.Package_Unit_ID
            LEFT JOIN ItemUnit RU ON RU.Unit_ID = OI.Retail_Unit_ID
            LEFT JOIN ItemOrigin IOrg ON IOrg.Origin_ID = OI.Origin_ID
            LEFT JOIN ItemOrigin CP ON CP.Origin_ID = OI.CountryProc_ID
            LEFT JOIN CreditReasons CR ON CR.CreditReason_ID = OI.CreditReason_ID
            LEFT JOIN CostAdjustmentReason CAR ON CAR.CostAdjustmentReason_ID = OI.CostAdjustmentReason_ID
    WHERE   II.Default_Identifier = 1
GO


