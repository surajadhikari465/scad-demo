CREATE PROCEDURE [dbo].[DeleteInstructionMembersForPublishedKit]
		@InstructionListId  int
AS
BEGIN

DECLARE @PublishedStatusId int = (select StatusID from Status where statuscode='P')

DELETE dbo.InstructionListMember
Where InstructionListId = @InstructionListId and IsDeleted=1

Update InstructionList
SET StatusId = @PublishedStatusId,
    LastUpdatedDateUtc = GETUTCDATE()
Where InstructionListId = @InstructionListId

END