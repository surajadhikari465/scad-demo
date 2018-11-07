IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_GetSubTeamUserName]') AND xtype IN (N'FN', N'IF', N'TF'))
    DROP FUNCTION dbo.fn_GetSubTeamUserName
GO

CREATE FUNCTION [dbo].[fn_GetSubTeamUserName]
    (@StoreNo   int,
     @SubTeamNo int,
     @PreOrder int,
	 @User varchar(13))
RETURNS VARCHAR(MAX)
AS
BEGIN

	DECLARE @ReturnMsg VARCHAR(MAX)
	DECLARE @Username VARCHAR(13)
	DECLARE @Count Int

    SET @ReturnMsg = ''	

	IF @PreOrder = -1 -- check items regardless of preorder flag
		BEGIN
			SELECT @Count = COUNT(UserName) FROM tmpOrdersAllocateItems WHERE Store_No = @StoreNo AND SubTeam_No = @SubTeamNo AND UserName <> @User	
			IF @Count > 0
				BEGIN
					SELECT DISTINCT @Username = UserName FROM tmpOrdersAllocateItems WHERE Store_No = @StoreNo AND SubTeam_No = @SubTeamNo AND UserName <> @User	
					SET @ReturnMsg = 'Items for this Warehouse and Subteam are locked.' + CHAR(13) + UPPER(@Username) + ' has already started a session.'
				END
		END
	ELSE
		BEGIN
			SELECT @Count = COUNT(UserName) FROM tmpOrdersAllocateItems WHERE Store_No = @StoreNo AND SubTeam_No = @SubTeamNo AND Pre_Order = @PreOrder	AND UserName <> @User	
			IF @Count > 0
				BEGIN
					SELECT DISTINCT @Username = UserName FROM tmpOrdersAllocateItems WHERE Store_No = @StoreNo AND SubTeam_No = @SubTeamNo AND Pre_Order = @PreOrder AND UserName <> @User	
					IF @PreOrder = 0
						SET @ReturnMsg = 'Non-PreOrder items for this Warehouse and Subteam are locked.' + CHAR(13) + UPPER(@Username) + ' has already started a session.'				
					ELSE
						SET @ReturnMsg = 'PreOrder items for this Warehouse and Subteam are locked.' + CHAR(13) + UPPER(@Username) + ' has already started a session.'
				END		
		END

    RETURN @ReturnMsg

END

GO