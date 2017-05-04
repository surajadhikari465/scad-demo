create table monitor.JobStatus
(
	JobName nvarchar(255) not null,
	Status nvarchar(50) not null,
	Region nvarchar(2) null,
	Properties nvarchar(max) null,
	ModifiedDate datetime2 (7) null,
	InsertDate datetime2 (7)  constraint [DF_MonitorJobStatus_InsertDate] default (sysdatetime()) NOT NULL,
)