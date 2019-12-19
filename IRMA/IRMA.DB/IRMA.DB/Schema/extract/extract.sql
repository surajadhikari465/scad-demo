﻿CREATE SCHEMA [extract]
AUTHORIZATION [dbo];
GO

GRANT INSERT ON SCHEMA :: [extract] TO [IRSUser];
GO
GRANT SELECT ON SCHEMA :: [extract] TO [IRSUser];
GO

GRANT INSERT ON SCHEMA :: [extract] TO [IRMAClientRole];
GO
GRANT SELECT ON SCHEMA :: [extract] TO [IRMAClientRole];
GO

GRANT SELECT ON SCHEMA :: [extract] TO [IRMAReports];
GO