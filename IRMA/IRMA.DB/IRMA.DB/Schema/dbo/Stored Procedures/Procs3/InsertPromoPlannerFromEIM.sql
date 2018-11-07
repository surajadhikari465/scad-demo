CREATE PROCEDURE dbo.[InsertPromoPlannerFromEIM]
        @Item_Key  int,
        @Store_No int,
        @PriceChgTypeID tinyint,
        @Start_Date smalldatetime,
        @Multiple tinyint,
        @Price smallmoney,
        @Sale_Multiple tinyint,
        @Sale_Price smallmoney,
        @Sale_Cost smallmoney,
        @Sale_End_Date smalldatetime,
        @Identifier varchar(13),
        @Dept_No int,
        @Vendor_Id int,
        @ProjUnits int,
        @Comment1 char(50), 
        @Comment2 char(50),
        @BillBack numeric(6,2)
AS

BEGIN
    SET NOCOUNT ON

     INSERT INTO [dbo].[PriceBatchPromo]
           ([Item_Key],
           [Store_No],
           [PriceChgTypeID],
           [Start_Date],
           [Multiple],
           [Price],
           [Sale_Multiple],
           [Sale_Price],
           [Sale_Cost],
           [Sale_End_Date],
           [Identifier],
           [Dept_No],
           [Vendor_Id],
           [ProjUnits],
           [Comment1],
           [Comment2],
           [BillBack])
     VALUES
           (@Item_Key,
           @Store_No,
           @PriceChgTypeID,
           @Start_Date,
           @Multiple,
           @Price,
           @Sale_Multiple,
           @Sale_Price,
           @Sale_Cost,
           @Sale_End_Date,
           @Identifier,
           @Dept_No,
           @Vendor_Id,
           @ProjUnits,
           @Comment1,
           @Comment2,
           @BillBack);

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPromoPlannerFromEIM] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPromoPlannerFromEIM] TO [IRMAPromoRole]
    AS [dbo];

