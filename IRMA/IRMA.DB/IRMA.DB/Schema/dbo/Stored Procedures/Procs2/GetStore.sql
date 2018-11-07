CREATE PROCEDURE dbo.GetStore 
	@Store_No int
AS
BEGIN
    SET NOCOUNT ON

	SELECT [Store_No], [Store_Name], [Phone_Number], [Mega_Store], [Distribution_Center], [Manufacturer], 
			[WFM_Store], [Internal], [TelnetUser], [TelnetPassword], 
			[BatchID], [BatchRecords], [BusinessUnit_ID], [Zone_ID], 
			[UNFI_Store], [LastRecvLogDate], [LastRecvLog_No], [RecvLogUser_ID], [StoreAbbr], 
			[EXEWarehouse], [Regional], Store.TaxJurisdictionID, TJ.TaxJurisdictionDesc, PSI_Store_No,
			Store.StoreJurisdictionID, SJ.StoreJurisdictionDesc
			,[GeoCode], PLUMStoreNo
    FROM Store (nolock)
    LEFT JOIN
		TaxJurisdiction TJ (nolock)
		ON TJ.TaxJurisdictionID = Store.TaxJurisdictionID
    LEFT JOIN
		StoreJurisdiction SJ (nolock)
		ON SJ.StoreJurisdictionID = Store.StoreJurisdictionID
    WHERE Store_No = @Store_No

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStore] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStore] TO [IRMAReportsRole]
    AS [dbo];

