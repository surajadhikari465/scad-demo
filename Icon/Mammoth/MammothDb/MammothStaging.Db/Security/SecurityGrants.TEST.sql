-- Service Account
GRANT SELECT, INSERT, UPDATE, DELETE on SCHEMA::[dbo] to [WFM\MammothTest]

-- IRMA Developers
GRANT SELECT on SCHEMA::[dbo] to [WFM\IRMA.Developers]
GRANT SELECT on SCHEMA::[icon] to [WFM\IRMA.Developers]
GRANT SELECT on SCHEMA::[irma] to [WFM\IRMA.Developers]
GRANT SELECT on SCHEMA::[etl] to [WFM\IRMA.Developers]
GRANT SHOWPLAN to [WFM\IRMA.Developers]