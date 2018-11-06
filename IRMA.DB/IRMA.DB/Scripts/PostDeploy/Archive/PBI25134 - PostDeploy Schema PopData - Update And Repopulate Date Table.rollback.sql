update  d
set	   d.[Date_Key] = b.[Date_Key]
      ,d.[Year] = b.[Year]
      ,d.[Quarter] = b.[Quarter]
      ,d.[Period] = b.[Period]
      ,d.[Week] = b.[Week]
      ,d.[Day_Name] = b.[Day_Name]
      ,d.[Day_Of_Week] = b.[Day_Of_Week]
      ,d.[Day_Of_Month] = b.[Day_Of_Month]
      ,d.[Day_Of_Year] = b.[Day_Of_Year]
from	Date d
join	Date_Backup b on d.Date_Key = b.Date_Key 

GO

delete d
from Date d
left join Date_Backup b on d.Date_Key = b.Date_Key
where b.Date_Key is null
and d.Date_Key >  '2017-09-24'

GO

drop table dbo.Date_Backup
GO
