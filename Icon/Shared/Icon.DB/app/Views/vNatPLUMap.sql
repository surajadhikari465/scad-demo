CREATE VIEW [app].[vNatPLUMap]
AS

select
	sc.scanCode
	,pm.*
from
	ScanCode sc
	join PLUMap pm
		on sc.itemID = pm.itemID