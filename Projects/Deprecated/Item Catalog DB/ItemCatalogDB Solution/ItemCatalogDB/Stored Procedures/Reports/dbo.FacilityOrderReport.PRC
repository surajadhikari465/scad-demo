
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].FacilityOrderReport') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].FacilityOrderReport
GO

CREATE PROCEDURE [dbo].[FacilityOrderReport]
 @Facility_ID INT,
 @Subteam_No varchar(1024),
 @Store_No varchar(1024),
 @Status_ID INT,
 @SortLevel1 INT,
 @SortLevel2 INT,
 @SortLevel3 INT,
 @StartDate DATETIME,
 @EndDate DATETIME
WITH RECOMPILE
AS
BEGIN


/*
=============================================
Author:		Brian Robichaud
Create date: 6/19/2009
Description:	Facility Order Report
=============================================

##############################
Mod History
##############################

---------------------------------------------------------------------------------------------------------------------------------
TFS 11464
Tom Lux
1/1/10
Changed store and subteam params to varchar so they can accept pipe-delimited list of values.
---------------------------------------------------------------------------------------------------------------------------------

EXEC FacilityOrderReport 720,NULL,NULL,1,1,2,3,'2009-6-7','2009-6-11'

FL
FacilityOrderReport 2255, null, null, 3, 3, 2, 1, '2009-12-1', '2009-12-5'
FacilityOrderReport 2255, null, '801|804|805|806', 3, 3, 2, 1, '2009-12-1', '2009-12-5'

---------------------------------------------------------------------------------------------------------------------------------
TFS 13667 
Min Zhao
9/13/13
Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
---------------------------------------------------------------------------------------------------------------------------------

*/
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET NOCOUNT ON;

DECLARE @SQL VARCHAR(5000)
DECLARE @tblSubTeam TABLE (SubTeam_No int)
DECLARE @tblStore TABLE (Store_No int)
DECLARE @SubTeamList varchar(5000)
DECLARE @StoreList varchar(5000)

SELECT @SQL = 
	            'SELECT OH.OrderHeader_ID, S.Store_Name, OH.Transfer_To_SubTeam, OH.OrderDate, OH.Expected_Date, U.FullName, OH.SentDate, OH.WarehouseSentDate, 
	                         OH.CloseDate, OH.ApprovedDate, OH.UploadedDate, 
                                         (SELECT (ISNULL(SUM(OI.LineItemCost),0) + ISNULL(SUM(OI.LineItemFreight),0) + ISNULL(SUM(OI.LineItemHandling),0) + ISNULL(SUM(OI.HandlingCharge * OI.QuantityOrdered),0))  FROM OrderItem OI WHERE OI.OrderHeader_ID = OH.OrderHeader_ID) AS POTotal,
										(SELECT SUM(OI2.QuantityOrdered) FROM OrderItem OI2 WHERE OI2.OrderHeader_ID = OH.OrderHeader_ID) AS CaseTotal
 	            FROM OrderHeader OH
 	            JOIN Vendor RECV ON RECV.Vendor_ID = OH.ReceiveLocation_ID
	            JOIN Store S ON S.Store_No = RECV.Store_No
	            JOIN Users U ON U.User_ID = OH.CreatedBy
	            WHERE OH.Vendor_ID = ' + CAST(@Facility_ID AS VARCHAR(20)) + ' AND OH.Expected_Date >= ''' + CAST(@StartDate AS VARCHAR(20)) + ''' AND OH.Expected_Date < ''' + CAST(dateadd(d,1,@EndDate) AS VARCHAR(20))+ ''''


IF @SubTeam_No IS NOT NULL
	BEGIN
		INSERT INTO @tblSubTeam
			SELECT Key_Value FROM dbo.fn_Parse_List(@SubTeam_No,'|')

		IF (SELECT COUNT(SubTeam_No) FROM @tblSubTeam) > 0
			BEGIN
				SELECT @SubTeamList = COALESCE(@SubTeamList + ''', ', '') + '''' + CAST(SubTeam_No AS VARCHAR(10)) FROM @tblSubTeam
				SELECT @SQL = @SQL + ' AND OH.Transfer_To_SubTeam IN (' + @SubTeamList + ''')'
			END
	END

IF @Store_No IS NOT NULL
	BEGIN
		INSERT INTO @tblStore
			SELECT Key_Value FROM dbo.fn_Parse_List(@Store_No,'|')

		IF (SELECT COUNT(Store_No) FROM @tblStore) > 0
			BEGIN
				SELECT @StoreList = COALESCE(@StoreList + ''', ', '') + '''' + CAST(Store_No AS VARCHAR(10)) FROM @tblStore
				SELECT @SQL = @SQL + ' AND S.Store_No IN (' + @StoreList + ''')'
			END
	END

IF @Status_ID = 1
	SELECT @SQL = @SQL + ' AND OH.SentDate IS NOT NULL'

IF @Status_ID = 2
	SELECT @SQL = @SQL + ' AND OH.WarehouseSentDate IS NOT NULL'

IF @Status_ID = 3
	SELECT @SQL = @SQL + ' AND OH.CloseDate IS NOT NULL'

IF @Status_ID = 4
	SELECT @SQL = @SQL + ' AND OH.ApprovedDate IS NOT NULL'

IF @Status_ID = 5
	SELECT @SQL = @SQL + ' AND OH.UploadedDate IS NOT NULL'


IF @SortLevel1 = 1
	SELECT @SQL = @SQL + ' ORDER BY S.Store_Name'

IF @SortLevel1 = 2
	SELECT @SQL = @SQL + ' ORDER BY OH.Transfer_To_SubTeam'

IF @SortLevel1 = 3
	SELECT @SQL = @SQL + ' ORDER BY OH.Expected_Date'


IF @SortLevel2 = 1
	SELECT @SQL = @SQL + ' ,S.Store_Name'

IF @SortLevel2 = 2
	SELECT @SQL = @SQL + ' ,OH.Transfer_To_SubTeam'

IF @SortLevel2 = 3
	SELECT @SQL = @SQL + ',OH.Expected_Date'


IF @SortLevel3 = 1
	SELECT @SQL = @SQL + ' ,S.Store_Name'

IF @SortLevel3 = 2
	SELECT @SQL = @SQL + ' ,OH.Transfer_To_SubTeam'

IF @SortLevel3 = 3
	SELECT @SQL = @SQL + ',OH.Expected_Date'

EXEC (@SQL)
END
GO


