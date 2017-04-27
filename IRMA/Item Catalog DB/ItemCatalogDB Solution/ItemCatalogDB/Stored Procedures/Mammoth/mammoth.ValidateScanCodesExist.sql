-- =============================================
-- Author:		Scherping, Matthew
-- Create date: 08-FEB-16
-- Description:	Returns scan codes that don't exist.
-- =============================================
CREATE PROCEDURE [mammoth].[ValidateScanCodesExist]
	@ScanCodes dbo.IdentifiersType readonly	
AS
BEGIN
	select *
	from @ScanCodes sc
	where not exists
	(
		select 1 from dbo.ItemIdentifier ii where ii.Identifier = sc.Identifier
	)
END