--*******************************************************************************
--	Date:		11/14/2013
--	Created By: Benjamin Sims
--	Purpose:	This script creates the 'IdentifierProduct' table which will host
--				the MainID column being sent from the Icon database.
--				The MainID column will be needed when sending price  or promo batches 
--				to R10 (Retalix POS) so it can map back to a consolidated item
--*******************************************************************************

-- Create Table
IF OBJECT_ID('IdentifierProduct','U') IS NOT NULL
BEGIN
	DROP TABLE IdentifierProduct
END

CREATE TABLE dbo.IdentifierProduct
(
	IdentifierProductID int NOT NULL IDENTITY(1,1),
	Identifier_ID		int NOT NULL,
	MainID				int NOT NULL,
	CONSTRAINT pk_IdentifierProduct_IdentifierProductID PRIMARY KEY (IdentifierProductID),
	CONSTRAINT fk_IdentifierProduct_ItemIdentifier FOREIGN KEY (Identifier_ID) 
		REFERENCES ItemIdentifier (Identifier_ID),
)

-- Create Unique Constraint on MainID
IF NOT EXISTS (SELECT * FROM sys.indexes si WHERE si.name = 'idx_IdentifierProduct_MainID_NotNull' AND si.object_id = OBJECT_ID('IdentifierProduct'))
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX idx_IdentifierProduct_MainID_NotNull ON IdentifierProduct (MainID)
END

-- Create Nonclustered Index on Identifier_ID
IF NOT EXISTS (SELECT * FROM sys.indexes si WHERE si.name = 'idx_IdentifierProduct_IdentifierID' AND si.object_id = OBJECT_ID('IdentifierProduct'))
BEGIN
	CREATE NONCLUSTERED INDEX idx_IdentifierProduct_IdentifierID ON IdentifierProduct (Identifier_ID)
END

-- Enable Change Tracking
IF OBJECT_ID('IdentifierProduct', 'U') IS NOT NULL
BEGIN
	ALTER TABLE [dbo].[IdentifierProduct]
	ENABLE CHANGE_TRACKING
	WITH (TRACK_COLUMNS_UPDATED = ON)
END