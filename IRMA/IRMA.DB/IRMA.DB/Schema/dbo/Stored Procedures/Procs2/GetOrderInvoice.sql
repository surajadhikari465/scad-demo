CREATE PROCEDURE dbo.GetOrderInvoice 
	@OrderHeader_ID int 

AS 
-- **************************************************************************
-- Procedure: GetOrderInvoice
--    Author: n/a
--      Date: n/a
--
-- Description: Called from OrderStatus.vb
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/14	KM		3744	Added update history template; replaced SUM(oi.ReceivedItemCost) with oh.AdjustedReceivedCost; coding standards
-- 2011/12/16	KM		3744	Added oh.AdjustedReceivedCost to Group By clause to fix build error
-- **************************************************************************
BEGIN
	SELECT 
		st.SubTeam_Name,
		st.SubTeam_No, 
		DiscountType,
		QuantityDiscount, 
		InvoiceCost			= ISNULL(T2.InvoiceCost, 0), 
		InvoiceFreight		= ISNULL(T2.InvoiceFreight, 0), 
		OrderCost			= ISNULL(T1.OrderCost, 0),
		OrderFreight		= ISNULL(T1.OrderFreight, 0)
    
	FROM 
		SubTeam			(nolock) st
		INNER JOIN (
						(SELECT
							SubTeam_No,
							InvoiceCost,
							InvoiceFreight 
						
						FROM
							OrderInvoice 
                 
						WHERE
							OrderInvoice.OrderHeader_ID = @OrderHeader_ID
						)		T2
                 
								FULL OUTER JOIN
												(SELECT 
													SubTeam_No		= oh.Transfer_To_SubTeam, 
													OrderCost		= oh.AdjustedReceivedCost,
													OrderFreight	= SUM(oi.ReceivedItemFreight),
													oh.DiscountType,
													oh.QuantityDiscount
						                        
												FROM
													OrderHeader				(nolock) oh
													INNER JOIN OrderItem	(nolock) oi on oh.OrderHeader_ID = oi.OrderHeader_ID
						                                 
												WHERE oi.OrderHeader_ID		= @OrderHeader_ID
																			AND oh.Transfer_To_SubTeam IS NOT NULL
																			
												GROUP BY
													oh.Transfer_To_SubTeam,
													oh.DiscountType,
													oh.QuantityDiscount,
													oh.AdjustedReceivedCost
												) T1 on T2.SubTeam_No = T1.SubTeam_No
					
					) on ISNULL(T2.SubTeam_No, T1.SubTeam_No) = st.SubTeam_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInvoice] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInvoice] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInvoice] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInvoice] TO [IRMAReportsRole]
    AS [dbo];

