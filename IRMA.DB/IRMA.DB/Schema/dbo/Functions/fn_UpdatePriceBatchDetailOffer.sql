﻿CREATE   FUNCTION [dbo].[fn_UpdatePriceBatchDetailOffer](
	@DetailIDList varchar(8000),
    @DetailIDListSep char(1))
RETURNS varchar(8000)

	BEGIN
		DECLARE @lookupID varchar(20)
		DECLARE @tempID int
		
		SET @lookupID = @DetailIDList
	
		-- takes in delimited list of PriceBatchDetailID values; 
		-- query looks for other price batch detail records for the same Offer_ID/Store_No as the existing
		-- PriceBatchDetailID and appends those new details to the list, which it returns to the calling query.
		
		DECLARE priceBatchIDCursor CURSOR
		READ_ONLY
		FOR     
			SELECT PriceBatchDetailID FROM PriceBatchDetail 
			WHERE Offer_ID = (SELECT Offer_ID FROM PriceBatchDetail WHERE PriceBatchDetailID = @lookupID)
				AND Store_No = (SELECT Store_No FROM PriceBatchDetail WHERE PriceBatchDetailID = @lookupID)
				AND PriceBatchDetailID <> @lookupID

		OPEN priceBatchIDCursor
		FETCH NEXT FROM priceBatchIDCursor INTO @tempID
		WHILE (@@fetch_status <> -1)
		BEGIN
			IF (@@fetch_status <> -2)
			BEGIN
				--append next ID to @DetailIDList w/ a separator
				SET @DetailIDList = @DetailIDList + @DetailIDListSep + CAST(@tempID AS varchar(10))
			END
			FETCH NEXT FROM priceBatchIDCursor INTO @tempID
		END 
		CLOSE priceBatchIDCursor
		DEALLOCATE priceBatchIDCursor	
		
		--chop off last separator value
		--SET @DetailIDList = SUBSTRING(@DetailIDList,0,LEN(@DetailIDList))
		RETURN @DetailIDList
  	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_UpdatePriceBatchDetailOffer] TO [IRMAClientRole]
    AS [dbo];

