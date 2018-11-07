CREATE PROCEDURE [dbo].[GetItemAttributeText_1]
AS
BEGIN
    SET NOCOUNT ON
		SELECT
		DISTINCT CASE 
			WHEN Text_1 = '' THEN ''
			WHEN Text_1 IS NULL THEN ''
			ELSE Text_1
		END as OrderBy,		
		CASE 
			WHEN Text_1 = '' THEN 'Commodity Not Assigned'
			WHEN Text_1 IS NULL THEN 'Commodity Not Assigned'
			ELSE Text_1 
		END as ItemAttributeText1
	FROM 
		ItemAttribute
	ORDER BY
		OrderBy
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemAttributeText_1] TO [IRMAReportsRole]
    AS [dbo];

