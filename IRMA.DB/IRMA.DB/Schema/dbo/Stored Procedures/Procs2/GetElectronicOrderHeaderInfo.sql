CREATE PROCEDURE dbo.GetElectronicOrderHeaderInfo
    @OrderHeader_ID int

AS

   -- **************************************************************************
   -- Procedure: GetElectronicOrderHeaderInfo()
   --    Author: Ron Savage
   --      Date: 2010ish
   --
   -- Description:
   -- This procedure gets the order header data used to form the XML message sent to the DVO
   -- web service to send an IRMA order electronically through DVO.
   --
   -- Modification History:
   -- Date			Init	Comment
   -- 07/18/2013	KM		Include oh.InvoiceNumber in the result set;
   -- 10/08/2012	RDE		Add Return_Order for POFlip Credit Suport
   -- 03/28/2012	RS		Set join to OrderExportExternalSource to a LEFT OUTER JOIN so orders from
   --						IRMA can also be processed.
   -- 03/16/2012	RS		Added case statement around SubTeam_No (POS_Dept) to match the logic
   --						around the PeopleSoft sub-team number to keep them consistent.  Also
   --						added two new fields, the External System description and External
   --						System order number so DVO can identify electronic orders from IRMA
   --						that originated in DVO and not create a new order (TFS 4673).
   -- 07/10/2009	DS		Added Case Statement to handle DVO Output of Type 1 Orders w/Product Type 3 (supply) - sending new column to DVO instead of NULL
   -- ***********************************************************************************************************************************************************************************************************************

BEGIN
    SET NOCOUNT ON

    SELECT
       OH.OrderHeader_ID,
       U.Email,
       V.PS_Vendor_ID,
       CONVERT(VARCHAR(10), OH.Expected_Date, 101) AS Expected_Date,
       OH.OrderHeaderDesc,
       S.BusinessUnit_ID,

       CASE WHEN (OH.Electronic_Order = 1 and OH.OrderType_ID = 1 and OH.ProductType_ID = 3)
          THEN ST2.PS_SubTeam_No
          ELSE ST.PS_SubTeam_No
       END AS SubTeam_No,

       CASE WHEN (OH.Electronic_Order = 1 and OH.OrderType_ID = 1 and OH.ProductType_ID = 3)
          THEN ST2.SubTeam_No
          ELSE ST.SubTeam_No
       END AS POS_Dept,

       OH.IsDropShipment AS DropShip,
       oes.Description,
       oh.OrderExternalSourceOrderId,
	   oh.Return_Order AS isCredit,
	   oh.InvoiceNumber AS InvoiceNumber
    
	FROM
       OrderHeader OH

       JOIN Users U
          ON U.User_ID = OH.CreatedBy

       JOIN Vendor V
          ON V.Vendor_ID = OH.Vendor_ID

       JOIN Vendor RecVend
          ON RecVend.Vendor_ID = OH.ReceiveLocation_ID

       JOIN Store S
          ON S.Store_No = RecVend.Store_No

       JOIN StoreSubTeam ST
          ON ST.SubTeam_No = OH.Transfer_To_SubTeam
             AND ST.Store_No = S.Store_No

       LEFT OUTER JOIN OrderExternalSource oes
          on oes.id = oh.OrderExternalSourceId

       LEFT JOIN StoreSubTeam ST2
          ON ST2.SubTeam_No = OH.SupplyTransferToSubTeam
             AND ST2.Store_No = S.Store_No
    
	WHERE
       OH.OrderHeader_ID = @OrderHeader_ID
	   
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetElectronicOrderHeaderInfo] TO [IRMAClientRole]
    AS [dbo];

