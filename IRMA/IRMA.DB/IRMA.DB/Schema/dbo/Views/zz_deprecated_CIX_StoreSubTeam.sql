
create view [zz_deprecated_CIX_StoreSubTeam] as
select
   s.Store_no       Store_No,
   st.Team_no       Team_No,
   st.SubTeam_no    SubTeam_No,
   1                CasePriceDiscount,
   1                CostFactor,
   null               ICVID
from
   Store s,
   SubTeam st
