IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].GetAllClasses') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure [dbo].GetAllClasses
	END
GO

CREATE Procedure dbo.GetAllClasses
AS

    SELECT   ClassName, ClassID
    FROM     NatItemClass
    ORDER BY ClassName

GO

