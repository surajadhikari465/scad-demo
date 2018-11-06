SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertODBCError]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertODBCError]
GO


CREATE PROCEDURE dbo.InsertODBCError
@ODBCStart datetime,
@ODBCEnd datetime,
@ErrorNumber int, 
@ErrorDescription varchar(4096),
@ODBCCall varchar(4096)
AS 

INSERT INTO ODBCErrorLog (SystemTime, ODBCStart, ODBCEnd, ErrorNumber, ErrorDescription, ODBCCall, UserName, Computer)
VALUES (GetDate(), @ODBCStart, @ODBCEnd, @ErrorNumber, @ErrorDescription, @ODBCCall, SUSER_NAME(), HOST_NAME())
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


