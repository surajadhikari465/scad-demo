CREATE TYPE [dbo].[ScanCodeBusinessUnitIdType] AS TABLE
(
	ScanCode NVARCHAR(13) NOT NULL,
	BusinessUnitID INT NOT NULL
)
GO

GRANT EXEC ON TYPE::[dbo].[ScanCodeBusinessUnitIdType] TO [warp-role]
GO