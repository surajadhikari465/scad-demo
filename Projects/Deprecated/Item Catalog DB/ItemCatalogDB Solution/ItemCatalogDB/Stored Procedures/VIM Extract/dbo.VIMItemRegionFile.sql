CREATE PROCEDURE dbo.VIMItemRegionFile
AS
   /****************************************************************************************
     Procedure: VIMItemRegionFile
        Author: SO
          Date: no idea

     Description:
     This procedure builds the Item information records to be exported and sent to the
     VIM (Virtual Item Master) database at Central.

     Modification History:
     Date        Init Comment
     12/18/2006  RS   Added an update from the dbo.NAT_PLU_lookup reference table to get
                      the old CIX NAT_PLUs into the extract record for the VIM.
                      The NAT PLUs could not be directly added to the ItemIdentifier table
                      because some of them conflicted with Identifiers for other items
                      causing duplicate records to be sent to the VIM.

     11/07/2007  FN   Added join to JDA_HierarchyMapping to pull regional hierarchy information.
                      This is a temporary fix until a full solution can be coded.

     02/13/2008  FN   Added check to filter SKUs from upload if the instamnce flag POSPush_ExcludeSKUIdentifiers is set
     03/31/2008  FN   When building REG_HIER_REF, put in region check to build this value from historic JDA values
                          when the region is 'MA'
     04/29/2009  JA   Added new instance data flag VIM_ExcludeSKUIdentifiers to manage the filtering out of SKU from VIM extract.
	 07/16/2015	 DN   There already exists an Instance Flag in IRMA, FourLevelHierarchy, that only MA has on. This drives the visibility of some UI fields related to the MA hierarchy and the hierarchy format that is transmitted to the VIM. We propose changing the logic within VIMItemRegionFile to leverage this Instance Flag.
					  The SQL would change from this :
					  CASE (select top 1 runmode from conversion_runmode)  WHEN 'MA' THEN
					  to this
					  CASE (select ISNULL(FlagValue,0) from InstanceDataFlags WHERE FlagKey = 'FourLevelHierarchy')
                      WHEN 1 THEN        
   ****************************************************************************************/
begin
   SET NOCOUNT ON
   SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

   /**************************************************************************
    Create the temporary table for Identifier, National identifier
   ***************************************************************************/
   DECLARE @VIMItemRegion TABLE (Item_Key int,  Identifier varchar (13),   National_Identifier varchar (13));

   /**************************************************************************
    Find out if SKU identifiers should be excluded from the upload
   ***************************************************************************/
    DECLARE @ExcludeSKUIdentifiers bit
    SELECT @ExcludeSKUIdentifiers = ISNULL([dbo].[fn_InstanceDataValue] ('VIM_ExcludeSKUIdentifiers', NULL), 0)


   /**************************************************************************
    Insert the normal identifiers into the temp table
   ***************************************************************************/
   INSERT INTO  @VIMItemRegion
      SELECT
         I.Item_Key,
         Identifier,
         NULL
      FROM
         Item I (nolock)

         JOIN ItemIdentifier II (nolock)
            ON I.Item_Key = II.Item_Key
      WHERE
         I.deleted_item = 0
         AND II.Deleted_Identifier = 0
      -- AND Default_Identifier = 1 (removed because the VIM needs all of the identifiers to map against the POS)
         AND (@ExcludeSKUIdentifiers = 0 OR (@ExcludeSKUIdentifiers = 1 AND II.IdentifierType <> 'S')); -- Filter SKUs from uplaod if not sent to POS


   /**************************************************************************
     Update the temp table with the NAT Identifiers from the temporary
     CIX NAT UPC lookup table. 12/18/2006  RS
   ***************************************************************************/
   UPDATE VIR
      SET
         VIR.national_identifier = pu.nat_plu
      FROM
         item I (nolock)

         JOIN @VIMItemRegion VIR
            on I.Item_Key = VIR.Item_Key

         JOIN dbo.NAT_PLU_lookup pu
            on pu.upcno = VIR.Identifier
      WHERE
         I.Deleted_Item = 0;

   /**************************************************************************
    Update the temp table with any NAT Identifiers in the ItemIdentifier table
    These identifiers will override any NAT Identifiers from the CIX NAT UPC
    table.
   ***************************************************************************/
   UPDATE VIR
      SET
         VIR.national_identifier = II.Identifier
      FROM
         item I (nolock)

         JOIN ItemIdentifier II
            on I.Item_Key = II.Item_Key

         JOIN @VIMItemRegion VIR
            on I.Item_Key = VIR.Item_Key
      WHERE
         I.Deleted_Item = 0
         AND II.Deleted_Identifier = 0
         AND II.National_Identifier = 1;

   /**************************************************************************
    Join our temp table with the Item information tables to create the
    Item Region extract file data.
   ***************************************************************************/
SELECT

      SUBSTRING ('0000000000000', 1, 13 - LEN(Identifier)) + Identifier AS UPC,
      CASE WHEN National_Identifier IS NOT NULL THEN SUBSTRING ('0000000000000', 1, 13 - LEN(National_Identifier)) + National_Identifier ELSE '0000000000000' END AS NAT_UPC,
      SUBSTRING ('000000000000000', 1, 15 - LEN(Item.Item_Key)) + CONVERT(varchar (15), Item.Item_Key) AS SKU_REGIONAL,
     -- (select PrimaryRegionCode from InstanceData) AS REGION,
     (select top 1 runmode from conversion_runmode) AS REGION, --GET REGION
      LEFT (Item_Description, 50) AS LONG_DESCRIPTION,
      POS_Description AS POS_DESCRIPTION,
      --LEFT(SubTeam_No, 3) AS POS_DEPT,
      st.Dept_No AS POS_DEPT,
      Package_Desc2 AS ITEM_SIZE,
      LEFT(Unit_Name, 5) AS ITEM_UOM,
      LEFT (Brand_Name, 10) AS BRAND,
          --CASE (select top 1 runmode from conversion_runmode)
          --              WHEN 'MA' THEN  (case when (jda_dept<>0) then (right('000'+convert(varchar(3),jda_dept),3)) else '' end)+
			CASE (select ISNULL(FlagValue,0) from InstanceDataFlags WHERE FlagKey = 'FourLevelHierarchy') 
						WHEN 1
						THEN  (case when (jda_dept<>0) then (right('000'+convert(varchar(3),jda_dept),3)) else '' end)+
                                                        (case when (jda_subdept<>0) then (right('000'+convert(varchar(3),jda_subdept),3)) else '' end)+
                                                        (case when (jda_class<>0) then (right('000'+convert(varchar(3),jda_class),3)) else '' end)+
                                                        (case when (jda_subclass<>0) then (right('000'+convert(varchar(3),jda_subclass),3)) else '' end)
                        ELSE CAST(ITEM.CLASSID as varchar(12))
          END AS REG_HIER_REF,
               dbo.fn_GetItemStatus(Item.Item_Key) AS ITEM_STATUS,
      CONVERT(VARCHAR(10), INSERT_DATE,101) AS CREATE_DATE
   FROM
      @VIMItemRegion VIR
      INNER JOIN Item (NOLOCK) ON (VIR.Item_Key = Item.Item_Key)
	  INNER JOIN SubTeam st (NOLOCK) ON Item.SubTeam_No = st.SubTeam_No
     LEFT JOIN JDA_HierarchyMapping (NOLOCK) on (Item.prodhierarchylevel4_id = JDA_HierarchyMapping.prodhierarchylevel4_id)
      LEFT JOIN ItemUnit (NOLOCK) ON (Item.Package_Unit_Id = ItemUnit.Unit_Id)
      LEFT JOIN ItemBrand (NOLOCK)ON Item.Brand_ID = ItemBrand.Brand_ID
      where VIR.Item_Key in 
		(select item_key
        from storeitemvendor siv
		join Store s on siv.Store_No = s.Store_No
			and s.Mega_Store = 0
		where s.store_no in 
			(select store_no 
			from storeregionmapping
            where region_code in 
				(select top 1 runmode
				from conversion_runmode)
			)
        )

   ORDER BY
      VIR.Item_Key;




   SET NOCOUNT OFF;

end
GO
