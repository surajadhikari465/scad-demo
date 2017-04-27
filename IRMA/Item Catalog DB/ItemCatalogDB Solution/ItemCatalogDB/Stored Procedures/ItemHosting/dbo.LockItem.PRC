SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LockItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LockItem]
GO


CREATE PROCEDURE dbo.LockItem 
    @Item_Key int, 
    @User_ID int 
AS 
BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    UPDATE Item
    SET User_ID = @User_ID, User_ID_Date = GETDATE()
    WHERE Item_Key = @Item_Key AND User_ID IS NULL

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
        SELECT Item.User_ID, CONVERT(varchar(255), ISNULL(User_ID_Date, ''), 121) As User_ID_Date, FullName
        FROM Item
        INNER JOIN Users (nolock) ON Users.User_ID = Item.User_ID
        WHERE Item_Key = @Item_Key

        SELECT @error_no = @@ERROR
    END

    SET NOCOUNT OFF

    IF @error_no <> 0
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('LockItem failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


