
create view [zz_deprecated_CIX_Team] as
select distinct
   dp_group_number  Team_No,
   dp_dept_desc     Team_Name,
   dp_pos_dept_desc Team_Abbreviation
from
   [dbo].dept
where
   dp_dept_number = dp_group_number
   and 	dp_dept_number not in (4999, 9999)
