create view [zz_deprecated_Dist_Sub_Team_View] AS
select distinct
        a.SubTeam_No as me,
        a.SubTeam_No as you
from
        SubTeam a