﻿
create view [zz_deprecated_CIX_itemUnit] as
select
   case measure_name
        when 'UNIT'
        then uom
        else measure_name end
                    Unit_Name,
   case uom  
		when 'LB' then 1
		--when 'GR' then 1
		--when 'CG' then 1
		--when 'OZ' then 1
		--when 'FZ' then 1
		--when 'KG' then 1
		--when 'RW' then 1
		else    0 end
		            Weight_Unit,
   null             User_ID,
   uom              Unit_Abbreviation,
   null             UnitSysCode,
   --case uom  
		--when 'PK' then 1
		--when 'SZ' then 1
		--else    0 end
   0                IsPackageUnit,
   case when uom in ('KG' ,'HG' ,'BC' ,'LB' ,'HB' ,'QB' ,'FW' ,'FP' ,'OK' ,'OG' ,'OP' ,'OH' ,'OQ' ,'OB')
	then uom else null end	PlumUnitAbbr
-- is this right?  PlumUnitAbbr just thrown on to make it finish.
from
   [dbo].cxbuomcr u
where
	uom <> 'RW'
