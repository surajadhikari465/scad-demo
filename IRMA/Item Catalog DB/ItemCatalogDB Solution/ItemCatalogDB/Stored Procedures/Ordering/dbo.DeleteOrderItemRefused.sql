
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[DeleteOrderItemRefused]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[DeleteOrderItemRefused]
GO

CREATE PROCEDURE [dbo].[DeleteOrderItemRefused]
	@OrderItemRefusedID int
AS 
-- **************************************************************************
-- Procedure: DeleteOrderItemRefused()
--    Author: Faisal Ahmed
--      Date: 03/08/2013
--
-- Description:
-- This procedure deletes a record from OrderItemRefused table
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/08/2013	FA   	8325	Initial Code
BEGIN

	DELETE OrderItemRefused
	WHERE OrderItemRefusedID = @OrderItemRefusedID
END