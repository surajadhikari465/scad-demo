CREATE TABLE infor.MessageArchiveProductOutOfSync
(
	MessageArchiveId int primary key identity(1, 1),
	ItemId int not null,
	ScanCode nvarchar(100) not null,
	InforMessageId uniqueidentifier null,
	Context nvarchar(max) not null,
	ErrorCode nvarchar(100) null,
	ErrorDetails nvarchar(max) null,
    InsertDate datetime2(7) constraint DF_MessageArchiveProductOutOfSync_InsertDate default (sysdatetime()) not null,
)
