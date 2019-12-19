-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetStores]
	@Region as char(3)
AS
BEGIN
    if @Region = 'CEN'
		begin
		   select s.ID, s.STORE_NAME, 10 as StoreOOsCount,
					33 as StoreOOSPercen,
					18 as TimesScanned
					 from STORE S
		end
    else
    begin
		select s.ID, s.STORE_NAME, 10 as StoreOOsCount,
				33 as StoreOOSPercen,
				18 as TimesScanned
				 from STORE S join REGION R on r.ID = s.REGION_ID
				 where r.REGION_ABBR = @Region
    end
	
END
