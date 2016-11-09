
CREATE PROCEDURE app.[PLUMapImportTEST]
   @ItemList app.PLUMAPIMPORTTYPE READONLY,@UserName NVARCHAR(255)
AS
/*

	This procedure takes a list of scan codes with regional alternate values (mappings)
	and updates a PLU-mapping table (updates existing entries or adds missing entries).

	The scan code coming in must match a scan code derived from the Item.itemID value in the mapping table.

	An entry is only added if the scan code coming in exists in the ScanCode table.

*/
   /*
   Update existing entries.
   1. If a NULL or '0' are passed, the regional PLU entry is cleared (set to NULL).
   2. If a '' (empty string) is passed, the regional PLU entry is not changed (set to existing value).
   */
   BEGIN TRY
      DECLARE @Err_Message NVARCHAR(255)

      -- Validate Spreadsheet for Duplicates within Columns
         IF EXISTS ( SELECT i.flPLU,Count(i.flPLU) FROM @ItemList i WHERE i.flPLU IS NOT NULL AND i.flPLU <> '0' AND i.flPLU <> '' GROUP BY i.flPLU HAVING Count(i.flPLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: flPLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.maPLU,Count(i.maPLU) FROM @ItemList i WHERE i.maPLU IS NOT NULL AND i.maPLU <> '0' AND i.maPLU <> '' GROUP BY i.maPLU HAVING Count(i.maPLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: maPLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.mwPLU,Count(i.mwPLU) FROM @ItemList i WHERE i.mwPLU IS NOT NULL AND i.mwPLU <> '0' AND i.mwPLU <> '' GROUP BY i.mwPLU HAVING Count(i.mwPLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: mwPLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.naPLU,Count(i.naPLU) FROM @ItemList i WHERE i.naPLU IS NOT NULL AND i.naPLU <> '0' AND i.naPLU <> '' GROUP BY i.naPLU HAVING Count(i.naPLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: naPLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.ncPLU,Count(i.ncPLU) FROM @ItemList i WHERE i.ncPLU IS NOT NULL AND i.ncPLU <> '0' AND i.ncPLU <> '' GROUP BY i.ncPLU HAVING Count(i.ncPLU) > 1 )
           BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: ncPLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.nePLU,Count(i.nePLU) FROM @ItemList i WHERE i.nePLU IS NOT NULL AND i.nePLU <> '0' AND i.nePLU <> '' GROUP BY i.nePLU HAVING Count(i.nePLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: nePLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.pnPLU,Count(i.pnPLU) FROM @ItemList i WHERE i.pnPLU IS NOT NULL AND i.pnPLU <> '0' AND i.pnPLU <> '' GROUP BY i.pnPLU HAVING Count(i.pnPLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: pnPLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.rmPLU,Count(i.rmPLU) FROM @ItemList i WHERE i.rmPLU IS NOT NULL AND i.rmPLU <> '0' AND i.rmPLU <> '' GROUP BY i.rmPLU HAVING Count(i.rmPLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: rmPLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.soPLU,Count(i.soPLU) FROM @ItemList i WHERE i.soPLU IS NOT NULL AND i.soPLU <> '0' AND i.soPLU <> '' GROUP BY i.soPLU HAVING Count(i.soPLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: soPLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.spPLU,Count(i.spPLU) FROM @ItemList i WHERE i.spPLU IS NOT NULL AND i.spPLU <> '0' AND i.spPLU <> '' GROUP BY i.spPLU HAVING Count(i.spPLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: spPLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.swPLU,Count(i.swPLU) FROM @ItemList i WHERE i.swPLU IS NOT NULL AND i.swPLU <> '0' AND i.swPLU <> '' GROUP BY i.swPLU HAVING Count(i.swPLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: swPLU' RAISERROR (@Err_Message,11,1) END
         IF EXISTS ( SELECT i.ukPLU,Count(i.ukPLU) FROM @ItemList i WHERE i.ukPLU IS NOT NULL AND i.ukPLU <> '0' AND i.ukPLU <> '' GROUP BY i.ukPLU HAVING Count(i.ukPLU) > 1 )
            BEGIN  SET @Err_Message = 'Duplicate PLUs found on spreadsheet in column: ukPLU' RAISERROR (@Err_Message,11,1) END

   --Clear out any previous mappings 
      UPDATE plumap SET flPLU = NULL FROM plumap p WHERE p.flPLU IN ( SELECT i.flPLU FROM @ItemList i )
      UPDATE plumap SET maPLU = NULL FROM plumap p WHERE p.maPLU IN ( SELECT i.maPLU FROM @ItemList i )
      UPDATE plumap SET mwPLU = NULL FROM plumap p WHERE p.mwPLU IN ( SELECT i.mwPLU FROM @ItemList i )
      UPDATE plumap SET naPLU = NULL FROM plumap p WHERE p.naPLU IN ( SELECT i.naPLU FROM @ItemList i )
      UPDATE plumap SET ncPLU = NULL FROM plumap p WHERE p.ncPLU IN ( SELECT i.ncPLU FROM @ItemList i )
      UPDATE plumap SET nePLU = NULL FROM plumap p WHERE p.nePLU IN ( SELECT i.nePLU FROM @ItemList i )
      UPDATE plumap SET pnPLU = NULL FROM plumap p WHERE p.pnPLU IN ( SELECT i.pnPLU FROM @ItemList i )
      UPDATE plumap SET rmPLU = NULL FROM plumap p WHERE p.rmPLU IN ( SELECT i.rmPLU FROM @ItemList i )
      UPDATE plumap SET soPLU = NULL FROM plumap p WHERE p.soPLU IN ( SELECT i.soPLU FROM @ItemList i )
      UPDATE plumap SET spPLU = NULL FROM plumap p WHERE p.spPLU IN ( SELECT i.spPLU FROM @ItemList i )
      UPDATE plumap SET swPLU = NULL FROM plumap p WHERE p.swPLU IN ( SELECT i.swPLU FROM @ItemList i )
      UPDATE plumap SET ukPLU = NULL FROM plumap p WHERE p.ukPLU IN ( SELECT i.ukPLU FROM @ItemList i )

	update PLUMap
	set
		flPLU = case when isnull(i.flPLU, '0') = '0' then null else
			case when isnull(i.flPLU, '0') = '' then p.flPLU else i.flPLU end end
		,maPLU = case when isnull(i.maPLU, '0') = '0' then null else
			case when isnull(i.maPLU, '0') = '' then p.maPLU else i.maPLU end end
		,mwPLU = case when isnull(i.mwPLU, '0') = '0' then null else
			case when isnull(i.mwPLU, '0') = '' then p.mwPLU else i.mwPLU end end
		,naPLU = case when isnull(i.naPLU, '0') = '0' then null else
			case when isnull(i.naPLU, '0') = '' then p.naPLU else i.naPLU end end
		,ncPLU = case when isnull(i.ncPLU, '0') = '0' then null else
			case when isnull(i.ncPLU, '0') = '' then p.ncPLU else i.ncPLU end end
		,nePLU = case when isnull(i.nePLU, '0') = '0' then null else
			case when isnull(i.nePLU, '0') = '' then p.nePLU else i.nePLU end end
		,pnPLU = case when isnull(i.pnPLU, '0') = '0' then null else
			case when isnull(i.pnPLU, '0') = '' then p.pnPLU else i.pnPLU end end
		,rmPLU = case when isnull(i.rmPLU, '0') = '0' then null else
			case when isnull(i.rmPLU, '0') = '' then p.rmPLU else i.rmPLU end end
		,soPLU = case when isnull(i.soPLU, '0') = '0' then null else
			case when isnull(i.soPLU, '0') = '' then p.soPLU else i.soPLU end end
		,spPLU = case when isnull(i.spPLU, '0') = '0' then null else
			case when isnull(i.spPLU, '0') = '' then p.spPLU else i.spPLU end end
		,swPLU = case when isnull(i.swPLU, '0') = '0' then null else
			case when isnull(i.swPLU, '0') = '' then p.swPLU else i.swPLU end end
		,ukPLU = case when isnull(i.ukPLU, '0') = '0' then null else
			case when isnull(i.ukPLU, '0') = '' then p.ukPLU else i.ukPLU end end
	from PLUMap p
	join ScanCode sc
		on p.itemID = sc.itemID -- Update existing itemID entries in our PLU mapping table.
	join @ItemList i
		on sc.scanCode = i.nationalPLU -- Link the nat PLU coming in to scan code entry.


	/*
		Insert new entries.
		If '0' or '' (empty) are passed, we set the regional PLU entry to NULL.
		No isnull() check is needed around the incoming values because it will still result in the entry getting a NULL.
	*/
	insert into PLUMap
		select
			itemID = sc.itemID
			,flPLU = case when i.flPLU = '0' or i.flPLU = '' then null else i.flPLU end
			,maPLU = case when i.maPLU = '0' or i.maPLU = '' then null else i.maPLU end
			,mwPLU = case when i.mwPLU = '0' or i.mwPLU = '' then null else i.mwPLU end
			,naPLU = case when i.naPLU = '0' or i.naPLU = '' then null else i.naPLU end
			,ncPLU = case when i.ncPLU = '0' or i.ncPLU = '' then null else i.ncPLU end
			,nePLU = case when i.nePLU = '0' or i.nePLU = '' then null else i.nePLU end
			,pnPLU = case when i.pnPLU = '0' or i.pnPLU = '' then null else i.pnPLU end
			,rmPLU = case when i.rmPLU = '0' or i.rmPLU = '' then null else i.rmPLU end
			,soPLU = case when i.soPLU = '0' or i.soPLU = '' then null else i.soPLU end
			,spPLU = case when i.spPLU = '0' or i.spPLU = '' then null else i.spPLU end
			,swPLU = case when i.swPLU = '0' or i.swPLU = '' then null else i.swPLU end
			,ukPLU = case when i.ukPLU = '0' or i.ukPLU = '' then null else i.ukPLU end
		from @ItemList i
		join ScanCode sc
			on i.nationalPLU = sc.scanCode
		left join PLUMap p
			on sc.itemID = p.itemID
		where
			p.itemID is null -- Add missing nat PLU entries.

   END TRY

   BEGIN CATCH
      DECLARE @ErrorMessage NVARCHAR(4000);
      DECLARE @ErrorSeverity INT;
      DECLARE @ErrorState INT;

      SELECT @ErrorMessage = Error_Message(),@ErrorSeverity = Error_Severity(),@ErrorState = Error_State();

      -- Use RAISERROR inside the CATCH block to return 
      -- error information about the original error that 
      -- caused execution to jump to the CATCH block.
      RAISERROR (@ErrorMessage,-- Message text.
                 @ErrorSeverity,-- Severity.
                 @ErrorState -- State.
      );
   END CATCH;