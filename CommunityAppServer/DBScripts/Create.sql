USE master
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = N'CommunityDB')
BEGIN
    DROP DATABASE CommunityDB
END
GO

CREATE DATABASE CommunityDB;
GO

USE CommunityDB;
GO

CREATE TABLE Account (
  ID INT IDENTITY(1,1) PRIMARY KEY,
  Email NVARCHAR(100) UNIQUE,
  Name NVARCHAR(20),
  Password NVARCHAR(255),
  CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Community (
  ComId INT IDENTITY(1,1) PRIMARY KEY,
  ComName NVARCHAR(15),
  Body TEXT, 
  ComCode NVARCHAR(20),
  Picture NVARCHAR(50),
  GatePhoneNum VARCHAR(15),
  CreatedAt DATETIME DEFAULT GETDATE()
);


CREATE TABLE Members (
  ComId INT, 
  UserId INT,
  [Role] NVARCHAR(20),
  Balance INT,
  UnitNum INT,
  IsLiable BIT, 
  IsResident BIT,
  IsManager BIT,
  IsProvider BIT,
  PRIMARY KEY (ComId, UserId),
  FOREIGN KEY (UserId) REFERENCES Account(ID),
  FOREIGN KEY (ComId) REFERENCES Community(ComId)
);



CREATE TABLE Types (
  TypeNum INT PRIMARY KEY,
  [Type] NVARCHAR(10)
);

CREATE TABLE Priority (
  PriorityNum INT PRIMARY KEY,
  Priority NVARCHAR(10)
);

CREATE TABLE Status (
  StatNum INT PRIMARY KEY,
  Status NVARCHAR(10) 
);

CREATE TABLE Report (
  ReportId INT IDENTITY(1,1) PRIMARY KEY,
  UserId INT,
  ComId INT,
  Text TEXT,
  Priority INT,
  Type INT,
  Status INT,
  IsAnon BIT,
  CreatedAt DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (UserId) REFERENCES Account(ID),
  FOREIGN KEY (ComId) REFERENCES Community(ComId),
  FOREIGN KEY (Priority) REFERENCES Priority(PriorityNum),
  FOREIGN KEY (Type) REFERENCES Types(TypeNum),
  FOREIGN KEY (Status) REFERENCES Status(StatNum)
);

CREATE TABLE ReportFiles (
  ReportId INT,
  FileName NVARCHAR(255),  -- Added a column to store the file name
  RepFileExt NVARCHAR(5), 
  PRIMARY KEY (ReportId, FileName),
  FOREIGN KEY (ReportId) REFERENCES Report(ReportId)
);

CREATE TABLE Notices (
  NoticeId INT IDENTITY(1,1) PRIMARY KEY,
  UserId INT,
  Title NVARCHAR(100), 
  Text TEXT,
  StartTime DATETIME,
  EndTime DATETIME,
  CreatedAt DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (UserId) REFERENCES Account(ID)
);



CREATE TABLE CommunityNotices (
  NoticeId INT,
  ComId INT,
  PRIMARY KEY (NoticeId, ComId),
  FOREIGN KEY (NoticeId) REFERENCES Notices(NoticeId),
  FOREIGN KEY (ComId) REFERENCES Community(ComId)
);

CREATE TABLE CommunityReports (
  ReportId INT,
  ComId INT,
  PRIMARY KEY (ReportId, ComId),
  FOREIGN KEY (ReportId) REFERENCES Report(ReportId),
  FOREIGN KEY (ComId) REFERENCES Community(ComId)
);
CREATE TABLE NoticeFiles (
  NoticeId INT,
  FileName NVARCHAR(255),  -- Added a column to store the file name
  NoticeFileExt NVARCHAR(5), 
  PRIMARY KEY (NoticeId, FileName),
  FOREIGN KEY (NoticeId) REFERENCES Notices(NoticeId)
);

CREATE TABLE Payments 
(
  PaymentId INT IDENTITY(1,1) PRIMARY KEY,
  ComId INT,
  UserId INT,
  Amount INT,
  MarkedPayed BIT,
  WasPayed BIT,
  PayFrom DATE,
  PayUntil DATE,
  FOREIGN KEY (ComId) REFERENCES Community(ComId),
  FOREIGN KEY (UserId) REFERENCES Account(ID)
);

CREATE TABLE TenantRoom (
  ComId INT,
  Status NVARCHAR(10),
  KeyHolderId INT,
  SessionStart DATETIME,
  SessionEnd DATETIME,
  PRIMARY KEY (ComId, KeyHolderId),
  FOREIGN KEY (ComId) REFERENCES Community(ComId),
  FOREIGN KEY (KeyHolderId) REFERENCES Account(ID)
);

CREATE TABLE RoomRequests (
  RequestId INT IDENTITY(1,1) PRIMARY KEY,
  UserId INT,
  ComId INT,
  StartTime DATETIME,
  EndTime DATETIME,
  Text TEXT,
  IsApproved BIT,
  CreatedAt DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (UserId) REFERENCES Account(ID),
  FOREIGN KEY (ComId) REFERENCES Community(ComId)
);




INSERT INTO Account (Email, Name, Password) VALUES ('admin@gmail.com', 'admin', 'admin1');
GO

INSERT INTO [Community] (ComName, Body, ComCode, GatePhoneNum) VALUES ('Kehila', 'Welcome', '123', '0528185522')
GO

INSERT INTO [Members] 
    (UserId, ComId, [Role], Balance, UnitNum, IsLiable, IsResident, IsManager, IsProvider) 
VALUES 
    (1, 1, 'Manager', 100, 1, 1, 1, 1, 1);
GO



CREATE LOGIN [AdminLogin] WITH PASSWORD = 'ComPass';
Go

CREATE USER [Manager] FOR LOGIN [AdminLogin];
Go

ALTER ROLE db_owner ADD MEMBER [Manager]
Go

/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=CommunityDB;User ID=AdminLogin;Password=ComPass;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context CommunityDBContext -DataAnnotations –force
*/

select * from Account
select * from Members
select * from Community