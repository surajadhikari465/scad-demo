IF exists (SELECT * FROM dbo.sysobjects where id = object_id(N'[dbo].[GetReasonCodes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetReasonCodes]
GO

-- =============================================
-- Author:		Anjana Maganti
-- Create date: August 30th 2011
-- Description:	To get the reason codes based on the Region the user connects to.
-- =============================================
CREATE PROCEDURE [dbo].[GetReasonCodes] 
	@UserID as int
AS
DECLARE @RegionID as int,
@IRMAServer varchar(6),
          @IRMADatabase varchar(50),
          @DBString varchar(max),
          @Sql nvarchar(max)
BEGIN

	set @RegionID = (SELECT RegionID from users where userID = @UserID)

			select @IRMAServer = IRMAServer , @IRMADatabase = IRMADataBase
				 from Regions where RegionID = @RegionID
				
				set @DBString = '[' + @IRMAServer + '].[' + @IRMADatabase + '].[dbo].'
			
				
				exec (@DBString + 'reasoncodes_getdetailsfortype ''CA''')
				
		
end	
GO
