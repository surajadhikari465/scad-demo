   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCompetitivePriceTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCompetitivePriceTypes]
GO

CREATE PROCEDURE [dbo].[GetCompetitivePriceTypes] 
AS
BEGIN

SELECT
	CompetitivePriceTypeID,
	Description
FROM
	CompetitivePriceType
ORDER BY
	Description

END 
go  