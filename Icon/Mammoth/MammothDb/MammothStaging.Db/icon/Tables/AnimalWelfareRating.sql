CREATE TABLE [icon].[AnimalWelfareRating] (
    [AnimalWelfareRatingId] INT           NOT NULL,
    [Description]           NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AnimalWelfareRating] PRIMARY KEY CLUSTERED ([AnimalWelfareRatingId] ASC) WITH (FILLFACTOR = 100)
);

