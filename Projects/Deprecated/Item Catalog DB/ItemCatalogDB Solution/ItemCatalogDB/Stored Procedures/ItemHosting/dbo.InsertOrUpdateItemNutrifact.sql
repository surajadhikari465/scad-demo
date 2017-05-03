IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateItemNutrifact]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[InsertOrUpdateItemNutrifact]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-03-23
-- Description:	Used to add or update an
--				item-to-nutrifact mapping.
-- =============================================

CREATE PROCEDURE dbo.InsertOrUpdateItemNutrifact
	@ItemKey		int,
	@NutrifactId	int
AS
BEGIN
	
	set nocount on

	; -- Need this for SQL Compatibility Level 90 because MERGE is NOT recognized as a reserved word at this level, resulting in a parsing error at compat level lower than 100.
    merge 
		ItemNutrition with (updlock, rowlock) inu
	using
		(select @ItemKey as ItemKey, @NutrifactId as NutrifactId) as input
	on
		inu.ItemKey = input.ItemKey
	when matched then
		update set
			inu.NutriFactsID = input.NutrifactId
	when not matched then
		insert (ItemKey, NutriFactsID) values (@ItemKey, @NutrifactId);
			
END
GO
