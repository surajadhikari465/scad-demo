CREATE PROCEDURE dbo.[GetItemChangeInfo]
    @Item_Key int
AS 

BEGIN
    SET NOCOUNT ON
     
	SELECT	
		ICH.Item_Key, 
		ICH.Item_Description, 
		ICH.Sign_Description, 
		ICH.SubTeam_No, 
		U.UserName,       
		IT.LastModifiedUser_ID, 
		IT.LastModifiedDate AS User_ID_Date,
		ICH.Insert_Date AS Insert_Date 

	FROM ItemChangeHistory ICH (NOLOCK)

	INNER JOIN
		ItemIdentifier II (NOLOCK)
		ON (II.Item_Key = ICH.Item_Key AND II.Default_Identifier = 1)
	INNER JOIN
		SubTeam ST (NOLOCK)
		ON ST.SubTeam_No = ICH.SubTeam_No
	INNER JOIN
		Item IT (NOLOCK)
		ON IT.Item_Key = ICH.Item_Key
	LEFT JOIN  
		Users U (NOLOCK)
		ON U.User_ID = IT.LastModifiedUser_ID
	WHERE 
		ICH.Item_Key = @Item_Key AND 
		ICH.Insert_Date = (SELECT TOP 1 Insert_Date FROM ItemChangeHistory WHERE Item_Key = @Item_Key ORDER BY Insert_Date DESC)
		
  	SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemChangeInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemChangeInfo] TO [IRMAClientRole]
    AS [dbo];

