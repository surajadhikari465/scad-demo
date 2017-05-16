if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateLink]
GO

CREATE PROCEDURE dbo.UpdateLink
	@HelpLinksID int,
	@LinkDescription varchar(200),
	@LinkURL varchar(max),
	@UpdatedUserID int,
	@OrderOfAppearance int

AS
BEGIN

	update HelpLinks
		set LinkDescription = @LinkDescription
		, LinkURL = @LinkURL
		, UpdatedDate = getdate()
		, UpdatedUserID = @UpdatedUserID
		, OrderOfAppearance = @OrderOfAppearance
	where HelpLinksID = @HelpLinksID

END

GO

--grant exec on UpdateLink to MultiPOToolUsers