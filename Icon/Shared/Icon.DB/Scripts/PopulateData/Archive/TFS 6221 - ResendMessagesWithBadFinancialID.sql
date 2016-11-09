begin
	
	declare @ItemsThatNeedToBeFixed app.UpdatedItemIDsType

	insert into @ItemsThatNeedToBeFixed select distinct itemID from app.MessageQueueProduct mq where len(mq.FinancialClassId) > 4

	exec app.GenerateItemUpdateMessages @ItemsThatNeedToBeFixed

end
