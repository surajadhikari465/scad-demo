CREATE TABLE [dbo].[LoadRBX] (
    [Dept]        CHAR (4)     NOT NULL,
    [upcno]       VARCHAR (12) NOT NULL,
    [Description] VARCHAR (30) NULL,
    [PackSize]    VARCHAR (13) NULL,
    [UnitAbbr]    VARCHAR (2)  NULL,
    [Override]    CHAR (1)     NOT NULL,
    [TaxFlag1]    CHAR (1)     NOT NULL,
    [TaxFlag2]    CHAR (1)     NOT NULL,
    [TaxFlag3]    CHAR (1)     NOT NULL,
    [TaxFlag4]    CHAR (1)     NOT NULL,
    [Store_No]    INT          NOT NULL
);

