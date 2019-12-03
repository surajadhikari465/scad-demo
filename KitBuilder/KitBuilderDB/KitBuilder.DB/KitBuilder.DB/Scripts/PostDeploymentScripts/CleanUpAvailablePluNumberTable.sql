declare @PluTable as table (PluNumber int)
insert into @PluTable
select PluNumber 
from [dbo].[InstructionList] il
join [dbo].[InstructionListMember] ilm on il.InstructionListId = ilm.InstructionListId

update apn
set apn.InUse = 0
from [dbo].[AvailablePluNumber] apn
left join @PluTable pt on apn.PluNumber = pt.PluNumber
where pt.PluNumber is null
and InUse = 1