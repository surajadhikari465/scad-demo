
IF exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReasonCodes_DisableMapping]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReasonCodes_DisableMapping]
GO

/****** Object:  StoredProcedure [dbo].[ReasonCodes_DisableMapping]    Script Date: 07/18/2011 12:20:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Disables a Reason Code Mapping
-- =============================================

CREATE PROCEDURE [dbo].[ReasonCodes_DisableMapping]
@ReasonCodeTypeAbbr char(2),
@ReasonCode nchar(3)
AS
BEGIN

	DECLARE @ReasonCodeTypeID int, @ReasonCodeDetailID int

	SELECT @ReasonCodeTypeID = (SELECT ReasonCodeTypeID FROM ReasonCodeType WHERE ReasonCodeTypeAbbr = @ReasonCodeTypeAbbr )
	SELECT @ReasonCodeDetailID = (SELECT ReasonCodeDetailID FROM ReasonCodeDetail WHERE ReasonCode = @ReasonCode )

	IF EXISTS (SELECT ReasonCodeMappingID FROM ReasonCodeMappings WHERE ReasonCodeTypeID = @ReasonCodeTypeID and ReasonCodeDetailID = @ReasonCodeDetailID)
	BEGIN
		UPDATE ReasonCodeMappings SET Disabled = 1 
		WHERE ReasonCodeTypeID = @ReasonCodeTypeID AND ReasonCodeDetailID = @ReasonCodeDetailID 
	END
END
GO
