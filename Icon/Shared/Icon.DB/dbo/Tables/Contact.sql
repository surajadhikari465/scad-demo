CREATE TABLE [dbo].[Contact]
(
    [ContactId] INT NOT NULL IDENTITY CONSTRAINT pk_Contact PRIMARY KEY,
    [HierarchyClassID] INT NOT NULL CONSTRAINT fk_Contact_HierarchyClass FOREIGN KEY REFERENCES dbo.HierarchyClass(hierarchyClassID), 
    [ContactTypeId] INT NOT NULL CONSTRAINT fk_Contact_ContactType FOREIGN KEY REFERENCES dbo.ContactType(ContactTypeId), 
    [ContactName] NVARCHAR(255) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL,
    [Title] NVARCHAR(255) NULL,
    [AddressLine1] NVARCHAR(255) NULL,
    [AddressLine2] NVARCHAR(255) NULL,
    [City] NVARCHAR(255) NULL,
    [State] NVARCHAR(255) NULL,
    [ZipCode] NVARCHAR(15) NULL,
    [Country] NVARCHAR(255) NULL,
    [PhoneNumber1] NVARCHAR(30) NULL,
    [PhoneNumber2] NVARCHAR(30) NULL,
    [WebsiteURL] NVARCHAR(255) NULL,
    [InsertedDate] DateTime2(7) NOT NULL CONSTRAINT df_Contact_InsertDate DEFAULT GetDate(),
    [ModifiedDate] DateTime2(7) NULL,

    CONSTRAINT [ak_Contact] UNIQUE NONCLUSTERED(HierarchyClassID, ContactTypeID, Email ASC) WITH (FILLFACTOR = 80)
)
