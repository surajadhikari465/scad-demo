--Backup the date records to be updated/repopulated
IF OBJECT_ID(N'dbo.Date_Backup', N'U') IS NOT NULL
BEGIN
  drop table dbo.Date_Backup
END

select * into Date_Backup from date where Date_Key > '2017-09-24' 

GO

update Date
set Year = '2017',
	Quarter = 5,
	Period = Period + 13,
	Day_Of_Year = Day_Of_Year + 364
where Date_Key < '2018-01-01'
and Year = '2018'

GO

update Date
set [Year] = YEAR(Date_Key),
	[Quarter] = DATEPART(QUARTER, Date_Key),
	[Period] = MONTH(Date_Key), 
	[Week] = CASE DATEPART(weekday,DATEADD(MM, DATEDIFF(MM,0,Date_Key), 0))
				WHEN 1 -- 1st day of the month is Sunday
					THEN CASE DATEPART(weekday,Date_Key)
							WHEN 1 --Sunday
							THEN DATEPART(WEEK, Date_Key) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,Date_Key), 0)) + 1
						ELSE DATEPART(WEEK, Date_Key) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,Date_Key), 0)) + 2
						END
					ELSE CASE DATEPART(weekday,Date_Key)
							WHEN 1 
							THEN DATEPART(WEEK, Date_Key) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,Date_Key), 0)) 
							ELSE DATEPART(WEEK, Date_Key) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,Date_Key), 0)) + 1
						END
			END,
	[Day_Name] = DATENAME(weekday, Date_Key), 
	[Day_Of_Week] = CASE DATEPART(weekday,Date_Key)
					WHEN 1 THEN 7
					ELSE DATEPART(weekday,Date_Key) - 1
					END,
	[Day_Of_Month] = DATEPART(DAY,Date_Key),
	[Day_Of_Year] = DATEPART(dayofyear, Date_Key)
where Date_Key >= '2018-01-01'

GO

declare @date as datetime = '2020-09-28 00:00:00'

while @date < '2023-01-01'
BEGIN
	insert into Date 
           ([Date_Key]
           ,[Year]
           ,[Quarter]
           ,[Period]
           ,[Week]
           ,[Day_Name]
           ,[Day_Of_Week]
           ,[Day_Of_Month]
           ,[Day_Of_Year])
	select  @date, 
			YEAR(@date), 
			DATEPART(QUARTER, @date), 
			MONTH(@date), 
			CASE DATEPART(weekday,DATEADD(MM, DATEDIFF(MM,0,@date), 0))
				WHEN 1 -- 1st day of the month is Sunday
					THEN CASE DATEPART(weekday,@date)
							WHEN 1 --Sunday
							THEN DATEPART(WEEK, @date) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,@date), 0)) + 1
						ELSE DATEPART(WEEK, @date) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,@date), 0)) + 2
						END
					ELSE CASE DATEPART(weekday,@date)
							WHEN 1 
							THEN DATEPART(WEEK, @date) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,@date), 0)) 
							ELSE DATEPART(WEEK, @date) - DATEPART(WEEK, DATEADD(MM, DATEDIFF(MM,0,@date), 0)) + 1
						END
			END,
			DATENAME(weekday, @date), 
			CASE DATEPART(weekday,@date)
				WHEN 1 THEN 7
				ELSE DATEPART(weekday,@date) - 1
			END,
			DATEPART(DAY,@date), 
			DATEPART(dayofyear, @date)

	set @date = DATEADD(d, 1, @date)
END 