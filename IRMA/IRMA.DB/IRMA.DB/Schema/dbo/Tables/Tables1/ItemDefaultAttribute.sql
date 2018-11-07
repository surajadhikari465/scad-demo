CREATE TABLE [dbo].[ItemDefaultAttribute] (
    [ItemDefaultAttribute_ID] INT           IDENTITY (1, 1) NOT NULL,
    [AttributeName]           VARCHAR (50)  NOT NULL,
    [AttributeTable]          VARCHAR (50)  NOT NULL,
    [AttributeField]          VARCHAR (50)  NOT NULL,
    [Type]                    TINYINT       NOT NULL,
    [Active]                  BIT           NOT NULL,
    [ControlOrder]            TINYINT       NOT NULL,
    [ControlType]             TINYINT       NOT NULL,
    [PopulateProcedure]       VARCHAR (100) NULL,
    [IndexField]              VARCHAR (50)  NULL,
    [DescriptionField]        VARCHAR (50)  NULL,
    CONSTRAINT [PK_ItemDefaultAttribute] PRIMARY KEY CLUSTERED ([ItemDefaultAttribute_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemDefaultAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemDefaultAttribute] TO [IRMAReportsRole]
    AS [dbo];

