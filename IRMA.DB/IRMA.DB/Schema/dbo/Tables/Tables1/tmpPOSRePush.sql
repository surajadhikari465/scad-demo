CREATE TABLE [dbo].[tmpPOSRePush] (
    [Item_Key] INT NOT NULL,
    [Store_No] INT NOT NULL,
    CONSTRAINT [PK_tmpPOSRePush] PRIMARY KEY CLUSTERED ([Item_Key] ASC, [Store_No] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpPOSRePush] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpPOSRePush] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpPOSRePush] TO [IRMAReportsRole]
    AS [dbo];

