IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SLIM_RejectItemRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SLIM_RejectItemRequest]
GO
/****** Object:  StoredProcedure [dbo].[SLIM_RejectItemRequest]    Script Date: 07/11/2007 09:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SLIM_RejectItemRequest]
 @requestId int,
 @ProcessedBy varchar(25),
 @Comments varchar(255)
AS
	-- **************************************************************************
	-- Procedure: SLIM_RejectItemRequest()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	Update ItemRequest
	set 
		ItemStatus_ID = (select statusid from slim_statustypes where status = 'rejected'),
		ProcessedBy = @ProcessedBy,
		Comments = @Comments
	WHERE 
		ItemRequest_ID=@requestId

	COMMIT TRAN
END
GO