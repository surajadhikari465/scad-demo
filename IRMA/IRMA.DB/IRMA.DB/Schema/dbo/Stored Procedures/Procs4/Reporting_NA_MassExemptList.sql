-- =============================================
-- Author:		VC
-- Create date: 4/10/2007
-- Description:	Report - Ma Exempt List
-- =============================================
CREATE PROCEDURE [dbo].[Reporting_NA_MassExemptList]
@Store_No int
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
-- Insert statements for procedure here
Select DISTINCT
--S.Store_No,
S.Store_Name,
ST.SubTeam_Name [DEPT],
II.Identifier [UPC],
B.Brand_Name,
SIA.Item_Key,
I.Item_Description [Description],
I.Package_Desc2,
IU.Unit_Name [UOM],
CASE WHEN PCT.On_Sale = 1 
	THEN P.Sale_Price
	ELSE P.Price 
	END [Current Retail],
dbo.fn_GetAlternateUPC (SIA.Item_key) [Alternate UPC]
FROM Store S
INNER JOIN Vendor V
ON S.Store_No = V.Store_no
LEFT JOIN StoreItemAttribute SIA
ON S.Store_No = SIA.Store_No
LEFT JOIN ItemIdentifier II
ON SIA.Item_Key = II.Item_Key
LEFT JOIN Item I
ON SIA.Item_Key = I.Item_Key
INNER JOIN PRICE P
ON S.Store_No = P.Store_No  AND P.Item_Key = I.Item_Key
INNER JOIN PriceChgType PCT (nolock)
ON PCT.PriceChgTypeID = P.PriceChgTypeID
LEFT JOIN SubTeam ST
ON I.SubTeam_No = ST.SubTeam_No
LEFT JOIN ItemBrand B
ON B.Brand_ID = I.Brand_ID
LEFT JOIN ItemUnit IU
ON IU.Unit_ID = I.Retail_Unit_ID
WHERE
SIA.Exempt = 1
AND S.Store_No = @Store_No
--GROUP BY
ORDER BY ST.SubTeam_Name, II.Identifier
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_NA_MassExemptList] TO [IRMAReportsRole]
    AS [dbo];

