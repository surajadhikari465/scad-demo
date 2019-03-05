DECLARE @scriptKey varchar(128);
SET @scriptKey = 'UpdateMammothClosedStores'

IF(NOT EXISTS(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey
	
	--PBI 29402: Update Mammoth Web Support to hide closed stores
	--in addition to not showing closed stores in MWS, we need to update Mammoth
	--locales which are already closed in Icon but have not been updated with close dates in mammoth
	--(Icon data queried from PRD on 1/10/19)
	UPDATE dbo.Locales_FL
		SET LocaleOpenDate='2007-09-28', LocaleCloseDate='2018-01-07', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10274;
	UPDATE dbo.Locales_FL
		SET LocaleOpenDate='1998-04-29', LocaleCloseDate='2016-11-09', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10085;
	UPDATE dbo.Locales_MA
		SET LocaleOpenDate='2007-09-28', LocaleCloseDate='2017-06-22', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10325;
	UPDATE dbo.Locales_MA
		SET LocaleOpenDate='2007-09-28', LocaleCloseDate='2018-08-28', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10309;
	UPDATE dbo.Locales_MA
		SET LocaleOpenDate='1996-08-30', LocaleCloseDate='2018-03-13', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10052;
	UPDATE dbo.Locales_MA
		SET LocaleOpenDate='1997-01-08', LocaleCloseDate='2016-10-12', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10067;
	UPDATE dbo.Locales_MA
		SET LocaleOpenDate='1996-08-30', LocaleCloseDate='2016-09-13', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10058;
	UPDATE dbo.Locales_MW
		SET LocaleOpenDate='2007-09-28', LocaleCloseDate='2017-03-13', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10369;
	UPDATE dbo.Locales_MW
		SET LocaleOpenDate='1996-02-14', LocaleCloseDate='2017-03-21', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10040;
	UPDATE dbo.Locales_MW
		SET LocaleOpenDate='2002-11-06', LocaleCloseDate='2015-08-26', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10164;
	UPDATE dbo.Locales_MW
		SET LocaleOpenDate='1997-02-05', LocaleCloseDate='2017-01-25', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10068;
	UPDATE dbo.Locales_MW
		SET LocaleOpenDate='1997-12-29', LocaleCloseDate='2017-10-24', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10081;
	UPDATE dbo.Locales_MW
		SET LocaleOpenDate='1995-05-14', LocaleCloseDate='2016-03-15', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10035;
	UPDATE dbo.Locales_NA
		SET LocaleOpenDate='1999-04-30', LocaleCloseDate='2017-03-01', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10093;
	UPDATE dbo.Locales_NA
		SET LocaleOpenDate='2016-08-01', LocaleCloseDate='2017-04-28', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10679;
	UPDATE dbo.Locales_NC
		SET LocaleOpenDate='2012-10-24', LocaleCloseDate='2017-02-13', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10479;
	UPDATE dbo.Locales_PN
		SET LocaleOpenDate='2014-05-21', LocaleCloseDate='2017-04-07', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10564;
	UPDATE dbo.Locales_RM
		SET LocaleOpenDate='2007-09-28', LocaleCloseDate='2017-02-26', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10301;
	UPDATE dbo.Locales_RM
		SET LocaleOpenDate='2007-09-28', LocaleCloseDate='2017-11-12', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10405;
	UPDATE dbo.Locales_RM
		SET LocaleOpenDate='2002-02-06', LocaleCloseDate='2018-05-23', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10141;
	UPDATE dbo.Locales_RM
		SET LocaleOpenDate='2007-09-28', LocaleCloseDate='2017-03-13', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10314;
	UPDATE dbo.Locales_RM
		SET LocaleOpenDate='2014-05-30', LocaleCloseDate='2017-03-19', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10532;
	UPDATE dbo.Locales_RM
		SET LocaleOpenDate='2007-09-28', LocaleCloseDate='2017-10-17', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10440;
	UPDATE dbo.Locales_SO
		SET LocaleOpenDate='2001-11-01', LocaleCloseDate='2017-10-06', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10184;
	UPDATE dbo.Locales_SO
		SET LocaleOpenDate='2014-09-11', LocaleCloseDate='2017-02-22', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10550;
	UPDATE dbo.Locales_SP
		SET LocaleOpenDate='2014-04-24', LocaleCloseDate='2017-02-10', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10582;
	UPDATE dbo.Locales_SP
		SET LocaleOpenDate='2011-06-29', LocaleCloseDate='2017-02-22', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10401;
	UPDATE dbo.Locales_SP
		SET LocaleOpenDate='2007-09-28', LocaleCloseDate='2017-08-04', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10383;
	UPDATE dbo.Locales_SW
		SET LocaleOpenDate='2007-09-28', LocaleCloseDate='2015-02-16', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10296;
	UPDATE dbo.Locales_SW
		SET LocaleOpenDate='1991-09-01', LocaleCloseDate='2016-05-22', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10007;
	UPDATE dbo.Locales_SW
		SET LocaleOpenDate='1993-10-21', LocaleCloseDate='2015-04-04', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10025;
	UPDATE dbo.Locales_SW
		SET LocaleOpenDate='2016-01-26', LocaleCloseDate='2018-05-22', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10656;
	UPDATE dbo.Locales_SW
		SET LocaleOpenDate='2016-03-16', LocaleCloseDate='2017-11-13', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 10663;
	UPDATE dbo.Locales_UK
		SET LocaleOpenDate='2012-11-07', LocaleCloseDate='2017-11-28', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 11114;
	UPDATE dbo.Locales_UK
		SET LocaleOpenDate='2011-11-16', LocaleCloseDate='2017-11-28', ModifiedDate=GETDATE()
		WHERE BusinessUnitID = 11111

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO