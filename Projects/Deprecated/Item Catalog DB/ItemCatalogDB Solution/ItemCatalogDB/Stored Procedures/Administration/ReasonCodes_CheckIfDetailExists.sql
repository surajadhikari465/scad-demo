
IF exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReasonCodes_CheckIfDetailExists]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReasonCodes_CheckIfDetailExists]
GO

/****** Object:  StoredProcedure [dbo].[ReasonCodes_CheckIfDetailExists]    Script Date: 07/18/2011 12:20:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ================================================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Check if there is an existing Reason Code Detail
-- ================================================================

CREATE PROCEDURE [dbo].[ReasonCodes_CheckIfDetailExists]
@ReasonCode char(3)
AS
BEGIN

	SELECT * FROM ReasonCodeDetail WHERE ReasonCode = @ReasonCode
	
END

GO
