
IF exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReasonCodes_CreateType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReasonCodes_CreateType]
GO

/****** Object:  StoredProcedure [dbo].[ReasonCodes_CreateType]    Script Date: 07/18/2011 12:20:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Mugdha Deshpande
-- Create date: 06/09/2011
-- Description:	Inserts a new Reason Code Type
-- =============================================

CREATE PROCEDURE [dbo].[ReasonCodes_CreateType]
@ReasonCodeTypeAbbr char(2),
@ReasonCodeTypeDesc varchar(50)
AS
BEGIN

	INSERT INTO ReasonCodeType (ReasonCodeTypeAbbr, ReasonCodeTypeDesc)
	Values(@ReasonCodeTypeAbbr, @ReasonCodeTypeDesc)
	
END

GO
