/****** Object:  StoredProcedure [dbo].[ItemDefaultAttributes_UpdateItemDefaultAttribute]    Script Date: 07/25/2012 14:57:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemDefaultAttributes_UpdateItemDefaultAttribute]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ItemDefaultAttributes_UpdateItemDefaultAttribute]
GO

/****** Object:  StoredProcedure [dbo].[ItemDefaultAttributes_UpdateItemDefaultAttribute]    Script Date: 07/25/2012 14:57:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ItemDefaultAttributes_UpdateItemDefaultAttribute]
	@itemDefaultAttribute_ID int,
	@attributeName VARCHAR(50),
	@active BIT,
	@controlOrder TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE ItemDefaultAttribute SET
    AttributeName = @attributeName,
    Active = @active,
    ControlOrder = @controlOrder
    WHERE 
    ItemDefaultAttribute_ID = @itemDefaultAttribute_ID
    
END

GO


