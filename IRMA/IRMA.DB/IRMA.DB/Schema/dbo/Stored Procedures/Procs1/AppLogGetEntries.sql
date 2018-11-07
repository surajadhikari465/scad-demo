/*
grant exec on dbo.[AppLogGetEntries] to IRMAClientRole, IRMAAdminRole, IRMASchedJobsRole, IRMASupportRole, IRMAReportsRole
*/

create procedure [dbo].[AppLogGetEntries]
	@appName varchar(64)
	,@startDate datetime
	,@endDate datetime
	,@SeachTable int = 1
as

Declare   @LogSearch as TABLE
(	AppName varchar(50) ,
	EnvShortName varchar(5) ,
	Id int,
	LogDate datetime,
	ApplicationID uniqueidentifier ,
	HostName varchar(64) ,
	UserName varchar(64) ,
	Thread varchar(255) ,
	Level varchar(50) ,
	Logger varchar(255) ,
	Message varchar(4000) ,
	Exception varchar(2000),
	InsertDate datetime 
	)
If (@SeachTable = 1 OR @SeachTable = 3)
INSERT INTO @LogSearch
select top 2000
	AppName = aca.name
	,EnvShortName = ace.shortname
	,al.*
from
	applog al (nolock)
	join appconfigapp aca (nolock)
	on al.applicationid = aca.applicationid
	join appconfigenv ace (nolock)
	on aca.environmentid = ace.environmentid
where
	replace(lower(aca.name), ' ', '') = replace(lower(@appName), ' ', '') -- Compare lowercase names with spaces removed.
	and al.logdate between isnull(@startDate, al.logdate) and isnull(@endDate, al.logdate)
order by
	logdate desc
	
	If (@SeachTable = 2 OR @SeachTable = 3)
	INSERT INTO @LogSearch
select top 2000
	AppName = aca.name
	,EnvShortName = ace.shortname
	,al.*
from
	AppLogArchive al (nolock)
	join appconfigapp aca (nolock)
	on al.applicationid = aca.applicationid
	join appconfigenv ace (nolock)
	on aca.environmentid = ace.environmentid
where
	replace(lower(aca.name), ' ', '') = replace(lower(@appName), ' ', '') -- Compare lowercase names with spaces removed.
	and al.logdate between isnull(@startDate, al.logdate) and isnull(@endDate, al.logdate)
order by
	logdate desc
	
	Select top 2000 * from @LogSearch order by LogDate DESC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogGetEntries] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogGetEntries] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogGetEntries] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogGetEntries] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppLogGetEntries] TO [IRMAReportsRole]
    AS [dbo];

