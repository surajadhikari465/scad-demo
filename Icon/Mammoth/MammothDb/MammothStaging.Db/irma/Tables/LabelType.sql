CREATE TABLE [irma].[LabelType] (
    [Region]        NCHAR (2)   NOT NULL,
    [LabelType_ID]  INT         NOT NULL,
    [LabelTypeDesc] VARCHAR (4) NULL,
    CONSTRAINT [PK_LabelType] PRIMARY KEY CLUSTERED ([Region] ASC, [LabelType_ID] ASC) WITH (FILLFACTOR = 100)
);

