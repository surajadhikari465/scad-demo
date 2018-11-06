SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[BuggyCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[BuggyCount]
GO


CREATE PROCEDURE dbo.BuggyCount
    @Store_No int,
    @StartDate datetime,
    @EndDate datetime,
    @SubtractDays smallint,
    @Store bit,                -- Next 3 bits are mutually exclusive - option group - but one is required
    @Team bit,
    @SubTeam bit,
    @Team_No int,           -- Next 2 int are optional and mutually exclusive 
    @SubTeam_No int
AS
BEGIN
    SET NOCOUNT ON

    IF @Store = 1

        SELECT Grouper, DT, COUNT(Sales) AS CustomerCount, SUM(Sales) AS TotalSales
        FROM
        	 (SELECT SUM (Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount) AS Sales, 
                     CONVERT(varchar(10),Time_Key, 21) AS DT,
                     Store_Name as Grouper
        	    FROM 
                    Sales_Fact (nolock) 
                    INNER JOIN 
                        Item (nolock) 
                        ON Sales_Fact.Item_Key = Item.Item_Key AND Sales_Account IS NULL
        			INNER JOIN 
                        StoreSubteam (nolock) 
                        ON (Sales_Fact.SubTeam_No = StoreSubTeam.SubTeam_No AND StoreSubTeam.Store_No = Sales_Fact.Store_No)
                    INNER JOIN
                        Store (nolock)
                        ON Store.Store_No = Sales_Fact.Store_No
					WHERE 
                    (Time_Key >= DATEADD(d, -1 * @SubtractDays, @StartDate) 
                    AND Time_Key < DATEADD(d, -1 * @SubtractDays + 1, @EndDate)) 
        	        AND Sales_Fact.Store_No = @Store_No
					GROUP BY Store_Name, Time_Key, Register_No, Transaction_No) AS T1 
        GROUP BY Grouper, DT

    ELSE

        IF @Team = 1

            SELECT Grouper, DT, COUNT(Sales) AS CustomerCount, SUM(Sales) AS TotalSales
            FROM
            	 (SELECT SUM (Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount) AS Sales, 
                         CONVERT(varchar(10),Time_Key, 21) AS DT,
                         Team_Name as Grouper
            	    FROM 
                        Sales_Fact (nolock) 
                        INNER JOIN 
                            Item (nolock) 
                            ON Sales_Fact.Item_Key = Item.Item_Key AND Sales_Account IS NULL
            			INNER JOIN 
                            StoreSubteam (nolock) 
                            ON (Sales_Fact.SubTeam_No = StoreSubTeam.SubTeam_No AND StoreSubTeam.Store_No = Sales_Fact.Store_No)
                       	INNER JOIN
                           Team (nolock)
                           ON Team.Team_No = StoreSubTeam.Team_No 
						INNER JOIN 
						   SubTeam 
						   ON SubTeam.SubTeam_No = Sales_Fact.SubTeam_No AND SubTeam.Retail = 1
					WHERE 
                        (Time_Key >= DATEADD(d, -1 * @SubtractDays, @StartDate) 
                        AND Time_Key < DATEADD(d, -1 * @SubtractDays + 1, @EndDate)) 
            	        AND Sales_Fact.Store_No = @Store_No
                        AND StoreSubTeam.Team_No = ISNULL(@Team_No, StoreSubTeam.Team_No)
			GROUP BY Team_Name, Time_Key, Register_No, Transaction_No) AS T1 
            GROUP BY Grouper, DT

        ELSE

            SELECT Grouper, DT, COUNT(Sales) AS CustomerCount, SUM(Sales) AS TotalSales
            FROM
            	 (SELECT SUM (Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount - Store_Coupon_Amount) AS Sales, 
                         CONVERT(varchar(10),Time_Key, 21) AS DT,
                         SubTeam_Name as Grouper
            	    FROM 
                        Sales_Fact (nolock) 
                        INNER JOIN 
                            Item (nolock) 
                            ON Sales_Fact.Item_Key = Item.Item_Key AND Sales_Account IS NULL
            			INNER JOIN 
                            StoreSubteam (nolock) 
                            ON (Sales_Fact.SubTeam_No = StoreSubTeam.SubTeam_No AND StoreSubTeam.Store_No = Sales_Fact.Store_No)
						INNER JOIN
                            SubTeam (nolock)
                            ON SubTeam.SubTeam_No = StoreSubTeam.SubTeam_No  AND SubTeam.Retail = 1  
            	    WHERE 
                        (Time_Key >= DATEADD(d, -1 * @SubtractDays, @StartDate) 
                        AND Time_Key < DATEADD(d, -1 * @SubtractDays + 1, @EndDate)) 
            	        AND Sales_Fact.Store_No = @Store_No
                        AND StoreSubTeam.SubTeam_No = ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No)
						
            	    GROUP BY SubTeam_Name, Time_Key, Register_No, Transaction_No) AS T1 
            GROUP BY Grouper, DT                    
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


