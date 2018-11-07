CREATE PROCEDURE dbo.[GetOrderSendInfo]
    @Vendor_ID INT
	/*

	Created by Alexa Horvath, Jan 2009
	for selecting order send info: the vendor email, fax and 

	grant exec on GetOrderSendInfo to IRMAClientRole
	grant exec on GetOrderSendInfo to IRMAReportsRole
	grant exec on GetOrderSendInfo to IRMASchedJobsRole
	grant exec on GetOrderSendInfo to IRMASupportRole

		*/

AS 

    SELECT Fax, Email
	FROM Vendor
	WHERE  Vendor_ID = @Vendor_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderSendInfo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderSendInfo] TO [IRMAClientRole]
    AS [dbo];

