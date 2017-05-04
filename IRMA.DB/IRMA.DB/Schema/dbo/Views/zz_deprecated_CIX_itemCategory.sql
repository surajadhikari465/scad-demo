
create view [zz_deprecated_CIX_itemCategory] as
select distinct
   c.class_name + cast(c.class as char) Category_Name,
   u.dept       SubTeam_No,
   null         User_ID,
   null			SubTeam_Type_ID
from
   [dbo].cxbupcmr u,
   [dbo].cmmclasr c
where
   c.class = u.class
