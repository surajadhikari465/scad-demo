
--tables
grant select on ErrorLog to MultiPOToolUsers
grant select on Exception to MultiPOToolUsers
grant select, insert, alter, update, references on POHeader to MultiPOToolUsers
grant select, insert, alter on POItem to MultiPOToolUsers
grant select on POItemException to MultiPOToolUsers
grant select on PONumber to MultiPOToolUsers
grant select on POType to MultiPOToolUsers
grant select on PushToIRMAQueue to MultiPOToolUsers
grant select on Regions to MultiPOToolUsers
grant select on UploadSessionHistory to MultiPOToolUsers
grant select on Users to MultiPOToolUsers
grant select on ValidationQueue to MultiPOToolUsers
grant select, insert on ErrorLog to MultiPOToolUsers
grant select on Version to MultiPOToolUsers

 
--stored procedures
grant exec on AssignPONumbers to MultiPOToolUsers
grant exec on ConfirmPOInIRMA to MultiPOToolUsers
grant exec on DeletePOs to MultiPOToolUsers
grant exec on DeletePONumber to MultiPOToolUsers
grant exec on DeleteUser to MultiPOToolUsers
grant exec on GetAllUsers to MultiPOToolUsers
grant exec on GetUnusedPONumbersByUser to MultiPOToolUsers
grant exec on GetAvailablePONumberIDsByUserID to MultiPOToolUsers
grant exec on GetPOsPushedToIRMA to MultiPOToolUsers
grant exec on GetPOsReadyToPushByUser to MultiPOToolUsers
grant exec on GetPOTypes to MultiPOToolUsers
grant exec on GetRegionsByUser to MultiPOToolUsers
grant exec on GetRegionsToPush to MultiPOToolUsers
grant exec on GetRegionsWithOrdersInQueue to MultiPOToolUsers
grant exec on GetSessionsWithExceptionsByUserID to MultiPOToolUsers
grant exec on GetUserByID to MultiPOToolUsers
grant exec on GetUserIDByUserName to MultiPOToolUsers
grant exec on InsertPushQueue to MultiPOToolUsers
grant exec on InsertSessionHistory to MultiPOToolUsers
grant exec on InsertUser to MultiPOToolUsers
grant exec on InsertErrorLog to MultiPOToolUsers
grant exec on PushPOsToIRMA to MultiPOToolUsers
grant exec on UpdateUser to MultiPOToolUsers
grant exec on ValidatePODataInIRMA to MultiPOToolUsers
grant exec on GetPOHeadersByUploadSessionID to MultiPOToolUsers
grant exec on GetPOItemsByUploadSessionID to MultiPOToolUsers
grant exec on GetLinks to MultiPOToolUsers
grant exec on InsertLink to MultiPOToolUsers
grant exec on UpdateLink to MultiPOToolUsers
grant exec on DeleteLink to MultiPOToolUsers
grant exec on DeleteLink to MultiPOToolUsers
grant exec on GetValidationEmailInfo to MultiPOToolUsers
grant exec on GetAppSetting to MultiPOToolUser
grant exec on GetReasonCodes to MultiPOToolUsers

grant exec on GetVersion to MultiPOToolUsers
grant exec on GetExceptionItemsByUploadSession to MultiPOToolUsers

grant exec on GetExceptionHeadersByUploadSession to MultiPOToolUsers

grant exec on GetStoreNamesbyRegion to MultiPOToolUsers
grant exec on GetSubTeamNamesbyRegion to MultiPOToolUsers
grant exec on GetVendorNamesbyRegion to MultiPOToolUsers

grant exec on GetRegionalUsers to MultiPOToolUsers

grant exec on SendAutoPushFailureNotification to MultiPOToolUsers
GO

-- Read access for report user.
declare @rptUserMap table(
	LoginName nvarchar(max),
	DBname nvarchar(max),
	Username nvarchar(max), 
	AliasName nvarchar(max)
)

insert into @rptUserMap
	EXEC master..sp_msloginmappings 'irmareports'

if exists (
	select * from @rptUserMap where dbname like db_name() -- We should be executing against a POET DB, so we check for report user mapping in DB where this script is being run.
)
begin
	print 'Adding IRMAReports to DB_DataReader role...'
	exec sp_addrolemember N'db_datareader', N'IRMAReports'
end
go

