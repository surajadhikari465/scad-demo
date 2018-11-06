CREATE PROCEDURE dbo.InsertOrderInvoice 
	@OrderHeader_ID		int,
	@SubTeam_No			int,
	@InvoiceCost		smallmoney
AS
-- ****************************************************************************************************************
-- Procedure: InsertOrderInvoice
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/26	KM		3744	Added update history template; extension change;
-- ****************************************************************************************************************

INSERT INTO OrderInvoice 
(
	OrderHeader_ID, 
	SubTeam_No, 
	InvoiceCost
)

VALUES 
(
	@OrderHeader_ID, 
	@SubTeam_No, 
	@InvoiceCost
)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderInvoice] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderInvoice] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderInvoice] TO [IRMAReportsRole]
    AS [dbo];

