CREATE TYPE [app].[CancelAllSalesType] AS TABLE
(
	ScanCode NVARCHAR(13) NOT NULL,
	BusinessUnitID INT NOT NULL,
	EndDate DATETIME2(7) NOT NULL,
	EventCreatedDate DATETIME2(7) NOT NULL
)