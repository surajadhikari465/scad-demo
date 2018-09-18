﻿create table AppLog(
	AppLogID    int identity constraint PK_AppLog primary key clustered,
	AppID       int not null,
	Level       nvarchar(100),
	Logger      nvarchar(255),
	UserName    nvarchar(255),
	MachineName nvarchar(255),
	InsertDate  datetime2(3) not null default GetDate(),
	LogDate     datetime2(3),
  Thread      nvarchar(100),
	Message     nvarchar(max),
  CallSite    nvarchar(max),
  Exception   nvarchar(max),
	StackTrace  nvarchar(max),

  index ix_InsertDate nonclustered(InsertDate),
) on [primary]
