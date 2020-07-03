DO $$
BEGIN
IF NOT EXISTS (SELECT schema_name FROM information_schema.schemata WHERE schema_name = 'gpm') THEN
    CREATE SCHEMA gpm;
END IF;
END $$