
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-10-04
-- Description:	Called from the POS Push Controller.
--				Takes a collection of IRMAPush entities
--				and inserts them to app.IRMAPush.
-- =============================================

CREATE PROCEDURE [app].[StageIrmaPushData]
	@IrmaPushData app.IrmaPushType readonly
AS
BEGIN

	set nocount on;

	insert into app.IRMAPush select * from @IrmaPushData

END
GO
