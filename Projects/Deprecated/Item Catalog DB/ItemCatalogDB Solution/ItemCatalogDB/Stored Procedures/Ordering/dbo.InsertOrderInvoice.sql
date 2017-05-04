SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertOrderInvoice]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertOrderInvoice]
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO