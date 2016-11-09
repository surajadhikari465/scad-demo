print N'Populating ScanCode...'

SET IDENTITY_INSERT ScanCode ON
GO

insert into ScanCode(scanCodeID, itemID, scanCode, scanCodeTypeID, localeID)
values	(	9673	,	0	,	'203'	,	2	,	1	),
		(	9674	,	1	,	'204'	,	2	,	1	),
		(	9676	,	3	,	'206'	,	2	,	1	),
		(	9677	,	4	,	'207'	,	2	,	1	),
		(	9678	,	5	,	'209'	,	2	,	1	),
		(	14778	,	819	,	'6394'	,	2	,	1	),
		(	14877	,	918	,	'8635'	,	2	,	1	),
		(	15044	,	1085	,	'10005'	,	2	,	1	),
		(	15045	,	1086	,	'10006'	,	2	,	1	),
		(	15048	,	1089	,	'10010'	,	2	,	1	)
GO

SET IDENTITY_INSERT ScanCode OFF
GO