CREATE PROCEDURE dbo.ZeroMovementReport
    @BeginDate varchar(10),
    @EndDate varchar(10),
    @SubTeam_No int,
	@Store_No varchar(8000)
WITH RECOMPILE
AS

BEGIN

	SET NOCOUNT ON

	CREATE TABLE #ItemList (Item_Key int PRIMARY KEY)
	CREATE TABLE #SoldList (Date_Key smalldatetime, Item_Key int PRIMARY KEY)

	/* This is the list of all items for the Selected team */
	
    /* This list is everything in ItemCatalog, it should be
     * all subteam items authorized for that store.
     */
    INSERT INTO #ItemList
    SELECT i.item_key 
	FROM item i (nolock)
	INNER JOIN subteam st (nolock) 
        ON i.subteam_no = st.subteam_no
	INNER JOIN storesubteam sst (nolock)
		on st.subteam_no = sst.subteam_no 
	INNER JOIN store s (nolock)
		ON sst.store_no = s.store_no
	INNER JOIN storeitemvendor siv (nolock)
        ON i.item_key = siv.item_key
            AND siv.store_no = s.store_no
	INNER JOIN dbo.fn_Parse_List(@Store_No, '|') ip 
        ON siv.Store_No = ip.Key_Value
	WHERE i.deleted_item = 0
	AND i.retail_sale = 1
	AND i.subteam_no = ISNULL(@SubTeam_No, i.subteam_no) 
	AND i.INSERT_date <= CONVERT(smalldatetime, @EndDate)
	GROUP BY i.item_key

	/* This is the list of items for the Selecteed team that have had sales since 
	   the begin date specified */
	INSERT INTO #SoldList
	SELECT  max(date_key), ssbi.item_key 
	FROM sales_sumbyitem ssbi(nolock)
	INNER JOIN 
		subteam st(nolock) 
		on st.subteam_no = ssbi.subteam_no
	INNER JOIN dbo.fn_Parse_List(@Store_No, '|') ip 
        ON ssbi.Store_No = ip.Key_Value
	where ssbi.date_key >= CONVERT(smalldatetime, @BeginDate)  --BeginDate
	AND ssbi.date_key <= CONVERT(smalldatetime, @EndDate)  --EndDate
	AND ssbi.subteam_no = ISNULL(@SubTeam_No, ssbi.subteam_no)  --Subteam_No
	GROUP BY ssbi.item_key


	/* This is the list of items for the selected team that have not had sales 
	in the period specified */

	SELECT 
        st.SubTeam_Name, 
        II.Identifier, 
        i.Item_Description, 
        i.INSERT_Date, 
        s.store_no,
        s.store_name
	FROM ItemIdentifier II(nolock)
	INNER JOIN 
		Item I(nolock) 
		ON II.Item_Key = I.Item_Key
	INNER JOIN 
		Subteam st (nolock) 
		ON i.subteam_no = st.subteam_no
	INNER JOIN storesubteam sst (nolock)
		ON st.subteam_no = sst.subteam_no 
	INNER JOIN store s (nolock)
		ON sst.store_no = s.store_no
	INNER JOIN dbo.fn_Parse_List(@Store_No, '|') ip 
        ON sst.Store_No = ip.Key_Value
	WHERE I.Item_Key  IN 
		(SELECT item_key 
            FROM #ItemList 
            WHERE item_key NOT IN 
                (SELECT item_key FROM #SoldList)
        )
		AND Default_Identifier = 1

	ORDER BY s.store_name, st.SubTeam_Name, i.Item_Description

	DROP TABLE #ItemList
	DROP TABLE #SoldList

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ZeroMovementReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ZeroMovementReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ZeroMovementReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ZeroMovementReport] TO [IRMAReportsRole]
    AS [dbo];

