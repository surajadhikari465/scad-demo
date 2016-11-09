create procedure dbo.EInvoicing_UpdateConfigValue
	@ElementName varchar(255),
    @SACType int,
    @IsSAC bit,
	@SubTeam_No int, 
    @IsAllowance int, 
    @Label varchar(2048),
	@IsHeaderElement bit,
	@IsItemElement bit,
	@IsDisabled bit
as
begin

	Update EInvoicing_Config
	set SACCodeType = @SACType,
	IsSacCode = @IsSAC, 
	ChargeOrAllowance = @IsAllowance,
	SubTeam_No = @SubTeam_no,
	Label = @Label, 
	IsHeaderElement = @IsHeaderElement ,
	IsItemElement = @IsItemElement, 
	Disabled = @IsDisabled,
	NeedsConfig = 0
	where ElementName = @ElementName

end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_UpdateConfigValue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_UpdateConfigValue] TO [IRMAClientRole]
    AS [dbo];

