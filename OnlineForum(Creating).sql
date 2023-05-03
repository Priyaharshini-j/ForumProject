CREATE DATABASE OnlineForum;

--Creating a table with user login credentials
CREATE TABLE Users(
Id int identity(1,1) PRIMARY KEY,
Name VARCHAR(30),
Email VARCHAR(90) UNIQUE,
Password VARCHAR(50) NOT NULL,
SecurityQn VARCHAR(MAX),
SecurityAns VARCHAR(225));


--Creating a table with Discussion Forum
CREATE TABLE Forum(
ForumId INT IDENTITY(1,1) PRIMARY KEY,
--Id means UserId 
Email VARCHAR(90) NOT NULL,
Category NVARCHAR(70) NOT NULL,
Title NVARCHAR(125) UNIQUE NOT NULL,
Content NVARCHAR(MAX) NOT NULL,
CreatedDate DATETIME DEFAULT SYSDATETIME() NOT NULL,
CONSTRAINT fk_Email FOREIGN KEY (Email) REFERENCES Users(Email) );

DROP TABLE Replies

--Creating a tale for replies
CREATE TABLE Replies(
ReplyId INT IDENTITY(1,1) PRIMARY KEY,
ForumId INT NOT NULL,
Email VARCHAR(90) NOT NULL,
ReplyContent NVARCHAR(MAX) NOT NULL,
ReplyCreated DATETIME DEFAULT SYSDATETIME() NOT NULL,
CONSTRAINT fk_UserEmail FOREIGN KEY (Email) REFERENCES Users(Email),
CONSTRAINT fk_ForumId FOREIGN KEY (ForumId) REFERENCES Forum(ForumId));

--Creating a table for Polls
CREATE TABLE Polls(
PollId INT IDENTITY(1,1) PRIMARY KEY,
--Id means UserId 
Id INT NOT NULL,
Category NVARCHAR(70) NOT NULL,
PollTitle NVARCHAR(125) UNIQUE NOT NULL,
CreatedDate DATETIME DEFAULT SYSDATETIME() NOT NULL,
CONSTRAINT fk_Id_For_Polls FOREIGN KEY (Id) REFERENCES Users(Id) );

--Creating a table PollResult
CREATE TABLE PollResult(
PollRes_Id INT IDENTITY(1,1) PRIMARY KEY,
PollId INT NOT NULL,
UserId INT NOT NULL,
VoteOptions NVARCHAR(200) NOT NULL,
VoteDate DATETIME DEFAULT SYSDATETIME() NOT NULL,
CONSTRAINT fk_pollRes_id FOREIGN KEY (UserId) REFERENCES Users(Id),
CONSTRAINT fk_PollId FOREIGN KEY (PollId) REFERENCES Polls(PollId)
);

 CREATE TABLE Memory(
 Memory_Id INT IDENTITY(1,1) PRIMARY KEY,
 UserId INT NOT NULL,
 Title NVARCHAR(200) NOT NULL,
 Description NVARCHAR(MAX) NOT NULL,
 MemoryDate DATETIME DEFAULT SYSDATETIME() NOT NULL,
 CONSTRAINT fk_mem_USERID FOREIGN KEY(UserId) REFERENCES Users(Id));


 SELECT * FROM Users;
 SELECT * FROM Forum;
 SELECT * FROM Replies;
 SELECT * FROM PollResult;
 SELECT * FROM Polls;
 SELECT * FROM Memory;

 --Stored procedure for inserting the user
 CREATE OR ALTER PROCEDURE InsertUser
 @Name VarcHar(30),
 @Email VARCHAR(90),
 @Password VARCHAR(50),
 @SecurityQn VARCHAR(MAX),
 @SecurityAns VARCHAR(225)
 AS
 BEGIN
 INSERT INTO Users
 VALUES (@Name,@Email,@Password,@SecurityQn,@SecurityAns)
 END

 CREATE OR ALTER PROCEDURE FetchAllForum
 AS
 BEGIN
 SELECT * FROM Forum
 END

CREATE OR ALTER PROCEDURE InsertForum
    @Email VARCHAR(70),
    @Category VARCHAR(70),
    @Title VARCHAR(125),
    @Content VARCHAR(MAX),
    @CreatedDate DATETIME
AS
BEGIN
    
    INSERT INTO Forum 
    VALUES (@Email, @Category, @Title, @Content, @CreatedDate)
END

CREATE OR ALTER PROCEDURE ForumById
@Email VARCHAR(90)
AS
BEGIN 
SELECT * from Forum
WHERE Email=@Email
END

CREATE OR ALTER PROCEDURE EditUser
@Id int,
@Name VarcHar(30),
@Email VARCHAR(90),
@Password VARCHAR(50),
@SecurityQn VARCHAR(MAX),
@SecurityAns VARCHAR(225)
AS
BEGIN
UPDATE Users SET Name= '@Name', Email='@Email', Password= '@Password' , SecurityQn ='@SecurityQn', SecurityAns='@SecurityAns' WHERE Id=@Id;
END

CREATE OR ALTER PROCEDURE DeleteUser
@Id int
AS
BEGIN
DELETE FROM Users WHERE Id=@Id;
Delete FROM Forum WHERE Id= @Id;
DELETE FROM Replies WHERE Id=@Id;
DELETE FROM PollResult WHERE UserId=@Id;
DELETE FROM Polls WHERE Id=@Id;
DELETE FROM Memory WHERE UserId = @Id;
END


CREATE OR ALTER PROCEDURE GetUserById 
@Id INT
AS
SELECT * FROM Users WHERE Id=@Id;

CREATE OR ALTER PROCEDURE AppendReplies
@ForumId int,
@Email VARCHAR(90),
@ReplyContent VARCHAR(MAX),
@replyDate DATETIME
AS
BEGIN
INSERT INTO Replies VALUES(@ForumId,@Email,@ReplyContent,@replyDate)
END

CREATE OR ALTER PROCEDURE GetUserId
@Email VARCHAR(90)
AS
BEGIN
SELECT Id From Users WHERE Email=@Email;
END

CREATE OR ALTER PROCEDURE ForumSearch
@searchString VARCHAR(MAX)
AS
SELECT * FROM Forum WHERE Email LIKE '%@searchString%' OR
Category LIKE '%@searchString%' OR
Title LIKE '%@searchString%' OR
Content LIKE '%@searchString%'

EXEC ForumSearch ;






