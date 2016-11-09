EXECUTE sp_addlinkedserver @server = N'TEST - IRMA (UK)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDT-UK\UKT';


GO
EXECUTE sp_addlinkedserver @server = N'TEST - IRMA (SW)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDT-SW\SWT';


GO
EXECUTE sp_addlinkedserver @server = N'TEST - IRMA (SP)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDT-SP\SPT', @catalog = N'ItemCatalog_Test';


GO
EXECUTE sp_addlinkedserver @server = N'TEST - IRMA (PN)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDT-PN\PNT', @catalog = N'ItemCatalog_Test';


GO
EXECUTE sp_addlinkedserver @server = N'TEST - IRMA (NE)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDT-NE\NET', @catalog = N'ItemCatalog_Test';


GO
EXECUTE sp_addlinkedserver @server = N'TEST - IRMA (NC)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDT-NC\NCT', @catalog = N'ItemCatalog_Test';


GO
EXECUTE sp_addlinkedserver @server = N'TEST - ICON', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'CEWD1815\SQLSHARED2012D', @catalog = N'ICON';


GO
EXECUTE sp_addlinkedserver @server = N'DEV - IRMA (SO)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDD-SO\SOD';


GO
EXECUTE sp_addlinkedserver @server = N'DEV - IRMA (RM)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDD-RM\RMD';


GO
EXECUTE sp_addlinkedserver @server = N'DEV - IRMA (NA)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDD-NA\NAD';


GO
EXECUTE sp_addlinkedserver @server = N'DEV - IRMA (MW)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDD-MW\MWD';


GO
EXECUTE sp_addlinkedserver @server = N'DEV - IRMA (MA)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDD-MA\MAD', @catalog = N'ItemCatalog_Test';


GO
EXECUTE sp_addlinkedserver @server = N'DEV - IRMA (FL)', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'IDD-FL\FLD';


GO
EXECUTE sp_addlinkedserver @server = N'DEV - ICON', @srvproduct = N'', @provider = N'SQLNCLI', @datasrc = N'CEWD1815\SQLSHARED2012D', @catalog = N'ICONDEV';

