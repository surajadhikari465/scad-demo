if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertLink]
GO

CREATE PROCEDURE dbo.InsertLink
	@LinkDescription varchar(200),
	@LinkURL varchar(max),
	@UpdatedUserID int,
	@OrderOfAppearance int

AS
BEGIN

	insert into HelpLinks
		(LinkDescription
		, LinkURL
		, UpdatedDate
		, UpdatedUserID
		, OrderOfAppearance)
	values
		(@LinkDescription
		, @LinkURL
		, getdate()
		, @UpdatedUserID
		, @OrderOfAppearance)
END

GO

--grant exec on InsertLink to MultiPOToolUsers
--exec InsertLink 'Doonesbury','http://www.doonesbury.com/strip/dailydose/',1
