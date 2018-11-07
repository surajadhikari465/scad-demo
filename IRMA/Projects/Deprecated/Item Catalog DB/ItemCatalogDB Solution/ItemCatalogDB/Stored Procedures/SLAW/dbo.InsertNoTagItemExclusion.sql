if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertNoTagItemExclusion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertNoTagItemExclusion]
GO

-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-02
-- Description:	Inserts items that were excluded by the
--				no-tag logic into the exclusions table.
-- =============================================

CREATE PROCEDURE dbo.InsertNoTagItemExclusion
	@ExcludedItemKeys dbo.IntType readonly,
	@ExcludedIdentifiers dbo.IdentifiersType readonly,
	@PriceBatchHeaderId int = null,
	@StoreNumber int
AS
BEGIN
	set nocount on

	if exists (select Identifier from @ExcludedIdentifiers)
		begin
			insert into 
				dbo.NoTagItemExclusion
			select
				Identifier,
				null,
				@StoreNumber,
				SYSDATETIME()
			from
				@ExcludedIdentifiers
		end
	else
		begin
			insert into 
				dbo.NoTagItemExclusion
			select
				pbd.Identifier,
				@PriceBatchHeaderId,
				@StoreNumber,
				SYSDATETIME()
			from
				@ExcludedItemKeys ik
				join PriceBatchDetail (nolock) pbd on ik.[Key] = pbd.Item_Key and @PriceBatchHeaderId = pbd.PriceBatchHeaderID and @StoreNumber = pbd.Store_No
		end
END
GO
