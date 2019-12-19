IF OBJECT_ID('dbo.[tmp_ExistingIconAttributes]', 'U') IS NOT NULL 
 DROP TABLE dbo.tmp_ExistingIconAttributes; 

CREATE TABLE tmp_ExistingIconAttributes
(
   GroupCode Varchar(20),
   GroupDesc varchar(200)
)
IF OBJECT_ID('dbo.[tmp_NewInforAttributes]', 'U') IS NOT NULL 
 DROP TABLE dbo.tmp_NewInforAttributes; 

CREATE TABLE tmp_NewInforAttributes
(
   GroupCode Varchar(20),
   GroupDesc varchar(200)
)

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
SELECT traitGroupCode, traitGroupDesc
from Traitgroup
where traitGroupID=1

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('AnimalWelFareRating', 'AWR')


insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('Biodynamic', 'BIO')


insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('CheeseMilkType', 'CMT')


insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('CheeseRaw', 'CR')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('EcoScaleRating', 'ECO')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('GlutenFree', 'GF')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('Kosher', 'KSH')
insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('Msc', 'Msc')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('NonGmo', 'NGM')


insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('Organic', 'OG')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('PremiumBodyCare', 'PBC')


insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('FreshOrFrozen', 'SFF')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('SeafoodCatchType', 'SFT')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('Vegan', 'VGN')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('Vegetarian', 'VEG')
insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('WholeTrade', 'WT')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('PastureRaised', 'PAS')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('FreeRange', 'FRR')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('DryAged', 'DAG')
insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('AirChilled', 'ACH')

insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('MadeInHouse', 'ABV')
insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('DeliverySystem', 'DS')


insert into tmp_ExistingIconAttributes (GroupCode,GroupDesc)
values('DrainedWeightUom', 'DMU')

--s



insert into tmp_NewInforAttributes (GroupCode,GroupDesc)
select traitcode, displayname
FROM Attributes where traitcode='' or traitcode is null
