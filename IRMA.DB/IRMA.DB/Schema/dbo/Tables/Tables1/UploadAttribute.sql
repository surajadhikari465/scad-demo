CREATE TABLE [dbo].[UploadAttribute] (
    [UploadAttribute_ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]                     VARCHAR (50)   NOT NULL,
    [TableName]                VARCHAR (50)   NOT NULL,
    [ColumnNameOrKey]          VARCHAR (50)   NOT NULL,
    [ControlType]              VARCHAR (50)   NOT NULL,
    [DbDataType]               VARCHAR (50)   NOT NULL,
    [Size]                     INT            NULL,
    [IsRequiredValue]          BIT            DEFAULT ((0)) NOT NULL,
    [IsCalculated]             BIT            DEFAULT ((0)) NOT NULL,
    [OptionalMinValue]         VARCHAR (50)   NULL,
    [OptionalMaxValue]         VARCHAR (50)   NULL,
    [IsActive]                 BIT            DEFAULT ((1)) NOT NULL,
    [DisplayFormatString]      VARCHAR (50)   NULL,
    [PopulateProcedure]        VARCHAR (50)   NULL,
    [PopulateIndexField]       VARCHAR (50)   NULL,
    [PopulateDescriptionField] VARCHAR (50)   NULL,
    [SpreadsheetPosition]      INT            DEFAULT ((1)) NOT NULL,
    [ValueListStaticData]      VARCHAR (4000) NULL,
    [DefaultValue]             VARCHAR (4500) NULL,
    CONSTRAINT [PK_UploadAttribute] PRIMARY KEY CLUSTERED ([UploadAttribute_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadAttribute] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadAttribute] TO [IRMAReportsRole]
    AS [dbo];

