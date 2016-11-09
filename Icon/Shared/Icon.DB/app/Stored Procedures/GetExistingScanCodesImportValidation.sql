
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-06-19
-- Description:	Takes a list of scan codes and
--				returns the ones that already
--				exist in the ScanCode table.
-- =============================================
CREATE PROCEDURE [app].[GetExistingScanCodesImportValidation]
	@ScanCodes app.ScanCodeListType READONLY
AS
BEGIN	
	select
		scs.ScanCode
	from
		@ScanCodes scs
		left join ScanCode sc on scs.ScanCode = sc.scanCode
	where
		sc.scanCode is not null
END
GO
