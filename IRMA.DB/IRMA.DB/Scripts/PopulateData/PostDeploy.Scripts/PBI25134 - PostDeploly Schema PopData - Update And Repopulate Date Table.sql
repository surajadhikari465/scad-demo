--Backup the date records to be updated/repopulated
IF OBJECT_ID(N'dbo.Date_Backup', N'U') IS NOT NULL
BEGIN
  drop table dbo.Date_Backup
END

select * into Date_Backup from date where Date_Key > '2017-09-24' 

GO

delete Date
where Date_key > '2017-12-31'

GO

update Date
set Year = '2017',
	Quarter = 5,
	Period = Period + 13,
	Day_Of_Year = Day_Of_Year + 364
where Date_Key < '2018-01-01'
and Year = '2018'

GO

declare @date as datetime = '2018-01-01'

while @date < '2023-01-01'
BEGIN
	insert into Date
	select @date, YEAR(@date), DATEPART(QUARTER, @date), MONTH(@date), DATEPART(WEEK, @DATE)  -
    DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,@DATE), 0))+ 1, DATENAME(weekday, @date), DATEPART(weekday,@date), DATEPART(DAY,@date), DATEPART(dayofyear, @date)
	set @date = DATEADD(d, 1, @date)
END 