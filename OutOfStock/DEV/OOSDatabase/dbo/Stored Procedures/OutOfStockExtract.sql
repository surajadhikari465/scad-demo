
CREATE PROCEDURE dbo.OutOfStockExtract 
	@DaysBack int,
	@IncludeScansMissingDetails BIT
AS
  BEGIN

      DECLARE @BeginDate as datetime
      DECLARE @EndDate as datetime



      set @EndDate = cast(getdate() as date) -- get the date the job is run, without a timestamp.  example: 06-01-2020
      set @BeginDate = dateadd(DAY, @DaysBack, @EndDate) -- calculate days back ;using the new @EndDate

      Declare @Message varchar(255)
      set @Message = 'OutOfStock Extract: ' + convert(varchar(100), @BeginDate,10) + ' - ' + convert(varchar(100), @EndDate,10)
      raiserror(@Message, 0,1) with nowait
      
      SELECT id
      INTO   #reportids
      FROM   report_header rh
      WHERE  rh.offsetcorrectedcreatedate >= @BeginDate and rh.OffsetCorrectedCreateDate < @EndDate

      CREATE NONCLUSTERED INDEX ix_reportids
        ON #reportids(id)

      SELECT rh.id,
             rh.store_id,
             rh.offsetcorrectedcreatedate,
             rd.brand_name,
             rd.upc,
             rd.long_description,
             rd.ps_team,
             rd.ps_subteam,
             rd.category_name,
             rd.class_name,
             rd.vendor_key,
             rd.vin, 
			 rd.DAYSOFMOVEMENT,
			 rd.MOVEMENT, 
			 rd.EFF_PRICE
      INTO   #reportdata
      FROM   #reportids
             INNER JOIN dbo.report_header rh
                     ON rh.id = #reportids.id
             INNER JOIN dbo.report_detail rd
                     ON rh.id = rd.report_header_id

      IF @IncludeScansMissingDetails = 1
        INSERT INTO #reportdata
                    (id,
                     store_id,
                     offsetcorrectedcreatedate,
                     upc)
        SELECT rh.id,
               rh.store_id,
               rh.offsetcorrectedcreatedate,
               missing.upc
        FROM   #reportids
               INNER JOIN scansmissingvimdata missing
                       ON missing.report_header_id = #reportids.id
               INNER JOIN dbo.report_header rh
                       ON missing.report_header_id = rh.id

      SELECT r.region_abbr                       AS Region,
             s.store_abbreviation                AS StoreAbbr,
             s.ps_bu                             AS PS_BU,
             rd.offsetcorrectedcreatedate        AS OosScannedOn,
             rd.brand_name                       AS Brand,
             rd.upc                              AS UPC,
             rd.long_description                 AS ItemDescription,
             rd.ps_team                          AS Team,
             rd.ps_subteam                       AS Subteam,
             rd.category_name                    AS Category,
             rd.class_name                       AS Class,
             Cast(rd.vendor_key AS VARCHAR(100)) AS Vendor_Key,
             Cast(rd.vin AS VARCHAR(25))         AS VIN,
			 COALESCE(rd.DAYSOFMOVEMENT,0)		AS DaysWithMovementLast12Weeks,
			 COALESCE(rd.MOVEMENT,0)			AS AvgMovement,
			 rd.EFF_PRICE						 AS Price ,
			 ROUND(COALESCE(rd.DAYSOFMOVEMENT,0) *  COALESCE(rd.MOVEMENT,0) * rd.EFF_PRICE,2) AS Sales
      FROM   #reportdata rd
             INNER JOIN dbo.store s
                     ON rd.store_id = s.id
             INNER JOIN dbo.region r
                     ON s.region_id = r.id
      ORDER  BY s.store_abbreviation,
                rd.offsetcorrectedcreatedate DESC

      DROP TABLE #reportdata

      DROP TABLE #reportids
  END