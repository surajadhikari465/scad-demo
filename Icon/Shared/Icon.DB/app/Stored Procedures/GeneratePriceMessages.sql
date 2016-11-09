
-- =============================================
-- Author:		Blake Jones
-- Create date: 2014-09-10
-- Description:	Called from the POS Push Controller.
--				Used to bulk insert a set of
--				Price messages.
-- =============================================

CREATE PROCEDURE [app].[GeneratePriceMessages]
	@PriceMessages app.PriceMessageType readonly
AS
BEGIN

	set nocount on;

	insert into app.MessageQueuePrice select * from @PriceMessages

END