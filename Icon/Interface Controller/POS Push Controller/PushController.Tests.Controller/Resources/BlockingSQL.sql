begin tran
	select top(1) * from app.MessageQueueItemLocale with (tablockx, holdlock)
	waitfor delay '00:00:40';
rollback tran