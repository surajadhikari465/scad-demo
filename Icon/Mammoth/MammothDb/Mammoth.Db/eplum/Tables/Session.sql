CREATE TABLE eplum.Session
(
	SessionID NVARCHAR(30) CONSTRAINT [Session_SessionID_DF] DEFAULT ('00000') NULL
)
GO

GRANT SELECT, INSERT, UPDATE ON eplum.Session TO dds_eplum_role
GO