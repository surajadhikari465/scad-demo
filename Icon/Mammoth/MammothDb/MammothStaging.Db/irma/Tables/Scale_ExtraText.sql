CREATE TABLE [irma].[Scale_ExtraText] (
	[Region]			 NCHAR(2)		NOT NULL,
    [Scale_ExtraText_ID] INT            NOT NULL,
    [Scale_LabelType_ID] INT            NOT NULL,
    [Description]        VARCHAR (50)   NOT NULL,
    [ExtraText]          VARCHAR (4200) NOT NULL
);

