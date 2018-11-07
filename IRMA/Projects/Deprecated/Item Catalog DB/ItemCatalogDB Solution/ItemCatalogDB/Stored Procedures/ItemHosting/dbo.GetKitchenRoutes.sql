-- =============================================
-- Author:		Robert Shurbet
-- Create date: 5.24.07
-- Description:	Gets the list of available Kitchen Route values.
-- =============================================
if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetKitchenRoutes') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetKitchenRoutes
GO
CREATE PROCEDURE [dbo].[GetKitchenRoutes]
AS 
BEGIN
	SELECT KitchenRoute_ID, Value
	FROM KitchenRoute (nolock)
	ORDER BY Value
END
GO
