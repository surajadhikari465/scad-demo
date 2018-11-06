CREATE TABLE [dbo].[Scale_RandomWeightType] (
    [Scale_RandomWeightType_ID] INT          IDENTITY (1, 1) NOT NULL,
    [Description]               VARCHAR (50) NULL,
    CONSTRAINT [PK_Scale_RandomWeightType] PRIMARY KEY CLUSTERED ([Scale_RandomWeightType_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_RandomWeightType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Scale_RandomWeightType] TO [IRMASLIMRole]
    AS [dbo];

