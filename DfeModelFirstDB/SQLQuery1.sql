USE [aspnet-DfeDemo-UserData]

CREATE TABLE [User]
(
	Id nvarchar(450) PRIMARY KEY CLUSTERED,
    Firstname nvarchar(50),
    Surname nvarchar(50),
	Email nvarchar(80)
);

CREATE TABLE LAData
(
	Id int PRIMARY KEY CLUSTERED IDENTITY(1,1),
	[Name] nvarchar(50)
)

CREATE TABLE UserCase
(
	CaseID int PRIMARY KEY CLUSTERED IDENTITY(1,1),
    Firstname nvarchar(50),
   Surname nvarchar(50),
    DateOfBirth date,
    LAId int,
    CaseStatus bit,
    Bullying bit,
    Absence bit,
    Other bit,
    UserID nvarchar(450)
)