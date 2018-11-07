CREATE VIEW [dbo].[DC_EXE_PROCESS_STAGING]
AS
SELECT     Dist_Center, WareHouse, Product_ID, ProductDetail, Inv_Status, Tot_BOH, Tot_Flowthru_BOH, Tot_Rcv_Qty, Unextract_Rcv_Qty, Tot_To_Ship_Qty, 
                      Tot_Rush_To_Ship_Ord_Qty, Tot_UnExtract_Ship_Ord_Qty, Markout_Qty, Tot_UnExtract_Adj_Qty, Tot_In_Trans_BOH, Tot_Buyer_Resrv_BOH, DateCreated, 
                      TimeCreated, UnitShipCase
FROM         dbo.Warehouse_Inventory
GO
GRANT SELECT
    ON OBJECT::[dbo].[DC_EXE_PROCESS_STAGING] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[DC_EXE_PROCESS_STAGING] TO [IRMADCAnalysisRole]
    AS [dbo];

