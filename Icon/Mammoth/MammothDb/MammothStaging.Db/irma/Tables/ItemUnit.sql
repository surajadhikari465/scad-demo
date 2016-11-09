CREATE TABLE [irma].[ItemUnit] (
    [Region]              NCHAR (2)    NOT NULL,
    [Unit_ID]             INT          NOT NULL,
    [Unit_Name]           VARCHAR (25) NOT NULL,
    [Weight_Unit]         BIT          CONSTRAINT [DF__ItemUnit__Weight__430CD787] DEFAULT ((0)) NOT NULL,
    [User_ID]             INT          NULL,
    [Unit_Abbreviation]   VARCHAR (5)  NULL,
    [UnitSysCode]         VARCHAR (5)  NULL,
    [IsPackageUnit]       BIT          CONSTRAINT [DF_ItemUnit_IsPackageUnit] DEFAULT ((0)) NOT NULL,
    [PlumUnitAbbr]        VARCHAR (5)  NULL,
    [EDISysCode]          CHAR (2)     NULL,
    [LastUpdateTimestamp] DATETIME     NULL,
    CONSTRAINT [PK_ItemUnit_Unit_ID] PRIMARY KEY CLUSTERED ([Region] ASC, [Unit_ID] ASC) WITH (FILLFACTOR = 100)
);



