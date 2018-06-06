CREATE TABLE eplum.Session
(
	SessionID NVARCHAR(2) NULL
)
GO

GRANT SELECT, INSERT, UPDATE ON eplum.Session TO dds_eplum_role
GO