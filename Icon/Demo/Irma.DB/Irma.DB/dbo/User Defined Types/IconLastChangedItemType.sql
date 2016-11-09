CREATE TYPE [dbo].[IconLastChangedItemType] AS TABLE (
    [ItemId]               INT            NOT NULL,
    [ValidationDate]       NVARCHAR (255) NOT NULL,
    [ScanCode]             NVARCHAR (13)  NOT NULL,
    [ScanCodeType]         NVARCHAR (255) NOT NULL,
    [ProductDescription]   NVARCHAR (255) NOT NULL,
    [PosDescription]       NVARCHAR (255) NOT NULL,
    [PackageUnit]          NVARCHAR (255) NOT NULL,
    [FoodStampEligible]    NVARCHAR (255) NOT NULL,
    [Tare]                 NVARCHAR (255) NULL,
    [BrandId]              INT            NOT NULL,
    [BrandName]            NVARCHAR (35)  NOT NULL,
    [TaxClassName]         NVARCHAR (50)  NOT NULL,
    [NationalClassCode]    NVARCHAR (50)  NOT NULL,
    [AreNutriFactsUpdated] BIT            NULL,
    [SubTeamName]          NVARCHAR (255) NULL,
    [SubTeamNo]            INT            NOT NULL,
    [DeptNo]               INT            NOT NULL,
    [SubTeamNotAligned]    BIT            NOT NULL,
    [RetailUom]            NVARCHAR (4)   NOT NULL,
    [RetailSize]           DECIMAL (9, 4) NOT NULL,
    PRIMARY KEY CLUSTERED ([ItemId] ASC));




GO
GRANT EXECUTE
    ON TYPE::[dbo].[IconLastChangedItemType] TO [IConInterface];

