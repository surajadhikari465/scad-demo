CREATE TABLE [dbo].[RawScans] (
    [id]        INT           IDENTITY (1, 1) NOT NULL,
    [createdOn] DATETIME      CONSTRAINT [df_RawScanbs_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [Message]   VARCHAR (MAX) NOT NULL,
    [Status]    VARCHAR (10)  CONSTRAINT [df_RawScans_Status] DEFAULT ('new') NOT NULL,
    [ElapsedMS] BIGINT        NULL,
    CONSTRAINT [pk_RawScans] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_RawScans_Status]
    ON [dbo].[RawScans]([Status] ASC) WITH (FILLFACTOR = 80);


GO
create trigger [dbo].[TR_RawScan_AfterInsert_CopyToBaseScanTbl]
on [dbo].[RawScans]
after insert
as
begin

/*
Author: Tom Lux
Date: Sept, 2022

When this was written, it was checked into Azure repo here:
https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/dbo/Tables/RawScans.sql&version=GBmaster

Main tech doc(s) here: https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/_documentation/

The JSON functions herein require at least SQL v2016 and compatibility level 130.

Normally, an error inside a table trigger, like the one herein, causes the transaction against the parent table to fail,
which would cause the incoming scan data (from TMs in stores) to not be saved to RawScans table, which is bad.
This is because the default behavior for xact-abort (transaction abort on error) is ‘ON’ inside triggers, 
so we turn it ‘OFF’ because we do not want an error in this bolt-on table-trigger-solution to change the behavior 
of original code/tables/dependencies; we’re trying to be non-impactful and create a secondary/alternate/side-by-side solution
for our business partners.

There is logic in the OOS Scan Processor app to update or “correct” the scan date based on the source region’s offset from 
Central Time; this logic is replicated during this RawScans trigger.

The catch block of the trigger logs error details if we fail to copy a RawScan row into the new base-scan table.
This extra logic in the catch block can be disabled with an app-config setting.
If the catch block's app-config setting is missing or invalid, the catch block will execute and write an app-log 
entry for errors, meaning the catch-block logic is "enabled" by default.

Logging: This process only logs warnings and errors.
Use query: select top 10 a.appname, al.* from app a join applog al on a.AppID=al.AppID where al.AppID = (select appid from app where appname='Raw Scan Copy To Base Scan') order by applogid desc

*See bottom of file in repo for example data for known issues and testing results (these notes will not be contained inside the trigger code in the DB).


** WARNING / LIMITATION **:
When multiple rows are inserted into RawScans in a single statement (multi-row insert), if any single row in that set
fails in this trigger during insert to BaseScanDetail (like if the set contains dup rows for a user's saved scan),
the full batch of rows is lost.  We could add logic to handle multi-row datasets, but for now we're assuming this will 
never happen in Prod because the use case is a user saving a single scan on a handheld in a store, so there should never
be bulk/multi-row inserts into RawScans.  If there are no conflicts or errors in the multi-row set, it will create the 
entries in Base-Scan table fine.

*/


	-- Capture how many rows are being inserted so we can log warning if > 1, (multi-row inserts not expected or perfectly-handled yet).
	declare @rowCount int = @@ROWCOUNT

	begin try
		declare @rawScanIdRef int, @msg nvarchar(max), @timeNow datetime
		
		set xact_abort off -- Ensure this work does not cause parent insert into RawScans to abort/fail.

		if @rowCount > 1
		begin
			begin try
				select @rawScanIdRef = (select IDENT_CURRENT('rawscans'))
				select @msg = 'WARNING: Multi-row insert; All base-scan rows lost if trigger logic fails; Last RawScanIdRef=' + cast(@rawScanIdRef as varchar) + ', RowCount=' + cast(@rowCount as varchar)
				print @msg
				select @timeNow = getdate()
				exec dbo.WriteAppLog @AppRef='base scan', @Level='warn', @Logger='TR_RawScan_AfterInsert_CopyToBaseScanTbl', @LogDate=@timeNow, @Thread='0', @Message=@msg, @CallSite='multirow check', @StackTrace='OOS DB > RawScans table > trigger'
			end try
			begin catch
				print 'Error trying to log multi-row warning: ' + ERROR_MESSAGE()
			end catch
		end;

		/*
		We're extracting core bits of the raw scan data and inserting it into a separate table for business reporting.
		*/
		insert into BaseScanDetail (RegionAbbr, StoreAbbr, PS_BU, OffsetCorrectedScanDate, UPC)
		select
			RegionAbbr,
			StoreAbbr,
			PS_BU,
			OffsetCorrectedScanDate,
			UPC
		from (
			select
				RegionAbbr,
				StoreAbbr,
				PS_BU = (select top 1 ps_bu from store where STORE_ABBREVIATION = StoreAbbr), -- There are dup entries in store table, so we'll grab first one because they should have same BU#.
				OffsetCorrectedScanDate = DATEADD(hour, r.TimeOffsetFromCentral, ScanDate),
				UPC -- This value is built during outer-apply from UPC List.
			from
			(
				-- Using subselect to grab values from JSON field, especially nested JSON array/list value for UPCs, as this helps split that list into rows (via outer-apply).
				-- This is where we grab all the values we need from the row being inserted into RawScans.
				select
					ScanDate = cast(JSON_VALUE(INSERTED.message, '$.ScanDate') as datetime2),
					RegionAbbr = JSON_VALUE(INSERTED.message, '$.RegionAbbrev'),
					StoreAbbr = JSON_VALUE(INSERTED.message, '$.StoreAbbrev'),
					upcList=JSON_query(INSERTED.message, '$.Upcs'), * from INSERTED
			) incomingData
			join region r on RegionAbbr = r.REGION_ABBR -- Join to region to get biz unit #.
			outer apply openjson(incomingData.upcList) with (UPC NVARCHAR(25) '$')  -- Split list of UPCs into rows for each.
		) WithDups
		group by RegionAbbr, StoreAbbr, PS_BU, OffsetCorrectedScanDate, UPC

		/*
		The above group-by is just a way to remove duplicates because biz partners do not want/need two of the same UPCs in an OOS scan batch.
		If we don't do this, the unique constraint on the target base-scan table fails and we lose the entire batch of OOS UPCs.
		*/

	end try  
	begin catch
		-- Adding ability to enable or disable the output and applog entry in catch block.
		declare @catchLogicEnabled bit = coalesce((select value from appconfig ac join app a on ac.appid=a.appid where a.appname = 'Raw Scan Copy To Base Scan' and ac.[key] = 'Catch Block Processing Enabled'), 0)
		if @catchLogicEnabled=0 return;

	-- inserted row/field isnt available in catch block, so will not try to capture other values yet.
	--	declare @msg nvarchar(max) = 'Error copying new raw-scan row to base-Scan table, raw-json=' + INSERTED.message + ': ' + ERROR_MESSAGE()

		select @rawScanIdRef = (select IDENT_CURRENT('rawscans'))
		select @msg = 'Error copying new raw-scan row to base-Scan table; RawScanIdRef=' + cast(@rawScanIdRef as varchar) + ': ' + ERROR_MESSAGE()
		print @msg
		select @timeNow = getdate()
		exec dbo.WriteAppLog @AppRef='base scan', @Level='error', @Logger='TR_RawScan_AfterInsert_CopyToBaseScanTbl', @LogDate=@timeNow, @Thread='0', @Message=@msg, @CallSite='catch block', @StackTrace='OOS DB > RawScans table > trigger'
	end catch

	-- We don't have to change xact-abort setting cuz it's set at runtime, so it'll be back on the next time this is fired.
end;

/*
EXAMPLE DATA

Found examples of different raw scans with same created-on date:
createdon	message
2022-09-18 17:19:49.700	{"ScanDate":"2022-09-18T17:19:49.8346389-05:00","RegionAbbrev":"MA","StoreAbbrev":"FAV","Upcs":["0085177000301","0085177000310","0087606300785","0009948243568","0003619212218","0009948243569","0009948248696","0009948248695","0085155400600","0009948243436","0009948250022","0085002122821","0081000378082","0009948240690","0009948241898","0009948251251","0009948246368"],"UserName":"rivase","UserEmail":"Edwin.Rivas2@wholefoods.com","SessionId":"6bf4108e-3c55-4c33-9051-22016564d55f"}
2022-09-18 17:19:49.700	{"ScanDate":"2022-09-18T17:19:49.8346389-05:00","RegionAbbrev":"MW","StoreAbbrev":"HAL","Upcs":["0008811015055"],"UserName":"erasmo.gomez","UserEmail":"Erasmo.Gomez@wholefoods.com","SessionId":"4f65c276-9373-4b2f-ba43-3337ef8e16a8"}
createdon	message
2022-09-18 08:24:49.783	{"ScanDate":"2022-09-18T08:24:49.9301698-05:00","RegionAbbrev":"NE","StoreAbbrev":"MHW","Upcs":["0099000094338"],"UserName":"2360756","UserEmail":"Ryan.Gonzalez@wholefoods.com","SessionId":"75d03644-484f-4aab-aff6-1c35f3e120d7"}
2022-09-18 08:24:49.783	{"ScanDate":"2022-09-18T08:24:49.9301698-05:00","RegionAbbrev":"NE","StoreAbbrev":"SLW","Upcs":["0099000094238","0081003420036","0079487840124"],"UserName":"2311525","UserEmail":"Robert.Kern@wholefoods.com","SessionId":"2d387dda-039d-4164-abcc-9810d0137ea7"}

Example of dups with only unique session ID:
createdon	message
2022-09-17 21:43:10.440	{"ScanDate":"2022-09-17T21:43:10.5452198-05:00","RegionAbbrev":"MW","StoreAbbrev":"HDP","Upcs":["0099000094438","0003338300404"],"UserName":"Josue.Vazquez","UserEmail":"Josue.Vazquez@wholefoods.com","SessionId":"cb99e48b-a423-4afe-899f-136eefe37d41"}
2022-09-17 21:43:10.440	{"ScanDate":"2022-09-17T21:43:10.5452198-05:00","RegionAbbrev":"MW","StoreAbbrev":"HDP","Upcs":["0099000094438","0003338300404"],"UserName":"Josue.Vazquez","UserEmail":"Josue.Vazquez@wholefoods.com","SessionId":"874ac6f8-5741-45a0-aef5-ea1126b07a46"}
2022-09-17 21:43:10.627	{"ScanDate":"2022-09-17T21:43:10.7305928-05:00","RegionAbbrev":"MW","StoreAbbrev":"HDP","Upcs":["0099000094438","0003338300404"],"UserName":"Josue.Vazquez","UserEmail":"Josue.Vazquez@wholefoods.com","SessionId":"e3873d0f-fd5b-4a17-941a-ec0ec5ab97ec"}
createdon	message
2022-09-17 21:21:35.870	{"ScanDate":"2022-09-17T21:21:35.9617344-05:00","RegionAbbrev":"NC","StoreAbbrev":"SMT","Upcs":["0099000097138","0029471700000","0029200800000"],"UserName":"Nancy.Angel","UserEmail":"Nancy.Angel@wholefoods.com","SessionId":"53398faa-ea36-407b-a7ce-5262ea93d5a8"}
2022-09-17 21:21:35.870	{"ScanDate":"2022-09-17T21:21:35.9617344-05:00","RegionAbbrev":"NC","StoreAbbrev":"SMT","Upcs":["0099000097138","0029471700000","0029200800000"],"UserName":"Nancy.Angel","UserEmail":"Nancy.Angel@wholefoods.com","SessionId":"6cdf4e8d-fb4b-4654-83d7-b529ed70bf1b"}

Here's 3 dups, but one was a few MS later, so i assume it gets counted separately by business, but definitely gets shown as 2 diff entries in new base-scan table.
createdon	message
2022-09-18 21:09:21.950	{"ScanDate":"2022-09-18T21:09:22.177523-05:00","RegionAbbrev":"NC","StoreAbbrev":"CON","Upcs":["0085000389852","0085000389853","0085000389854","0085000389851","0018099900026","0085269700149","0085269700148","0081957301179","0085002902334","0081957301647","0009948249001","0086876700030","0081957301639","0081957301171","0426068863001","0009948228598","0081915602018","0081705302076","0079285001530","0001878777232","0006574323359","0076430220421","0076430221559","0087863900128","0009948250316","0086000402520","0081783501003","0081004146011","0079285089995","0001878778405","0009948241324","0009948248646","0009948246892","0009948246893","S4627212230113276","0063687422084","0063687422082","0400163809864","0007852202123","0018713200506","0071833422113","0084132010961","0087863900021","0009948250000","0065801011594","0085412400777","0085174100815","0009948245145","0009948249307","0009948245380","0085621000893","0085001199658","0009948246414","0002107801957","0003660209192","0083578700037","0081640102327","0065801011794","0065801011789","N4129240353513392","0009948228740","0065801011840","0060506906724","0009070003110","0065801011667","0002188835101","0086000197331","0089507000208","0065801012332","N4129183371013180","0062660800097","V4617384395013224","0063125715984","N4139181013113228","N4129062447313228"],"UserName":"2376292","UserEmail":"Muhammad.Ammar@wholefoods.com","SessionId":"36851197-f1e0-4160-a9c0-218dd946630d"}
2022-09-18 21:09:21.950	{"ScanDate":"2022-09-18T21:09:22.177523-05:00","RegionAbbrev":"NC","StoreAbbrev":"CON","Upcs":["0085000389852","0085000389853","0085000389854","0085000389851","0018099900026","0085269700149","0085269700148","0081957301179","0085002902334","0081957301647","0009948249001","0086876700030","0081957301639","0081957301171","0426068863001","0009948228598","0081915602018","0081705302076","0079285001530","0001878777232","0006574323359","0076430220421","0076430221559","0087863900128","0009948250316","0086000402520","0081783501003","0081004146011","0079285089995","0001878778405","0009948241324","0009948248646","0009948246892","0009948246893","S4627212230113276","0063687422084","0063687422082","0400163809864","0007852202123","0018713200506","0071833422113","0084132010961","0087863900021","0009948250000","0065801011594","0085412400777","0085174100815","0009948245145","0009948249307","0009948245380","0085621000893","0085001199658","0009948246414","0002107801957","0003660209192","0083578700037","0081640102327","0065801011794","0065801011789","N4129240353513392","0009948228740","0065801011840","0060506906724","0009070003110","0065801011667","0002188835101","0086000197331","0089507000208","0065801012332","N4129183371013180","0062660800097","V4617384395013224","0063125715984","N4139181013113228","N4129062447313228"],"UserName":"2376292","UserEmail":"Muhammad.Ammar@wholefoods.com","SessionId":"7947070a-2942-408d-8a78-3825c1f71c6b"}
2022-09-18 21:09:21.963	{"ScanDate":"2022-09-18T21:09:22.196366-05:00","RegionAbbrev":"NC","StoreAbbrev":"CON","Upcs":["0085000389852","0085000389853","0085000389854","0085000389851","0018099900026","0085269700149","0085269700148","0081957301179","0085002902334","0081957301647","0009948249001","0086876700030","0081957301639","0081957301171","0426068863001","0009948228598","0081915602018","0081705302076","0079285001530","0001878777232","0006574323359","0076430220421","0076430221559","0087863900128","0009948250316","0086000402520","0081783501003","0081004146011","0079285089995","0001878778405","0009948241324","0009948248646","0009948246892","0009948246893","S4627212230113276","0063687422084","0063687422082","0400163809864","0007852202123","0018713200506","0071833422113","0084132010961","0087863900021","0009948250000","0065801011594","0085412400777","0085174100815","0009948245145","0009948249307","0009948245380","0085621000893","0085001199658","0009948246414","0002107801957","0003660209192","0083578700037","0081640102327","0065801011794","0065801011789","N4129240353513392","0009948228740","0065801011840","0060506906724","0009070003110","0065801011667","0002188835101","0086000197331","0089507000208","0065801012332","N4129183371013180","0062660800097","V4617384395013224","0063125715984","N4139181013113228","N4129062447313228"],"UserName":"2376292","UserEmail":"Muhammad.Ammar@wholefoods.com","SessionId":"d72ef438-ba20-4f4d-a926-8524fb7c9933"}
confirmed that 2 distinct copies were imported; this query shows: select * from basescandetail where OffsetCorrectedScanDate between '2022-09-18 19:09:22' and '2022-09-18 19:09:23' order by upc
createdOn	Message
2022-09-20 21:39:03.707	{"ScanDate":"2022-09-20T21:39:03.8276131-05:00","RegionAbbrev":"SP","StoreAbbrev":"GLN","Upcs":["0099000098138","0002531761500","0002531761600","0029039200000","0002228570000","0029016000000"],"UserName":"Anthony.Monsalvo","UserEmail":"Anthony.Monsalvo@wholefoods.com","SessionId":"4fa5c22e-59ee-4d48-b3b5-70c1830246cd"}
2022-09-20 21:39:03.723	{"ScanDate":"2022-09-20T21:39:03.8276131-05:00","RegionAbbrev":"SP","StoreAbbrev":"GLN","Upcs":["0099000098138","0002531761500","0002531761600","0029039200000","0002228570000","0029016000000"],"UserName":"Anthony.Monsalvo","UserEmail":"Anthony.Monsalvo@wholefoods.com","SessionId":"e8ce0784-5ba5-4750-81d3-dd00da5f2196"}
2022-09-20 21:39:03.770	{"ScanDate":"2022-09-20T21:39:03.8963429-05:00","RegionAbbrev":"SP","StoreAbbrev":"GLN","Upcs":["0099000098138","0002531761500","0002531761600","0029039200000","0002228570000","0029016000000"],"UserName":"Anthony.Monsalvo","UserEmail":"Anthony.Monsalvo@wholefoods.com","SessionId":"af8a4886-1dd9-434b-a235-8a9c7b700c3d"}

** Performance Testing **
[Process Large Real Dataset]
- Target for testing: QA OOS DB (cewd8529)
- Data processed: 24988 total rows for RawScans table (actual RawScans rows copied from Prod)
- Date range: All RawScans table entries for 9/17/22 through 9/22/22 (6 days)
- RawScans failing copy to new BaseScanDetail table: 2806
- Process: Staged (into separate table) Prod data, then inserted these one at a time into RawScans table, which fires new trigger,
  generates new data, and logs issues.
- BaseScanDetail rows generated: 273018
- Duration: 3 min, 22 sec
- Rows processed per second: 123
- Result Summary: SUCCESS -- The sample data processed here represents 6 full days of user's scans in Prod (all regions) and includes the heavy (weekly) scan day of Sunday, which was 9/18/22 for this dataset.
  Once live, this process should have no problem keeping up with scans flowing in.  Also, for this test in the QA DB, more than 10% of the RawScans rows from Prod failed insert into
  new BaseScanDetail because the PS_BU was missing from the Stores table.  This logic takes a little extra time inside the trigger, so overall processing time would have been lower if all biz units were defined.
  Regardless, each scan will process in milliseconds.


*/