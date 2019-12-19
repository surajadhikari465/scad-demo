:: This file runs the SyncMammoth package with values for package parameters.
:: Set the package parameters in this file in order to update how the application is run.
:: Parameters: 
::	Environment: Specifies the IRMA environment to run against. Possible expected values are: Test0, Test1, QA0, QA1, Prod
::	MammothDatabase: Name of the Mammoth database. Change based on environment.
::	MammothServer: Name of the Mammoth server. Change based on environment.
::	Regions: A comma delimited list of Region codes. Specifies what IRMA regions to load into Mammoth in order to sync Mammoth's data. Full region list: FL,MA,MW,NA,NC,NE,PN,RM,SO,SP,SW
"C:\Program Files\Microsoft SQL Server\140\DTS\Binn\DTExec.exe"^
	/file "SyncMammoth.dtsx"^
	/SET "\Package.Variables[$Package::Environment]";Test0^
	/SET "\Package.Variables[$Package::MammothDatabase]";"Mammoth_Dev"^
	/SET "\Package.Variables[$Package::MammothServer]";"MAMMOTH-DB01-DEV\MAMMOTH"^
	/SET "\Package.Variables[$Package::Regions]";"FL,MA,MW,NA,NC,NE,PN,RM,SO,SP,SW"^