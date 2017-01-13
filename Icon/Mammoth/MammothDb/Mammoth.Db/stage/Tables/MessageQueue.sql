CREATE TABLE [stage].[MessageQueue]
(
	[MessageQueueId]	int not null,
	[Timestamp]			datetime2 not null,
	[TransactionId]		uniqueidentifier not null
)

