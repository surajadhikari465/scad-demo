CREATE TABLE [dbo].[FSCustomer] (
    [Customer_ID]     INT           IDENTITY (1, 1) NOT NULL,
    [Organization_ID] INT           NOT NULL,
    [Customer_Code]   INT           NOT NULL,
    [CustomerName]    VARCHAR (50)  NOT NULL,
    [Address_Line_1]  VARCHAR (50)  NULL,
    [Address_Line_2]  VARCHAR (50)  NULL,
    [City]            VARCHAR (30)  NULL,
    [State]           VARCHAR (2)   NULL,
    [Zip_Code]        VARCHAR (10)  NULL,
    [Phone]           VARCHAR (20)  NULL,
    [Phone_Ext]       VARCHAR (5)   NULL,
    [Fax]             VARCHAR (20)  NULL,
    [Email_Address]   VARCHAR (50)  NULL,
    [Birthday]        VARCHAR (20)  NULL,
    [Comment]         VARCHAR (255) NULL,
    [ActiveCustomer]  BIT           CONSTRAINT [DF__FSCustome__Activ__03B16C81] DEFAULT ((0)) NOT NULL,
    [User_ID]         INT           NULL,
    CONSTRAINT [PK_FSCustomer_Customer_ID] PRIMARY KEY CLUSTERED ([Customer_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__FSCustome__User___04A590BA] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK_FSOrganization_Organization_ID] FOREIGN KEY ([Organization_ID]) REFERENCES [dbo].[FSOrganization] ([Organization_ID])
);


GO
CREATE NONCLUSTERED INDEX [idxCustomerName]
    ON [dbo].[FSCustomer]([CustomerName] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxOrganizationUserID]
    ON [dbo].[FSCustomer]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxOrganizationID]
    ON [dbo].[FSCustomer]([Organization_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxCustomerCode]
    ON [dbo].[FSCustomer]([Customer_Code] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[FSCustomer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[FSCustomer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[FSCustomer] TO [IRMAReportsRole]
    AS [dbo];

