IF EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'[dbo].[DC_ONHAND]') AND OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW [dbo].[DC_ONHAND]
GO


CREATE VIEW [dbo].[DC_ONHAND]
AS
SELECT     dbo.OnHand.Item_Key, dbo.OnHand.Store_No, dbo.OnHand.SubTeam_No, dbo.OnHand.Quantity, dbo.OnHand.Weight, dbo.OnHand.LastReset, 
                      dbo.SubTeam.SubTeam_Name, dbo.Store.Store_Name
FROM         dbo.OnHand INNER JOIN
                      dbo.SubTeam ON dbo.OnHand.SubTeam_No = dbo.SubTeam.SubTeam_No INNER JOIN
                      dbo.Store ON dbo.OnHand.Store_No = dbo.Store.Store_No

GO
