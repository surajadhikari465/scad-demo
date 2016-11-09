CREATE TYPE [dbo].[IconItemWithSubteamType] AS TABLE (
    [ItemId]             INT            NOT NULL,
    [ValidationDate]     NVARCHAR (255) NULL,
    [ScanCode]           NVARCHAR (13)  NOT NULL,
    [ScanCodeType]       NVARCHAR (255) NULL,
    [ProductDescription] NVARCHAR (255) NULL,
    [PosDescription]     NVARCHAR (255) NULL,
    [PackageUnit]        NVARCHAR (255) NULL,
    [FoodStampEligible]  NVARCHAR (255) NULL,
    [Tare]               NVARCHAR (255) NULL,
    [BrandId]            INT            NULL,
    [BrandName]          NVARCHAR (35)  NULL,
    [TaxClassName]       NVARCHAR (50)  NULL,
    [SubTeamName]        NVARCHAR (255) NULL,
    [SubTeamNo]          INT            NOT NULL,
    [DeptNo]             INT            NOT NULL,
    [SubTeamNotAligned]  BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([ItemId] ASC));


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IconItemWithSubteamType] TO [IConInterface];

