CREATE TABLE [dbo].[VendorRequest] (
    [VendorRequest_ID]    INT           IDENTITY (1, 1) NOT NULL,
    [VendorStatus_ID]     SMALLINT      NULL,
    [Vendor_Key]          VARCHAR (10)  NULL,
    [CompanyName]         VARCHAR (50)  NOT NULL,
    [City]                VARCHAR (30)  NULL,
    [State]               VARCHAR (3)   NULL,
    [ZipCode]             VARCHAR (10)  NULL,
    [Phone]               VARCHAR (20)  NULL,
    [User_Store]          INT           NULL,
    [UserAccessLevel_ID]  SMALLINT      NULL,
    [User_ID]             INT           NULL,
    [PS_Vendor_ID]        VARCHAR (10)  NULL,
    [InsuranceNumber]     NCHAR (40)    NULL,
    [Email]               VARCHAR (50)  NULL,
    [Insert_Date]         DATETIME      DEFAULT (getdate()) NULL,
    [IRMA_Add_Date]       DATETIME      NULL,
    [Ready_To_Apply]      BIT           NULL,
    [Address_Line_1]      VARCHAR (50)  NULL,
    [Address_Line_2]      VARCHAR (50)  NULL,
    [Comment]             VARCHAR (255) NULL,
    [Fax]                 VARCHAR (20)  NULL,
    [PS_Export_Vendor_ID] VARCHAR (10)  NULL,
    CONSTRAINT [PK_VendorRequest] PRIMARY KEY CLUSTERED ([VendorRequest_ID] ASC),
    CONSTRAINT [FK_VendorRequest_UserAccess] FOREIGN KEY ([UserAccessLevel_ID]) REFERENCES [dbo].[UserAccess] ([UserAccessLevel_ID]),
    CONSTRAINT [FK_VendorRequest_VendorRequestStatus] FOREIGN KEY ([VendorStatus_ID]) REFERENCES [dbo].[VendorRequest_Status] ([VendorStatus_ID])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[VendorRequest] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[VendorRequest] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorRequest] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[VendorRequest] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[VendorRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[VendorRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[VendorRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorRequest] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[VendorRequest] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[VendorRequest] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorRequest] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[VendorRequest] TO [IRMASLIMRole]
    AS [dbo];

