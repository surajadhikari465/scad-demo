create procedure dbo.EInvoicing_getSubTeams
as
begin
	SELECT	SubTeam_No, 
			SubTeam_Name, 
			GLPurchaseAcct, 
			SubTeam_Name + ' [' + cast(GLPurchaseAcct as varchar(100)) +']' as Display 
	FROM	SubTeam (nolock) 
	WHERE	GLPurchaseAcct IS NOT NULL
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_getSubTeams] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_getSubTeams] TO [IRMAClientRole]
    AS [dbo];

