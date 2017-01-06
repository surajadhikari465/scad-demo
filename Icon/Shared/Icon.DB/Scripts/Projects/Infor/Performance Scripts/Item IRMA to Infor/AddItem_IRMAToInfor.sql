declare @beginDateTime datetime2 = '2016-11-03 12:00:00',
		@endDateTime datetime2 = GETDATE()

select
	'Add Item' 'Transaction',
	'IRMA' 'Source System',
	'IRMA' 'Component',
	RetrievedTime.InsertDate 'In Timestamp',
	SentTime.InsertDate 'Out Timestamp',
	DATEDIFF(ms, RetrievedTime.InsertDate, SentTime.InsertDate) 'elapsed time (ms)',
	SentTime.MessageID,
	RetrievedTime.AppLogID,
	SentTime.AppLogID
from
	(
		select
			SUBSTRING(Message, CHARINDEX('ScanCodes: ', Message) + LEN('ScanCodes: '), LEN(Message) - CHARINDEX('ScanCodes:', Message) + LEN('ScanCodes: ')) 'ScanCode',
			al.InsertDate,
			al.AppLogID
		from app.AppLog al 
		where al.appID = 15
			and Message like '%GetNewItemsQuery retrieved 1 items%'
	) RetrievedTime
join
	(
		select
			SUBSTRING(Message, CHARINDEX('ScanCodes: ', Message) + LEN('ScanCodes: '), LEN(Message) - CHARINDEX('ScanCodes:', Message) + LEN('ScanCodes: ')) 'ScanCode',
			SUBSTRING(Message, CHARINDEX('MessageHistoryId: ', Message) + LEN('MessageHistoryId: ') + 1, CHARINDEX(',', Message) - (CHARINDEX('MessageHistoryId: ', Message) + LEN('MessageHistoryId: ') + 1)) 'MessageID',
			al.InsertDate,
			al.AppLogID
		from app.AppLog al 
		where al.appID = 15
			and Message like 'InforItemService sent message to Infor.%'
	) SentTime on RetrievedTime.ScanCode = SentTime.ScanCode and RetrievedTime.AppLogID = SentTime.AppLogID - 1
where SentTime.InsertDate between @beginDateTime and @endDateTime