CREATE TYPE gpm.BusinessUnitIdsType AS TABLE
(
	BusinessUnitId INT
)
GO

GRANT EXEC ON TYPE::[gpm].[BusinessUnitIdsType] TO [MammothRole]
GO