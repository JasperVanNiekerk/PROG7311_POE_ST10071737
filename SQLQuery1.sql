-- Create the FarmerRequest table
CREATE TABLE FarmerRequest (
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    RequestDate DATE NOT NULL DEFAULT GETDATE(),
    IsApproved BIT NOT NULL DEFAULT 0
);

-- Create the Farmer table
CREATE TABLE Farmer (
    FarmerId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(100) NOT NULL
);

-- Create the Employee table
CREATE TABLE Employee (
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(100) NOT NULL
);

-- Create the Category table
CREATE TABLE Category (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName VARCHAR(50) NOT NULL
);

-- Create the Product table
CREATE TABLE Product (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Description VARCHAR(MAX) NOT NULL,
    CategoryId INT FOREIGN KEY REFERENCES Category(CategoryId),
    ProductionDate DATE NOT NULL,
    FarmerId INT FOREIGN KEY REFERENCES Farmer(FarmerId)
);