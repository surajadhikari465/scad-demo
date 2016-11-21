CREATE TABLE infor.MessageArchiveProduct
(
	MessageArchiveId int primary key identity(1, 1),
	ItemId int not null,
	ScanCode nvarchar(13) not null,
	EsbMessageId uniqueidentifier not null,
	Context nvarchar(max) not null,
	ErrorCode nvarchar(100) null,
	ErrorDetails nvarchar(max) null,
    InsertDate datetime2(7) constraint DF_MessageArchiveProduct_InsertDate default (sysdatetime()) not null,
)
