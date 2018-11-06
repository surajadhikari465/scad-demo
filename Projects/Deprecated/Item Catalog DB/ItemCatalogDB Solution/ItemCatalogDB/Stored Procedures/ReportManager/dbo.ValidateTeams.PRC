SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ValidateTeams]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ValidateTeams]
GO

CREATE PROCEDURE dbo.ValidateTeams
@Store_No as int,
@Zone_ID as tinyint
AS

SELECT Distinct Store.Store_No, Team_No, WFM_Store, Zone_Id 
FROM Store (NOLOCK) INNER JOIN StoreSubteam (NOLOCK) ON Store.Store_no = StoreSubteam.Store_No 
WHERE ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND Zone_Id = @Zone_ID



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


