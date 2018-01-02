CREATE TYPE dbo.ItemKeyAndStoreNoType AS TABLE
(
	Item_Key INT NOT NULL,
	Store_No INT NOT NULL
)

GO
GRANT EXECUTE
    ON TYPE::[dbo].[ItemKeyAndStoreNoType] TO [IRMAClientRole];