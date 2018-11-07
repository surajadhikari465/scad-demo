---------------------------------------------------------------------------------------------------
--Region config data
if not exists (select * from Regions)
	begin
		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (1, 'MA', 'Mid Atlantic', 'idd-ma', 'ItemCatalog_Test')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (2, 'MW', 'Midwest', 'idd-mw', 'ItemCatalog_Test')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (3, 'NA', 'North Atlantic', 'idd-na', 'ItemCatalog_Test')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (4, 'NC', 'Northern California', 'idd-nc', 'ItemCatalog_Test')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (5, 'NE', 'North East', 'idd-ne', 'ItemCatalog_Test')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (6, 'PN', 'Pacific Northwest', 'idd-pn', 'ItemCatalog_Test')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (7, 'RM', 'Rocky Mountain', 'idd-rm', 'ItemCatalog_Test')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (8, 'SP', 'Southern Pacific', 'idd-sp', 'ItemCatalog_Test')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (9, 'SW', 'Southwest', 'idd-sw', 'ItemCatalog_Test')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (10, 'FL', 'Florida', 'idd-fl', 'ItemCatalog_Test')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (11, 'GL', 'Global', 'null', 'null')
	end
GO