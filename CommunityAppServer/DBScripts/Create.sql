﻿USE master;
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
  CreatedAt DATETIME DEFAULT GETDATE(),
  ProfilePhotoFileName NVARCHAR(255) NULL
);


CREATE TABLE Community (
  ComId INT IDENTITY(1,1) PRIMARY KEY,
  ComName NVARCHAR(50) NOT NULL,
  ComDesc NVARCHAR(MAX), 
  ComCode NVARCHAR(50) UNIQUE NOT NULL,
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



CREATE TABLE Report (
  ReportId INT IDENTITY(1,1) PRIMARY KEY,
  UserId INT NOT NULL,
  ComId INT NOT NULL,
  Title NVARCHAR(255) NOT NULL DEFAULT '',
  ReportDesc NVARCHAR(MAX),
  CreatedAt DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (UserId) REFERENCES Account(ID) ON DELETE CASCADE,
  FOREIGN KEY (ComId) REFERENCES Community(ComId) ON DELETE CASCADE
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




-- Insert Initial Data
INSERT INTO Account (Email, Name, Password, PhoneNumber) VALUES 
('manager@m.com', 'manager', 'manager1', '0539192233'),
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


INSERT INTO Report (UserId, ComId, ReportDesc, Title)
VALUES 
  (1, 1, 'Encountered a mess in the trash room.', 'Watch out!'),
  (1, 1, 'The elevator has been stuck for hours, causing inconvenience.', 'Elevator Problem');

INSERT INTO Notices (UserId, ComId, Title, NoticeDesc, StartTime, EndTime)
VALUES
    (1, 1, 'Beware of power outage between 8 AM and 2 PM', 'During Monday, March 11th, there will be no power due to maintenance.', '2025-03-11 08:00:00', '2025-03-11 14:00:00'),
    (1, 1, 'Community Meeting Reminder', 'A community meeting will be held at the main hall on March 15th at 6 PM.', '2025-03-15 18:00:00', '2025-03-15 20:00:00'),
    (1, 1, 'Water Supply Interruption', 'Water supply will be temporarily unavailable on March 20th from 10 AM to 4 PM due to pipeline maintenance.', '2025-03-20 10:00:00', '2025-03-20 16:00:00'),
    (1, 1, 'Garbage Collection Delay', 'Garbage collection will be delayed by a day due to a public holiday. Please place bins out on Tuesday instead of Monday.', '2025-03-18 00:00:00', '2025-03-18 23:59:59'),
    (1, 1, 'Nothing', 'Blank.', NULL, NULL);

    INSERT INTO Expenses (ComId, Title, Description, Amount, ExpenseDate)
VALUES 
(1, N'Electricity Bill', N'Monthly invoice for building electricity usage', 1580.75, '2025-05-01'),

(1, N'Cleaning Services', N'Weekly cleaning of public areas for April', 1200.00, '2025-04-30'),

(1, N'Elevator Maintenance', N'Regular maintenance and inspection of the elevator', 780.00, '2025-04-15'),

(1, N'Pipe Repair', N'Leak repair in the main pipeline on the 2nd floor', 450.50, '2025-03-27'),

(1, N'LED Lighting Installation', N'Replacement of lighting fixtures on floors 1–3 with energy-efficient LEDs', 2100.00, '2025-03-10'),

(1, N'Gardening Services', N'Monthly garden maintenance – trimming and watering', 900.00, '2025-04-05'),

(1, N'Building Insurance (Annual)', N'Renewal of full-coverage building insurance including liability', 3650.00, '2025-01-10');


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


--alter login [AdminLogin] WITH PASSWORD = 'ComPass';

CREATE LOGIN [AdminLogin] WITH PASSWORD = 'ComPass';
Go

CREATE USER [Manager] FOR LOGIN [AdminLogin];
Go

ALTER ROLE db_owner ADD MEMBER [Manager]
Go

/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=CommunityDB;User ID=AdminLogin;Password=ComPass;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context CommunityDBContext -DataAnnotations –force
*/


