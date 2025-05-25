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
  PhoneNumber VARCHAR(20),
  CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Community (
  ComId INT IDENTITY(1,1) PRIMARY KEY,
  ComName NVARCHAR(50) NOT NULL,
  ComDesc NVARCHAR(MAX), 
  ComCode NVARCHAR(50) UNIQUE NOT NULL,
  Picture NVARCHAR(255),
  GatePhoneNum VARCHAR(15),
  CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Members (
  ComId INT, 
  UserId INT,
  [Role] NVARCHAR(50),
  Balance INT DEFAULT 0,
  UnitNum INT DEFAULT 0,
  IsLiable BIT DEFAULT 0, 
  IsResident BIT DEFAULT 0,
  IsManager BIT DEFAULT 0,
  IsProvider BIT DEFAULT 0,
  IsApproved BIT DEFAULT 0,
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
    ComId INT NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    NoticeDesc NVARCHAR(MAX),
    StartTime DATETIME,
    EndTime DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Account(ID) ON DELETE CASCADE,
    FOREIGN KEY (ComId) REFERENCES Community(ComId) ON DELETE CASCADE -- הוספת מפתח זר לקהילה
);

CREATE INDEX IX_Notices_UserId ON Notices(UserId);
CREATE INDEX IX_Notices_ComId ON Notices(ComId); 
CREATE INDEX IX_Notices_StartTime ON Notices(StartTime);
CREATE INDEX IX_Notices_EndTime ON Notices(EndTime);

CREATE TABLE Payments (
  PaymentId INT IDENTITY(1,1) PRIMARY KEY,
  ComId INT NOT NULL,
  UserId INT NOT NULL,
  Amount INT NOT NULL,
  Details NVARCHAR(200),
  MarkedPayed BIT DEFAULT 0,
  WasPayed BIT DEFAULT 0,
  PayFrom DATE,
  PayUntil DATE,
  FOREIGN KEY (ComId) REFERENCES Community(ComId) ON DELETE CASCADE,
  FOREIGN KEY (UserId) REFERENCES Account(ID) ON DELETE CASCADE
);

CREATE TABLE Expenses (
    ExpenseId INT PRIMARY KEY IDENTITY(1,1),
    ComId INT NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NULL, 
    Amount DECIMAL(10,2) NOT NULL, 
    ExpenseDate DATE NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), 
    FOREIGN KEY (ComId) REFERENCES Community(ComId)
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
  Status NVARCHAR(20),
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
INSERT INTO Account (Email, Name, Password, PhoneNumber) VALUES 
('a', 'a', 'a', '0528182233'),
('b', 'b', 'b', '0501418822'),
('c@example.com', 'Charlie', 'pass123', '0500000003'),
('d@example.com', 'Dana', 'pass123', '0500000004'),
('e@example.com', 'Eli', 'pass123', '0500000005'),
('f@example.com', 'Farah', 'pass123', '0500000006'),
('g@example.com', 'Gil', 'pass123', '0500000007'),
('h@example.com', 'Hila', 'pass123', '0500000008'),
('i@example.com', 'Ilan', 'pass123', '0500000009'),
('j@example.com', 'Julia', 'pass123', '0500000010'),
('k@example.com', 'Kfir', 'pass123', '0500000011'),
('l@example.com', 'Lior', 'pass123', '0500000012');


INSERT INTO Community (ComName, ComDesc, ComCode, GatePhoneNum) 
VALUES 
  ('Kehila', 'Welcome', '123', '0528185522'),
  ('Kehila2', 'Hola! Glad to have you!', '456', '0508182244'),
  ('Kehila3', 'Shalom! Excited to have you back', '789', '0501812244');

INSERT INTO Members (UserId, ComId, [Role], Balance, UnitNum, IsLiable, IsResident, IsManager, IsProvider, IsApproved) 
VALUES 
  (1, 1, 'Manager', 100, 1, 1, 1, 1, 0, 1),
  (1, 2, 'Resident', 50, 7, 1, 1, 0, 0, 0),
  (1, 3, 'Resident', 0, 13, 0, 1, 0, 0, 0),
  (2, 1, 'Resident', 0, 13, 0, 1, 0, 0, 0),
  (3, 1, 'Resident', 20, 5, 0, 1, 0, 0, 0),
  (4, 1, 'Liable, Resident', 150, 6, 1, 1, 0, 0, 0),
  (5, 1, 'Resident', -10, 8, 0, 1, 0, 0, 1),
  (6, 1, 'Provider', 0, NULL, 0, 0, 0, 1, 0),
  (7, 1, 'Resident, Provider', 30, 9, 0, 1, 0, 1, 1),
  (8, 1, 'Manager, Liable, Resident', 300, 2, 1, 1, 1, 0, 0),
  (9, 1, 'Resident', 50, 10, 0, 1, 0, 0, 1),
  (10, 1, 'Liable, Resident', 80, 3, 1, 1, 0, 0, 1),
  (11, 1, 'Resident', 0, 11, 0, 1, 0, 0, 1),
  (12, 1, 'Resident, Provider', 70, 12, 0, 1, 0, 1, 1);

INSERT INTO Priority (PriorityNum, Priority) VALUES 
(0,'Low'),
(1, 'Normal');

INSERT INTO Status (StatNum, [Status]) VALUES 
(0,'No Status'),
(1, 'Normal');

INSERT INTO Report (UserId, ComId, ReportDesc, [Priority], [Status], IsAnon, Title)
VALUES 
  (1, 1, 'Encountered a mess in the trash room.', 1, 1, 0, 'Watch out!'),
  (1, 1, 'The elevator has been stuck for hours, causing inconvenience.', 1, 1, 1, 'Elevator Problem');

INSERT INTO Notices (UserId, ComId, Title, NoticeDesc, StartTime, EndTime)
VALUES
    (1, 1, 'Beware of power outage between 8 AM and 2 PM', 'During Monday, March 11th, there will be no power due to maintenance.', '2025-03-11 08:00:00', '2025-03-11 14:00:00'),
    (1, 1, 'Community Meeting Reminder', 'A community meeting will be held at the main hall on March 15th at 6 PM.', '2025-03-15 18:00:00', '2025-03-15 20:00:00'),
    (1, 1, 'Water Supply Interruption', 'Water supply will be temporarily unavailable on March 20th from 10 AM to 4 PM due to pipeline maintenance.', '2025-03-20 10:00:00', '2025-03-20 16:00:00'),
    (1, 1, 'Garbage Collection Delay', 'Garbage collection will be delayed by a day due to a public holiday. Please place bins out on Tuesday instead of Monday.', '2025-03-18 00:00:00', '2025-03-18 23:59:59'),
    (1, 1, 'Nothing', 'Blank.', NULL, NULL);

-- Insert into TenantRoom
INSERT INTO TenantRoom (ComId, Status, KeyHolderId, SessionStart, SessionEnd)
VALUES 
(1, 'Occupied', 1, '2025-04-21 08:00:00', '2025-04-21 20:00:00');

-- Insert into RoomRequests
INSERT INTO RoomRequests (UserId, ComId, StartTime, EndTime, Text, IsApproved)
VALUES
    (1, 1, '2025-05-20 22:00:00', '2025-05-21 00:00:00', 'a', 1), -- Starts 15 minutes before now, ends 2 hours after.
    (1, 1, '2025-05-22 10:00:00', '2025-05-22 12:00:00', 'b', 1),       -- Tomorrow
    (1, 1, '2025-05-23 18:00:00', '2025-05-23 20:00:00', 'c', 1),      -- The day after tomorrow
    (1, 1, '2025-05-24 18:00:00', '2025-05-25 20:00:00', 'd', 0),      -- The day after tomorrow
    (1, 1, '2025-05-24 18:00:00', '2025-05-26 20:00:00', 'e', 1),       -- The day after tomorrow
    (3, 1, '2025-06-24 18:00:00', '2025-06-26 20:00:00', 'f', 1);       -- The day after tomorrow



-- Select Data for Debugging
SELECT * FROM Account;
SELECT * FROM Members;
SELECT * FROM Community;
SELECT * FROM Report;
SELECT * FROM Notices;
SELECT * FROM RoomRequests;
SELECT * FROM Expenses;
SELECT * FROM Payments;




CREATE LOGIN [AdminLogin] WITH PASSWORD = 'ComPass';
Go

CREATE USER [Manager] FOR LOGIN [AdminLogin];
Go

ALTER ROLE db_owner ADD MEMBER [Manager]
Go

/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=CommunityDB;User ID=AdminLogin;Password=ComPass;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context CommunityDBContext -DataAnnotations –force
*/


