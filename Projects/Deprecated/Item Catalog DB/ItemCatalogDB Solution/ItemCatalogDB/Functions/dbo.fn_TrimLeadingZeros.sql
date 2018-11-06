SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_TrimLeadingZeros') 
    DROP FUNCTION fn_TrimLeadingZeros
GO

CREATE FUNCTION [dbo].[fn_TrimLeadingZeros] ( @Input VARCHAR(50) )
RETURNS VARCHAR(50)
AS
BEGIN
    RETURN REPLACE(LTRIM(REPLACE(@Input, '0', ' ')), ' ', '0')
END
GO
