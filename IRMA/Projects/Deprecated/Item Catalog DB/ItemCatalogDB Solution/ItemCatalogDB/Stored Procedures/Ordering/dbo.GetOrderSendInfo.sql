IF EXISTS (
       SELECT *
       FROM   dbo.sysobjects
       WHERE  id = OBJECT_ID(N'[dbo].[GetOrderSendInfo]')
              AND OBJECTPROPERTY(id, N'IsProcedure') = 1
   )
    DROP PROCEDURE [dbo].[GetOrderSendInfo]
GO


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

