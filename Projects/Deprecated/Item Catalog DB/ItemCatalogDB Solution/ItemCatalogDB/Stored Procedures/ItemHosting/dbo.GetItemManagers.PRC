 -- =============================================
-- Author:		Robert Shurbet
-- Create date: 5.23.07
-- Description:	Retrieves the values from the ItemManager table.
-- =============================================
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemManagers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemManagers]
GO
CREATE PROCEDURE [dbo].[GetItemManagers]
AS 

BEGIN
	SELECT Manager_ID, Value
	FROM ItemManager (nolock)
	ORDER BY Value
END
GO