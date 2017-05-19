

if object_id('tempdb..#iconNationalClasses') IS NOT NULL 
	drop table #iconNationalClasses
create table #iconNationalClasses
(
	Id int primary key,
	ParentId int null,
	[Level] int,
	Name nvarchar(255),
	Code nvarchar(255) null
)

if object_id('tempdb..#iconView') IS NOT NULL 
	drop table #iconView
create table #iconView
(
	IconFamilyId int,
	FamilyName nvarchar(255),
	IconCategoryId int,
	CategoryName nvarchar(255),
	IconSubCategoryId int,
	SubCategoryName nvarchar(255),
	IconClassId int,
	ClassName nvarchar(255),
	Code nvarchar(255)
)

if object_id('tempdb..#irmaView') IS NOT NULL 
	drop table #irmaView
create table #irmaView
(
	IrmaFamilyId int,
	FamilyName nvarchar(255),
	IrmaCategoryId int,
	CategoryName nvarchar(255),
	IrmaSubCategoryId int,
	SubCategoryName nvarchar(255),
	IrmaClassId int,
	ClassName nvarchar(255),
	Code nvarchar(255)
)

bulk insert #iconNationalClasses
from '\\irmadevfile\Data\Icon\iconnationalclasses_5.txt'
with 
(
	FIRSTROW = 2,
	FIELDTERMINATOR = '|',
	ROWTERMINATOR = '\n'
);

insert into #iconView
select 
	family.Id
	,family.Name
	,category.Id
	,category.Name
	,subCategory.Id
	,subCategory.Name
	,class.Id
	,class.Name
	,class.Code
from #iconNationalClasses family
left join #iconNationalClasses category on family.Id = category.ParentId
left join #iconNationalClasses subCategory on category.Id = subCategory.ParentId
left join #iconNationalClasses class on subCategory.Id = class.ParentId
where family.Level = 1

insert into #irmaView
select 
	family.NatFamilyID IrmaFamilyId
	,LTRIM(RTRIM(SUBSTRING(family.NatFamilyName, 0, CHARINDEX('-', family.NatFamilyName)))) FamilyName
	,family.NatFamilyID IrmaCategoryId
	,LTRIM(RTRIM(SUBSTRING(family.NatFamilyName, CHARINDEX('-', family.NatFamilyName) + 1, LEN(family.NatFamilyName)))) CategoryName
	,category.NatCatID IrmaSubCategoryId
	,category.NatCatName SubCategoryName
	,class.ClassID IrmaClassId
	,class.ClassName ClassName
	,class.ClassID
from NatItemFamily family
left join NatItemCat category on family.NatFamilyID = category.NatFamilyID
left join NatItemClass class on category.NatCatID = class.NatCatID

--Insert ValidatedNationalClass records
insert into ValidatedNationalClass(IconId, IrmaId, Level)
select distinct 
	icon.IconFamilyId,
	irma.IrmaFamilyId,
	1
from #irmaView irma
join #iconView icon on irma.FamilyName = icon.FamilyName
where not exists 
	(
		select 1
		from ValidatedNationalClass vnc 
		where vnc.IconId = icon.IconFamilyId and vnc.IrmaId = irma.IrmaFamilyId and Level = 1
	)

insert into ValidatedNationalClass(IconId, IrmaId, Level)
select distinct 
	icon.IconCategoryId,
	irma.IrmaCategoryId,
	2
from #irmaView irma
join #iconView icon on irma.FamilyName = icon.FamilyName
	and irma.CategoryName = icon.CategoryName
where not exists 
	(
		select 1
		from ValidatedNationalClass vnc 
		where vnc.IconId = icon.IconCategoryId 
			and vnc.IrmaId = irma.IrmaCategoryId 
			and Level = 2
	)

insert into ValidatedNationalClass(IconId, IrmaId, Level)
select distinct 
	icon.IconSubCategoryId,
	irma.IrmaSubCategoryId,
	3
from #irmaView irma
join #iconView icon on irma.FamilyName = icon.FamilyName
	and irma.CategoryName = icon.CategoryName
	and irma.SubCategoryName = icon.SubCategoryName
where not exists 
	(
		select 1
		from ValidatedNationalClass vnc 
		where vnc.IconId = icon.IconSubCategoryId 
			and vnc.IrmaId = irma.IrmaSubCategoryId 
			and Level = 3
	)

insert into ValidatedNationalClass(IconId, IrmaId, Level)
select distinct 
	icon.IconClassId,
	irma.IrmaClassId,
	4
from #irmaView irma
join #iconView icon on irma.Code = icon.Code
where not exists 
	(
		select 1
		from ValidatedNationalClass vnc 
		where vnc.IconId = icon.IconClassId 
			and vnc.IrmaId = irma.IrmaClassId 
			and Level = 4
	)

--Validation
--select count(*) from ValidatedNationalClass

--select * from #iconView
--order by FamilyName, CategoryName, SubCategoryName, ClassName
--select * from #irmaView
--order by FamilyName, CategoryName, SubCategoryName, ClassName