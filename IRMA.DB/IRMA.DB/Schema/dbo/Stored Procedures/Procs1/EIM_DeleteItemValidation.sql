CREATE PROCEDURE dbo.EIM_DeleteItemValidation
    @Item_Key int
    
    
AS

BEGIN
    SET NOCOUNT ON
    
    DECLARE @ValidationSummary TABLE (ValidationCode INT, IsWarning bit, Description varchar(500))
    
    
    
        
        -- Is Item in pending Batch
        
        if exists (select * FROM PriceBatchDetail (nolock) PBD           
                    INNER JOIN                                             
                        PriceBatchHeader (nolock) PBH            
                        ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
                    WHERE PBD.Item_Key = @Item_Key 
                        AND ISNULL(PBH.PriceBatchStatusID, 0) between 1 and 5 
                        AND (PBD.ItemChgTypeID IS NOT NULL or PBD.PriceChgTypeID IS NOT NULL)
                        AND ISNULL(PBD.Expired, 0) = 0 
                        AND NOT(PBD.ItemChgTypeID = 6 AND PBD.StartDate > GetDate()))
                        Begin
                        Insert into @ValidationSummary
                        Select  ValidationCode, dbo.fn_IsWarningValidationCode(508), Description from ValidationCode
                        where ValidationCode = 508
                        END
                        Else
                        Begin
                        Insert into @ValidationSummary values (0, 0, 'Success')
                                                END
                                                
                                                
        -- Is Item in PO - Sent or Open status
        
        if exists (select * FROM OrderHeader (nolock) oh                 
                    INNER JOIN                                            
                        OrderItem (nolock) oi    
                        ON oh.OrderHeader_ID = oi.OrderHeader_ID
                    WHERE oi.Item_Key = @Item_Key 
                                                AND (oh.CloseDate is null))
                        Begin
                        Insert into @ValidationSummary 
                        Select  ValidationCode, dbo.fn_IsWarningValidationCode(507), Description from ValidationCode
                        where ValidationCode = 507
                        End
                        Else
                        Begin
                        Insert into @ValidationSummary values (0, 0, 'Success')
                                                END
        
        -- Does Item have sales in the last 90 days
        
        if exists (select * from Sales_SumByItem (nolock)
                                                WHERE Item_Key = @Item_Key
                                                AND Sales_Amount > 0 
                                                AND Date_Key > dateadd(day, -90, getdate()))
                        Begin
                        Insert into @ValidationSummary 
                        Select  ValidationCode, dbo.fn_IsWarningValidationCode(509), Description from ValidationCode
                        where ValidationCode = 509
                        End
                        Else
                        Begin
                        Insert into @ValidationSummary values (0, 0, 'Success')
                                                END
        
        -- Does Item have excessive inventory levels - ItemHistory table
        
        -- This is for future implementation
        
        
        Select * from @ValidationSummary
                
                
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_DeleteItemValidation] TO [IRMAClientRole]
    AS [dbo];

