
CREATE TRIGGER ddl_log
ON DATABASE
FOR DDL_DATABASE_LEVEL_EVENTS 
AS

	DECLARE @data XML
	SET @data = EVENTDATA()

	INSERT INTO dbo.ddl_log 
	    (
		 EventType
		,PostTime
		,SPID
		,ServerName
		,LoginName
		,UserName
		,DatabaseName
		,SchemaName
		,ObjectName
		,ObjectType
		,TSQLCommand
		)
	VALUES 
	   (
		 @data.value('(/EVENT_INSTANCE/EventType)[1]',    'varchar(100)')
	        ,@data.value('(/EVENT_INSTANCE/PostTime)[1]',     'varchar(25)')
		,@data.value('(/EVENT_INSTANCE/SPID)[1]',         'varchar(10)')
		,@data.value('(/EVENT_INSTANCE/ServerName)[1]',   'varchar(50)')
		,@data.value('(/EVENT_INSTANCE/LoginName)[1]',    'varchar(50)')
		,@data.value('(/EVENT_INSTANCE/UserName)[1]',     'varchar(50)')
		,@data.value('(/EVENT_INSTANCE/DatabaseName)[1]', 'varchar(50)')
		,@data.value('(/EVENT_INSTANCE/SchemaName)[1]',   'varchar(50)')
		,@data.value('(/EVENT_INSTANCE/ObjectName)[1]',   'varchar(50)')
		,@data.value('(/EVENT_INSTANCE/ObjectType)[1]',   'varchar(25)')
		,@data.value('(/EVENT_INSTANCE/TSQLCommand)[1]',  'varchar(2000)')
	   );

