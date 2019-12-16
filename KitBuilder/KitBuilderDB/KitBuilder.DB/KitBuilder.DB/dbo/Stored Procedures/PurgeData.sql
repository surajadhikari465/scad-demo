create proc [dbo].[PurgeData] 
	@batchSize int = 50000
as
  set nocount on;
  declare @error_no int, @error_msg varchar(max)
  select @error_no = 0
  
  declare @sql nvarchar(max),
          @currentHour int = (select DatePart(hour, GetDate())),
		  @purgeJobName varchar(50)= 'Kit Builder Data Purge Service';

	update PurgeTableInfo set IsDailyPurgeCompleted = 0
	where Cast(GetDate() as date) <> Cast(IsNull(LastPurgedDate, GetDate()) as date)
	  and PurgeJobName = @purgeJobName
	  and IsDailyPurgeCompleted = 1;

	--Create delete commands according to PurgeTableInfo table
	if(object_id('tempdb..#deleteCommand')is not null) 
			drop table #deleteCommand;
    
    select FormatMessage('declare @currentDate as date = cast(DateAdd(d, -%i, GetUtcDate()) as date);
                          delete top(%i) from [%s].[%s] where [%s] is null or [%s] < @currentDate;
                          update PurgeTableInfo set LastPurgedDate = GetDate(),
                            IsDailyPurgeCompleted = IsNull((select top 1 0 from [%s].[%s] where [%s] < @currentDate), 1) 
                          where ID = %i;',
                          DaysToKeep, @batchSize, SchemaName, TableName, ReferenceColumn, ReferenceColumn, SchemaName, TableName, ReferenceColumn, ID) Command
	  
    into #deleteCommand
	from PurgeTableInfo A
    join sys.schemas D on D.name = A.SchemaName 
    join sys.tables B on B.name = A.TableName and B.schema_id = D.schema_id
    join sys.columns C on C.name = A.ReferenceColumn and C.object_id = B.object_id
  where C.system_type_id in(select system_type_id from sys.types where name like 'date%')
	  and A.IsDailyPurge = 1
	  and A.IsDailyPurgeCompleted = 0
	  and A.PurgeJobName = @purgeJobName
	  and (@currentHour >= A.TimeToStart and @currentHour < A.TimeToEnd)

	declare cur cursor for select Command from #deleteCommand;
	open cur

	fetch next from cur into @sql;

	while @@fetch_status = 0
	begin
    begin try
		  exec(@sql);
    end try
    begin catch
		select @error_no = @@ERROR
		select @error_msg = ERROR_MESSAGE()
        
		declare @Severity smallint
        select @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('[PurgeData] failed with ERROR: %d', @Severity, 1, @error_msg)
    end catch

		fetch next from cur into @sql;
	end

  close cur;
	deallocate cur;

	if(object_id('tempdb..#deleteCommand')is not null)
			drop table #deleteCommand

  set nocount off;