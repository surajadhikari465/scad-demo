
/****** Object:  StoredProcedure [dbo].[UpdateInstanceDataFlagsStoreOverride]    Script Date: 11/09/2009 11:41:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateInstanceDataFlagsStoreOverride]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateInstanceDataFlagsStoreOverride]
GO
-- =============================================
-- Author:		Brian Robichaud
-- Create date: 11-10-09
-- Description:	Update InstanceDataFlag StoreOverride values
-- =============================================
CREATE PROCEDURE dbo.UpdateInstanceDataFlagsStoreOverride
	@Store_No int,
	@FlagKey varchar(50),
	@FlagValue bit
AS
BEGIN
	update InstanceDataFlagsStoreOverride
	set FlagValue = @FlagValue
	where Store_No = @Store_No
	and FlagKey = @FlagKey
END
GO

--grant exec on dbo.UpdateInstanceDataFlagsStoreOverride to IRMAAdminRole