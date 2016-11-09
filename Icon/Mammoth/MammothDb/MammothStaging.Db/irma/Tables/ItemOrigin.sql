CREATE TABLE [irma].[ItemOrigin] (
    [Region]      NCHAR (2)    NOT NULL,
    [Origin_ID]   INT          NOT NULL,
    [Origin_Name] VARCHAR (25) NOT NULL,
    [User_ID]     INT          NULL,
    CONSTRAINT [PK_ItemOrigin_Origin_ID] PRIMARY KEY CLUSTERED ([Region] ASC, [Origin_ID] ASC) WITH (FILLFACTOR = 100)
);



