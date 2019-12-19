CREATE FUNCTION [dbo].[get_times_scanned]
   (
	@in_upc VARCHAR(30),
    @in_store_id VARCHAR(10),
    @in_date DATETIME,
    @days_back INT
   )
returns INT
AS
BEGIN
      DECLARE @times_scanned INT
SET @times_scanned =( 
select count(0) 
    from REPORT_DETAIL rd1, REPORT_HEADER rh1
    where rd1.REPORT_HEADER_ID = rh1.id
    and rh1.STORE_ID = @in_store_id
    and rd1.upc = @in_upc
    and rh1.CREATED_DATE between DATEADD(day, -1 * @days_back, @in_date) and @in_date
    group by upc
);
RETURN @times_scanned
END
GO

