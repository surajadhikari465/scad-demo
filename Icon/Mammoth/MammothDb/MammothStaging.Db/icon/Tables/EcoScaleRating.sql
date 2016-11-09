CREATE TABLE [icon].[EcoScaleRating] (
    [EcoScaleRatingId] INT           NOT NULL,
    [Description]      NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EcoScaleRating] PRIMARY KEY CLUSTERED ([EcoScaleRatingId] ASC) WITH (FILLFACTOR = 100)
);

