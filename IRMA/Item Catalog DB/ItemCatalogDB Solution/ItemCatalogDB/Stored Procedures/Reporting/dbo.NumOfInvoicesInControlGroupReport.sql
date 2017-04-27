SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[NumOfInvoicesInControlGroupReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
   drop procedure [dbo].[NumOfInvoicesInControlGroupReport]
GO

CREATE PROCEDURE [dbo].[NumOfInvoicesInControlGroupReport]
@ControlGroup_ID int,
@PoNumber_Id int,
@BeginDate DateTime,
@EndDate DateTime
AS
BEGIN
SET NOCOUNT ON
	    Select count(*) as Count from fn_3WayMatchDetails(@ControlGroup_ID,@PoNumber_Id,@BeginDate,@EndDate)
SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO