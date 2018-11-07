CREATE PROCEDURE dbo.UpdateOrderCancelSend  
    @OrderHeader_ID int
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/24/2011	Tom Lux			759		Added reset of POCostDate (related to lead-time), since it's based on Sent Date.
*/
BEGIN
    SET NOCOUNT ON
    
    UPDATE OrderHeader
    SET Sent = 0, 
        SentToFaxDate = NULL, 
        SentToEmailDate = NULL,
        SentToElectronicDate = NULL,
        SentDate = NULL,
        User_ID = NULL,
        Fax_Order = 0,
        Email_Order = 0,
        Electronic_Order = 0,
        OverrideTransmissionMethod = 0, --Robert Shurbet TFS8316 20090216 Pull in for email po functionality  
		POCostDate = NULL -- Based on SentDate, so needs to be cleared.
    WHERE OrderHeader_ID = @OrderHeader_ID
   
    /* Robert Shurbet TFS8316 20090216 - Delete any override rows from the override transmission table */
	IF EXISTS (SELECT OrderHeader_ID FROM OrderTransmissionOverride WHERE OrderHeader_ID = @OrderHeader_ID)
			DELETE FROM OrderTransmissionOverride WHERE OrderHeader_ID = @OrderHeader_ID
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderCancelSend] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderCancelSend] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderCancelSend] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderCancelSend] TO [IRMAReportsRole]
    AS [dbo];

