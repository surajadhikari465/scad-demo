
--***********************************************************************
-- Create a view joining all the temporary summary tables created above
-- for inserting data into the Item_Temp table.
--***********************************************************************

create view [dbo].[zz_deprecated_Item_Temp_Load]  as
select
    a.Item_Description                                         as Item_Description,
    a.SubTeam_No                                               as SubTeam_No,
    a.Package_Desc2                                            as Package_Desc2,
    a.Package_Unit_ID                                          as Package_Unit_ID,
    a.Category_ID                                              as Category_ID,
    a.Deleted_Item                                             as Deleted_Item,
    a.Discontinue_Item                                         as Discontinue_Item,
    a.POS_Description                                          as POS_Description,
    a.Price_Required                                           as Price_Required,
    a.ItemType_ID                                              as Item_Type_ID,
    a.Not_AvailableNote                                        as Not_AvailableNote,
    a.Insert_Date                                              as Insert_Date,
    ic.Category_Name                                           as Category_Name,
    cast(cast(a.CIX_upcno as bigint) as varchar(12))           as Identifier, -- 09/30/2006 RS - Removes leading 0s
    p.holdprice--/p.holdpm                                       
																as Price,
    case 
       when salprend < getdate() then null
       when salprend is null     then null
    else
		case p.ptype
		   when 'SAL' then p.salprend
		   when 'LIN' then p.salprend
		   when 'EDV' then p.salprend
		   when 'CMP' then p.salprend
		   when 'CRD' then p.salprend
		   when 'ISS' then p.salprend
		   when 'TPR' then p.salprend
		   else  null  
		end
	end                                                        as Sale_End_Date,
    case cst.avgcost
       when 0 then 1
       when null then 0
       else cst.avgcost
    end                                                        as AvgCost,
    v.Vendor_Key                                               as Vendor_Key,
    case
       when cst.casecost is null then 0
       when cst.casesize is null then 0
       else cst.casecost/cst.casesize
    end                                                        as UnitCost,
    case cst.casesize
       when 0    then 1
       when null then 1
       else      cst.casesize
    end                                                        as Package_Desc1, --case qty 10/02/2006 RS set to CIX case size
    c.Team_No                                                  as Team_No,
    c.Team_Name                                                as Team_Name,
    b.SubTeam_Name                                             as SubTeam_Name,
    a.SubTeam_No                                               as Dept_No,
    b.Target_Margin                                            as Target_Margin,
    case a.Chain_Code
       when a.CIX_upcno then 1
       when ''          then 1
       else                  0
    end                                                        as Default_Identifier,
    st.BusinessUnit_ID                                         as Business_Unit,
    case cuv.prim_vend
       when 'Y' then 1
       else          0
    end
                                                            as isPrimary,
    case 
       when salprend < getdate() then 0
       when salprend is null     then 0
    else
		case p.ptype
		   when 'SAL' then 1
		   when 'LIN' then 1
		   when 'EDV' then 1
		   when 'CMP' then 1
		   when 'CRD' then 1
		   when 'ISS' then 1
		   when 'TPR' then 1
		   else            0
		end
	end                                                        as onPromotion,
    case 
       when salprend < getdate() then 0
       when salprend is null     then 0
    else
		case p.ptype
		   when 'EDV' then 1
		   else            0
		end
	end                                                        as EDLP, -- RS 11/02/2006 Added field
    case
       when substring(a.CIX_upcno, 8, 5) = '00000' then 0
        else dbo.fn_CalcCheckDigit(a.CIX_upcno)
    end                                                        as CheckDigit,         -- RS 09/30/2006
    'B'                                                        as IdentifierType,
  -- Here is where you map the tax flags in cix to tax class id's in irma.
  -- presumably items in cix can have no more than one tax flag turned on.
    case charindex('Y', p.tax1 + p.tax2 + p.tax3 + p.tax4)
       when 1 then 1
       when 2 then 1
       when 3 then 1
       when 4 then 1
       else 2 end                                              as TaxClassID,
    case p.rescode
       when '9' then 1
       when '09' then 1
       else  0  end                                            as StopSale,
    a.LabelType_ID                                             as LabelTypeID,
    case
		when a.scale_uom = 'BC'then 0 -- added by BM 
		when (dbo.fn_IsScaleIdentifier(cast(cast(a.CIX_upcno as bigint) as varchar(12))) = 1 ) then 1
		else a.CostedByWeight 
    end														   as CostedByWeight,
    case
		when (a.scale_uom = 'BC') then (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA')  -- added by BM 
		when (dbo.fn_IsScaleIdentifier(cast(cast(a.CIX_upcno as bigint) as varchar(12))) = 1 ) then (select unit_id from itemunit where unit_abbreviation = 'lb')
		else a.Retail_Unit_ID
    end                                                        as Cost_Unit_ID, --BM cost and retail ids should match
    a.Freight_Unit_ID                                          as Freight_Unit_ID,
	case  -- these get updated at the end for costed by weight
		when (a.scale_uom = 'BC') then (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA')  -- added by BM
		when (cst.casesize is null) then (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA')
		when (cst.casesize = 1) then (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA')
		when (cst.casesize = 0) then (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA')
		else (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'CS') 
	end								                            as Vendor_Unit_ID,
   case  -- these get updated at the end for costed by weight
		when (a.scale_uom = 'BC') then (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA')  -- added by BM
		when (cst.casesize is null) then (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA')
		when (cst.casesize = 1) then (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA')
		when (cst.casesize = 0) then (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA')
		else (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'CS') 
    end															as Distribution_Unit_ID,
    case 
		when (a.scale_uom = 'BC') then (select Unit_Id from ItemUnit where  Unit_Abbreviation = 'EA')  -- added by BM
		when (dbo.fn_IsScaleIdentifier(cast(cast(a.CIX_upcno as bigint) as varchar(12))) = 1 ) then (select unit_id from itemunit where unit_abbreviation = 'lb')
		else a.Retail_Unit_ID
    end                                                       as Retail_Unit_ID,
    cst.warehouse                                              as Vendor_Item_ID,
    case 
       when salprend < getdate() then 0
       when salprend is null     then 0
    else
		case p.ptype
		   when 'SAL' then p.effprice--/p.effpm
		   when 'LIN' then p.effprice--/p.effpm
		   when 'EDV' then p.effprice--/p.effpm
		   when 'CMP' then p.effprice--/p.effpm
		   when 'CRD' then p.effprice--/p.effpm
		   when 'ISS' then p.effprice--/p.effpm
		   when 'TPR' then p.effprice--/p.effpm
		   else            0            
		end
	end                                                        as saleprice,
    p.holdprice--/p.holdpm                                       
																as posprice,
    case 
       when salprend < getdate() then 0
       when salprend is null     then 0
    else
		case p.ptype
		   when 'SAL' then p.effprice--/p.effpm
		   when 'LIN' then p.effprice--/p.effpm
		   when 'EDV' then p.effprice--/p.effpm
		   when 'CMP' then p.effprice--/p.effpm
		   when 'CRD' then p.effprice--/p.effpm
		   when 'ISS' then p.effprice--/p.effpm
		   when 'TPR' then p.effprice--/p.effpm
		   else            0            
		end
	end                                                       as possaleprice,
    cast(cast(a.master_upc as bigint) as varchar(12))         as master_upc,
    case 
       when salprend < getdate() then null
       when salprend is null     then null
    else
		case p.ptype
		   when 'SAL' then p.lastdate
		   when 'LIN' then p.lastdate
		   when 'EDV' then p.lastdate
		   when 'CMP' then p.lastdate
		   when 'CRD' then p.lastdate
		   when 'ISS' then p.lastdate
		   when 'TPR' then p.lastdate
		   else null  
		end
	end                                                      as sale_start_date,
    case p.ptype
       when 'CMP' then 1
       else 0      end                                         as cmp,
    a.Food_Stamps                                              as Food_Stamps,
   p.tare                                                  as PosTare,
   case
      when p.linkcode = '' then ''
      else cast(cast(p.linkcode as bigint) as varchar(12))
   end                                                        as LinkCode,  -- 09/30/2006 RS - Removes leading 0s
   case p.jrnprint
      when 'Y' then 1
      else 0 end                                              as GrillPrint,
    case p.rescode
       when '9' then 9
       when '09' then 9
       else  p.agecode  end                                           as AgeCode,
   case p.visual
      when 'Y' then 1
      else 0 end                                              as VisualVerify,
   case p.si3
      when 'Y' then 1
      else 0 end                                              as SrCitizenDiscount,
    a.QtyProhibit                                              as QtyProhibit,
    a.GroupList                                                as GroupList,
   null                                                       as item_key_temp,
   null                                                       as item_key_temp2,
    0                                                          as PricingMethod_ID,
    a.ShelfLife_Id                                             as ShelfLife_Id,
    a.ShelfLife_Length                                as ShelfLife_Length,
   p.holdpm                                     as Multiple,
   case
      when p.msrp > 0 then p.msrp
      else (p.holdprice/p.holdpm)
   end                                      as MSRPPrice, -- RS 11/02/2006
   1                                       as MSRPMultiple,
   p.effpm                                     as Sale_Multiple,
   0                                      as Sale_Earned_Disc1,
    0                                     as Sale_Earned_Disc2,
    0                                     as Sale_Earned_Disc3,
    a.scaledesc1                                as scaledesc1,
    a.ingredients                               as ingredients,
   a.brand_id                                as brandid,
   a.Discountable                             as discountable,
   case p.itemdisc when 'Y' then 1 else 0 end		as IBM_Discount,
   a.ClassID                                 as natclassid,
   a.scaledesc2								as scaledesc2,
   a.scaleForcedtare						as scaleforcedtare,
   a.scaletare								as scaletare,
   a.scaledesc3                                as scaledesc3,
   a.scaledesc4                                as scaledesc4,
   case p.agecode
		when 2 then 1
		else 0 end							as restricted_hours,
   case
      when cuv.prim_vend = 'Y' then 1
      when cuv.authrzd   = 'Y' then 1
      else 0
   end                                      as isauthorized -- Added 11/03/2006 RS.
FROM
    Store st,               -- For every store - RS.
    Item_Temp_Staging a,
    SubTeam b,
    Team c,
    ItemCategory ic,
    cxbstorr s,
    upc_vendr cuv,
    Vendor v,
    cxspricd p,
    cost_vendr cst,
    ItemBrand ib
WHERE
    -- Join the SubTeam table
    b.SubTeam_No     = a.SubTeam_No and

    -- Join the Team table
    c.Team_No        = b.Team_No and

    -- Join the ItemCategory table
    ic.Category_ID   = a.Category_ID and

     -- Join the ItemBrand table
    ib.Brand_ID   = a.brand_id and

    -- Join the cxbstorr table
    s.store        = st.Store_No and

    -- Join the upc_vendr table
    cuv.upcno        = a.CIX_upcno and
    cuv.store        = s.store and

    -- Join the Vendor table
    v.Vendor_Key     = cuv.vendor and

    -- Join the cxspricd table
    p.upcno          = a.CIX_upcno and
    p.pzone          = st.Zone_ID and
    p.store          = (select
                     max(z.store)
                   from
                     cxspricd z
                   where
                     z.upcno=a.CIX_upcno
                     and z.pzone=st.Zone_ID
                     and (z.store=s.store or z.store=0)) and

    -- Join the cost_vendr table
    cst.upcno        = a.CIX_upcno and
    cst.vendor       = v.Vendor_Key and
    cst.store        = (select
                           max(y.store)
                        from
                           cost_vendr y
                        where
                           y.upcno = a.CIX_upcno
                           and y.vendor = v.Vendor_Key
                           and(y.store=s.store or y.store=0))

    --and wfs1.upcno = a.CIX_upcno
    --and wfs2.upcno = a.CIX_upcno
    --and wfs3.num = wfs2.ingr_no
