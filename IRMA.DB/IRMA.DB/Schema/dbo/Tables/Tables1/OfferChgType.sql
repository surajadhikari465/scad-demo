CREATE TABLE [dbo].[OfferChgType] (
    [OfferChgTypeID]   TINYINT      NOT NULL,
    [OfferChgTypeDesc] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_OfferChgType] PRIMARY KEY CLUSTERED ([OfferChgTypeID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[OfferChgType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OfferChgType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[OfferChgType] TO [IRMAReportsRole]
    AS [dbo];

