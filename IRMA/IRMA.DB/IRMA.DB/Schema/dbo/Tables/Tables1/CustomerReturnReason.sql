CREATE TABLE [dbo].[CustomerReturnReason] (
    [CustReturnReasonID] INT           NOT NULL,
    [CustReturnReason]   VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_CustomerReturnReason] PRIMARY KEY CLUSTERED ([CustReturnReasonID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CustomerReturnReason] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CustomerReturnReason] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CustomerReturnReason] TO [IRMAReportsRole]
    AS [dbo];

