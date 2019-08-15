CREATE PROCEDURE [dbo].[GetInstructionListDataForESB]
	@InstructionListId  int = 0
AS
BEGIN

	SET NOCOUNT ON
	Declare @InstructionTypeId int = (SELECT InstructionTypeId from InstructionList Where InstructionListId= @InstructionListId)
    Declare @CookingInstructionTypeId  int = (SELECT InstructionTypeId from InstructionType Where Name= 'Cooking')
	Declare @GenericInstructionTypeId int= (SELECT InstructionTypeId from InstructionType Where Name= 'Generic')    

	IF( @InstructionTypeId = @CookingInstructionTypeId)
	BEGIN
	      SELECT 'CookingInstructions' as InstructionCode,
				'Cooking Instructions' as InstructionDescription,
				'CLI_'+ CAST(InstructionList.InstructionListId as varchar(20)) as InstructionId,
				'KBI_' + CAST(PluNumber as varchar(20)) as Id,
		         [Group]+ '_'+ CAST(Sequence as varchar(20))  as Sequence,
				 Member as Description,
				 'Replace' as ActionCode
		  from 
			InstructionList
			inner join InstructionListMember on InstructionList.InstructionListId = InstructionListMember.InstructionListId
			WHERE InstructionList.InstructionListId = @InstructionListId
			  AND ISNULL(IsDeleted, 0 ) = 0
		    ORDER BY [Group],Sequence
	END

  ELSE IF( @InstructionTypeId = @GenericInstructionTypeId)

  BEGIN
      SELECT   'GenericInstructions' as InstructionCode,
	            'Generic Instructions' as InstructionDescription,
				 Name as InstructionId,
				'KBI_' + CAST(PluNumber as varchar(20))  as Id,
		        [Group]+ '_'+ CAST(Sequence as varchar(20))  as Sequence,
				 Member as Description,
				 CASE WHEN Isdeleted = '1' THEN  'Delete' else 'AddOrUpdate' END as ActionCode
		  from 
			InstructionList
			inner join InstructionListMember on InstructionList.InstructionListId = InstructionListMember.InstructionListId
			WHERE InstructionList.InstructionListId = @InstructionListId
			ORDER BY [Group],Sequence

  END
	SET NOCOUNT OFF
END