CREATE TABLE [dbo].[ItemCustomerFacingScale] (
    [Item_Key]                      INT NOT NULL,
    [CustomerFacingScaleDepartment] BIT NOT NULL,
    [SendToScale]                   BIT NOT NULL,
    CONSTRAINT [PK_ItemCustomerFacingScale] PRIMARY KEY CLUSTERED ([Item_Key] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_ItemCustomerFacingScale_SendToScale]
    ON [dbo].[ItemCustomerFacingScale]([SendToScale] ASC) WITH (FILLFACTOR = 80);


GO
GRANT ALTER
    ON OBJECT::[dbo].[ItemCustomerFacingScale] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemCustomerFacingScale] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemCustomerFacingScale] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemCustomerFacingScale] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemCustomerFacingScale] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[ItemCustomerFacingScale] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemCustomerFacingScale] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemCustomerFacingScale] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemCustomerFacingScale] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemCustomerFacingScale] TO [IRSUser]
    AS [dbo];

