SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetUserDetails')
	BEGIN
		DROP Procedure [dbo].SOG_GetUserDetails
	END
GO

CREATE PROCEDURE dbo.SOG_GetUserDetails
	@UserName varchar(50)
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetUserDetails()
--    Author: Billy Blackerby
--      Date: 3/12/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return user details specific to that application
--
-- Modification History:
-- Date			Init	Comment
-- 03/12/2009	BBB		Creation
-- 03/24/2009	BBB		Added UserName to output; changed input to UserName
-- 04/07/2009	BBB		Added Telxon_Store_Limit to data output
-- 04/27/2009	BBB		Added columns for Warehouse and Buyer, moved existing
--						Admin column to look at ConfigurationAdmin key
-- 06/26/2009	BBB		Added DCAdmin to output as Schedule
-- 08/05/2009	BBB		Added SuperUser to output
-- 11/23/2009	MEP		Changed ConfigurationAdmin to SystemConfigurationAdministrator for SOX purposes
-- **************************************************************************
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT 
		[UserName]	=	u.UserName,
		[UserID]	=	u.User_ID,
		[Email]		=	u.EMail,
		[Admin]		=	u.SystemConfigurationAdministrator,
		[Warehouse]	=	u.Warehouse,
		[Schedule]	=	u.DCAdmin,
		[Buyer]		=	u.Buyer,
		[SuperUser]	=	u.SuperUser,
		[StoreNo]	=	ISNULL(u.Telxon_Store_Limit, 0)
	FROM 
		Users					(nolock) u
	WHERE 
		u.UserName				= @UserName
		AND u.AccountEnabled	= 1

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO