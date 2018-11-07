CREATE PROCEDURE [dbo].[GetTotalRefused]
	@OrderHeader_ID int,
	@TotalRefusedAmount money output 
AS
-- **************************************************************************************************************************
-- Procedure: GetTotalRefused()
--    Author: Faisal Ahmed
--      Date: 03/11/2013
--
-- Description:
-- This procedure is calculates total refused amount of a PO
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/11/2013	FA   	8325	Initial code
-- **************************************************************************************************************************
BEGIN
	DECLARE @TotalRefused money

	-- temp table for storing refused quantity and cost
	CREATE TABLE #tmpRefusedItems
	(
		InvoiceCost			decimal(18,4),
		RefusedQuantity		decimal(18,4)		
	)
	
	-- stores data from paper invoices
	INSERT INTO #tmpRefusedItems
	SELECT
		ISNULL(oir.InvoiceCost, 0) as InvoiceCost,
		ISNULL(oir.RefusedQuantity, 0) as RefusedQuantity
	FROM
		OrderItemRefused oir (nolock)
	WHERE oir.OrderHeader_ID = @OrderHeader_ID
	
	-- calculates refused total amount
	SELECT 
		@TotalRefused = SUM(ISNULL(t.InvoiceCost, 0) * ISNULL(t.RefusedQuantity, 0))
	FROM
		#tmpRefusedItems t (nolock)
	
	DROP TABLE #tmpRefusedItems
	
	UPDATE OrderHeader
	SET TotalRefused = ISNULL(@TotalRefused, 0)
	WHERE OrderHeader_ID = @OrderHeader_ID

	SELECT @TotalRefusedAmount = ISNULL(@TotalRefused, 0)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTotalRefused] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTotalRefused] TO [IRMAReportsRole]
    AS [dbo];

