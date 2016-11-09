
create view [zz_deprecated_CIX_itemBrand_itembrand] as
select distinct
   case len(ltrim(rtrim(substring(isnull(u.itembrand,u.miscalpha1),1,25)))) when 0 then 'NO BRAND' else ltrim(ltrim(rtrim(substring(isnull(u.itembrand,u.miscalpha1),1,25)))) end brand_name,
   null          user_id
from
   [dbo].cxbupcmr u
where
   itembrand is not null