/*
	Retreives records from ItemGroupMembers Table by GroupId
*/

CREATE  PROCEDURE [dbo].[EPromotions_GetGroupItems] 
	@GroupID as Integer
AS
SELECT DISTINCT 
                      igm.Group_ID, igm.Item_Key, igm.modifieddate, igm.User_ID, i.Item_Description, igm.Identifier, ISNULL(igm.OfferChgTypeID, 1) 
                      AS OfferChgTypeId
FROM         dbo.ItemGroupMembers AS igm INNER JOIN
                      dbo.Item AS i ON i.Item_Key = igm.Item_Key
	WHERE Group_Id = @GroupId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetGroupItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetGroupItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetGroupItems] TO [IRMAReportsRole]
    AS [dbo];

