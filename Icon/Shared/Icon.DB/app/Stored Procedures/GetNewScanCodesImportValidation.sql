
-- =============================================
-- Author:		Tom Lux
-- Create date: February 104
-- Description:	Takes a list of scan codes and
--				returns the ones that don't already
--				exist in the ScanCode table.
-- =============================================

CREATE PROCEDURE [app].[GetNewScanCodesImportValidation]
	@ScanCodes app.ScanCodeListType READONLY
AS
BEGIN
	select
		scs.ScanCode
	from
		@ScanCodes scs
		left join ScanCode sc
			on scs.ScanCode = sc.scanCode
	where
		sc.scanCode is null
END
GO
