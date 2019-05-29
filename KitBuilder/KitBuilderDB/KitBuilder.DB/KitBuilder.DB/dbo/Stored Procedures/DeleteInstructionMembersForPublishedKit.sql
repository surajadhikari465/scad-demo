CREATE PROCEDURE [dbo].[DeleteInstructionMembersForPublishedKit]
		@InstructionListId  int
AS
BEGIN

DECLARE @PublishedStatusId int = (select StatusID from Status where statuscode='PR')


UPDATE AvailablePluNumber
SET InUse = 0, 
    LastUpdatedDateUtc = GETUTCDATE()
WHERE PluNumber IN  (SELECT PluNumber FROM dbo.InstructionListMember
					 WHERE InstructionListId = @InstructionListId and IsDeleted=1)

DELETE dbo.InstructionListMember
WHERE InstructionListId = @InstructionListId and IsDeleted=1

Update InstructionList
SET StatusId = @PublishedStatusId,
    LastUpdatedDateUtc = GETUTCDATE()
WHERE InstructionListId = @InstructionListId

END