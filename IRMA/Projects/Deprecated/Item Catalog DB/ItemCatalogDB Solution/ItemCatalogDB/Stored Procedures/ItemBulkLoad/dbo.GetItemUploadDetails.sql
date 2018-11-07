if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetItemUploadDetails') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetItemUploadDetails
GO


CREATE PROCEDURE dbo.GetItemUploadDetails
    @ItemUploadHeader_ID int 
AS

BEGIN
    SET NOCOUNT ON

	-- Get the Good ones first
	(SELECT 
		ItemUploadDetail_ID,
		ItemIdentifier,
		POSDescription,
		Description,
		TaxClassID,
		FoodStamps,
		RestrictedHours,
		EmployeeDiscountable,
		Discontinued,
		NationalClassID,
		UPD.SubTeam_No,
		S.SubTeam_Name,
		ItemIdentifierValid,
		SubTeamAllowed,
		Uploaded,
		Item_Key
	FROM
		ItemUploadDetail UPD
		LEFT OUTER JOIN SubTeam S on UPD.SubTeam_No = S.SubTeam_No
	WHERE 
		ItemUploadHeader_ID = @ItemUploadHeader_ID
		AND ItemIdentifierValid = 1 AND SubTeamAllowed = 1)
	
	UNION 

	-- Get the Invalid Identifiers next
	(SELECT 
		ItemUploadDetail_ID,
		ItemIdentifier,
		POSDescription,
		Description,
		TaxClassID,
		FoodStamps,
		RestrictedHours,
		EmployeeDiscountable,
		Discontinued,
		NationalClassID,
		UPD.SubTeam_No,
		S.SubTeam_Name,
		ItemIdentifierValid,
		SubTeamAllowed,
		Uploaded,
		Item_Key
	FROM
		ItemUploadDetail UPD
		LEFT OUTER JOIN SubTeam S on UPD.SubTeam_No = S.SubTeam_No
	WHERE 
		ItemUploadHeader_ID = @ItemUploadHeader_ID
		AND ItemIdentifierValid = 0)

	UNION 

	-- Finally get the Good Identifiers, but disallowed SubTeams
	(SELECT 
		ItemUploadDetail_ID,
		ItemIdentifier,
		POSDescription,
		Description,
		TaxClassID,
		FoodStamps,
		RestrictedHours,
		EmployeeDiscountable,
		Discontinued,
		NationalClassID,
		UPD.SubTeam_No,
		S.SubTeam_Name,
		ItemIdentifierValid,
		SubTeamAllowed,
		Uploaded,
		Item_Key
	FROM
		ItemUploadDetail UPD
		LEFT OUTER JOIN SubTeam S on UPD.SubTeam_No = S.SubTeam_No
	WHERE 
		ItemUploadHeader_ID = @ItemUploadHeader_ID
		AND ItemIdentifierValid = 1 AND SubTeamAllowed = 0)

	ORDER BY 
		ItemIdentifierValid DESC, SubTeamAllowed DESC, Uploaded DESC, SubTeam_No


    SET NOCOUNT OFF
END

GO
