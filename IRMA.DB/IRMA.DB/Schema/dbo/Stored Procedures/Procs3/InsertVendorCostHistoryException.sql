CREATE PROCEDURE dbo.InsertVendorCostHistoryException
	@Vendor_ID varchar(10), --'5103'
	@Item_ID varchar(20), --'209'
	@Sku_Regional bigint, --164140
	@UPC bigint,--3373900209
	@Item_Description varchar(50),--null
	@MSRP decimal(9, 2),--0
	@Case_Size decimal(9, 2),--1.0
	@OrigCase_Size decimal(9, 2),--1
	@Case_Price decimal(9, 3),--13.85
	@OrigUnit_Price decimal(9, 3),--27.3
	@Status_flag varchar(2),--'C', 
	@Start_Date datetime,--'02/23/2005'
	@End_Date datetime,--'12/31/1899',
    @Store_No int,-- 101, 
    @ExDescription varchar(50),--'Cost Change Difference is between %5 and %1000'
    @ExSeverity int, --3 
	@RuleID tinyint--2 

AS

BEGIN
    SET NOCOUNT ON
    
    DECLARE @SubTeam_No int
    select @SubTeam_No = (select SubTeam_No from item where item_key = @Sku_Regional) 

   INSERT INTO VendorCostHistoryExceptions (Vendor_ID, Item_ID, Sku_Regional, UPC, Item_Description, MSRP, OrigCase_Size,
                                          UserCase_Size, Case_Size, OrigUnit_Price, UserUnit_Price, Case_Price, Status_flag, UserStart_Date,
                                          Start_Date, UserEnd_Date, End_Date, Store_no, SubTeam_No, ExStatus, ExRuleID, 
                                          ExDescription, ExSeverity, User_ID, VCH_ID, LastModified, InsertDate)
               Values(@Vendor_ID, @Item_ID, @Sku_Regional, @UPC, @Item_Description, @MSRP, @OrigCase_Size, null, @Case_Size, @OrigUnit_Price, null,
                      @Case_Price, @Status_flag, null, @Start_Date, null, @End_Date, @Store_No, @SubTeam_No, 0, @RuleID, @ExDescription, @ExSeverity, null, null,null,getdate())

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendorCostHistoryException] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendorCostHistoryException] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendorCostHistoryException] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertVendorCostHistoryException] TO [IRMAAVCIRole]
    AS [dbo];

