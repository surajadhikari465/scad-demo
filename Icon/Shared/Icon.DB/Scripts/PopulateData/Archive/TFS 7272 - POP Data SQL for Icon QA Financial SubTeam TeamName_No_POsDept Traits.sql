
BEGIN

-- ====================================================
-- Declare Internal Variables
-- ====================================================
declare @finanncialClassID int;
declare @posDeptNoTraitID int;
declare @teamNumberTraitID int;
declare @teamNameTraitID int;


-- Traits

SET @posDeptNoTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PDN');
SET @teamNumberTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'NUM');
SET @teamNameTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'NAM');

-- Hierarchy

SET @finanncialClassID	= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Financial')

--Pos dept

IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Grocery (1000)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '242' from HierarchyClass mrc where mrc.hierarchyClassName = 'Grocery (1000)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Frozen (1100)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '243' from HierarchyClass mrc where mrc.hierarchyClassName = 'Frozen (1100)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Dairy (1300)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '244' from HierarchyClass mrc where mrc.hierarchyClassName = 'Dairy (1300)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Bulk (1400)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '245' from HierarchyClass mrc where mrc.hierarchyClassName = 'Bulk (1400)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Produce (1700)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '246' from HierarchyClass mrc where mrc.hierarchyClassName = 'Produce (1700)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Floral (1800)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '247' from HierarchyClass mrc where mrc.hierarchyClassName = 'Floral (1800)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Beer (2100)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '248' from HierarchyClass mrc where mrc.hierarchyClassName = 'Beer (2100)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Wine (2200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '249' from HierarchyClass mrc where mrc.hierarchyClassName = 'Wine (2200)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Cheese (2300)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '252' from HierarchyClass mrc where mrc.hierarchyClassName = 'Cheese (2300)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Meat (2700)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '254' from HierarchyClass mrc where mrc.hierarchyClassName = 'Meat (2700)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Seafood (2800)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '256' from HierarchyClass mrc where mrc.hierarchyClassName = 'Seafood (2800)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Supplements (3000)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '257' from HierarchyClass mrc where mrc.hierarchyClassName = 'Supplements (3000)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Body Care (3700)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '258' from HierarchyClass mrc where mrc.hierarchyClassName = 'Body Care (3700)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Bakery (4200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '262' from HierarchyClass mrc where mrc.hierarchyClassName = 'Bakery (4200)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Prepared Foods (4900)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '263' from HierarchyClass mrc where mrc.hierarchyClassName = 'Prepared Foods (4900)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Lifestyle (6460)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '259' from HierarchyClass mrc where mrc.hierarchyClassName = 'Lifestyle (6460)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Customer Service Non Margin (7000)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '269' from HierarchyClass mrc where mrc.hierarchyClassName = 'Customer Service Non Margin (7000)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Sushi (6200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '267' from HierarchyClass mrc where mrc.hierarchyClassName = 'Sushi (6200)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Coffee Bar (4800)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '266' from HierarchyClass mrc where mrc.hierarchyClassName = 'Coffee Bar (4800)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Juice Bar (4500)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '264' from HierarchyClass mrc where mrc.hierarchyClassName = 'Juice Bar (4500)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Spirits (2220)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '253' from HierarchyClass mrc where mrc.hierarchyClassName = 'Spirits (2220)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Misc Third Party Vendors (6250)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '268' from HierarchyClass mrc where mrc.hierarchyClassName = 'Misc Third Party Vendors (6250)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Marketing (8270)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '272' from HierarchyClass mrc where mrc.hierarchyClassName = 'Marketing (8270)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @posDeptNoTraitID
								where hc.hierarchyClassName = 'Customer Service Margin (7200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @posDeptNoTraitID, mrc.hierarchyClassID, null, '271' from HierarchyClass mrc where mrc.hierarchyClassName = 'Customer Service Margin (7200)' and mrc.hierarchyID = @finanncialClassID; 
END





--Team number


IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Grocery (1000)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '100' from HierarchyClass mrc where mrc.hierarchyClassName = 'Grocery (1000)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Frozen (1100)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '100' from HierarchyClass mrc where mrc.hierarchyClassName = 'Frozen (1100)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Dairy (1300)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '100' from HierarchyClass mrc where mrc.hierarchyClassName = 'Dairy (1300)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Bulk (1400)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '100' from HierarchyClass mrc where mrc.hierarchyClassName = 'Bulk (1400)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Produce (1700)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '120' from HierarchyClass mrc where mrc.hierarchyClassName = 'Produce (1700)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Floral (1800)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '120' from HierarchyClass mrc where mrc.hierarchyClassName = 'Floral (1800)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Beer (2100)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '140' from HierarchyClass mrc where mrc.hierarchyClassName = 'Beer (2100)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Wine (2200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '140' from HierarchyClass mrc where mrc.hierarchyClassName = 'Wine (2200)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Cheese (2300)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '140' from HierarchyClass mrc where mrc.hierarchyClassName = 'Cheese (2300)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Meat (2700)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '160' from HierarchyClass mrc where mrc.hierarchyClassName = 'Meat (2700)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Seafood (2800)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '180' from HierarchyClass mrc where mrc.hierarchyClassName = 'Seafood (2800)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Supplements (3000)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '200' from HierarchyClass mrc where mrc.hierarchyClassName = 'Supplements (3000)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Body Care (3700)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '200' from HierarchyClass mrc where mrc.hierarchyClassName = 'Body Care (3700)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Bakery (4200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '220' from HierarchyClass mrc where mrc.hierarchyClassName = 'Bakery (4200)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Prepared Foods (4900)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '240' from HierarchyClass mrc where mrc.hierarchyClassName = 'Prepared Foods (4900)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Lifestyle (6460)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '200' from HierarchyClass mrc where mrc.hierarchyClassName = 'Lifestyle (6460)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Customer Service Non Margin (7000)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '320' from HierarchyClass mrc where mrc.hierarchyClassName = 'Customer Service Non Margin (7000)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Sushi (6200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '260' from HierarchyClass mrc where mrc.hierarchyClassName = 'Sushi (6200)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Spirits (2220)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '140' from HierarchyClass mrc where mrc.hierarchyClassName = 'Spirits (2220)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Misc Third Party Vendors (6250)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '260' from HierarchyClass mrc where mrc.hierarchyClassName = 'Misc Third Party Vendors (6250)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Marketing (8270)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '420' from HierarchyClass mrc where mrc.hierarchyClassName = 'Marketing (8270)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNumberTraitID
								where hc.hierarchyClassName = 'Customer Service Margin (7200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNumberTraitID, mrc.hierarchyClassID, null, '320' from HierarchyClass mrc where mrc.hierarchyClassName = 'Customer Service Margin (7200)' and mrc.hierarchyID = @finanncialClassID; 
END




---Team Name


IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Grocery (1000)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Grocery' from HierarchyClass mrc where mrc.hierarchyClassName = 'Grocery (1000)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Frozen (1100)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Grocery' from HierarchyClass mrc where mrc.hierarchyClassName = 'Frozen (1100)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Dairy (1300)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Grocery' from HierarchyClass mrc where mrc.hierarchyClassName = 'Dairy (1300)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Bulk (1400)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Grocery' from HierarchyClass mrc where mrc.hierarchyClassName = 'Bulk (1400)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Produce (1700)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Produce' from HierarchyClass mrc where mrc.hierarchyClassName = 'Produce (1700)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Floral (1800)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Produce' from HierarchyClass mrc where mrc.hierarchyClassName = 'Floral (1800)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Beer (2100)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Specialty' from HierarchyClass mrc where mrc.hierarchyClassName = 'Beer (2100)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Wine (2200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Specialty' from HierarchyClass mrc where mrc.hierarchyClassName = 'Wine (2200)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Cheese (2300)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Specialty' from HierarchyClass mrc where mrc.hierarchyClassName = 'Cheese (2300)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Meat (2700)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Meat' from HierarchyClass mrc where mrc.hierarchyClassName = 'Meat (2700)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Seafood (2800)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Seafood' from HierarchyClass mrc where mrc.hierarchyClassName = 'Seafood (2800)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Supplements (3000)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Whole Body' from HierarchyClass mrc where mrc.hierarchyClassName = 'Supplements (3000)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Body Care (3700)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Whole Body' from HierarchyClass mrc where mrc.hierarchyClassName = 'Body Care (3700)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Bakery (4200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Bakery' from HierarchyClass mrc where mrc.hierarchyClassName = 'Bakery (4200)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Prepared Foods (4900)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Prepared Foods' from HierarchyClass mrc where mrc.hierarchyClassName = 'Prepared Foods (4900)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Lifestyle (6460)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Whole Body' from HierarchyClass mrc where mrc.hierarchyClassName = 'Lifestyle (6460)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Customer Service Non Margin (7000)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Customer Service' from HierarchyClass mrc where mrc.hierarchyClassName = 'Customer Service Non Margin (7000)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Sushi (6200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Third Party Vendors' from HierarchyClass mrc where mrc.hierarchyClassName = 'Sushi (6200)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Spirits (2220)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Specialty' from HierarchyClass mrc where mrc.hierarchyClassName = 'Spirits (2220)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Misc Third Party Vendors (6250)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Third Party Vendors' from HierarchyClass mrc where mrc.hierarchyClassName = 'Misc Third Party Vendors (6250)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Marketing (8270)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Marketing' from HierarchyClass mrc where mrc.hierarchyClassName = 'Marketing (8270)' and mrc.hierarchyID = @finanncialClassID; 
END
IF NOT EXISTS (SELECT 1 FROM HierarchyClass hc
								join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @teamNameTraitID
								where hc.hierarchyClassName = 'Customer Service Margin (7200)'  and hc.hierarchyID = @finanncialClassID)
BEGIN 
   insert into HierarchyClassTrait select @teamNameTraitID, mrc.hierarchyClassID, null, 'Customer Service' from HierarchyClass mrc where mrc.hierarchyClassName = 'Customer Service Margin (7200)' and mrc.hierarchyID = @finanncialClassID; 
END
end