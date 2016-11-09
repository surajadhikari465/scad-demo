CREATE TABLE [dbo].[ValidatedBrand] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [IrmaBrandId] INT NOT NULL,
    [IconBrandId] INT NOT NULL,
    CONSTRAINT [PK_ValidatedBrandId] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ItemBrand_Brand_ID] FOREIGN KEY ([IrmaBrandId]) REFERENCES [dbo].[ItemBrand] ([Brand_ID]),
    CONSTRAINT [AK_ValidatedBrand_IconBrandId] UNIQUE NONCLUSTERED ([IconBrandId] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT ALTER
    ON OBJECT::[dbo].[ValidatedBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ValidatedBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ValidatedBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidatedBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ValidatedBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[ValidatedBrand] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ValidatedBrand] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ValidatedBrand] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidatedBrand] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ValidatedBrand] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ValidatedBrand] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ValidatedBrand] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidatedBrand] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ValidatedBrand] TO [IConInterface]
    AS [dbo];

