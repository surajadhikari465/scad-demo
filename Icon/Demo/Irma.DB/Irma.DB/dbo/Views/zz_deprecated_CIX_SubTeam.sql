
create view [zz_deprecated_CIX_SubTeam] as
  select
    d.dp_dept_number    SubTeam_No,
    t.Team_No           Team_No,
    right(d.dp_dept_number,3) + ' ' + d.dp_dept_desc      SubTeam_Name,
    d.dp_pos_dept_desc  SubTeam_Abbreviation,
    d.dp_dept_number    Dept_No,
    null                SubDept_No,
    null                Buyer_User_ID,
    0                   Target_Margin,
    null                JDA,
    null                GLPurchaseAcct,
    null                GLDistributionAcct,
    null                GLTransferAcct,
    null                GLSalesAcct,
    null                Transfer_To_Markup,
    0                   EXEWarehouseSent,
    sd.scale_dept       ScaleDept,  -- 10/06/2006 RS.
    1                   Retail,
    0                   EXEDistributed,
    1					SubTeamType_ID,
    NULL				PurchaseThresholdCouponAvailable
from
   dept d

   join Team t
      on ( t.Team_No = d.dp_group_number )

   join CIX_groups g
      on ( g.dp_group_number = d.dp_group_number )

   left join CIX_Scale_Dept sd
      on ( cast(sd.cix_dept as integer) = cast(d.dp_dept_number as integer) )
where
   -- Allow Groups to be entered as a SubTeam if the group has no other
   -- members. 10/04/2006 RS.
   ( t.Team_No <> d.dp_dept_number
         or g.grp_count = 1 ) and
	d.dp_dept_number not in (4999, 9999)
