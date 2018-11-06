
IF exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReasonCodes_UpdateType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReasonCodes_UpdateType]
GO

/****** Object:  StoredProcedure [dbo].[ReasonCodes_UpdateType]    Script Date: 07/18/2011 12:20:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Update an Existing Reason Code Type Description
-- ============================================================

CREATE PROCEDURE [dbo].[ReasonCodes_UpdateType]
@ReasonCodeTypeAbbr char(2),
@ReasonCodeTypeDesc varchar(50)
AS
BEGIN

	UPDATE ReasonCodeType SET ReasonCodeTypeDesc = @ReasonCodeTypeDesc
	WHERE ReasonCodeTypeAbbr = @ReasonCodeTypeAbbr
	
END

GO
