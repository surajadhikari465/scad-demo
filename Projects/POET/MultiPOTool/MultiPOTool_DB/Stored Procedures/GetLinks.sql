if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLinks]
GO

CREATE PROCEDURE dbo.GetLinks

AS
BEGIN

	select HelpLinksID, LinkDescription, LinkURL, OrderOfAppearance 
	from HelpLinks
	order by OrderOfAppearance

END

GO

--grant exec on GetLinks to MultiPOToolUsers
--exec GetLinks