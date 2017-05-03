/****** Object:  StoredProcedure [dbo].[UpdateInstanceData]    Script Date: 1/11/2007 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UpdateInstanceData]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[UpdateInstanceData]
GO

/****** Object:  StoredProcedure [dbo].[UpdateInstanceData]    Script Date: 1/11/2007 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[UpdateInstanceData] 
	@PluDigitsSentToScale varchar(20),
	@UGCulture varchar(5),
	@UGDateMask varchar(12)
AS
BEGIN
    SET NOCOUNT ON

	UPDATE dbo.InstanceData SET PluDigitsSentToScale = @PluDigitsSentToScale,  UG_Culture = @UGCulture,  UG_DateMask = @UGDateMask
    
    SET NOCOUNT OFF
END
GO
 