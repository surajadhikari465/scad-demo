SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[InsertOrderItemRefused]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[InsertOrderItemRefused]
GO

CREATE PROCEDURE [dbo].[InsertOrderItemRefused]
	@OrderHeader_ID int,
	@OrderItem_ID int,
	@Identifier varchar(13),
	@VendorItemNumber varchar (255),
	@Description varchar (60),
	@Unit varchar (25),
	@InvoiceQuantity decimal(18,4),
	@InvoiceCost money,
	@RefusedQuantity decimal(18,4),
	@DiscrepancyCodeID int,
	@UserAddedEntry bit,
	@eInvoiceId integer

AS 

-- **************************************************************************
-- Procedure: InsertOrderItemRefused()
--    Author: Faisal Ahmed
--      Date: 03/04/2013
--
-- Description:
-- This procedure inserts a record into OrderItemRefused table
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/04/2013	FA   	8325	Initial Code
-- **************************************************************************

BEGIN

	INSERT INTO OrderItemRefused
		(OrderHeader_ID, OrderItem_ID, Identifier, VendorItemNumber, [Description], Unit, InvoiceQuantity, InvoiceCost, RefusedQuantity, DiscrepancyCodeID, UserAddedEntry, eInvoice_Id) 
		VALUES (@OrderHeader_ID, @OrderItem_ID, @Identifier, @VendorItemNumber, @Description, @Unit, @InvoiceQuantity, @InvoiceCost, @RefusedQuantity, @DiscrepancyCodeID, @UserAddedEntry, @eInvoiceId)
END
GO