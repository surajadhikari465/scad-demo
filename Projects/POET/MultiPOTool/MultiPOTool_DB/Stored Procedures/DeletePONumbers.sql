
/****** Object:  StoredProcedure [dbo].[DeletePONumber]    Script Date: 08/20/2010 10:30:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeletePONumber]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeletePONumber]
GO

/****** Object:  StoredProcedure [dbo].[DeletePONumber]    Script Date: 08/20/2010 10:30:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Brian Robichaud
-- Create date: 08-19-2010
-- Description:	Set the DeleteDate datetime in the PONumber table
-- =============================================
CREATE PROCEDURE [dbo].[DeletePONumber]
	@PONumber INT
AS
BEGIN
	Update PONumber
	Set DeleteDate = GetDate()
	Where PONumber = @PONumber
END

GO


