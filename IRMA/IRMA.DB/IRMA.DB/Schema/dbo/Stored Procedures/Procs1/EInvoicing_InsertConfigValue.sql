create procedure dbo.EInvoicing_InsertConfigValue
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

Insert into EInvoicing_Config
(
	ElementName, 
	SacCodeType,
	IsSacCode,
	SubTeam_No,
	ChargeOrAllowance, 
	Label,
	IsHeaderElement,
	IsItemElement,
	Disabled,
	NeedsConfig
)
values
(
	@ElementName ,
    @SACType ,
    @IsSAC ,
	@SubTeam_No , 
    @IsAllowance , 
    @Label,
	@IsHeaderElement,
	@IsItemElement,
	@IsDisabled,
	0 
)


end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertConfigValue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_InsertConfigValue] TO [IRMAClientRole]
    AS [dbo];

