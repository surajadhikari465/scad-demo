CREATE TABLE [dbo].[VendorRequest_Status] (
    [VendorStatus_ID]    SMALLINT     IDENTITY (1, 1) NOT NULL,
    [VendorStatus_Level] VARCHAR (50) NULL,
    CONSTRAINT [PK_VendorRequest_Status] PRIMARY KEY CLUSTERED ([VendorStatus_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorRequest_Status] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[VendorRequest_Status] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[VendorRequest_Status] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorRequest_Status] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[VendorRequest_Status] TO [IRMASLIMRole]
    AS [dbo];

