IF(NOT EXISTS (SELECT * FROM app.RetentionPolicy rp WHERE rp.[Table] = 'R10MessageResponse'))
BEGIN
	INSERT INTO app.RetentionPolicy([Server], [Database], [Schema], [Table], DaysToKeep)
	VALUES (@@SERVERNAME, 'Icon', 'app', 'R10MessageResponse', 10)
END