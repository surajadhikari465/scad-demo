declare @beginDateTime datetime2 = '2016-11-03 12:00:00',
		@endDateTime datetime2 = '2016-11-04 14:00:00'

select 
	MIN(DATEDIFF(ms, CONVERT(datetime2, SUBSTRING(a.Context, a.MessageParseTimeStartIndex, a.MessageParseTimeEndIndex - MessageParseTimeStartIndex)), a.InsertDate)) 'elapsed time (ms)'
from 
	(
		select *,
			CHARINDEX('"MessageParseTime":"', a.Context) + LEN('"MessageParseTime":"') 'MessageParseTimeStartIndex',
			LEN(a.Context) - 1 'MessageParseTimeEndIndex'
		from infor.MessageArchiveHierarchy a
	) a
where a.HierarchyName <> 'Brands'
	and a.HierarchyClassName like 'PTedit%'
	and a.InsertDate between @beginDateTime and @endDateTime
