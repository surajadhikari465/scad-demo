GRANT SELECT ON ItemVendor  TO IconInterface;
GRANT SELECT ON StoreItemVendor  TO IconInterface;
GRANT SELECT ON Users  TO IconInterface;
GRANT SELECT ON Team     TO IconInterface;
GRANT SELECT ON amz.EventType  TO IconInterface;
GRANT SELECT ON OrderItem      TO IconInterface;
GRANT SELECT ON Item           TO IconInterface;
GRANT SELECT ON ItemIdentifier TO IconInterface;
GRANT SELECT ON ItemUnit       TO IconInterface;
GRANT SELECT ON ItemHistory		TO IconInterface;
GRANT SELECT ON Vendor  TO IconInterface;
GRANT SELECT ON Store   TO IconInterface;
GRANT SELECT ON SubTeam  TO IconInterface;
GRANT SELECT ON OrderHeader  TO IconInterface;
GRANT SELECT ON ExternalOrderInformation TO IconInterface;
GRANT SELECT ON OrderExternalSource      TO IconInterface;
GRANT SELECT ON AppConfigApp  TO IconInterface;
GRANT SELECT ON Version  TO IconInterface;
GRANT SELECT ON AppConfigEnv  TO IconInterface;
GRANT SELECT ON AppConfigApp  TO IconInterface;
GRANT INSERT ON AppLog TO IconInterface;
GRANT SELECT ON amz.DeletedOrderItem To IconInterface;
GRANT SELECT on dbo.DeletedOrderItem To IconInterface;
GRANT SELECT on dbo.DeletedOrder To IconInterface;
GRANT INSERT,Update,SELECT ON amz.MessageArchive  TO IconInterface;
GRANT SELECT,UPDATE ON amz.ReceiptQueue TO IconInterface;
GRANT SELECT,UPDATE ON amz.OrderQueue TO IconInterface;
-- 7/28/23 - For "IRMA Inventory Producer" app:
GRANT INSERT ON amz.OrderQueue TO IconInterface;
GRANT SELECT,UPDATE ON amz.InventoryQueue TO IconInterface;
GRANT INSERT ON [amz].[MessageArchiveEvent] TO IconInterface AS [dbo];
GRANT EXECUTE
    ON OBJECT::[amz].[DequeueInventory] TO IconInterface
    AS [dbo];
GRANT EXECUTE
    ON OBJECT::[amz].[DequeueOrders] TO IconInterface
    AS [dbo];
GRANT EXECUTE
    ON OBJECT::[amz].[DequeueReceipts] TO IconInterface
    AS [dbo];
GRANT EXECUTE
    ON OBJECT::[amz].[DequeueTransfers] TO IconInterface
    AS [dbo];
GRANT EXECUTE
    ON OBJECT::[amz].[GetUnsentInStockMessages] TO IconInterface
    AS [dbo];
