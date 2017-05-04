--Financial
select 
	null 'Financial Hierarchy ID', --financial hierarchy ID does not exist in IRMA
	null 'Subteam',
	st.SubTeam_Name 'Subteam Name'
from SubTeam st
order by st.SubTeam_Name