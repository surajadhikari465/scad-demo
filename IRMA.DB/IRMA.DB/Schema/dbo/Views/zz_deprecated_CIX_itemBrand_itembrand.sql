
create view [zz_deprecated_CIX_itemBrand_itembrand] as
select distinct
   case len(itembrand) when 0 then 'NO BRAND' else substring(itembrand,1,25) end  brand_name,
   null          user_id
from
   [dbo].cxbupcmr
where
   itembrand is not null
