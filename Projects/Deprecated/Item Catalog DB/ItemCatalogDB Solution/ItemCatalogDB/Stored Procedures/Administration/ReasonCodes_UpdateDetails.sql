
IF exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReasonCodes_UpdateDetails]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReasonCodes_UpdateDetails]
GO

/****** Object:  StoredProcedure [dbo].[ReasonCodes_UpdateDetails]    Script Date: 07/18/2011 12:20:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ============================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Updates an existing Reason Code Detail record
-- ============================================================

CREATE PROCEDURE [dbo].[ReasonCodes_UpdateDetails]
@ReasonCode char(3),
@ReasonCodeDesc varchar(50),
@ReasonCodeExtDesc varchar(MAX)
AS
BEGIN

	UPDATE ReasonCodeDetail SET ReasonCodeDesc = @ReasonCodeDesc, ReasonCodeExtDesc = @ReasonCodeExtDesc
	WHERE ReasonCode = @ReasonCode
	
END

GO
