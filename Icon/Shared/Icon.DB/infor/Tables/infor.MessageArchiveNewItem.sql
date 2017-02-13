create table infor.MessageArchiveNewItem
(
	MessageArchiveNewItemId int identity(1, 1) primary key,
	QueueId int,
	Region nvarchar(2),
	ScanCode nvarchar(13),
	MessageHistoryId int null,
	Context nvarchar(max),
	ErrorCode nvarchar(100),
	ErrorDetails nvarchar(max),
    InsertDate datetime2(7) constraint DF_MessageArchiveNewItem_InsertDate default (sysdatetime()) not null,
)