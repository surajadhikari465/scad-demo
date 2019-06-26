BEGIN
  IF Not Exists(SELECT 1 FROM sys.columns WHERE Name = N'OrderType_ID' AND Object_ID = Object_ID(N'amz.DeletedOrderItem'))
    ALTER TABLE amz.DeletedOrderItem ADD OrderType_ID INT;

  IF Not Exists(SELECT 1 FROM sys.columns WHERE Name = N'InsertDate' AND Object_ID = Object_ID(N'amz.DeletedOrderItem'))
    ALTER TABLE amz.DeletedOrderItem ADD InsertDate DateTime DEFAULT(GetDate());
END
GO

UPDATE A SET OrderType_ID = B.OrderType_ID
FROM amz.DeletedOrderItem A
INNER JOIN dbo.OrderHeader B on B.OrderHeader_ID = A.OrderHeader_ID
WHERE A.OrderType_ID is null;

UPDATE A SET OrderType_ID = B.OrderType_ID
FROM amz.DeletedOrderItem A
INNER JOIN dbo.DeletedOrder B on B.OrderHeader_ID = A.OrderHeader_ID
WHERE A.OrderType_ID is null;


DELETE FROM amz.DeletedOrderItem WHERE OrderType_ID IS NULL;
ALTER TABLE amz.DeletedOrderItem ALTER COLUMN [OrderType_ID] INT NOT NULL;