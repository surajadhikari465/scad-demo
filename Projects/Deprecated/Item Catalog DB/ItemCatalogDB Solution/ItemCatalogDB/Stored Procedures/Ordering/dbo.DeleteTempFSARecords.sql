 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteTempFSARecords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteTempFSARecords]
GO

CREATE PROCEDURE dbo.DeleteTempFSARecords
	@StoreNo int,
	@SubTeamNo int,
	@UserName varchar(15),
	@DeleteItem bit,
	@DeleteBOH bit,
	@PreOrder int
AS 

	IF @DeleteItem = 1
		BEGIN
			DELETE oi FROM tmpOrdersAllocateOrderItems oi
			INNER JOIN tmpOrdersAllocateItems i ON oi.Item_Key = i.Item_Key
			WHERE i.Store_No = @StoreNo	AND SubTeam_No = @SubTeamNo AND i.Pre_Order	=	(CASE WHEN 
																								@PreOrder =	-1 THEN i.Pre_Order
																							ELSE
																								@PreOrder
																						END)
																					
		END
		
	IF @DeleteBOH = 1
		BEGIN
			DELETE FROM tmpOrdersAllocateItems 
			WHERE Store_No = @StoreNo AND SubTeam_No = @SubTeamNo AND UserName = @UserName AND Pre_Order	=	(CASE WHEN 
																													@PreOrder =	-1 THEN Pre_Order
																												ELSE
																													@PreOrder
																												END)
		END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO