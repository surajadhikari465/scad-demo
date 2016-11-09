create table app.MessageQueueBusinessUnitInProcess
(
	ControllerInstanceId int not null,
	ControllerTypeId int not null,
	BusinessUnitInProcess int not null,
    constraint [PK_MessageQueueBusinessUnitInProcess] primary key clustered (ControllerInstanceId, ControllerTypeId),
	constraint [FK_MessageQueueBusinessUnitInProcess_MessageType] foreign key (ControllerTypeId) references app.MessageType(MessageTypeId)
)