CREATE TABLE [dbo].[ItemScaleRequest] (
    [ItemScaleRequest_ID]      INT            IDENTITY (1, 1) NOT NULL,
    [ItemRequest_ID]           INT            NOT NULL,
    [ScaleDescription1]        VARCHAR (64)   NULL,
    [ScaleDescription2]        VARCHAR (64)   NULL,
    [ScaleDescription3]        VARCHAR (64)   NULL,
    [ScaleDescription4]        VARCHAR (64)   NULL,
    [ShelfLife]                SMALLINT       NULL,
    [ScaleUomUnit_ID]          INT            NULL,
    [ScaleRandomWeightType_ID] INT            NULL,
    [Scale_Tare_ID]            INT            NULL,
    [Ingredients]              VARCHAR (4200) NULL,
    [FixedWeight]              VARCHAR (25)   NULL,
    [ByCount]                  INT            NULL,
    [ForceTare]                BIT            NULL,
    [LabelStyle]               INT            NULL,
    [ExtraTextLabelType]       INT            NULL,
    [ExtraText]                VARCHAR (4200) NULL,
    CONSTRAINT [PK_ItemScaleRequest] PRIMARY KEY CLUSTERED ([ItemScaleRequest_ID] ASC),
    CONSTRAINT [FK_ItemScaleRequest_ItemScaleRequest] FOREIGN KEY ([ItemScaleRequest_ID]) REFERENCES [dbo].[ItemScaleRequest] ([ItemScaleRequest_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScaleRequest] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemScaleRequest] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemScaleRequest] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemScaleRequest] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemScaleRequest] TO [IRMASLIMRole]
    AS [dbo];

