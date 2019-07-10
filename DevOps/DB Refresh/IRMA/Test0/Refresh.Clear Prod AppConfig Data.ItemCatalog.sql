use itemcatalog_Test

update appconfigapp
set configuration =
'<configuration>
  <appSettings>
    <add key="region" value="" />
  </appSettings>
</configuration>'
from appconfigapp a 
join appconfigenv e on a.environmentid = e.environmentid 
where e.shortname = 'PRD'

update appconfigvalue
set value = ''
from appconfigvalue v 
join appconfigapp a on v.environmentid = a.environmentid and v.applicationid = a.applicationid
join appconfigenv e on v.environmentid = e.environmentid 
where e.shortname = 'PRD'
