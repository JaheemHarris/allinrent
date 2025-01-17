-- Table: Admins
CREATE TABLE Admin (
    Id INTEGER PRIMARY KEY NOT NULL IDENTITY NOT FOR REPLICATION,
    UserName NVARCHAR(120) NOT NULL,
    EmailAddress NVARCHAR(120) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL
);

-- Table: User (Customers)
CREATE TABLE AuthUser (
    Id INTEGER PRIMARY KEY NOT NULL IDENTITY NOT FOR REPLICATION,
    FirstName NVARCHAR(120) NOT NULL,
    LastName NVARCHAR(120) NOT NULL,
    EmailAddress NVARCHAR(120) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(30) NOT NULL,
    IdNumber NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Status TINYINT NOT NULL CHECK (Status IN (0, 1, 2)) -- 0: Deactivated, 1: Activated, 2: Pending
);

-- Table: ItemType
CREATE TABLE ItemType (
    Id INTEGER PRIMARY KEY NOT NULL IDENTITY NOT FOR REPLICATION,
    Label NVARCHAR(180) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Table: Item
CREATE TABLE Item (
    Id INTEGER PRIMARY KEY NOT NULL IDENTITY NOT FOR REPLICATION,
    ItemTypeId INT NOT NULL,
    Name NVARCHAR(180) NOT NULL,
    Description NVARCHAR(MAX),
    ImageFile NVARCHAR(120),
    RentalFee DECIMAL(10, 2) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Item_ItemType FOREIGN KEY (ItemTypeId) REFERENCES ItemType(Id)
);

-- Table: Stock
CREATE TABLE Stock (
    Id INTEGER PRIMARY KEY NOT NULL IDENTITY NOT FOR REPLICATION,
    AdminId INT NOT NULL,
    ItemId INT NOT NULL,
    Quantity INT NOT NULL,
    Date DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Stock_Admin FOREIGN KEY (AdminId) REFERENCES Admin(Id),
    CONSTRAINT FK_Stock_Item FOREIGN KEY (ItemId) REFERENCES Item(Id)
);

-- Table: Rental
CREATE TABLE Rental (
    Id INTEGER PRIMARY KEY NOT NULL IDENTITY NOT FOR REPLICATION,
    UserId INT NOT NULL,
    ItemId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity >= 0),
    StartDate DATE NOT NULL,
    DurationDays INT NOT NULL CHECK (DurationDays > 0),
    UnitPrice DECIMAL(10, 2) NOT NULL CHECK (UnitPrice >= 0),
    Due DECIMAL(10, 2) NOT NULL CHECK (Due >= 0),
    ReturnDate DATE,
    RentalDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Rental_AuthUser FOREIGN KEY (UserId) REFERENCES AuthUser(Id),
    CONSTRAINT FK_Rental_Item FOREIGN KEY (ItemId) REFERENCES Item(Id)
);

CREATE INDEX IDX_Admin_EmailAddress
ON Admin (EmailAddress);

CREATE INDEX IDX_AuthUser_EmailAddress
ON AuthUser (EmailAddress);

CREATE VIEW View_Item AS
SELECT
    Item.Id,
    Item.ItemTypeId,
    ItemType.Label as ItemTypeLabel,
    Item.Name,
    Item.Description,
    Item.ImageFile,
    Item.RentalFee,
    Item.IsActive
FROM Item
JOIN ItemType ON Item.ItemTypeId = ItemType.Id
WHERE ItemType.IsActive = 1 AND Item.IsActive = 1

CREATE VIEW View_Rental AS
SELECT
    R.Id AS RentalId,
    R.UserId AS UserId,
    AU.FirstName AS FirstName,
    AU.LastName AS LastName,
    R.ItemId AS ItemId,
    I.Name AS ItemName,
    I.ItemTypeId AS ItemTypeId,
    IT.Label AS ItemTypeName,
    R.Quantity As Quantity,
    R.UnitPrice AS UnitPrice,
    R.StartDate AS StartDate,
    DATEADD(DAY, R.DurationDays, R.StartDate) AS DueDate,
    R.DurationDays AS DurationDays,
    R.Due AS Due,
    R.ReturnDate AS ReturnDate
FROM Rental R
JOIN AuthUser AU ON AU.Id = R.UserId
JOIN Item I ON R.ItemId = I.Id
JOIN ItemType IT ON IT.Id = I.ItemTypeId 

CREATE VIEW View_Stock AS
SELECT
    R.Id AS RentalId,
    R.UserId AS UserId,
    AU.FirstName AS FirstName,
    AU.LastName AS LastName,
    R.ItemId AS ItemId,
    I.Name AS ItemName,
    I.ItemTypeId AS ItemTypeId,
    IT.Label AS ItemTypeName,
    R.Quantity As Quantity,
    R.UnitPrice AS UnitPrice,
    R.StartDate AS StartDate,
    DATEADD(DAY, R.DurationDays, R.StartDate) AS DueDate,
    R.DurationDays AS DurationDays,
    R.Due AS Due,
    R.ReturnDate AS ReturnDate
FROM Rental R
JOIN AuthUser AU ON AU.Id = R.UserId
JOIN Item I ON R.ItemId = I.Id
JOIN ItemType IT ON IT.Id = I.ItemTypeId 