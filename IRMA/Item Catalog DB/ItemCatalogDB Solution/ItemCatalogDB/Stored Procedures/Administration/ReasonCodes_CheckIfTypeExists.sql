
IF exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReasonCodes_CheckIfTypeExists]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReasonCodes_CheckIfTypeExists]
GO

/****** Object:  StoredProcedure [dbo].[ReasonCodes_CheckIfTypeExists]    Script Date: 07/18/2011 12:20:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Check if there is an existing Reason Code Type
-- =============================================================

CREATE PROCEDURE [dbo].[ReasonCodes_CheckIfTypeExists]
@ReasonCodeTypeAbbr char(2)
AS
BEGIN

	SELECT * FROM ReasonCodeType WHERE ReasonCodeTypeAbbr = @ReasonCodeTypeAbbr
	
END

GO
