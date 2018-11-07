CREATE PROCEDURE dbo.InsertGLPushQueue
@Start_Date DateTime,
@End_Date DateTime,
@Modified_By int,
@Distributions bit,
@Transfers bit
AS

INSERT INTO GLPushQueue (Start_Date, End_Date, Modified_By, Distributions, Transfers)
VALUES (@Start_Date, @End_Date, @Modified_By, @Distributions, @Transfers)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertGLPushQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertGLPushQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertGLPushQueue] TO [IRMAReportsRole]
    AS [dbo];

