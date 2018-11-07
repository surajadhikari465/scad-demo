if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteLink]
GO

CREATE PROCEDURE dbo.DeleteLink
	@HelpLinksID int

AS
BEGIN

	delete from HelpLinks where HelpLinksID = @HelpLinksID

END

GO

--grant exec on DeleteLink to MultiPOToolUsers
--exec GetLinksByUserID 1