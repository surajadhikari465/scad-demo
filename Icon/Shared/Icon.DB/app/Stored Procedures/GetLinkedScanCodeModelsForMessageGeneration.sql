-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-12-11
-- Description:	Accepts a table of scan codes to business
--				units and returns the linked scan code
--				for any given scan code by business unit.
-- =============================================

CREATE PROCEDURE [app].[GetLinkedScanCodeModelsForMessageGeneration]
	@ScanCodesByBusinessUnit app.ScanCodesByBusinessUnitType readonly
AS
BEGIN
	set nocount on

	declare @BusinessUnitTraitId int = (select traitID from Trait where traitCode = 'BU')
	
    select
		scbu.ScanCode as ScanCode,
		cast(scbu.BusinessUnitId as int) as BusinessUnitId,
		linkParent.scanCode as LinkedScanCode
	from
		@ScanCodesByBusinessUnit scbu
		join ScanCode linkChild on scbu.ScanCode = linkChild.scanCode
		join LocaleTrait lt on scbu.BusinessUnitId = lt.traitValue and lt.traitID = @BusinessUnitTraitId
		join ItemLink il on linkChild.itemID = il.childItemID and lt.localeID = il.localeID
		join ScanCode linkParent on il.parentItemID = linkParent.itemID
END
