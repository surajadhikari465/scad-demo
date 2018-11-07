-- This script was created using WinSQL Professional
-- Timestamp: 11/3/2008 4:28:34 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: AssignPONumbers;1 - Script Date: 11/3/2008 4:28:34 PM
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AssignPONumbers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AssignPONumbers]
GO

CREATE PROCEDURE dbo.AssignPONumbers
	@RegionID int,
	@POTypeID int,
	@UserID int,
	@POCount int

AS
BEGIN


	declare @i int
	set @i = 0

	while @i < @POCount
	begin
------------------------------------------------------------------------------------------------
-- assign the next available PO Number

		declare @POIncrement int
		declare @PONumber int

		select @POIncrement = 
					(select
					max(POIncrement) + 1
					from PONumber 
					where POTypeID = @POTypeID
						and RegionID = @RegionID)
		
		select @POIncrement =isnull(@POIncrement,0)

		select @PONumber = 
			cast(@POTypeID as varchar(1)) +
			substring('00',1,2-len(@RegionID)) + cast(@RegionID as varchar(2)) +
			substring('000000',1,6-len(@POIncrement)) + cast(@POIncrement as varchar(6))

		insert into PONumber (
			POTypeID
			, RegionID
			, POIncrement
			, PONumber
			, DateAssigned	
			, PushedToIRMA
			, AssignedByUserID
			) 
		values (
			@POTypeID --POTypeID
			, @RegionID --RegionID
			, @POIncrement --POIncrement
			, @PONumber --PONumber
			, getdate() --DateAssigned	
			, 0 --PushedToIRMA
			, @UserID --AssignedByUserID
			)
	set @i = @i + 1
	end

END

GO
