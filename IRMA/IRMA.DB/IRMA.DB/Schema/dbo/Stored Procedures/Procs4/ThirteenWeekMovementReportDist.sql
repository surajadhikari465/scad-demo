CREATE PROCEDURE dbo.ThirteenWeekMovementReportDist
    @Zone_ID int,
    @Store_No int,
    @SubTeam_No int,
    @Category_ID int,
    @FamilyCode varchar(13),
    @Identifier varchar(13),
    @EndDateStr varchar(255),
    @NumWeeks int,
    @IncludeSaleAmt int,
    @ReportAsUnit int
WITH RECOMPILE
AS

 /*
--FOR DEBUGGING.  This next block is used to plug in paramaters
DECLARE @Zone_ID int,
        @Store_No int,
        @SubTeam_No int,
        @Category_ID int,
        @FamilyCode varchar(255),
        @Identifier varchar(255),
        @EndDateStr varchar(255),
        @NumWeeks int,
        @IncludeSaleAmt int,
        @ReportAsUnit int
 
SELECT  --@Zone_ID = 2,
        @Store_No = 126,
        @SubTeam_No = 1700,
        --@Category_ID int,
       -- @FamilyCode = '7230',
        --@Identifier = '923444',
        @EndDateStr = '20051016',
        @NumWeeks = 1,
        @IncludeSaleAmt = 1,
        @ReportAsUnit = 1
 */
 
BEGIN
    SET NOCOUNT ON
 
    --Either the front end must verify that the week entered is the end of the fiscal week (I.E. day_of_Week = 7)
    -- or the Stored Proc gets the next date that is the end of a fiscal week, unless the date past in is the
    --end of the fiscal week.  This query uses the calculated end of fiscal week.
    
--These two units are used in the cost conversion function, which is used to convert 
--UnitsReceived to CasesReceived if the user wants to display Case values instead of unit values
DECLARE @UnitID int, @CaseID int
SELECT @UnitID = Unit_ID FROM ItemUnit WHERE UnitSysCode = 'unit'
SELECT @CaseID = Unit_ID FROM ItemUnit WHERE UnitSysCode = 'case'
 
--find end of fiscal week
    DECLARE @ENDDateDT datetime
    SET @ENDDateDT = (SELECT TOP 1 Date_key FROM [date] WHERE date_key >= cast(@EndDateStr as datetime) and day_of_Week = 7 ORDER BY date_key asc)    
 
--determine what week number each fiscal period\week is for the report
    DECLARE @Week table(seq int identity(1,1), [period] int, [week] int)
    INSERT INTO @Week([period], [week])
    SELECT [period] int, [week] int
    FROM [date]
    WHERE date_key between dateadd(day, (-@NumWeeks * 7) + 1, @ENDDateDT) and  @ENDDateDT
    GROUP BY Period, [week]
    ORDER BY Period, [week]
    
--Match up the report week number with each date that will be reported on
    DECLARE @DateTmp table(seq int, DateKey smalldatetime primary key, [period] int, [week] int)
    INSERT INTO @DateTmp
    SELECT wk.seq, Date_key, [date].[period], [date].[week] 
    FROM [date]
        INNER JOIN
            @week wk
            on wk.period = [date].period and wk.[week] = [date].[week]
    WHERE date_key between dateadd(day, (-@NumWeeks * 7) + 1, @ENDDateDT) and  @ENDDateDT
    ORDER BY Date_Key
    
 
--determine which distribution\manufacturer "stores" will be reported on
    DECLARE @DistVend table (Vendor_ID int)
    INSERT INTO @DistVend     
    SELECT V.Vendor_ID 
    FROM Store S (NOLOCK) 
        INNER JOIN
           Vendor V (NOLOCK)
           on V.Store_no = S.Store_no
    WHERE (S.Manufacturer = 1 or S.Distribution_Center = 1) 
           AND S.Store_No = isnull(@Store_No, S.Store_no) 
           and Zone_ID = isnull(@Zone_ID, S.zone_ID)
 
--Get the actual data for the report (denoted by the fields in the temporary table)
    DECLARE @ItemTmp table(Seq int, identifier varchar(13), Item_description varchar(60), Brand varchar(25), quantity numeric(9,4), cost numeric(9,4))
    INSERT INTO @itemtmp
        SELECT dt.seq, identifier, item_description, isnull(IB.Brand_Name, ''),
                sum(CASE WHEN @ReportAsUnit = 1
                           THEN OI.unitsreceived 
                           ELSE dbo.fn_CostConversion(OI.UnitsReceived, 
                                                  @CaseID, 
                                                  CASE WHEN I.CostedByWeight = 1 
                                                          THEN OI.Package_Unit_ID 
                                                          ELSE @UnitID 
                                                       END, 
                                                  OI.Package_Desc1, 
                                                  OI.Package_Desc2, 
                                                  OI.Package_Unit_ID) 
                        END * CASE WHEN oh.return_order = 0 
                                    THEN 1 
                                    ELSE -1 
                                   END) as Quantity, 
               sum((OI.receiveditemcost + OI.receivedfreight) * CASE WHEN oh.return_order = 0 THEN 1 ELSE -1 END) as cost
        FROM orderitem oi(nolock)
            INNER JOIN
                orderheader oh(nolock) 
                on oi.orderheader_id = oh.orderheader_id
            INNER JOIN
                @DistVend DistVend
                on DistVend.Vendor_ID = OH.Vendor_ID
            INNER JOIN
                Vendor RcvVend (nolock)
                on RcvVend.Vendor_Id = OH.ReceiveLocation_ID
            INNER JOIN
                Store RcvStore (nolock)
                on RcvStore.Store_no = RcvVend.Store_no
            INNER JOIN
                Item I(nolock)
                ON I.Item_Key = oi.Item_Key
                AND ISNULL(I.Category_ID, 0) = ISNULL(@Category_ID, ISNULL(I.Category_ID, 0))            
            LEFT JOIN
                ItemBrand IB (nolock)
                on IB.Brand_Id = I.Brand_ID
            INNER JOIN            
                ItemIdentifier II (nolock)
                ON II.Item_Key = I.Item_Key and
                   II.Default_Identifier =  CASE WHEN isnull(@identifier, '') + isnull(@FamilyCode, '') = '' 
                                                    THEN 1 
                                                    ELSE II.Default_Identifier 
                                                 END        
            INNER JOIN
                @DateTmp DT
                on DT.datekey = cast(convert(char(12), oh.CloseDate, 112) as smalldatetime) 
        WHERE I.subteam_no = @SubTeam_No and OH.Transfer_To_Subteam = @SubTeam_no
              and dbo.fn_GetCustomerType(RcvStore.Store_No, RcvStore.Internal, RcvStore.BusinessUnit_ID) <> 1
              and (RcvStore.Mega_Store = 1 or RcvStore.WFM_Store = 1)
              and cast(convert(char(12), OI.DateReceived, 112) as smalldatetime) between dateadd(day, (-@NumWeeks * 7) + 1, @ENDDateDT) and  @ENDDateDT
              and cast(convert(char(12), OH.CloseDate, 112) as smalldatetime) between dateadd(day, (-@NumWeeks * 7) + 1, @ENDDateDT) and  @ENDDateDT
              and II.Identifier like CASE WHEN not(@Identifier is null) and @familyCode is null 
                                            THEN @Identifier
                                          WHEN @Identifier is null and not(@familyCode is null) 
                                            THEN @familycode + '%'
                                          WHEN @Identifier is null and @familyCode is null 
                                            THEN II.Identifier
                                          END                      
        GROUP BY dt.seq, I.item_key, I.item_description, II.identifier, IB.Brand_name
 
--Return the data to the report
    SELECT it.identifier, it.item_description, it.Brand,
           max(CASE WHEN seq = 1 THEN quantity ELSE 0 END) week1_qty, max(CASE WHEN seq = 1 THEN cost ELSE 0 END) Week1_Sales,
           max(CASE WHEN seq = 2 THEN quantity ELSE 0 END) week2_qty, max(CASE WHEN seq = 2 THEN cost ELSE 0 END) Week2_Sales,
           max(CASE WHEN seq = 3 THEN quantity ELSE 0 END) week3_qty, max(CASE WHEN seq = 3 THEN cost ELSE 0 END) Week3_Sales,
           max(CASE WHEN seq = 4 THEN quantity ELSE 0 END) week4_qty, max(CASE WHEN seq = 4 THEN cost ELSE 0 END) Week4_Sales,
           max(CASE WHEN seq = 5 THEN quantity ELSE 0 END) week5_qty, max(CASE WHEN seq = 5 THEN cost ELSE 0 END) Week5_Sales,
           max(CASE WHEN seq = 6 THEN quantity ELSE 0 END) week6_qty, max(CASE WHEN seq = 6 THEN cost ELSE 0 END) Week6_Sales,
           max(CASE WHEN seq = 7 THEN quantity ELSE 0 END) week7_qty, max(CASE WHEN seq = 7 THEN cost ELSE 0 END) Week7_Sales,
           max(CASE WHEN seq = 8 THEN quantity ELSE 0 END) week8_qty, max(CASE WHEN seq = 8 THEN cost ELSE 0 END) Week8_Sales,
           max(CASE WHEN seq = 9 THEN quantity ELSE 0 END) week9_qty, max(CASE WHEN seq = 9 THEN cost ELSE 0 END) Week9_Sales,
           max(CASE WHEN seq = 10 THEN quantity ELSE 0 END) week10_qty, max(CASE WHEN seq = 10 THEN cost ELSE 0 END) Week10_Sales,
           max(CASE WHEN seq = 11 THEN quantity ELSE 0 END) week11_qty, max(CASE WHEN seq = 11 THEN cost ELSE 0 END) Week11_Sales,
           max(CASE WHEN seq = 12 THEN quantity ELSE 0 END) week12_qty, max(CASE WHEN seq = 12 THEN cost ELSE 0 END) Week12_Sales,
           max(CASE WHEN seq = 13 THEN quantity ELSE 0 END) week13_qty, max(CASE WHEN seq = 13 THEN cost ELSE 0 END) Week13_Sales
    FROM @itemtmp IT
    GROUP BY it.identifier, it.item_description, it.brand
    ORDER BY it.identifier desc
 
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThirteenWeekMovementReportDist] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThirteenWeekMovementReportDist] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThirteenWeekMovementReportDist] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ThirteenWeekMovementReportDist] TO [IRMAReportsRole]
    AS [dbo];

