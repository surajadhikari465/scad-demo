
create procedure dbo.EXEInterfaces_ZeroShippedInsert
(
@uniqueid varchar(255),
@filename  varchar(255), 
@filetype  varchar(4),
@orderheader_id  int,
@identifier  varchar(13), 
@value  int
) as
BEGIN
      INSERT INTO
          dbo.EXEInterfaces_ZeroShippedOrdersValidationWorkspace
          (
            uniqueid ,
            filename ,
            filetype ,
            orderheader_id ,
            identifier ,
            value )
      VALUES
          (
            @uniqueid ,
            @filename ,
            @filetype ,
            @orderheader_id ,
            @identifier ,
            @value )
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedInsert] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EXEInterfaces_ZeroShippedInsert] TO [IRMAClientRole]
    AS [dbo];

