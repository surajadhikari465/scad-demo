
create view [zz_deprecated_CIX_Zone] as
select
	zonedesc    Zone_Name,
    r.region_id Region_ID,
    pzone       GLMarketingExpenseAcct
from
   [dbo].cxbzoner z,
   Region r
where
   r.regionname = 'Florida'
   and z.zonetype = 'H'
