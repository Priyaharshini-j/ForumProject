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
CREATE TABLE UserVoted(
voteId INT IDENTITY(1,1) PRIMARY KEY,
Email VARCHAR(90) NOT NULL,
PollId INT NOT NULL,
OptionId INT NOT NULL,
FOREIGN KEY(PollId) REFERENCES UserPoll(PollId),
FOREIGN KEY(Email) REFERENCES Users(Email)
)
Drop Table Memory;
 CREATE TABLE Memory(
 Memory_Id INT IDENTITY(1,1) PRIMARY KEY,
 Email VARCHAR(90) NOT NULL,
 Title NVARCHAR(200) NOT NULL,
 Description NVARCHAR(MAX) NOT NULL,
 MemoryDate DATETIME DEFAULT SYSDATETIME() NOT NULL,
 CONSTRAINT fk_mem_Email FOREIGN KEY(Email) REFERENCES Users(Email));

 CREATE TABLE UserPoll (
    
    PollId INT PRIMARY KEY IDENTITY(1,1),
    Email VARCHAR(90) NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Category NVARCHAR(255) NOT NULL,
    Question NVARCHAR(MAX) NOT NULL,
    Created DATETIME2 NOT NULL DEFAULT(GETDATE())
    FOREIGN KEY(Email) REFERENCES Users(Email)
);

CREATE TABLE PollOption (
    VoteId INT PRIMARY KEY IDENTITY(1,1),
    UserPollId INT NOT NULL,
    OptionId INT NOT NULL,
    OptionText NVARCHAR(255) NOT NULL,
    VoteCount INT ,
    VotePercentage DOUBLE PRECISION,
    FOREIGN KEY (UserPollId) REFERENCES UserPoll(PollId)
);

UPDATE PollOption SET VotePercentage=0, VoteCount=0
DElete from UserVoted

 SELECT * FROM Users;
 SELECT * FROM Forum;
 SELECT * FROM Replies;
 SELECT * FROM UserPoll;
 SELECT * FROM PollOption;
 SELECT * FROM Memory;
 SELECT * FROM UserVoted;
 DELETE FROM UserVoted WHERE voteId=8 OR voteId=9
 --Stored procedure for inserting the user
 CREATE OR ALTER PROCEDURE InsertUser
 @Name VarcHar(30),
 @Email VARCHAR(90),
 @Password VARCHAR(50),
 @SecurityQn VARCHAR(MAX),
 @SecurityAns VARCHAR(225)
 AS
 BEGIN
 INSERT INTO Users VALUES (@Name,@Email,@Password,@SecurityQn,@SecurityAns)
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
@Password VARCHAR(50),
@SecurityQn VARCHAR(MAX),
@SecurityAns VARCHAR(225)
AS
BEGIN
    UPDATE Users SET Name= @Name,  Password= @Password , SecurityQn =@SecurityQn, SecurityAns=@SecurityAns WHERE Id=@Id;
END


CREATE OR ALTER PROCEDURE DeleteUser
@Id int
AS
BEGIN
    DECLARE @Old_Email VARCHAR(90)
    DECLARE @PollCreatedByUser INT
    SELECT @Old_Email=Email FROM Users WHERE Id=@Id;
    SELECT @PollCreatedByUser= PollId  FROM UserPoll WHERE Email=@Old_Email;
    
    DELETE FROM Replies WHERE Email=@Old_Email;
    DELETE FROM Forum WHERE Email=@Old_Email;
    DELETE FROM UserVoted WHERE Email=@Old_Email;
    DELETE FROM PollOption WHERE UserPollId IN (SELECT PollId FROM UserPoll WHERE Email=@Old_Email);
    DELETE FROM UserPoll WHERE Email=@Old_Email;
    DELETE FROM Memory WHERE Email=@Old_Email;
    DELETE FROM Users WHERE Email=@Old_Email;
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

CREATE OR ALTER PROCEDURE CreatePoll 
@Email VARCHAR(90),
@Title VARCHAR(255),
@Category VARCHAR(255),
@Question VARCHAR(MAX),
@Created Datetime,
@PollId INT OUTPUT
AS
BEGIN
    INSERT INTO UserPoll VALUES(@Email,@Title,@Category,@Question,@Created);
    SET @PollId = SCOPE_IDENTITY();
    RETURN;
END

CREATE OR ALTER PROCEDURE CreateOptions
@PollId Int,
@OptionId Int,
@Option VARCHAR(255)
AS
INSERT INTO PollOption VALUES (@PollId,@OptionId,@Option,0,0.00)


CREATE OR ALTER PROCEDURE FetchAllPolls
AS
SELECT * FROM UserPoll;

CREATE OR ALTER PROCEDURE FetchPollOptions
@PollId int
AS
Select * FROM PollOption WHERE UserPollId = @PollId

EXEC FetchPollOptions 1;


CREATE OR ALTER PROCEDURE FetchPollsAndOptions
AS
BEGIN
    SELECT 
        up.PollId,
        up.Email,
        up.Title,
        up.Category,
        up.Question,
        up.Created,
        po.OptionId,
        po.OptionText,
        po.VotePercentage
    FROM 
        UserPoll up 
    LEFT JOIN 
        PollOption po ON up.PollId = po.UserPollId;
END



EXEC FetchPollsAndOptions;




CREATE OR ALTER PROCEDURE DeleteForum
@ForumId int
AS
BEGIN
DELETE FROM Replies WHERE ForumId = @ForumId;
DELETE FROM Forum WHere ForumId=@ForumId;
END


CREATE OR ALTER PROCEDURE EditForum
@Category VARCHAR(255),
@Title VARCHAR(125),
@Content VARCHAR(MAX),
@ForumCreated DATETIME,
@ForumId INT
AS
UPDATE Forum SET Category=@Category,Title=@Title,Content=@Content,CreatedDate=@ForumCreated WHERE ForumId=@ForumId;



CREATE OR ALTER PROCEDURE FetchPoll
@Email VARCHAR(90)
AS
BEGIN
    SELECT 
        up.PollId,
        up.Email,
        up.Title,
        up.Category,
        up.Question,
        up.Created,
        po.OptionText
    FROM 
        UserPoll up 
    LEFT JOIN 
        PollOption po ON up.PollId = po.UserPollId and up.Email=@Email;
END

EXEC FetchPoll 'jawahars1966@gmai.com'



CREATE OR ALTER PROCEDURE UpdateVote
@PollId INT,
@OptionId INT
AS
BEGIN
UPDATE PollOption SET VoteCount=VoteCount+1 WHERE OptionId=@OptionId AND UserPollId=@PollId;
UPDATE PollOption SET VotePercentage=100.0 * VoteCount / (SELECT SUM(VoteCount) FROM PollOption WHERE UserPollId = @PollId ) WHERE UserPollId = @PollId 
END


EXEC UpdateVote 5, 1;


CREATE OR ALTER PROCEDURE GetPoll
@PollId int
AS
BEGIN
    SELECT 
        up.PollId,
        up.Email,
        up.Title,
        up.Category,
        up.Question,
        up.Created,
        po.OptionText
    FROM 
        UserPoll up     
    LEFT JOIN 
        PollOption po ON up.PollId = po.UserPollId AND up.PollId=@PollId ;
END

CREATE OR ALTER PROCEDURE DeletePoll
@PollId int
AS
BEGIN
DELETE FROM UserVoted WHERE PollId=@PollId;
DELETE FROM PollOption WHERE UserPollId=@PollId;
DELETE FROM UserPoll WHERE PollID=@PollId;
END