USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = N'CommunityDB')
BEGIN
    ALTER DATABASE CommunityDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CommunityDB;
END
GO

CREATE DATABASE CommunityDB;
GO

USE CommunityDB;
GO

CREATE TABLE Account (
  ID INT IDENTITY(1,1) PRIMARY KEY,
  Email NVARCHAR(100) UNIQUE NOT NULL,
  Name NVARCHAR(50) NOT NULL,
  Password NVARCHAR(255) NOT NULL,
  CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Community (
  ComId INT IDENTITY(1,1) PRIMARY KEY,
  ComName NVARCHAR(50) NOT NULL,
  Body NVARCHAR(MAX), 
  ComCode NVARCHAR(50) UNIQUE NOT NULL,
  Picture NVARCHAR(255),
  GatePhoneNum VARCHAR(15),
  CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Members (
  ComId INT, 
  UserId INT,
  [Role] NVARCHAR(50) NOT NULL,
  Balance INT DEFAULT 0,
  UnitNum INT DEFAULT 0,
  IsLiable BIT DEFAULT 0, 
  IsResident BIT DEFAULT 0,
  IsManager BIT DEFAULT 0,
  IsProvider BIT DEFAULT 0,
  PRIMARY KEY (ComId, UserId),
  FOREIGN KEY (UserId) REFERENCES Account(ID) ON DELETE CASCADE,
  FOREIGN KEY (ComId) REFERENCES Community(ComId) ON DELETE CASCADE
);

CREATE TABLE Priority (
  PriorityNum INT PRIMARY KEY,
  Priority NVARCHAR(20) NOT NULL
);

CREATE TABLE Status (
  StatNum INT PRIMARY KEY,
  [Status] NVARCHAR(20) NOT NULL
);

CREATE TABLE Report (
  ReportId INT IDENTITY(1,1) PRIMARY KEY,
  UserId INT NOT NULL,
  ComId INT NOT NULL,
  Title NVARCHAR(255) NOT NULL DEFAULT '',
  ReportDesc NVARCHAR(MAX),
  Priority INT,
  Status INT,
  IsAnon BIT DEFAULT 0,
  CreatedAt DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (UserId) REFERENCES Account(ID) ON DELETE CASCADE,
  FOREIGN KEY (ComId) REFERENCES Community(ComId) ON DELETE CASCADE,
  FOREIGN KEY (Priority) REFERENCES Priority(PriorityNum),
  FOREIGN KEY (Status) REFERENCES Status(StatNum)
);

CREATE INDEX IX_Report_CreatedAt ON Report(CreatedAt);
CREATE INDEX IX_Report_UserId_ComId ON Report(UserId, ComId);

CREATE TABLE Notices (
  NoticeId INT IDENTITY(1,1) PRIMARY KEY,
  UserId INT NOT NULL,
  Title NVARCHAR(100) NOT NULL, 
  Text NVARCHAR(MAX),
  StartTime DATETIME,
  EndTime DATETIME,
  CreatedAt DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (UserId) REFERENCES Account(ID) ON DELETE CASCADE
);

CREATE INDEX IX_Notices_UserId ON Notices(UserId);
CREATE INDEX IX_Notices_StartTime ON Notices(StartTime);
CREATE INDEX IX_Notices_EndTime ON Notices(EndTime);

CREATE TABLE CommunityNotices (
  NoticeId INT,
  ComId INT,
  PRIMARY KEY (NoticeId, ComId),
  FOREIGN KEY (NoticeId) REFERENCES Notices(NoticeId) ON DELETE CASCADE,
  FOREIGN KEY (ComId) REFERENCES Community(ComId) ON DELETE CASCADE
);

CREATE TABLE Payments (
  PaymentId INT IDENTITY(1,1) PRIMARY KEY,
  ComId INT NOT NULL,
  UserId INT NOT NULL,
  Amount INT NOT NULL,
  MarkedPayed BIT DEFAULT 0,
  WasPayed BIT DEFAULT 0,
  PayFrom DATE NOT NULL,
  PayUntil DATE NOT NULL,
  FOREIGN KEY (ComId) REFERENCES Community(ComId) ON DELETE CASCADE,
  FOREIGN KEY (UserId) REFERENCES Account(ID) ON DELETE CASCADE
);

CREATE TABLE RoomRequests (
  RequestId INT IDENTITY(1,1) PRIMARY KEY,
  UserId INT NOT NULL,
  ComId INT NOT NULL,
  StartTime DATETIME NOT NULL,
  EndTime DATETIME NOT NULL,
  Text NVARCHAR(MAX),
  IsApproved BIT DEFAULT 0,
  CreatedAt DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (UserId) REFERENCES Account(ID) ON DELETE CASCADE,
  FOREIGN KEY (ComId) REFERENCES Community(ComId) ON DELETE CASCADE
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

-- Create the ReportFiles table
CREATE TABLE ReportFiles (
  ReportId INT,
  FileName NVARCHAR(255),  -- Added a column to store the file name
  RepFileExt NVARCHAR(5), 
  PRIMARY KEY (ReportId, FileName),
  FOREIGN KEY (ReportId) REFERENCES Report(ReportId)
);

-- Create the NoticeFiles table
CREATE TABLE NoticeFiles (
  NoticeId INT,
  FileName NVARCHAR(255),  -- Added a column to store the file name
  NoticeFileExt NVARCHAR(5), 
  PRIMARY KEY (NoticeId, FileName),
  FOREIGN KEY (NoticeId) REFERENCES Notices(NoticeId)
);

-- Insert Initial Data
INSERT INTO Account (Email, Name, Password) VALUES ('a', 'a', 'a');

INSERT INTO Community (ComName, Body, ComCode, GatePhoneNum) 
VALUES 
  ('Kehila', 'Welcome', '123', '0528185522'),
  ('Kehila2', 'Hola! Glad to have you!', '456', '0508182244'),
  ('Kehila3', 'Shalom! Excited to have you back', '789', '0501812244');

INSERT INTO Members (UserId, ComId, [Role], Balance, UnitNum, IsLiable, IsResident, IsManager, IsProvider) 
VALUES 
  (1, 1, 'Manager', 100, 1, 1, 1, 1, 1),
  (1, 2, 'Resident', 50, 7, 1, 1, 0, 0),
  (1, 3, 'Resident', 0, 13, 0, 1, 0, 0);

INSERT INTO Priority (PriorityNum, Priority) VALUES (1, 'Normal');

INSERT INTO Status (StatNum, [Status]) VALUES (1, 'Normal');

INSERT INTO Report (UserId, ComId, [Text], [Priority], [Status], IsAnon, Title)
VALUES 
  (1, 1, 'Encountered a mess in the trash room.', 1, 1, 0, 'Watch out!'),
  (1, 1, 'The elevator has been stuck for hours, causing inconvenience.', 1, 1, 1, 'Elevator Problem');

-- Select Data for Debugging
SELECT * FROM Account;
SELECT * FROM Members;
SELECT * FROM Community;
SELECT * FROM Report;
SELECT * FROM Notices;
SELECT * FROM CommunityNotices;
SELECT * FROM Members;

CREATE LOGIN [AdminLogin] WITH PASSWORD = 'ComPass';
Go

CREATE USER [Manager] FOR LOGIN [AdminLogin];
Go

ALTER ROLE db_owner ADD MEMBER [Manager]
Go

/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=CommunityDB;User ID=AdminLogin;Password=ComPass;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context CommunityDBContext -DataAnnotations –force
*/


