/*
All master pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.

Please add prints/logging for each statement/block of code appropriately, to allow for tracking, debugging, and troublshooting;
Example/Format:
print '[' + convert(nvarchar, getdate(), 121) + '] [MasterData] TFS ?????: PBI Desc -- Action details...'

*/

	IF NOT EXISTS(SELECT * FROM vim.StorePosType WHERE Name = 'CLOSED')
BEGIN
	SET IDENTITY_INSERT vim.StorePosType  ON
	insert into vim.StorePosType(StorePosTypeId, Name)
	values (7, 'CLOSED')
	SET IDENTITY_INSERT vim.StorePosType  OFF
END

GO

INSERT INTO [app].[App]
           ([AppName])
     VALUES
           ('Vim Locale Controller')
GO

set identity_insert  dbo.trait on 

--------------------- SLAW traits------------------------------------------
if not exists (Select 1 from trait where traitcode = 'AWR')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 76, 'AWR', 'Animal Welfare Rating', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
END

if not exists (Select 1 from trait where traitcode = 'BIO')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 77, 'BIO', 'Biodynamic', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'CMT')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 78, 'CMT', 'Cheese Milk Type', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'CR')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
SELECT 79, 'CR', 'Cheese Raw', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'HER')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 80, 'HER', 'Healthy Eating Rating', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'ACH')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 82, 'ACH', 'Air Chilled', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'DAG')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 83, 'DAG', 'Dry Aged', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'FRR')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 84, 'FRR', 'Free Range', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'GRF')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 85, 'GRF', 'Grass Fed', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'MIH')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 86, 'MIH', 'Made in House', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'MSC')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 87, 'MSC', 'MSC', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'PAS')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
SELECT 88, 'PAS', 'Pasture Raised', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'SFT')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 89, 'SFT', 'Seafood Catch Type', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end

if not exists (Select 1 from trait where traitcode = 'SFF')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 90, 'SFF', 'Fresh or Frozen', '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$', 1
end


--------------------------Nutrition Traits-------------------------------------------------

if not exists (Select 1 from trait where traitcode = 'RCN')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 91, 'RCN','Recipe Name', '', 1
END

if not exists (Select 1 from trait where traitcode = 'ALR')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 92,'ALR','Allergens', '', 1
END

if not exists (Select 1 from trait where traitcode = 'ING')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 93,'ING','Ingredients', '', 1
END

if not exists (Select 1 from trait where traitcode = 'SLM')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 94,'SLM','Selenium', '', 1
END

if not exists (Select 1 from trait where traitcode = 'PUF')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 95,'PUF','Polyunsaturated Fat', '', 1
END

if not exists (Select 1 from trait where traitcode = 'MUF')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 96,'MUF','Monounsaturated Fat', '', 1
END

if not exists (Select 1 from trait where traitcode = 'PTW')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 97,'PTW','Potassium Weight', '', 1
END

if not exists (Select 1 from trait where traitcode = 'PTP')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 98,'PTP','Potassium Percent', '', 1
END

if not exists (Select 1 from trait where traitcode = 'DFP')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 99,'DFP','Dietary Fiber Percent', '', 1
END

if not exists (Select 1 from trait where traitcode = 'SBF')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 100,'SBF','Soluble Fiber', '', 1
END

if not exists (Select 1 from trait where traitcode = 'ISF')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 101,'ISF','Insoluble Fiber', '', 1
END

if not exists (Select 1 from trait where traitcode = 'SAL')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 102,'SAL','Sugar Alcohol', '', 1
END

if not exists (Select 1 from trait where traitcode = 'OCH')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 103,'OCH','Other Carbohydrates', '', 1
END

if not exists (Select 1 from trait where traitcode = 'PPT')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 104,'PPT','Protein Percent', '', 1
END

if not exists (Select 1 from trait where traitcode = 'BCN')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 105,'BCN','Betacarotene', '', 1
END

if not exists (Select 1 from trait where traitcode = 'VTD')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 106,'VTD','Vitamin D', '', 1
END

if not exists (Select 1 from trait where traitcode = 'VTE')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 107,'VTE','Vitamin E', '', 1
END

if not exists (Select 1 from trait where traitcode = 'THM')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 108,'THM','Thiamin', '', 1
END

if not exists (Select 1 from trait where traitcode = 'RFN')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 109,'RFN','Riboflavin', '', 1
END

if not exists (Select 1 from trait where traitcode = 'NAC')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 110,'NAC','Niacin', '', 1
END

if not exists (Select 1 from trait where traitcode = 'VB6')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 111,'VB6','Vitamin B6', '', 1
END

if not exists (Select 1 from trait where traitcode = 'FLT')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 112,'FLT','Folate', '', 1
END

if not exists (Select 1 from trait where traitcode = 'B12')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 113,'B12','Vitamin B12', '', 1
END

if not exists (Select 1 from trait where traitcode = 'BTN')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 114,'BTN','Biotin', '', 1
END

if not exists (Select 1 from trait where traitcode = 'PAD')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 115,'PAD','Pantothenic Acid', '', 1
END

if not exists (Select 1 from trait where traitcode = 'PPH')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 116,'PPH','Phosphorous', '', 1
END

if not exists (Select 1 from trait where traitcode = 'IDN')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 117,'IDN','Iodine', '', 1
END

if not exists (Select 1 from trait where traitcode = 'MGM')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 118,'MGM','Magnesium', '', 1
END

if not exists (Select 1 from trait where traitcode = 'ZNC')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 119,'ZNC','Zinc', '', 1
END

if not exists (Select 1 from trait where traitcode = 'CPR')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 120,'CPR','Copper', '', 1
END

if not exists (Select 1 from trait where traitcode = 'TSF')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 121,'TSF','Transfat', '', 1
END

if not exists (Select 1 from trait where traitcode = 'O6F')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 122,'O6F','Om6 Fatty', '', 1
END

if not exists (Select 1 from trait where traitcode = 'O3F')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 123,'O3F','Om3 Fatty', '', 1
END

if not exists (Select 1 from trait where traitcode = 'STR')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 124,'STR','Starch', '', 1
END

if not exists (Select 1 from trait where traitcode = 'CHR')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 125,'CHR','Chloride', '', 1
END

if not exists (Select 1 from trait where traitcode = 'CHM')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 126,'CHM','Chromium', '', 1
END

if not exists (Select 1 from trait where traitcode = 'VTK')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 127,'VTK','Vitamin K', '', 1
END

if not exists (Select 1 from trait where traitcode = 'MGE')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 128,'MGE','Manganese', '', 1
END

if not exists (Select 1 from trait where traitcode = 'MBD')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 129,'MBD','Molybdenum', '', 1
END

if not exists (Select 1 from trait where traitcode = 'SLM')
begin
	insert into trait (traitID, traitCode, traitDesc, traitPattern, traitGroupID)
	SELECT 130,'SLM','Selenium', '', 1
END


set identity_insert  dbo.trait off



SET IDENTITY_INSERT [dbo].[Locale] ON 


IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1100)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1100, 1, N'365', CAST(N'2015-11-13' AS Date), NULL, 1, NULL)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1101)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1101, 1, N'365_Florida', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1102)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1102, 1, N'365_Mid Atlantic', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1103)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1103, 1, N'365_Mid West', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1104)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1104, 1, N'365_North Atlantic', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1105)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1105, 1, N'365_Northern California', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1106)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1106, 1, N'365_North East', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1107)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1107, 1, N'365_Pacific Northwest', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1108)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1108, 1, N'365_Rocky Mountain', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1109)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1109, 1, N'365_South', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1110)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1110, 1, N'365_Southern Pacific', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1111)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1111, 1, N'365_Southwest', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1112)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) 
	VALUES (1112, 1, N'365_United Kingdom', NULL, NULL, 2, 1100)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1113)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1113, 1, N'365_MET_FL', CAST(N'1980-09-21' AS Date), NULL, 3, 1101)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1114)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1114, 1, N'365_MET_DC', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1115)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1115, 1, N'365_MET_KY', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1116)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1116, 1, N'365_MET_MD', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1117)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1117, 1, N'365_MET_NJ', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1118)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1118, 1, N'365_MET_OH', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1119)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1119, 1, N'365_MET_PA', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1120)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1120, 1, N'365_MET_VA', CAST(N'1980-09-21' AS Date), NULL, 3, 1102)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1121)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1121, 1, N'365_MET_CHI', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1122)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1122, 1, N'365_MET_IA', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1123)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1123, 1, N'365_MET_IL', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1124)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1124, 1, N'365_MET_IN', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1125)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1125, 1, N'365_MET_MI', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1126)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1126, 1, N'365_MET_MN', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1127)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1127, 1, N'365_MET_MO', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1128)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1128, 1, N'365_MET_NEB', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1129)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1129, 1, N'365_MET_ON', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1130)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1130, 1, N'365_MET_WI', CAST(N'1980-09-21' AS Date), NULL, 3, 1103)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1131)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1131, 1, N'365_MET_BOS', CAST(N'1980-09-21' AS Date), NULL, 3, 1104)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1132)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1132, 1, N'365_MET_NA_OTHER', CAST(N'1980-09-21' AS Date), NULL, 3, 1104)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1133)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1133, 1, N'365_MET_EBY', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1134)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1134, 1, N'365_MET_FRS', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1135)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1135, 1, N'365_MET_MRN', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1136)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1136, 1, N'365_MET_NPA', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1137)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1137, 1, N'365_MET_PEN', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1138)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1138, 1, N'365_MET_REN', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1139)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1139, 1, N'365_MET_SAC', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1140)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1140, 1, N'365_MET_SFO', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1141)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1141, 1, N'365_MET_SJC', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1142)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1142, 1, N'365_MET_STZ', CAST(N'1980-09-21' AS Date), NULL, 3, 1105)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1143)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1143, 1, N'365_MET_LI', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1144)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1144, 1, N'365_MET_NE_CT', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1145)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1145, 1, N'365_MET_NE_NJ', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1146)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1146, 1, N'365_MET_NYC', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1147)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1147, 1, N'365_MET_UP_NY', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1148)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1148, 1, N'365_MET_WESTCH', CAST(N'1980-09-21' AS Date), NULL, 3, 1106)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1149)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1149, 1, N'365_MET_CP', CAST(N'1980-09-21' AS Date), NULL, 3, 1107)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1150)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1150, 1, N'365_MET_OR', CAST(N'1980-09-21' AS Date), NULL, 3, 1107)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1151)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1151, 1, N'365_MET_WA', CAST(N'1980-09-21' AS Date), NULL, 3, 1107)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1152)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1152, 1, N'365_MET_CO', CAST(N'1980-09-21' AS Date), NULL, 3, 1108)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1153)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1153, 1, N'365_MET_ID', CAST(N'1980-09-21' AS Date), NULL, 3, 1108)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1154)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1154, 1, N'365_MET_KS', CAST(N'1980-09-21' AS Date), NULL, 3, 1108)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1155)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1155, 1, N'365_MET_NM', CAST(N'1980-09-21' AS Date), NULL, 3, 1108)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1156)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1156, 1, N'365_MET_UT', CAST(N'1980-09-21' AS Date), NULL, 3, 1108)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1157)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1157, 1, N'365_MET_AL', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1158)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1158, 1, N'365_MET_GA', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1159)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1159, 1, N'365_MET_MS', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1160)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1160, 1, N'365_MET_NC', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1161)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1161, 1, N'365_MET_SC', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1162)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1162, 1, N'365_MET_TN', CAST(N'1980-09-21' AS Date), NULL, 3, 1109)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1163)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1163, 1, N'365_MET_AZ', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1164)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1164, 1, N'365_MET_HI', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1165)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1165, 1, N'365_MET_LA_EAST', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1166)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1166, 1, N'365_MET_LA_WEST', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1167)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1167, 1, N'365_MET_NV', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1168)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1168, 1, N'365_MET_OC', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1169)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1169, 1, N'365_MET_SD', CAST(N'1980-09-21' AS Date), NULL, 3, 1110)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1170)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1170, 1, N'365_MET_AUS', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1171)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1171, 1, N'365_MET_BR', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1172)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1172, 1, N'365_MET_DFW', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1173)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1173, 1, N'365_MET_HOU', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1174)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1174, 1, N'365_MET_LFY', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1175)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1175, 1, N'365_MET_LR', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1176)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1176, 1, N'365_MET_NO', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1177)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1177, 1, N'365_MET_OK', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1178)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1178, 1, N'365_MET_SA', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1179)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1179, 1, N'365_MET_SRV', CAST(N'1980-09-21' AS Date), NULL, 3, 1111)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1180)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1180, 1, N'365_MET_ENG', CAST(N'1980-09-21' AS Date), NULL, 3, 1112)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1181)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1181, 1, N'365_MET_LDN', CAST(N'1980-09-21' AS Date), NULL, 3, 1112)
END
IF NOT EXISTS (SELECT 1 FROM Locale WHERE localeID = 1182)
BEGIN
	INSERT [dbo].[Locale] ([localeID], [ownerOrgPartyID], [localeName], [localeOpenDate], [localeCloseDate], [localeTypeID], [parentLocaleID]) VALUES (1182, 1, N'365_MET_SCT', CAST(N'1980-09-21' AS Date), NULL, 3, 1112)
END
SET IDENTITY_INSERT [dbo].[Locale] OFF

