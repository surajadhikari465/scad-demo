﻿CREATE PROCEDURE dbo.IconInsertTlogReprocessRequest
(
@ReprocessRequest	dbo.TlogReprocessRequestType READONLY
)
AS

BEGIN
	MERGE TlogReprocessRequest AS trr
	USING
	(
		SELECT 
		rr.TransactionDate,
		s.Store_No,
		rr.BusinessUnit_ID
		FROM @ReprocessRequest rr
		INNER JOIN Store s (NOLOCK)
		ON rr.BusinessUnit_ID = s.BusinessUnit_ID 
	) r
	ON trr.Date_Key = r.TransactionDate AND
	trr.Store_No = r.Store_No
		WHEN NOT MATCHED THEN
		INSERT
		(
		Date_Key,
		Store_No,
		BusinessUnit_ID
		)
		VALUES
		(r.TransactionDate,
		r.Store_No,
		r.BusinessUnit_ID);
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconInsertTlogReprocessRequest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconInsertTlogReprocessRequest] TO [IRSUser]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IconInsertTlogReprocessRequest] TO [IConInterface]
    AS [dbo];

