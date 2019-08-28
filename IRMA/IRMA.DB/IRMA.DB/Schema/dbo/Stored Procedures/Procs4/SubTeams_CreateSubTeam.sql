CREATE PROCEDURE dbo.SubTeams_CreateSubTeam
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
            @FixedSpoilage int,
            @Beverage bit,
            @AlignedSubTeam bit,
            @IsDisabled bit = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO SubTeam
	(
            SubTeam_No, 
            Team_No, 
            SubTeam_Name, 
            SubTeam_Abbreviation,
            Dept_No, 
            SubDept_No, 
            Buyer_User_Id, 
            Target_Margin, 
            JDA, 
            GLPurchaseAcct, 
            GLDistributionAcct, 
            GLTransferAcct, 
            GLSalesAcct,
            GLPackagingAcct,
            GLSuppliesAcct, 
            Transfer_To_Markup, 
            EXEWarehouseSent, 
            ScaleDept, 
            Retail, 
            InventoryCountByCase,
            EXEDistributed, 
            SubTeamType_Id,
            FixedSpoilage,
            Beverage,
            AlignedSubTeam,
            IsDisabled
	) 
	VALUES
	(
            @SubTeam_No,
            @Team_No,
            @SubTeam_Name,
            @SubTeam_Abbreviation,
            @Dept_No,
            @SubDept_No,
            @Buyer_User_Id,
            @Target_Margin,
            @JDA,
            @GLPurchaseAcct,
            @GLDistributionAcct,
            @GLTransferAcct,
            @GLSalesAcct,
            @GLPackagingAcct,
            @GLSuppliesAcct,
            @Transfer_To_Markup,
            @EXEWarehouseSent,
            @ScaleDept,
            @Retail,
            @InventoryCountByCase,
            @EXEDistributed,
            @SubTeamType_Id,
            @FixedSpoilage,
            @Beverage,
            @AlignedSubTeam,
            @IsDisabled
	)

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
    ON OBJECT::[dbo].[SubTeams_CreateSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_CreateSubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_CreateSubTeam] TO [IRMAClientRole]
    AS [dbo];

