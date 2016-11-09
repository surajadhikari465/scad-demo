/*
All master pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.

Please add prints/logging for each statement/block of code appropriately, to allow for tracking, debugging, and troublshooting;
Example/Format:
print '[' + convert(nvarchar, getdate(), 121) + '] [MasterData] TFS ?????: PBI Desc -- Action details...'

*/

go
print '[' + convert(nvarchar, getdate(), 121) + '] [MasterData] TFS ?????: PBI Desc -- Action details...'

--Bug 11892:Tax Romance field allows 150 CHAR but is saying it must be under 50 when updating
update Trait
set traitPattern = '^[\w \-\\/%<>&=\+]{1,150}$'
where traitDesc = 'Tax Romance'

set identity_insert  dbo.trait on 

if not exists (Select 1 from trait where traitcode = 'CTF')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 130, 'CTF', 'Calories From Trans Fat', '', 1
END

if not exists (Select 1 from trait where traitcode = 'CSF')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 131, 'CSF', 'Calories Saturated Fat' , '', 1
END

if not exists (Select 1 from trait where traitcode = 'SPC')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 132, 'SPC', 'Serving Per Container', '', 1
END

if not exists (Select 1 from trait where traitcode = 'SSD')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 133, 'SSD', 'Serving Size Desc', '', 1
END

if not exists (Select 1 from trait where traitcode = 'SPP')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 134, 'SPP', 'Servings Per Portion' , '', 1
END

if not exists (Select 1 from trait where traitcode = 'SUT')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 135, 'SUT', 'Serving Units' , '', 1
END

if not exists (Select 1 from trait where traitcode = 'SWT')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 136, 'SWT', 'Size Weight' , '', 1
END

if not exists (Select 1 from trait where traitcode = 'TFW')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 137, 'TFW', 'Transfat Weight'  , '', 1
END

UPDATE trait
SET traitDesc = 'National Class Code'
WHERE traitDesc = 'Nationcal Class Code'

INSERT INTO [vim].[EventType]
           ([Name])
     VALUES
           ('NationalClassAdd'),
		   ('NationalClassUpdate'),
		   ('NationalClassDelete')


set identity_insert  dbo.trait off



