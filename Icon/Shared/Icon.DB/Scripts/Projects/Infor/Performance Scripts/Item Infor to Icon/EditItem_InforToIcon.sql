declare @beginDateTime datetime2 = '2016-11-03 12:00:00',
		@endDateTime datetime2 = GETDATE()

select 
	'Edit Item',
	'Infor' 'Source System',
	'Icon' 'Component',
	CONVERT(datetime2, SUBSTRING(a.Context, a.MessageParseTimeStartIndex, a.MessageParseTimeEndIndex - MessageParseTimeStartIndex)) 'In Timestamp',
	a.InsertDate 'Out Timestamp',
	DATEDIFF(ms, CONVERT(datetime2, SUBSTRING(a.Context, a.MessageParseTimeStartIndex, a.MessageParseTimeEndIndex - MessageParseTimeStartIndex)), a.InsertDate) 'elapsed time in milliseconds',
	InforMessageId 'MessageID'
from 
	(
		select *,
			CHARINDEX('"MessageParseTime":"', a.Context) + LEN('"MessageParseTime":"') 'MessageParseTimeStartIndex',
			LEN(a.Context) - 1 'MessageParseTimeEndIndex'
		from infor.MessageArchiveProduct a
	) a
where a.InsertDate between @beginDateTime and @endDateTime