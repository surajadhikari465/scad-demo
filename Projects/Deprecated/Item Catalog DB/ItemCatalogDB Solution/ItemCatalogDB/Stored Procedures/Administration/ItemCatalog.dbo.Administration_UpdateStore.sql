 /****** Object:  StoredProcedure [dbo].[Administration_UpdateStore]    Script Date: 09/26/2006 16:33:09 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_UpdateStore]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_UpdateStore]
GO

/****** Object:  StoredProcedure [dbo].[Administration_UpdateStore]    Script Date: 09/26/2006 16:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.Administration_UpdateStore
	@Store_No int,
	@POSSystemId int
AS 

BEGIN
	--updates STORE table w/ new data
	UPDATE Store
		SET POSSystemId = @POSSystemId
	WHERE Store_No = @Store_No
	 
END
GO

   