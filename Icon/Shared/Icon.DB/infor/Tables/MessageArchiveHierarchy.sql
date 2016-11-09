CREATE TABLE infor.MessageArchiveHierarchy
(
	MessageArchiveId int primary key identity(1, 1),
	HierarchyClassId int not null,
	HierarchyClassName nvarchar(255) not null,
	HierarchyName nvarchar(255),
	InforMessageId uniqueidentifier not null,
	Context nvarchar(max) not null,
	ErrorCode nvarchar(100) null,
	ErrorDetails nvarchar(max) null,
    InsertDate datetime2(7) constraint DF_MessageArchiveHierarchy_InsertDate default (sysdatetime()) not null,
)
