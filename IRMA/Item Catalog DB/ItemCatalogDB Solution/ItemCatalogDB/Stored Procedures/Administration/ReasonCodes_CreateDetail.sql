
IF exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReasonCodes_CreateDetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReasonCodes_CreateDetail]
GO

/****** Object:  StoredProcedure [dbo].[ReasonCodes_CreateDetail]    Script Date: 07/18/2011 12:20:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Inserts a new Reason Code Detail Record
-- =============================================

CREATE PROCEDURE [dbo].[ReasonCodes_CreateDetail]
@ReasonCode char(3),
@ReasonCodeDesc varchar(50),
@ReasonCodeExtDesc varchar(MAX)
AS
BEGIN

	INSERT INTO ReasonCodeDetail (ReasonCode, ReasonCodeDesc, ReasonCodeExtDesc)
	Values(@ReasonCode, @ReasonCodeDesc, @ReasonCodeExtDesc)
	
END

GO
