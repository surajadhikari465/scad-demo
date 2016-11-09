CREATE TABLE [dbo].[CostPromoCodeType] (
    [CostPromoCodeTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [CostPromoCode]       INT          NOT NULL,
    [CostPromoDesc]       VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CostPromoCodeType_CostPromoCodeTypeID] PRIMARY KEY CLUSTERED ([CostPromoCodeTypeID] ASC)
);





GO
ALTER TABLE [dbo].[CostPromoCodeType] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CostPromoCodeType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CostPromoCodeType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CostPromoCodeType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CostPromoCodeType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CostPromoCodeType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CostPromoCodeType] TO [IMHARole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CostPromoCodeType] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CostPromoCodeType] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[CostPromoCodeType] TO [iCONReportingRole]
    AS [dbo];

