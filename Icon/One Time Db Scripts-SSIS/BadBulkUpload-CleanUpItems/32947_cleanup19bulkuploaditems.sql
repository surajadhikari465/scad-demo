use icon
go

DECLARE @mfmtraitid INT = (
		SELECT traitID
		FROM Trait
		WHERE traitCode = 'MFM'
		)

declare @wfmlocaleid int = (select localeid from Locale where localeName = 'Whole Foods' and localetypeid = 1)

	

create table #tempHierarchies ( itemid int, scancode nvarchar(100), brandhierarchyid int, marchandiseHierarchyId int, taxHieararchyId int, NationalHierarchyId int, FinancialHieararchyId int)

insert into #tempHierarchies (itemid, scancode)
select itemID, scanCode 
from ScanCode 
where scancode in (
'376006350341',
'803318205259',
'60881952292',
'85000610807',
'89145100089',
'83309500051',
'83309500053',
'843700407116',
'67308774519',
'843701352753',
'76001895008',
'66152999114',
'6075318000',
'900466800009',
'900466800151',
'900466800246',
'370031606171',
'370031603161',
'4023226162'
)
-- Brand

update #tempHierarchies set brandhierarchyid = 120071 where scancode = '376006350341'
update #tempHierarchies set brandhierarchyid = 77891 where scancode = '803318205259'
update #tempHierarchies set brandhierarchyid = 2005983 where scancode = '60881952292'
update #tempHierarchies set brandhierarchyid =  59968 where scancode = '85000610807'
update #tempHierarchies set brandhierarchyid = 2005997 where scancode = '89145100089'
update #tempHierarchies set brandhierarchyid = 2005998 where scancode = '83309500051'
update #tempHierarchies set brandhierarchyid = 2005998 where scancode = '83309500053'
update #tempHierarchies set brandhierarchyid = 2006000 where scancode = '843700407116'
update #tempHierarchies set brandhierarchyid = 2006001 where scancode = '67308774519'
update #tempHierarchies set brandhierarchyid = 2005999 where scancode = '843701352753'
update #tempHierarchies set brandhierarchyid = 89332 where scancode = '76001895008'
update #tempHierarchies set brandhierarchyid = 88267 where scancode = '66152999114'
update #tempHierarchies set brandhierarchyid = 2003579 where scancode = '6075318000'
update #tempHierarchies set brandhierarchyid = 99851 where scancode = '900466800009'
update #tempHierarchies set brandhierarchyid = 99851 where scancode = '900466800151'
update #tempHierarchies set brandhierarchyid = 99851 where scancode = '900466800246'
update #tempHierarchies set brandhierarchyid = 90379 where scancode = '370031606171'
update #tempHierarchies set brandhierarchyid = 92000 where scancode = '370031603161'
update #tempHierarchies set brandhierarchyid =  96852 where scancode = '4023226162'

-- merchandise
update #tempHierarchies set marchandiseHierarchyId = 5000176 

-- tax 
update #tempHierarchies set taxHieararchyId = 84253 
update #tempHierarchies set taxHieararchyId =  84246 where scancode in ('843700407116','6075318000')

-- national
update #temphierarchies set nationalhierarchyid = 4002257 where scancode = '376006350341'
update #temphierarchies set nationalhierarchyid = 4002005 where scancode = '803318205259'
update #temphierarchies set nationalhierarchyid = 4002001 where scancode = '60881952292'
update #temphierarchies set nationalhierarchyid = 4001851 where scancode = '85000610807'
update #temphierarchies set nationalhierarchyid = 4001917 where scancode = '89145100089'
update #temphierarchies set nationalhierarchyid = 4002210 where scancode = '83309500051'
update #temphierarchies set nationalhierarchyid = 4002210 where scancode = '83309500053'
update #temphierarchies set nationalhierarchyid = 4002142 where scancode = '843700407116'
update #temphierarchies set nationalhierarchyid = 4002006 where scancode = '67308774519'
update #temphierarchies set nationalhierarchyid = 4002142 where scancode = '843701352753'
update #temphierarchies set nationalhierarchyid = 4002100 where scancode = '76001895008'
update #temphierarchies set nationalhierarchyid = 4001992 where scancode = '66152999114'
update #temphierarchies set nationalhierarchyid = 4002004 where scancode = '6075318000'
update #temphierarchies set nationalhierarchyid = 129297  where scancode = '900466800009'
update #temphierarchies set nationalhierarchyid = 129297  where scancode = '900466800151'
update #temphierarchies set nationalhierarchyid = 129297  where scancode = '900466800246'
update #temphierarchies set nationalhierarchyid = 4001958 where scancode = '370031606171'
update #temphierarchies set nationalhierarchyid = 4001958 where scancode = '370031603161'
update #temphierarchies set nationalhierarchyid = 4001911 where scancode = '4023226162'

--financial
update #tempHierarchies
set FinancialHieararchyId =mfm.traitvalue
FROM HierarchyClass
	LEFT JOIN hierarchyclasstrait mfm ON HierarchyClass.hierarchyClassID = mfm.hierarchyClassID
		AND mfm.traitID = @mfmtraitid
	WHERE HierarchyClass.HIERARCHYID = 1
		AND HierarchyClass.hierarchyClassID = #tempHierarchies.marchandiseHierarchyId


		
		
declare @existingIHCrecords int = (select count(*) from ItemHierarchyClass ihc inner join #tempHierarchies th on ihc.itemID = th.itemid)

if (@existingIHCrecords> 0)
	throw 50001, 'ItemHieararchyClass records already exist for these items. this was unexpected. stopping.', 1;

declare @invalidbrands int = (select count(*) from #tempHierarchies th left join HierarchyClass hc on th.brandhierarchyid = hc.hierarchyClassID and hc.hierarchyID = 2  where hc.hierarchyClassName is null)
declare @invalidmerchandise int = (select count(*) from #tempHierarchies th left join HierarchyClass hc on th.marchandiseHierarchyId = hc.hierarchyClassID and hc.hierarchyID = 1  where hc.hierarchyClassName is  null)
declare @invalidfinancial int = (select count(*) from #tempHierarchies th left join HierarchyClass hc on th.FinancialHieararchyId = hc.hierarchyClassID and hc.hierarchyID = 5 where   hc.hierarchyClassName is  null)
declare @invalidnational int = (select count(*) from #tempHierarchies th left join HierarchyClass hc on th.NationalHierarchyId = hc.hierarchyClassID  and hc.hierarchyID = 6  where hc.hierarchyClassName is  null)
declare @invalidtax int = (select count(*) from #tempHierarchies th left join HierarchyClass hc on th.taxHieararchyId = hc.hierarchyClassID  and hc.hierarchyID = 3  where hc.hierarchyClassName is  null)

if (@invalidbrands > 0 or @invalidmerchandise > 0 or @invalidfinancial > 0 or @invalidnational > 0 or @invalidtax > 0)
	throw 50001, 'Not all hierarchies are mapped to valid hierarchy class ids', 1;


begin try 
	begin transaction inserthierarchyclass

		insert into ItemHierarchyClass (hierarchyClassID, itemID, localeID)
		select  th.brandhierarchyid, th.itemid, @wfmlocaleid from #tempHierarchies th

		insert into ItemHierarchyClass (hierarchyClassID, itemID, localeID)
		select  th.marchandiseHierarchyId, th.itemid, @wfmlocaleid from #tempHierarchies th

		insert into ItemHierarchyClass (hierarchyClassID, itemID, localeID)
		select  th.taxHieararchyId, th.itemid, @wfmlocaleid from #tempHierarchies th

		insert into ItemHierarchyClass (hierarchyClassID, itemID, localeID)
		select  th.NationalHierarchyId, th.itemid, @wfmlocaleid from #tempHierarchies th

		insert into ItemHierarchyClass (hierarchyClassID, itemID, localeID)
		select  th.FinancialHieararchyId, th.itemid, @wfmlocaleid from #tempHierarchies th

	COMMIT TRANSACTION inserthierarchyclass
END TRY

BEGIN CATCH  
IF (@@TRANCOUNT > 0)
	BEGIN
		ROLLBACK TRANSACTION inserthierarchyclass
		PRINT 'Error detected, all changes reversed'
	END 
	SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_STATE() AS ErrorState,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage
END CATCH

drop table #tempHierarchies

print 'done'






