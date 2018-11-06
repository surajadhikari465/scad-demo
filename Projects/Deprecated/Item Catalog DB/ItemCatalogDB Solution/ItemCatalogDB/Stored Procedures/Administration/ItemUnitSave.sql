IF EXISTS (SELECT * FROM SysObjects where name = 'ItemUnitSave')
    drop procedure dbo.ItemUnitSave
Go

CREATE PROCEDURE dbo.ItemUnitSave  
 @UnitName AS varchar(200) ,  
 @WeightUnit AS bit ,  
 @UserId AS INTEGER ,  
 @UnitAbbreviation AS varchar(200) ,  
 @UnitSysCode AS varchar(200) ,  
 @IsPackageUnit AS bit ,  
 @plumunitabbreviation AS varchar(200) ,  
 @EDISysCode AS varchar(200),
 @Unit_ID as int = NULL
AS
BEGIN  
    --If @Unit_ID is a valid value, perform an update.
    IF  @Unit_ID > 0
    and @Unit_ID is not null
    and EXISTS ( SELECT * FROM ItemUnit WHERE Unit_ID = @Unit_ID )
    BEGIN
        UPDATE  itemunit  
        SET     unit_name = @unitname,
                weight_unit = @weightunit ,  
                unit_abbreviation = @unitabbreviation ,  
                unitsyscode = @unitsyscode ,  
                ispackageunit = @ispackageunit ,  
                user_id = @userid ,  
                plumunitabbr = @plumunitabbreviation ,  
                edisyscode = @edisyscode
        WHERE   Unit_ID = @Unit_ID
    END  
    ELSE  
    BEGIN  
        INSERT  INTO itemunit
        (       unit_name ,  
                weight_unit ,  
                unit_abbreviation ,  
                unitsyscode ,  
                ispackageunit ,  
                user_id ,  
                plumunitabbr ,  
                edisyscode )  
        VALUES  (  
               @unitname ,  
               @weightunit ,  
               @unitabbreviation ,  
               @unitsyscode ,  
               @ispackageunit ,  
               @userid ,  
               @plumunitabbreviation ,  
               @edisyscode )  
    END  
END  
go
 