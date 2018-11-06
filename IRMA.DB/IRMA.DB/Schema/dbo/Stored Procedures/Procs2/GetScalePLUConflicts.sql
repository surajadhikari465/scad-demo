CREATE PROCEDURE dbo.GetScalePLUConflicts 
	@Identifier varchar(13),
	@PLUDigits	integer = 4,
	@ScalePLU char(5),
	@SubTeam_No int
AS


/**********************************************************************************************************************************************************************************************************************************
Revision History
DEV					DATE					TASK						Description
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Min Zhao			Jul 28, 2011			1875					This stored proc is used to check if a to-be-added 4-digit PLU is already be used an existing scale identifier with 4-digit PLU sent to scale within the subteam.
                                                                    There're 4-digit and 5-digit PLUs for scaled items. Which ones in use is depended on a region's configuration.
																	This check for scale PLU conflicts is only needed for 4-digit PLUs. This is because a scale PLU has to be 11-digit long and in the format of
																	2XXXXX00000 (where X can be any number from 0 to 9). When a new identifier is to be added, a validation will occur to make sure that the 11-digit 
																	identifier is unique, and if it is a scale item identifier. If the validation is passed, it means that the 5 digits from the 2nd position to the 6th position 
																	in the identifier is unique (hasn't been used by another scale item). Therefore, the check for Scale PLU Conflicts is only needed for 4-digit PLU to be added 
																	to make sure that the 3rd position to the 6th position in the identifier is unique (hasn't been used by an existing 4-digit scale item).    
																	
Amudha Sethuraman	Sep 10, 2012			6744					Allow for Non type 2 identifiers to go to scale. Add logic for non type 2 PLUs.			
Denis Ng			Oct 17, 2012			6744					Validate 4 or 5 digit PLU for non-type-2 identifiers.
Denis Ng			Jan	28, 2013			9908					Added a new parameter @PLUDigits to determine the number of digits used to check for duplicate PLUs.
Denis Ng			Jan 30, 2013			9965					Added conditions to help determine type-2 identifiers.
Denis Ng			Jan 30, 2013			9908					Added a condition in the number of digits use when selecting conflicting scale PLU
**********************************************************************************************************************************************************************************************************************************/

BEGIN
    SET NOCOUNT ON
	DECLARE @Type2StartDigit AS INT

	-- Example		: 21234500000
	-- 4-digit PLU	: 2345
	-- 5-digit PLU	: 12345
    IF @PLUDigits = 5
		SET @Type2StartDigit = 2
	ELSE
		SET @Type2StartDigit = 3

    --VALIDATE 4 DIGIT SCALE PLU FOR TYPE 2 IDENTIFIER
    IF SUBSTRING(@Identifier,1,1) = '2' AND
	   RIGHT(@Identifier,5) = '00000' AND
	   LEN(@Identifier) = 11
		BEGIN 
		
		--CHECKS FOR EXISTING ITEM IDENTIFIERS THAT HAVE THE SAME 4-DIGIT SCALE PLU
		--WITHIN THE SCALE DEPT FOR THE SUBTEAM_NO PASSED IN
		SELECT II.Identifier, I.Item_Description
		FROM ItemIdentifier II
		INNER JOIN
			Item I
			ON I.Item_Key = II.Item_Key
		INNER JOIN
			SubTeam S
			ON S.Subteam_No = I.Subteam_No
		WHERE ((SUBSTRING(Identifier, @Type2StartDigit, @PLUDigits) = @ScalePLU) OR --ScalePLU
		       (RIGHT(RTRIM(Identifier),@PLUDigits) = @ScalePLU))
			AND II.Identifier <> @Identifier --EXCLUDE THE IDENTIFIER BEING ADDED/EDITED
			AND S.ScaleDept IN (SELECT ScaleDept 
								FROM SubTeam
								WHERE Subteam_No = @Subteam_No)	
			AND II.Scale_Identifier = 1
			AND II.Deleted_Identifier = 0
			AND II.Remove_Identifier = 0
			AND II.NumPluDigitsSentToScale = @PLUDigits
		ORDER BY I.Item_Description	
		END
	--VALIDATE 4 OR 5 DIGIT SCALE PLU FOR NON TYPE 2 IDENTIFIER
    ELSE
		BEGIN 

		--CHECKS FOR EXISTING ITEM IDENTIFIERS THAT HAVE THE SAME 4-DIGIT OR 5-DIGIT SCALE PLU
		--WITHIN THE SCALE DEPT FOR THE SUBTEAM_NO PASSED IN

		SELECT II.Identifier, I.Item_Description
		FROM ItemIdentifier II
		INNER JOIN
			Item I
			ON I.Item_Key = II.Item_Key
		INNER JOIN
			SubTeam S
			ON S.Subteam_No = I.Subteam_No
		WHERE
			RIGHT(@ScalePLU,@PLUDigits) IN (RIGHT(Identifier,@PLUDigits),SUBSTRING(Identifier,@Type2StartDigit,@PLUDigits))
			AND II.Identifier <> @Identifier --EXCLUDE THE IDENTIFIER BEING ADDED/EDITED
			AND S.ScaleDept IN (SELECT ScaleDept 
								FROM SubTeam
								WHERE Subteam_No = @Subteam_No)	
			AND II.Scale_Identifier = 1
			AND II.Deleted_Identifier = 0
			AND II.Remove_Identifier = 0
			AND II.NumPluDigitsSentToScale = @PLUDigits
		ORDER BY I.Item_Description	
		END

	
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScalePLUConflicts] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScalePLUConflicts] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScalePLUConflicts] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScalePLUConflicts] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetScalePLUConflicts] TO [IRMAReportsRole]
    AS [dbo];

