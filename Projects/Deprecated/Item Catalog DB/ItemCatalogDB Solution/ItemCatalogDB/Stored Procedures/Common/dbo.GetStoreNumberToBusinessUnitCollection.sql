if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreNumberToBusinessUnitCollection]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.GetStoreNumberToBusinessUnitCollection
go

-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-11-18
-- Description:	Returns all of the store numbers with
--				their corresponding business unit ID.
-- =============================================

CREATE PROCEDURE dbo.GetStoreNumberToBusinessUnitCollection
AS
BEGIN
	set nocount on

	select
		s.Store_No as StoreNumber,
		s.BusinessUnit_ID as BusinessUnit
	from
		Store s (nolock)
	where
		s.WFM_Store = 1 or s.Mega_Store = 1
	order by 
		s.Store_No
END
GO