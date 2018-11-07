CREATE PROCEDURE dbo.SLIM_StoreSpecialsStatus 
 @status int
AS
	-- **************************************************************************
	-- Procedure: SLIM_StoreSpecialsStatus()
	--    Author: ?
	--      Date: 03/24/2010
	--
	-- Description:
	-- This procedure is called from a SLIM.StoreSpecialsStatus.
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 03/24/2009	BSR		TFS 12299 Added Logic for status 3 (Processed) to 
	--						only pull records that haven not ended yet.
	-- 2013-09-10   FA		TF 13661 Add transaction isolation level
	-- **************************************************************************
BEGIN
	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	IF @status = 0 
	BEGIN
		SELECT	RequestId,
				iis.Item_Key, 
				iis.Store_No, 
				Store_Name,
				Price, 
				Multiple, 
				SalePrice, 
				SaleMultiple, 
				POSPrice, 
				POSSalePrice, 
				StartDate, 
				EndDate, 
				st.Status,
				RequestedBy,
				ProcessedBy,
				Comments,
				Identifier,
				SubTeam_Name,
				i.SubTeam_No,
				iis.Item_Description  
		FROM	SLIM_InStoreSpecials iis  
				INNER JOIN SLIM_StatusTypes st ON iis.Status = st.StatusId
				INNER JOIN Store s on iis.store_no = s.store_no
				INNER JOIN Item  i on i.Item_Key = iis.Item_Key
		ORDER BY
				RequestId DESC
	END
	ELSE
	BEGIN
		SELECT	RequestId,
				iis.Item_Key, 
				iis.Store_No, 
				Store_Name,
				Price, 
				Multiple, 
				SalePrice, 
				SaleMultiple, 
				POSPrice, 
				POSSalePrice, 
				StartDate, 
				EndDate, 
				st.Status,
				RequestedBy,
				ProcessedBy,
				Comments,
				Identifier,
				SubTeam_Name,
				i.SubTeam_No,
				iis.Item_Description 
		FROM	SLIM_InStoreSpecials iis  
				INNER JOIN SLIM_StatusTypes st ON iis.Status = st.StatusId
				INNER JOIN Store s on iis.store_no = s.store_no
				INNER JOIN Item  i on i.Item_Key = iis.Item_Key
		WHERE	iis.Status = @Status
		AND		EndDate >= CASE WHEN @Status = 3 THEN GetDate() ELSE EndDate END
		ORDER BY
				RequestId DESC

	END

	COMMIT TRAN
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_StoreSpecialsStatus] TO [IRMASLIMRole]
    AS [dbo];

