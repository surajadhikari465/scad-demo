declare @beginDateTime datetime2 = '2016-11-03 12:00:00',
		@endDateTime datetime2 = GETDATE()

select 
	AVG(DATEDIFF(ms, CONVERT(datetime2, SUBSTRING(a.Context, a.MessageParseTimeStartIndex, a.MessageParseTimeEndIndex - MessageParseTimeStartIndex)), a.InsertDate)) 'elapsed time in milliseconds'
from 
	(
		select *,
			CHARINDEX('"MessageParseTime":"', a.Context) + LEN('"MessageParseTime":"') 'MessageParseTimeStartIndex',
			LEN(a.Context) - 1 'MessageParseTimeEndIndex'
		from infor.MessageArchiveProduct a
	) a
where a.InsertDate between @beginDateTime and @endDateTime