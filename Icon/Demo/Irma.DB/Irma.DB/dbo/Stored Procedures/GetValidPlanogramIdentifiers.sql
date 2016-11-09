

-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-11-18
-- Description:	Returns identifiers that can be
--				included in the planogram print batch
--				request.
-- =============================================

CREATE PROCEDURE dbo.GetValidPlanogramIdentifiers
	@Identifiers dbo.IdentifiersType readonly,
	@StoreNumber int
AS
BEGIN
	set nocount on

    select
		input.Identifier
	from
		@Identifiers input
		join ValidatedScanCode (nolock) vsc on input.Identifier = vsc.ScanCode
		join ItemIdentifier (nolock) ii on input.Identifier = ii.Identifier
		join Item (nolock) i on ii.Item_Key = i.Item_Key
		join StoreItem (nolock) si on i.Item_Key = si.Item_Key and si.Store_No = @StoreNumber
	where
		ii.Remove_Identifier = 0 and
		ii.Deleted_Identifier = 0 and
		i.Remove_Item = 0 and
		i.Deleted_Item = 0 and
		si.Authorized = 1
END
GO