CREATE PROCEDURE [dbo].[DistInvFrght]
    @OrderHeader_ID int,
    @TotInvFrght money
AS
--20080611 - davestacey - removed redundant inline sql call for performance reasons
BEGIN    

	--4/6/2010 RDE  Removed broken functionality until it can be reworked. For now just update OrderInvoice.InvoiceFreight
	
	Update OrderInvoice
	set invoicefreight = @totinvfrght 
	where orderheader_id = @orderheader_id 

	
	
/*        
    DECLARE @EstFrghtTot money, @Variance decimal(18,4),    
            @Error_No int, @Severity smallint        
        
    SELECT @Error_No = 0    
    
    
    
    
DECLARE @Vendor_ID INT, @Store_No INT    
    DECLARE @OrderDate datetime    

    
    SELECT @Vendor_ID = OH.Vendor_ID, @Store_No = V.Store_No,     
           @OrderDate = OH.OrderDate    
    FROM dbo.OrderHeader OH (NOLOCK)    
        JOIN dbo.Vendor V (NOLOCK) ON V.Vendor_ID = OH.ReceiveLocation_ID    
    WHERE OrderHeader_ID = @OrderHeader_ID         
    
 DECLARE @VCI TABLE (Item_Key int unique, UnitFreight smallmoney)    
     
 INSERT @VCI (Item_Key, UnitFreight)  
  SELECT vc.Item_Key, MAX(vc.UnitFreight)  
  FROM dbo.fn_VendorCostItems(@Vendor_ID, @Store_No, @OrderDate) vc    
   INNER JOIN dbo.OrderItem OI (NOLOCK) ON OI.Item_Key = vc.Item_Key    
  WHERE OI.OrderHeader_Id = @OrderHeader_Id  
  GROUP BY vc.Item_Key  
    
    -- Calculate the total estimated freight based on the item price info    
    SELECT @EstFrghtTot = ISNULL(SUM(UnitsReceived * ISNULL(VC.UnitFreight, 0)), 0)    
    FROM dbo.OrderItem OI (NOLOCK)    
  LEFT JOIN @VCI VC ON OI.Item_key = VC.Item_Key    
  JOIN dbo.Price P (NOLOCK) ON P.Item_Key = OI.Item_Key    
  JOIN dbo.OrderHeader OH (NOLOCK) ON OI.OrderHeader_ID = OH.OrderHeader_ID    
  JOIN dbo.Vendor V (NOLOCK) ON OH.ReceiveLocation_ID = V.Vendor_ID AND P.Store_No = V.Store_No    
    WHERE OI.OrderHeader_ID = @OrderHeader_ID    
    
    
    
    
    BEGIN TRAN    
        
        
        
    -- Distribute the freight to the order items based upon the estimated freight vs. actual freight    
    IF @Error_No = 0    
    BEGIN    
        IF @EstFrghtTot > 0    
        BEGIN    
            SELECT @Variance = CASE WHEN @EstFrghtTot <> 0     
                                    THEN (@TotInvFrght - @EstFrghtTot) / @EstFrghtTot     
                                    ELSE 0 END    
            
            UPDATE dbo.OrderItem    
            SET ReceivedItemFreight = T1.ReceivedItemFreight, ReceivedFreight = T1.ReceivedFreight    
            FROM dbo.OrderItem OI (NOLOCK)    
            JOIN (SELECT OI.Item_Key,    
                               ((UnitsReceived * ISNULL(VC.UnitFreight,0)) * @Variance) +     
                                (UnitsReceived * ISNULL(VC.UnitFreight,0))     
                               AS ReceivedItemFreight,    
                               CASE WHEN UnitsReceived <> 0     
                                    THEN (((UnitsReceived * ISNULL(VC.UnitFreight,0)) * @Variance) +     
                                           (UnitsReceived * ISNULL(VC.UnitFreight,0))) / UnitsReceived     
                                    ELSE 0 END     
                               AS ReceivedFreight    
                        FROM dbo.OrderItem OI (NOLOCK)    
                            LEFT JOIN @VCI VC ON OI.Item_key = VC.Item_Key    
                            JOIN dbo.OrderHeader OH (NOLOCK) ON OI.OrderHeader_ID = OH.OrderHeader_ID    
                            JOIN dbo.Vendor V (NOLOCK) ON OH.ReceiveLocation_ID = V.Vendor_ID    
                        WHERE OI.OrderHeader_ID = @OrderHeader_ID) T1     
                        ON T1.Item_Key = OI.Item_Key    
            WHERE OI.OrderHeader_ID = @OrderHeader_ID    
            
            SELECT @Error_No = @@ERROR    
            
        END    
        ELSE     
        BEGIN    
        -- No estimated freight:    
        -- Set the orderitem received freight data to 0 because cannot distribute    
        -- to the items without estimated freight    
    
            UPDATE dbo.OrderItem    
            SET ReceivedItemFreight = 0, ReceivedFreight = 0    
            WHERE OrderHeader_ID = @OrderHeader_ID    
        
            SELECT @Error_No = @@ERROR    
        END    
    END    
    
    -- Update the OrderInvoice with freight distributed among the subteams based upon     
    -- percentage of subteam estitmated freight vs. the total estimated freight    
    IF @Error_No = 0    
    BEGIN    
        IF @EstFrghtTot > 0    
        BEGIN    
            UPDATE dbo.OrderInvoice    
            -- SubTeamFreight = (SubTeamEstimatedFreight / TotalEstimatedFreight) * TotalFreight    
            SET InvoiceFreight = ROUND(ISNULL((SELECT SUM(UnitsReceived * ISNULL(VC.UnitFreight,0))    
                                               FROM dbo.OrderItem OI (NOLOCK)    
                                                   LEFT JOIN @VCI VC ON OI.Item_key = VC.Item_Key    
                                                   JOIN dbo.Item (NOLOCK) ON Item.Item_Key = OI.Item_Key    
                                                   JOIN dbo.OrderHeader OH (NOLOCK) ON OI.OrderHeader_ID = OH.OrderHeader_ID    
                                                   JOIN dbo.Vendor V (NOLOCK) ON OH.ReceiveLocation_ID = V.Vendor_ID    
                                               WHERE OI.OrderHeader_ID = @OrderHeader_ID    
                                                     AND Item.SubTeam_No = OrderInvoice.SubTeam_No), 0) / @EstFrghtTot, 2)    
                                 * @TotInvFrght    
            WHERE OrderHeader_ID = @OrderHeader_ID    
    
    
    
    
            SELECT @Error_No = @@ERROR    
        END    
    
        IF @Error_No = 0    
        BEGIN    
            -- If the resulting SubTeam totals do not add up to the invoice total freight,    
            -- distribute the difference evenly across the subteams    
            DECLARE @TotSubTeamInvFrght smallmoney, @TotFrghtDiff smallmoney    
        
            SELECT @TotSubTeamInvFrght = SUM(InvoiceFreight)    
            FROM dbo.OrderInvoice (NOLOCK)     
            WHERE OrderHeader_ID = @OrderHeader_ID    
        
            IF ISNULL(@TotSubTeamInvFrght, 0) <> @TotInvFrght    
            BEGIN    
                DECLARE @SubTeamCount int    
        
                SELECT @SubTeamCount = COUNT(*) FROM dbo.OrderInvoice (NOLOCK) WHERE OrderHeader_ID = @OrderHeader_ID    
        
                SELECT @TotFrghtDiff = @TotInvFrght - ISNULL(@TotSubTeamInvFrght, 0)    
        
                UPDATE dbo.OrderInvoice    
                SET InvoiceFreight = CASE WHEN ISNULL(@SubTeamCount, 0) <> 0     
          THEN ROUND(ISNULL(InvoiceFreight, 0) +  (@TotFrghtDiff / @SubTeamCount), 2)     
          ELSE ROUND(ISNULL(InvoiceFreight, 0) +  (@TotFrghtDiff), 2) END     
                WHERE OrderHeader_ID = @OrderHeader_ID    
            END    
            ELSE    
            BEGIN    
                IF @TotSubTeamInvFrght IS NULL    
                    UPDATE dbo.OrderInvoice    
                    SET InvoiceFreight = 0    
                    WHERE InvoiceFreight IS NULL    
                    AND OrderHeader_ID = @OrderHeader_ID    
            END    
    
        END    
    END    
    
    IF @Error_No = 0    
        COMMIT TRAN    
    ELSE    
    BEGIN    
        ROLLBACK TRAN    
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)    
        RAISERROR ('DistInvFrght failed with @@ERROR: %d', @Severity, 1, @Error_No)    
    END    
    */
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DistInvFrght] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DistInvFrght] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DistInvFrght] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DistInvFrght] TO [IRMAReportsRole]
    AS [dbo];

