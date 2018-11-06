SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ControlGroup3WayMatchLogReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
   drop procedure [dbo].[ControlGroup3WayMatchLogReport]
GO


CREATE PROCEDURE [dbo].[ControlGroup3WayMatchLogReport]
@ControlGroup_ID int,
@User_ID int,
@Status_ID int

AS

-- ================================================================================
-- Author      :	Sekhara
-- Create date :    12/27/2007
-- Description :	ControlGroup3WayMatchLogReport
--
-- Modification History:
-- Date			Init	TFS		Comment
-- 12/26/2011	BAS		3744	coding standards
-- =================================================================================

BEGIN
    SET NOCOUNT ON
    Select 
		'ControlGroupID'	= OrderInvoice_ControlGroup_ID,
		'Expected'			= ExpectedGrossAmt,
		'ExpectedInvoices'	= ExpectedInvoiceCount,
		'Status'			= CGS.OrderInvoice_ControlGroupStatus_Desc,
		'RunningTotal'		= CurrentGrossAmt,
		'RunningInvoices'	= CurrentInvoiceCount,
		'UserID'			= u.userName,
		'DateTimeEntry'		= InsertDate,
		'InvoiceNumber'		= InvoiceNumber,
		'VendorID'			= Vendor_ID,
		'InvoiceAmount'		=	CASE 
									WHEN Return_Order = 1 THEN
										-(ISNUll(InvoiceCost,0)+ ISNULL(InvoiceFreight,0))
									ELSE
										(ISNUll(InvoiceCost,0)+ ISNULL(InvoiceFreight,0))
								END
		
      FROM
		  Orderinvoice_ControlGroupLog						(nolock) cgl
		  INNER JOIN dbo.OrderInvoice_ControlGroupStatus	(nolock) cgs	ON	cgl.OrderInvoice_ControlGroupStatus_ID	= cgs.OrderInvoice_ControlGroupStatus_ID
		  INNER JOIN dbo.users								(nolock) u		ON	cgl.UpdateUser_ID						= u.User_ID
      
	WHERE
		OrderInvoice_ControlGroup_ID				= ISNULL(@ControlGroup_ID,cgl.OrderInvoice_ControlGroup_ID)
		AND UpdateUser_ID							= ISNULL(@User_ID,cgl.UpdateUser_ID)
		AND cgl.OrderInvoice_ControlGroupStatus_ID	= ISNULL(@Status_ID,cgl.OrderInvoice_ControlGroupStatus_ID)
		
	ORDER BY InsertDate ASC
	
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO






