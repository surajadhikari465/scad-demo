if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBrands_ByNameStartsWith]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBrands_ByNameStartsWith]
GO

CREATE PROCEDURE [dbo].[GetBrands_ByNameStartsWith] 
	@Start varchar(52),
	@SubTeam_No int = NULL
AS

BEGIN

	SELECT @Start = @Start + '%'

IF @SubTeam_No IS NULL
BEGIN
	select 
		rtrim(Brand_Name) [Value],
		brand_id [ID] 
	from 
		ItemBrand 
	where 
		Brand_Name like @Start
	order by 
		Brand_Name
END
ELSE
BEGIN
	select 
		rtrim(IB.Brand_Name) [Value],
		IB.brand_id [ID] 
	from 
		ItemBrand IB
		INNER JOIN Item I (NOLOCK) ON I.Brand_ID = IB.Brand_ID
	where 
		IB.Brand_Name like @Start
		AND
		I.SubTeam_No = @SubTeam_No
	GROUP BY
		IB.Brand_Name,
		IB.brand_id
	order by 
		IB.Brand_Name
END

END
go