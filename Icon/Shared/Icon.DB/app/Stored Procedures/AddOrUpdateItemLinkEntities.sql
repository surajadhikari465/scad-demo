
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2016-05-27
-- Description:	Takes a collection of ItemLink entities
--				and performs a MERGE to either insert or
--				update the values in ItemLink.
-- =============================================

CREATE PROCEDURE [app].[AddOrUpdateItemLinkEntities] 
	@ItemLinkEntities app.ItemLinkEntityType readonly
AS
BEGIN
	set nocount on;

	if OBJECT_ID('tempdb..#ItemLinkEntities') is not null
		begin
			drop table #ItemLinkEntities;
		end

	create table #ItemLinkEntities
	(
		ParentItemId int not null,
		ChildItemId int not null,
		LocaleId int not null
	)

	insert into #ItemLinkEntities with (tablock) select * from @ItemLinkEntities 

	create clustered index [IX_ItemLinkKey_ItemLinkEntities] on #ItemLinkEntities (ParentItemId, ChildItemId, LocaleId)

	-- The locking hints here are designed to help with merge concurrency.
	merge
		dbo.ItemLink with (updlock, rowlock) il
	using
		#ItemLinkEntities ile
	on
		il.childItemId		= ile.ChildItemId and
		il.localeID			= ile.LocaleId
	when matched then
		update set 
			il.parentItemId = ile.ParentItemId
	when not matched then
		insert 
			(parentItemID, childItemID, localeID)
		values 
			(ile.ParentItemId, ile.ChildItemId, ile.LocaleId);
END
GO
