CREATE TABLE [dbo].[AttributeIdentifier] (
    [AttributeIdentifier_ID] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Screen_Text]            VARCHAR (50)   NULL,
    [field_type]             VARCHAR (50)   NULL,
    [combo_box]              BIT            DEFAULT ((0)) NULL,
    [max_width]              INT            NULL,
    [default_value]          VARCHAR (50)   NULL,
    [field_values]           VARCHAR (8000) NULL,
    [IsRequiredValue]        BIT            DEFAULT ((0)) NULL,
    CONSTRAINT [PK_AttributeIdentifier] PRIMARY KEY CLUSTERED ([AttributeIdentifier_ID] ASC)
);





GO
ALTER TABLE [dbo].[AttributeIdentifier] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT SELECT
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AttributeIdentifier] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[AttributeIdentifier] TO [iCONReportingRole]
    AS [dbo];

