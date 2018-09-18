create proc [dbo].[PurgeData] 
	@batchSize int = 50000
as
  set nocount on;
  declare @sql nvarchar(max),
          @currentHour int = (select DatePart(hour, GetDate()));

	update PurgeTableInfo set IsDailyPurgeCompleted = 0
	where Cast(GetDate() as date) <> Cast(IsNull(LastPurgedDate, GetDate()) as date)
	  and PurgeJobName = 'Data History Purge'
	  and IsDailyPurgeCompleted = 1;

	--Create delete commands according to PurgeTableInfo table
	if(object_id('tempdb..#deleteCommand')is not null) 
			drop table #deleteCommand;
    
    select FormatMessage('declare @currentDate as date = cast(DateAdd(d, -%i, GetDate()) as date);
                          delete top(%i) from [%s].[%s] where [%s] is null or [%s] < @currentDate;
                          update PurgeTableInfo set LastPurgedDate = GetDate(),
                            IsDailyPurgeCompleted = IsNull((select top 1 0 from [%s].[%s] where [%s] < @currentDate), 1) 
                          where ID = %i;',
                          DaysToKeep, @batchSize, SchemaName, TableName, ReferenceColumn, ReferenceColumn, SchemaName, TableName, ReferenceColumn, ID) Command
	  
  into #deleteCommand
	from PurgeTableInfo A
    join sys.tables B on B.name = A.TableName
    join sys.columns C on C.name = A.ReferenceColumn and C.object_id = B.object_id
  where C.system_type_id in(select system_type_id from sys.types where name like 'date%')
	  and A.IsDailyPurge = 1
	  and A.IsDailyPurgeCompleted = 0
	  and A.PurgeJobName = 'Data History Purge'
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
      --Should we do anything here?
    end catch

		fetch next from cur into @sql;
	end

  close cur;
	deallocate cur;

	if(object_id('tempdb..#deleteCommand')is not null)
			drop table #deleteCommand

  set nocount off;