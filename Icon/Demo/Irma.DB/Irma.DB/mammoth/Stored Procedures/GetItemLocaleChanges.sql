
CREATE PROCEDURE [mammoth].[GetItemLocaleChanges]
	@JobInstance int
AS
BEGIN
	SELECT
		q.QueueID			as QueueID,
		q.Identifier		as ScanCode,
		s.BusinessUnit_ID	as BusinessUnit,
		si.Authorized		as AuthorizedForSale,
		p.AgeCode			as AgeCode,
		p.IBM_Discount		as CaseDiscountEligible,
		p.Discountable		as TMDiscountEligible,
		p.Restricted_Hours  as RestrictedHours
	FROM
		[mammoth].[ItemLocaleChangeQueue]	q
		LEFT JOIN Price					p	on	q.Store_No	= p.Store_No
											AND q.Item_Key	= p.Item_Key
		LEFT JOIN StoreItem				si	on	q.Store_No	= si.Store_No
											AND q.Item_Key	= si.Item_Key
		JOIN Store						s	on	q.Store_No	= s.Store_No
	WHERE
		q.InProcessBy = @JobInstance
END

GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[GetItemLocaleChanges] TO [MammothRole]
    AS [dbo];

