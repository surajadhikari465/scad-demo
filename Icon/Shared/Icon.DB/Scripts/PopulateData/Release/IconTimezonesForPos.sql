/*
Update dbo.Timezone with data for the new posTimeZoneName column.
Messages to R10 will be build with values from this column
*/

declare @scriptKey varchar(128) =' TFS 15692: Timezones for R10'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print 'Executing :' + @scriptKey

	--populate the PosTimeZoneName column and make sure the old timezone names have the correcct values
	UPDATE [dbo].[Timezone]
	   SET [posTimeZoneName] = 'Hawaiian Standard Time', [timezoneName] = '(UTC-10:00) Hawaii'
	 WHERE [timezoneCode]='HST'
	UPDATE [dbo].[Timezone]
	   SET [posTimeZoneName] = 'Alaskan Standard Time', [timezoneName] = '(UTC-09:00) Alaska'
	 WHERE [timezoneCode]='AKST'
	UPDATE [dbo].[Timezone]
	   SET [posTimeZoneName] = 'Pacific Standard Time', [timezoneName] = '(UTC-08:00) Pacific Time (US & Canada)'
	 WHERE [timezoneCode]='PST'
	UPDATE [dbo].[Timezone]
	   SET [posTimeZoneName] = 'Mountain Standard Time', [timezoneName] = '(UTC-07:00) Mountain Time (US & Canada)'
	 WHERE [timezoneCode]='MST'
	UPDATE [dbo].[Timezone]
	   SET [posTimeZoneName] = 'Central Standard Time', [timezoneName] = '(UTC-06:00) Central Time (US & Canada)'
	 WHERE [timezoneCode]='CST'
	UPDATE [dbo].[Timezone]
	   SET [posTimeZoneName] = 'Eastern Standard Time', [timezoneName] = '(UTC-05:00) Eastern Time (US & Canada)'
	 WHERE [timezoneCode]='EST'
	UPDATE [dbo].[Timezone]
	   SET [posTimeZoneName] = 'Atlantic Standard Time', [timezoneName] = '(UTC-04:00) Atlantic Time (Canada)'
	 WHERE [timezoneCode]='AST'
	UPDATE [dbo].[Timezone]
	   SET [posTimeZoneName] = 'Greenwich Mean Time', [timezoneName] = '(UTC) Dublin, Edinburgh, Lisbon, London'
	 WHERE [timezoneCode]='GMT' 
	--add special timezone for Indiana 
	IF EXISTS (SELECT * FROM [dbo].[Timezone] WHERE [timezoneCode]='ESTIN')
	BEGIN
		UPDATE [dbo].[Timezone]
		   SET [timezoneCode] ='ESTIN', [posTimeZoneName] = 'US Eastern Standard Time', [timezoneName] = '(UTC-05:00) Indiana (East)', [gmtOffset] = -5
		 WHERE [timezoneCode]='ESTIN'
	END ELSE BEGIN
		INSERT INTO [dbo].[Timezone] ([timezoneCode],[posTimeZoneName],[timezoneName],[gmtOffset])
			 VALUES ('ESTIN', 'US Eastern Standard Time', '(UTC-05:00) Indiana (East)', -5)
	END
	--add special timezone for Arizona
	IF EXISTS (SELECT * FROM [dbo].[Timezone] WHERE [timezoneCode]='MSTAZ')
	BEGIN
		UPDATE [dbo].[Timezone]
		   SET [timezoneCode] ='MSTAZ', [posTimeZoneName] = 'US Mountain Standard Time', [timezoneName] = '(UTC-07:00) Arizona', [gmtOffset] = -7
		 WHERE [timezoneCode]='MSTAZ'
	END ELSE BEGIN
		INSERT INTO [dbo].[Timezone]  
				([timezoneCode],[posTimeZoneName],[timezoneName],[gmtOffset])
		 VALUES ('MSTAZ', 'US Mountain Standard Time', '(UTC-07:00) Arizona', -7)
	END 
	--delete unused Newfoundland time zone
	IF EXISTS (SELECT * FROM [dbo].[Timezone] WHERE [timezoneCode]='NST')
	BEGIN
		DELETE [dbo].[Timezone] WHERE [timezoneCode]='NST'
	END 	

	--record that this script has been run
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
	print 'Finished :' + @scriptKey
END
ELSE
BEGIN
	print 'Skipping :' + @scriptKey
END --TFS 15692: Timezones for R10
GO
