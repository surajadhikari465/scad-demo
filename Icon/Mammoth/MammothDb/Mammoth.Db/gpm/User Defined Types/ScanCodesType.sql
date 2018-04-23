CREATE TYPE gpm.ScanCodesType AS TABLE
(
	ScanCode nvarchar(13)
)
GO

GRANT EXEC ON TYPE::[gpm].[ScanCodesType] TO [MammothRole]
GO