SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_AddError')
	BEGIN
		DROP Procedure [dbo].SOG_AddError
	END
GO

CREATE PROCEDURE dbo.SOG_AddError
	@UserName		varchar(50),
	@Workstation	varchar(50),
	@ErrorMessage	varchar(max),
	@StackTrace		varchar(max),
	@CatalogErrorID	int OUTPUT
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_AddError()
--    Author: Billy Blackerby
--      Date: 3/25/2009
--
-- Description:
-- Utilized by StoreOrderGuide to insert an error
--
-- Modification History:
-- Date			Init	Comment
-- 03/25/2009	BBB		Creation
-- 04/08/2009	BBB		Added OUTPUT switch to CatalogErrorID
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	INSERT INTO 
		CatalogError
	(
		UserName,
		Workstation,
		ErrorMessage,
		StackTrace
	)
	VALUES
	(
		@UserName,
		@Workstation,
		@ErrorMessage,
		@StackTrace
	)
	
	--**************************************************************************
	--Return ID
	--**************************************************************************
	SET @CatalogErrorID = SCOPE_IDENTITY()
	
	RETURN
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 