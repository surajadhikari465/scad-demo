CREATE TABLE [dbo].[FSOrganization] (
    [Organization_ID]    INT           IDENTITY (1, 1) NOT NULL,
    [OrganizationName]   VARCHAR (50)  NOT NULL,
    [Address_Line_1]     VARCHAR (50)  NULL,
    [Address_Line_2]     VARCHAR (50)  NULL,
    [City]               VARCHAR (30)  NULL,
    [State]              VARCHAR (2)   NULL,
    [Zip_Code]           VARCHAR (10)  NULL,
    [Phone]              VARCHAR (20)  NULL,
    [Phone_Ext]          VARCHAR (5)   NULL,
    [Fax]                VARCHAR (20)  NULL,
    [Contact]            VARCHAR (45)  NULL,
    [Email_Address]      VARCHAR (50)  NULL,
    [Comment]            VARCHAR (255) NULL,
    [ActiveOrganization] BIT           CONSTRAINT [DF__FSOrganiz__Activ__6FAA73D4] DEFAULT ((0)) NOT NULL,
    [User_ID]            INT           NULL,
    CONSTRAINT [PK_FSOrganization_Organization_ID] PRIMARY KEY CLUSTERED ([Organization_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__FSOrganiz__User___709E980D] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
CREATE NONCLUSTERED INDEX [idxOrganizationName]
    ON [dbo].[FSOrganization]([OrganizationName] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxOrganizationUserID]
    ON [dbo].[FSOrganization]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[FSOrganization] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[FSOrganization] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[FSOrganization] TO [IRMAReportsRole]
    AS [dbo];

