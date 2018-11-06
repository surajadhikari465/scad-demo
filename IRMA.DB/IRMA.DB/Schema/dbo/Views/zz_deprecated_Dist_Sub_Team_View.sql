create view [zz_deprecated_Dist_Sub_Team_View] AS
select distinct
        a.SubTeam_No,
        a.Team_No
from
        SubTeam a
