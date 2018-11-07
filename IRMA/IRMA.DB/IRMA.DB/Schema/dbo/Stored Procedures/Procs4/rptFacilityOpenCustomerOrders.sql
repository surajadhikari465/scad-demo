CREATE PROCEDURE [dbo].[rptFacilityOpenCustomerOrders]
	(
		 @Facility_ID INT
		,@Expected_Date SMALLDATETIME
		,@Store INT
		,@FromSubTeam INT
		,@ToSubTeam INT
	)
AS
   -- **************************************************************************
   -- Procedure: rptFacilityOpenCustomerOrders()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date        Init	TFS		Comment
   -- 12/13/2010  BBB	13334	removed deprecated and unused code
   -- **************************************************************************
   
	DECLARE @Case INT
		SELECT @Case = Unit_ID FROM ItemUnit WHERE EDISysCode = 'CA'

SELECT	 CompanyName
		,OH.OrderHeader_ID
		,TF.SubTeam_Name AS FromSubTeam
		,TT.SubTeam_Name AS ToSubTeam
		,CASE WHEN OH.SentDate IS NULL THEN 'Not Sent' ELSE 'Sent' END AS OrderStatus
		,SUM(dbo.fn_CostConversion ( QuantityOrdered
									,@Case
									,QuantityUnit
									,OI.Package_Desc1
									,OI.Package_Desc2
									,OI.Package_Unit_ID )) AS Cases
		,FullName
FROM OrderHeader OH (NOLOCK)
INNER JOIN
	OrderItem OI (NOLOCK)
	ON OI.OrderHeader_ID = OH.OrderHeader_ID
INNER JOIN
	Item I (NOLOCK)
	ON I.Item_Key = OI.Item_Key
INNER JOIN
	Vendor V (NOLOCK)
	ON V.Vendor_ID = OH.ReceiveLocation_ID
INNER JOIN
	SubTeam TF (NOLOCK)
	ON TF.SubTeam_No = OH.Transfer_SubTeam
INNER JOIN
	SubTeam TT (NOLOCK)
	ON TT.SubTeam_No = OH.Transfer_To_SubTeam
INNER JOIN
	Users U (NOLOCK)
	ON U.User_ID = OH.CreatedBy
WHERE OH.Vendor_ID = @Facility_ID
AND CONVERT(VARCHAR(10),OH.Expected_Date,101) = @Expected_Date
AND (OH.ReceiveLocation_ID = @Store OR @Store IS NULL)
AND (OH.Transfer_SubTeam = @FromSubTeam OR @FromSubTeam IS NULL)
AND (OH.Transfer_To_SubTeam = @ToSubTeam OR @ToSubTeam IS NULL)
AND OH.CloseDate IS NULL
GROUP BY CompanyName, OH.OrderHeader_ID, TF.SubTeam_Name, TT.SubTeam_Name, OH.SentDate, FullName
ORDER BY CompanyName, OH.OrderHeader_ID, TF.SubTeam_Name, TT.SubTeam_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rptFacilityOpenCustomerOrders] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rptFacilityOpenCustomerOrders] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rptFacilityOpenCustomerOrders] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rptFacilityOpenCustomerOrders] TO [IRMAReportsRole]
    AS [dbo];

