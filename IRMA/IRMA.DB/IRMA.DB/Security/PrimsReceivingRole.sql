create role [PrimsReceivingRole]
	authorization [dbo]
go

---------------------------------------------------------
---------------------------------------------------------

-- Existing, separate permission; adding to new role.
grant exec on [dbo].[ReceiveOrderItem3] to [PrimsReceivingRole]

-- New perms to auto-close POs.
grant exec on [dbo].[UpdateOrderClosed] to [PrimsReceivingRole]
grant exec on [dbo].[AutomaticOrderOriginUpdate] to [PrimsReceivingRole]
go

-- New perm August, 2022.
grant exec on [dbo].[UpdateOrderHeaderRefuseReceivingAndClose] to [PrimsReceivingRole]

-- New order-info perms Oct, 2022.
grant exec on [dbo].[GetOrderItems] to [PrimsReceivingRole]
grant exec on [dbo].[GetOrderInfo] to [PrimsReceivingRole]

---------------------------------------------------------
---------------------------------------------------------

ALTER ROLE [PrimsReceivingRole] ADD MEMBER [prims-receipts]
go
