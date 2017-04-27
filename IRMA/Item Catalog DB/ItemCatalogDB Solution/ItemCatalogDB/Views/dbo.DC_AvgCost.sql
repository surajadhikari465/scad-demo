IF EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'[dbo].[DC_AVGCOST]') AND OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW DC_AVGCOST
GO
CREATE VIEW [dbo].[DC_AVGCOST]
AS
        SELECT
			a.Item_Key,
			i.item_description,
			a.effective_date,
			a.AvgCost,
			a.Quantity,	
			a.Store_No,
			s.Store_Name,
			s.UseAvgCostHistory,
			a.Subteam_No,
			st.Subteam_Name,
            st.Dept_No	
		FROM AvgCostHistory (nolock) a
			LEFT OUTER JOIN Store (nolock) s
				ON s.Store_No = a.Store_No
			LEFT OUTER JOIN Subteam (nolock) st
				ON st.Subteam_No = a.Subteam_No
			INNER JOIN item (nolock) i
				ON i.item_key = a.item_key

GO
