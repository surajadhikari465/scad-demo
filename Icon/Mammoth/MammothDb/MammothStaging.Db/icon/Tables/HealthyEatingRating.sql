CREATE TABLE [icon].[HealthyEatingRating] (
    [HealthyEatingRatingId] INT           NOT NULL,
    [Description]           NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_HealthyEatingRating] PRIMARY KEY CLUSTERED ([HealthyEatingRatingId] ASC) WITH (FILLFACTOR = 100)
);

