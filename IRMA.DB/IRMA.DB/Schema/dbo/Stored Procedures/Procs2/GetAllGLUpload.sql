CREATE PROCEDURE dbo.GetAllGLUpload 
AS 

DECLARE @StartDate datetime
DECLARE @EndDate datetime

SELECT @StartDate = GETDATE()
SELECT @EndDate = GETDATE()

DECLARE @Results TABLE (OrderType VARCHAR(1),SubTeam INT, Account VARCHAR(6), Unit VARCHAR(5), DeptId INT, Product INT, Amount MONEY, OrderHeader_ID VARCHAR(30), TransferUnit varchar(5), ThirdPartyFreightInvoice bit)
INSERT INTO @Results(SubTeam, Account, Unit, DeptId, Product, Amount, OrderHeader_ID, TransferUnit)
	EXEC GetGLUploadTransfers @StartDate, @EndDate, NULL

UPDATE @Results SET OrderType = 'T'

INSERT INTO @Results(SubTeam, Account, Unit, DeptId, Product, Amount, OrderHeader_ID, TransferUnit)
	exec GetGLUploadDistributions NULL,NULL,@StartDate,@EndDate,0,1

UPDATE @Results SET OrderType = 'D' WHERE OrderType is null

-- this is moved into here from the file creation, since the Inventory Adjustments don't use IRMA PO#s. 
-- this may impact appearance on reports, but it's a more accurate reflection of what gets into the files 
UPDATE @Results SET OrderHeader_ID = 'IRMA PO# ' + OrderHeader_ID

INSERT INTO @Results(SubTeam, Account, Unit, DeptId, Product, Amount, OrderHeader_ID)
--	exec GetGLUploadInventoryAdjustment NULL,NULL,@StartDate,@EndDate
--  Because the manual screen is not yet set up for GL, let's send NULLs so we can get a week if needed
	exec GetGLUploadInventoryAdjustment NULL,NULL,NULL,NULL
	
UPDATE @Results SET OrderType = 'I' WHERE OrderType is null

SELECT * FROM @Results ORDER BY Unit
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllGLUpload] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllGLUpload] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllGLUpload] TO [IRMASchedJobsRole]
    AS [dbo];

