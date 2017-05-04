IF EXISTS (SELECT * from dbo.sysobjects WHERE id = object_id(N'[dbo].[fn_HasAlignedSubteam]') AND xtype IN (N'FN', N'IF', N'TF'))
	DROP FUNCTION [dbo].[fn_HasAlignedSubteam]
GO

-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-02-03
-- Description:	Determines whether or not an item's
--				assigned subteam is currently
--				aligned with Icon.
-- =============================================

CREATE FUNCTION [dbo].[fn_HasAlignedSubteam] (@ItemKey int)
RETURNS bit
AS
BEGIN
	declare @HasAlignedSubteam bit

	select
		@HasAlignedSubteam = st.AlignedSubTeam
	from
		Item			(nolock)	i
		join SubTeam	(nolock)	st	on	i.SubTeam_No = st.SubTeam_No
	where
		i.Item_Key = @ItemKey

	-- If the return value is null (for example, when adding a new item via EIM), then the business logic will be that the item
	-- is considered subteam-aligned.
	return isnull(@HasAlignedSubteam, 1)
END
GO
