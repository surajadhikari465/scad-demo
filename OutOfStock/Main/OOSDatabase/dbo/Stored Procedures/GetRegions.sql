-- =============================================
-- Author:		Anjana Maganti
-- Create date: 10/26/2011
-- Description:	Toget all regions
-- =============================================
CREATE PROCEDURE [dbo].[GetRegions]
	
AS
BEGIN
	select * from REGION where IS_VISIBLE = 'true'
END
