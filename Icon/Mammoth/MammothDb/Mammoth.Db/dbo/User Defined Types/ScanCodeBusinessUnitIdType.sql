CREATE TYPE [dbo].[ScanCodeBusinessUnitIdType] AS TABLE
(
	ScanCode NVARCHAR(13) NOT NULL,
	BusinessUnitID INT NOT NULL
)