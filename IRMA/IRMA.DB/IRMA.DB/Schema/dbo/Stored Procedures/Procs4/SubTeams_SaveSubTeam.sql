CREATE PROCEDURE dbo.SubTeams_SaveSubTeam
	-- Add the parameters for the stored procedure here
	           
            @SubTeam_No int,
            @Team_No int,
            @SubTeam_Name varchar(100),
            @SubTeam_Abbreviation varchar(10),
            @Dept_No int,
            @SubDept_No int,
            @Buyer_User_Id int,
            @Target_Margin decimal(9,4),
            @JDA int,
            @GLPurchaseAcct int,
            @GLDistributionAcct int,
            @GLTransferAcct int,
            @GLSalesAcct int,
            @GLPackagingAcct int,
            @GLSuppliesAcct int,
            @Transfer_To_Markup decimal(9,4),
            @EXEWarehouseSent bit,
            @ScaleDept int,
            @Retail bit,
            @InventoryCountByCase bit,
            @EXEDistributed bit,
            @SubTeamType_Id int,
			@Distribution bit,
			@FixedSpoilage bit,
			@Beverage bit,
			@AlignedSubTeam bit
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE SubTeam
	SET	Team_No = @Team_No,
		SubTeam_Name = @SubTeam_Name ,
        SubTeam_Abbreviation = @SubTeam_Abbreviation,
        Dept_No =@Dept_No ,
        SubDept_No =@SubDept_No ,
        Buyer_User_Id =@Buyer_User_Id,
        Target_Margin =@Target_Margin,
        JDA =@JDA,
        GLPurchaseAcct =@GLPurchaseAcct ,
        GLDistributionAcct =@GLDistributionAcct ,
        GLTransferAcct =@GLTransferAcct,
        GLSalesAcct =@GLSalesAcct,
        GLPackagingAcct =@GLPackagingAcct,
        GLSuppliesAcct =@GLSuppliesAcct,
        Transfer_To_Markup =@Transfer_To_Markup,
        EXEWarehouseSent =@EXEWarehouseSent,
        ScaleDept =@ScaleDept,
        Retail =@Retail,
        InventoryCountByCase = @InventoryCountByCase,
        EXEDistributed =@EXEDistributed,
 		FixedSpoilage =@FixedSpoilage,
  		Beverage =@Beverage,
        SubTeamType_Id =@SubTeamType_Id,
		AlignedSubTeam = @AlignedSubTeam
	WHERE SubTeam_No = @SubTeam_No
	
	IF @Distribution = 1
	BEGIN
		insert into DistSubteam (DistSubTeam_No, RetailSubTeam_No)
		select @Subteam_No, @Subteam_No
		where @Subteam_No not in 
			(select DistSubTeam_No from DistSubTeam) 
	END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_SaveSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_SaveSubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_SaveSubTeam] TO [IRMAClientRole]
    AS [dbo];

