

/****** Object:  StoredProcedure [dbo].[GetVersion]    Script Date: 10/07/2009 12:01:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVersion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetVersion]
GO
/****** Object:  StoredProcedure [dbo].[GetVersion]    Script Date: 10/07/2009 11:58:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetVersion]
AS 
	SELECT * FROM Version
GO