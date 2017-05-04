/****** Object:  UserDefinedFunction [dbo].[fn_GetJobStatus]    Script Date: 10/03/2012 09:04:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetJobStatus]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetJobStatus]
GO

/****** Object:  UserDefinedFunction [dbo].[fn_GetJobStatus]    Script Date: 10/03/2012 09:04:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE FUNCTION [dbo].[fn_GetJobStatus]
    (@Classname VARCHAR(50))
RETURNS VARCHAR(50)
AS
BEGIN
	DECLARE @Status VARCHAR(50)
	
	SELECT @Status=Status  FROM JobStatus WHERE Classname=@Classname

    RETURN @Status
END
GO


