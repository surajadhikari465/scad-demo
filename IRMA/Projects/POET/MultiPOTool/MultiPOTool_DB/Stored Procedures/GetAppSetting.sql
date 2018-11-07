
/****** Object:  StoredProcedure [dbo].[GetAppSetting]    Script Date: 08/03/2010 10:48:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAppSetting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAppSetting]
GO

/****** Object:  StoredProcedure [dbo].[GetAppSetting]    Script Date: 08/03/2010 10:48:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Brian Robichaud
-- Create date: 08-03-2010
-- Description:	returns a app key's value
-- =============================================
CREATE PROCEDURE [dbo].[GetAppSetting]
	@KeyName varchar(30)
AS
BEGIN
	SELECT KeyValue
	FROM AppSettings
	WHERE KeyName = @KeyName
END

GO


