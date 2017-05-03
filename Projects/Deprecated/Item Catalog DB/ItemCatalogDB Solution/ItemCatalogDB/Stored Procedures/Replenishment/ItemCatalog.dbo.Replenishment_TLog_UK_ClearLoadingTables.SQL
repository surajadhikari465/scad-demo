IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Replenishment_TLog_UK_ClearLoadingTables]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Replenishment_TLog_UK_ClearLoadingTables]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.[Replenishment_TLog_UK_ClearLoadingTables]

AS

BEGIN
	delete from tlog_uk_item
	delete from tlog_uk_payment
	delete from tlog_uk_discounts
	delete from tlog_uk_offers
	delete from tlog_uk_voids
	delete from tlog_uk_transaction
END
GO