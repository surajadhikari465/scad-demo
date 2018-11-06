
IF exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReasonCodes_GetDetails]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReasonCodes_GetDetails]
GO

/****** Object:  StoredProcedure [dbo].[ReasonCodes_GetDetails]    Script Date: 07/18/2011 12:20:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Gets all the reason code details defined for the region
-- ====================================================================

CREATE PROCEDURE [dbo].[ReasonCodes_GetDetails] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT ReasonCodeDetailID, ReasonCode, ReasonCodeDesc, ReasonCodeExtDesc 
	FROM ReasonCodeDetail
	ORDER BY ReasonCodeDesc ASC
	
END

GO