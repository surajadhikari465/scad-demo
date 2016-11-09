
-- =============================================
-- Author:		Blake Jones
-- Create date: 2014-09-10
-- Description:	Called from the POS Push Controller.
--				Used to bulk insert a set of
--				ItemLocale messages.
-- =============================================

CREATE PROCEDURE [app].[GenerateItemLocaleMessages]
	@ItemLocaleMessages app.ItemLocaleMessageType readonly
AS
BEGIN	

	set nocount on;

	insert into app.MessageQueueItemLocale select * from @ItemLocaleMessages

END