CREATE TABLE [dbo].[RewardType] (
    [RewardType_ID] INT          IDENTITY (1, 1) NOT NULL,
    [Reward_Name]   VARCHAR (20) NULL,
    CONSTRAINT [PK_RewardType] PRIMARY KEY CLUSTERED ([RewardType_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[RewardType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[RewardType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[RewardType] TO [IRMAReportsRole]
    AS [dbo];

