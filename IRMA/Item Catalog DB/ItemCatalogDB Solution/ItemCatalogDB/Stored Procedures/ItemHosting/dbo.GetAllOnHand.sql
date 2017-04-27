SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAllOnHand]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAllOnHand]
GO

CREATE PROCEDURE dbo.GetAllOnHand  
    @Item_Key	int

AS 

-- **************************************************************************
-- Procedure: GetAllOnHand()
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2010/02/02	TL		11692	For LastOrderedCost column of select:
--								1) Removed where-clause filter on Orderi.UnitsReceived that was only allowing positive values.
--								2) Added divide-by-zero check against Orderi.UnitsReceived value.
-- 2011/12/19	KM		3744	Extension change to .sql; code formatting.
-- 2013/08/02	KM		13365	Use only AvgCost or LastCost depending on whether the region uses average cost to value inventory;
-- **************************************************************************

DECLARE
	@ReturnCostUnit varchar(5)	= dbo.fn_GetRetailUnitAbbreviation(@Item_Key),
	@UseAverageCost bit			= dbo.fn_InstanceDataValue('UseAvgCostforCostandMargin', null)

SELECT
	s.Store_No,
	s.Store_Name,
	SubTeam_Name,
	st.SubTeam_No,
	AvgWeight				= ISNULL(	CASE 
											WHEN Weight = 0 OR Quantity = 0	THEN 0
											ELSE Weight / Quantity
										END, 0),
	TotalUnits				= ISNULL(Quantity,0),
	TotalWeight				= ISNULL(Weight,0),
	RetailUOM				= @ReturnCostUnit,
	AvgCost					=	CASE 
									WHEN @UseAverageCost = 1
										THEN ISNULL((SELECT TOP 1
														AvgCost
													FROM
														AvgCostHistory (nolock)
													WHERE 
														Item_Key			= @Item_Key
														AND Store_No		= s.Store_No
														AND SubTeam_No		= ISNULL(oa.SubTeam_No, i.SubTeam_No)
														AND Effective_Date	<= GETDATE()
													ORDER BY 
														Effective_Date DESC), 0)
										ELSE 0
								END,	
	LastOrderedCost			=	CASE
									WHEN @UseAverageCost = 0
										THEN ISNULL((SELECT TOP 1
														(ReceivedItemCost + ReceivedItemFreight) /	CASE
																										WHEN UnitsReceived <> 0 THEN UnitsReceived 
																										ELSE 1
																									END
													FROM
														OrderItem				(nolock)	oi
														INNER JOIN OrderHeader	(nolock)	oh	ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
														INNER JOIN Vendor		(nolock)	sv	ON	oh.ReceiveLocation_ID	= sv.Vendor_ID
													WHERE
														oi.Item_Key					= @Item_Key
														AND sv.Store_No				= s.Store_No
														AND oh.Transfer_To_SubTeam	= ISNULL(oa.SubTeam_No, i.SubTeam_No)
														AND Return_Order = 0
			  
													ORDER BY
														DateReceived DESC), 0)
										ELSE 0
								END,
	LastCostUnit			= @ReturnCostUnit

FROM
	Item				(nolock)	i
	CROSS JOIN Store	(nolock)	s
	LEFT  JOIN OnHand	(nolock)	oa	ON  i.Item_Key		= oa.Item_Key
										AND s.Store_No		= oa.Store_No
	INNER JOIN SubTeam	(nolock)	st	ON st.SubTeam_No	= ISNULL(oa.SubTeam_No, i.SubTeam_No)

WHERE 
	i.Item_Key = @Item_Key

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO