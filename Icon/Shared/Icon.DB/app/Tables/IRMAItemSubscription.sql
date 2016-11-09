CREATE TABLE [app].[IRMAItemSubscription] (
    [IRMAItemSubscriptionID]      INT           IDENTITY (1, 1) NOT NULL,
    [regioncode]				  VARCHAR (2)   NOT NULL,
    [identifier]				  VARCHAR (13)  NOT NULL,
    [insertDate] DATETIME2 NOT NULL , 
	[deleteDate] DATETIME2 NULL DEFAULT getDate() , 
    CONSTRAINT [PK_IRMAItemSubscription] PRIMARY KEY CLUSTERED ([IRMAItemSubscriptionID] ASC) WITH (FILLFACTOR = 80)
);