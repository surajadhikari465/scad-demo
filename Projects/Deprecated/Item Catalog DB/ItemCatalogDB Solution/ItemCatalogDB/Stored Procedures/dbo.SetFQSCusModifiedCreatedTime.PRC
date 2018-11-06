SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetFQSCusModifiedCreatedTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetFQSCusModifiedCreatedTime]
GO

/****** Object:  Stored Procedure dbo.SetFQSCusModifiedCreatedTime    Script Date: 4/25/99 10:35:03 PM ******/
CREATE PROCEDURE dbo.SetFQSCusModifiedCreatedTime
@CusNumber int
AS
UPDATE FQSCustomer SET
ModifiedCreatedTime = GetDate()
WHERE CusNumber = @CusNumber

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


