CREATE TABLE [dbo].[SlimAccess] (
    [SlimAccess_ID]  INT           IDENTITY (1, 1) NOT NULL,
    [User_ID]        INT           NOT NULL,
    [UserAdmin]      BIT           NULL,
    [ItemRequest]    BIT           NULL,
    [VendorRequest]  BIT           NULL,
    [IRMAPush]       BIT           NULL,
    [StoreSpecials]  BIT           NULL,
    [RetailCost]     BIT           NULL,
    [Authorizations] BIT           NULL,
    [WebQuery]       BIT           NULL,
    [Insert_Date]    SMALLDATETIME DEFAULT (getdate()) NULL,
    [ScaleInfo]      BIT           DEFAULT ((0)) NOT NULL,
    [ECommerce]      BIT           NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[SlimAccess] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SlimAccess] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SlimAccess] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[SlimAccess] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SlimAccess] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SlimAccess] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SlimAccess] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SlimAccess] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SlimAccess] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[SlimAccess] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SlimAccess] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SlimAccess] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SlimAccess] TO [IRMASLIMRole]
    AS [dbo];

