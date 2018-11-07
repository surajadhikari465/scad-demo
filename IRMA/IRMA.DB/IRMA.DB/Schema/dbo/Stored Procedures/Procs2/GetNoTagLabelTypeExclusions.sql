
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-11-10
-- Description:	Returns items that have a LabelTypeDesc
--				of NONE.
-- =============================================

CREATE PROCEDURE [dbo].[GetNoTagLabelTypeExclusions]
	@ItemKeys dbo.IntType readonly
AS
BEGIN
	set nocount on

    declare @LabelTypeNONE int = (select LabelType_ID from LabelType where LabelTypeDesc = 'NONE')

	select
		ItemKey = i.Item_Key
	from
		@ItemKeys ik
		join Item (nolock) i on ik.[Key] = i.Item_Key
	where
		i.LabelType_ID = @LabelTypeNONE
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNoTagLabelTypeExclusions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNoTagLabelTypeExclusions] TO [IRSUser]
    AS [dbo];

