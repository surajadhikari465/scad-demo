SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeptComp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeptComp]
GO



SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.DeptComp
@Month int,
@Year int,
@Team_No int,
@SubTeam_No int,
@Store_No int,
@Zone_ID int,
@Identifier varchar(13),
@familycode varchar(13)
AS

/*DECLARE @Month int
DECLARE @Year int
DECLARE @Team_No int
DECLARE @SubTeam_No int
DECLARE @Store_No int
DECLARE @Zone_ID int
DECLARE @Identifier varchar(13)
DECLARE @familycode varchar(5)

SELECT @Month = 9
SELECT @Year = 2004
--LECT @Team_No int
SELECT @SubTeam_No = 2100
SELECT @Store_No = 101
--SELECT @Zone_ID int
--SELECT @Identifier varchar(13)
--SELECT @familycode varchar(5)*/

DECLARE @StoreSubTeam table(Store_no int,
                            Team_No int, 
                            SubTeam_No int)
INSERT INTO @StoreSubTeam
SELECT Store_No, Team_No, SubTeam_No
FROM StoreSubTeam (nolock)
WHERE ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No 
      AND ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No)= StoreSubTeam.SubTeam_No
      AND ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No 

SELECT @Identifier = ISNULL(@identifier,@familycode) + CASE WHEN @familycode IS NULL THEN '' ELSE '%' END


SELECT  tmp1.Date_key, 
        ISNULL(CAST(tmp2.LYtotalprice As varchar(12)),'') As LYTotalPrice, 
        ISNULL(CAST(tmp1.TYTotalPrice As varchar(12)),'') As TYTotalPrice
FROM (
      SELECT Date.date_key, 
             Date.day_of_Year, 
             SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount)AS TYTotalPrice
      FROM Date (nolock)
          INNER JOIN
                Sales_SumByItem (nolock)
                ON Sales_SumByItem.Date_key = Date.Date_Key
          INNER JOIN 
                Item (nolock) 
                ON Sales_SumByItem.Item_Key = Item.Item_Key
          INNER JOIN 
                ItemIdentifier (nolock) 
                ON (Item.Item_Key = ItemIdentifier.Item_Key)
                   and Default_Identifier = case when @Identifier is null then 1 else default_identifier end
          INNER JOIN 
                @StoreSubTeam SST 
                ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No
                    AND Sales_SumByItem.Store_No = SST.Store_No)
      WHERE Date.Period = @month and Date.Year = @year
           AND ISNULL(@Store_No, Sales_SumByItem.Store_No) = Sales_SumByItem.Store_No
           AND ItemIdentifier.identifier LIKE ISNULL(@identifier, ItemIdentifier.Identifier)
      GROUP BY Date.Date_key, Date.Day_of_Year      
    UNION
      SELECT Date.Date_key,
             Date.Day_of_Year,
             NULL
      FROM Date (nolock)
      WHERE Date.Period = @month
                AND Date.Year = @year
                AND Date_key NOT IN (SELECT Date.Date_key
                                     FROM Date (nolock)
                                        INNER JOIN
                                             sales_sumbyitem (nolock)
                                             ON Sales_SumByItem.Date_Key = Date.Date_Key
                                        INNER JOIN
                                             @StoreSubTeam SST 
                                             ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No
                                                 AND Sales_SumByItem.Store_No = SST.Store_No)
                                     WHERE Date.Period = @month AND Date.Year = @year
                                          AND ISNULL(@Store_No, Sales_SumByItem.Store_No) = Sales_SumByItem.Store_No 
                                    GROUP BY Date.Date_key)
	 ) As tmp1
        LEFT JOIN (
           SELECT  Date.Date_key, 
                   Date.Day_Of_Year, 
                   SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) 
                        + SUM(Promotion_Amount) AS LYTotalPrice
           FROM Date (nolock)
                 INNER JOIN
                    Sales_SumByItem (nolock)
                    ON Sales_SumByItem.Date_key = Date.Date_key
                 INNER JOIN 
                    Item (nolock) 
                    ON Sales_SumByItem.Item_Key = Item.Item_Key
            	 INNER JOIN 
                    ItemIdentifier (nolock) 
                    ON (Item.Item_Key = ItemIdentifier.Item_Key
                        AND ItemIdentifier.default_Identifier = 1)
                 INNER JOIN 
                    @StoreSubTeam SST 
                    ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No
                        AND Sales_SumByItem.Store_No = SST.Store_No)
           WHERE Date.Period = @month AND Date.Year = @year - 1
        	     AND ISNULL(@Store_No, Sales_SumByItem.Store_No) = Sales_SumByItem.Store_No 
            	 AND ItemIdentifier.identifier LIKE ISNULL(@identifier, ItemIdentifier.Identifier)
           GROUP BY Date.Date_key, Date.Day_of_Year
         UNION
            SELECT Date.Date_key,
                   Date.Day_Of_Year,
                   NULL
            FROM Date (nolock)
            WHERE Date.period = @month AND Date.Year = @year - 1
                  AND Date_key NOT IN (SELECT Date.Date_key
                                       FROM Date (nolock)
                                           INNER JOIN
                                               Sales_SumByItem (nolock)
                                               ON Sales_SumByItem.Date_key = Date.Date_key
                                           INNER JOIN
                                               @StoreSubTeam SST 
                                               ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No
                                                   AND Sales_SumByItem.Store_No = SST.Store_No)
                                       WHERE Date.Period = @month AND Date.Year = @year - 1 
                                             AND ISNULL(@Store_No, Sales_SumByItem.Store_No) = Sales_SumByItem.Store_No 
                                       GROUP BY Date.Date_key) 
          ) As tmp2 ON tmp1.Day_of_Year = tmp2.Day_Of_Year
ORDER BY tmp1.Date_Key ASC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

