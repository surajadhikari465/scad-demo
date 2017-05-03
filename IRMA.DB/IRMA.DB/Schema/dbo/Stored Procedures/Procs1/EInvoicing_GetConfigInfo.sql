create procedure dbo.EInvoicing_GetConfigInfo
as
begin
	


SELECT c.ElementName,
       c.Label,
       c.IsSacCode,
       c.IsHeaderElement,
       c.NeedsConfig,
       c.SacCodeType,
       c.SubTeam_No,
       c.IsItemElement,
       c.Disabled,
       CASE 
            WHEN c.ChargeOrAllowance = 1 THEN 0
            ELSE 1
       END AS IsAllowance  -- 1 = Charge -1 = Allowance
FROM   einvoicing_config(NOLOCK) c
       LEFT JOIN einvoicing_sactypes s
            ON  c.saccodetype = s.sactype_id
       LEFT JOIN subteam st
            ON  c.subteam_no = st.subteam_no 
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetConfigInfo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetConfigInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetConfigInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetConfigInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetConfigInfo] TO [IRMAReportsRole]
    AS [dbo];

