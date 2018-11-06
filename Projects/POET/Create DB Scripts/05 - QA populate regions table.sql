---------------------------------------------------------------------------------------------------
--Region config data
if not exists (select * from Regions)
	begin
		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (1, 'MA', 'Mid Atlantic', 'idq-ma', 'ItemCatalog')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (2, 'MW', 'Midwest', 'idq-mw', 'ItemCatalog')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (3, 'NA', 'North Atlantic', 'idq-na', 'ItemCatalog')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (4, 'NC', 'Northern California', 'idq-nc', 'ItemCatalog')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (5, 'NE', 'North East', 'idq-ne', 'ItemCatalog')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (6, 'PN', 'Pacific Northwest', 'idq-pn', 'ItemCatalog')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (7, 'RM', 'Rocky Mountain', 'idq-rm', 'ItemCatalog')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (8, 'SP', 'Southern Pacific', 'idq-sp', 'ItemCatalog')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (9, 'SW', 'Southwest', 'idq-sw', 'ItemCatalog')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (10, 'FL', 'Florida', 'idq-fl', 'ItemCatalog')

		INSERT INTO Regions(RegionID, RegionCode, RegionName, IRMAServer, IRMADatabase)
		VALUES (11, 'GL', 'Global', 'null', 'null')
	end
GO