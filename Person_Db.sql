Create database Person_Db
GO

USE Person_Db
GO

Create table Person
(
PersonId int primary key identity,
[Name] Varchar(50) not null,
DateOfBirth Date not null
)
go

insert into Person values
('Person1','2000-01-01')
go

select * from Person
go



--=================Function=================
CREATE FUNCTION fnCalculateAge
(
  @DOB DATE
)
RETURNS INT
AS
BEGIN
  DECLARE @AGE INT
  SET @AGE = DATEDIFF(YEAR, @DOB, GETDATE())-
  CASE
    WHEN (MONTH(@DOB) > MONTH(GETDATE())) OR
       (MONTH(@DOB) = MONTH(GETDATE()) AND
        DAY(@DOB) > DAY(GETDATE()))
    THEN 1
    ELSE 0
  END
  RETURN @AGE
END
GO

SELECT PersonId, Name, DateOfBirth, dbo.fnCalculateAge(DateOfBirth) AS Age 
FROM Person
GO

--===========Stored Procedure===================

-- Procedure for GetAllPerson
CREATE PROCEDURE spGetAllPerson
AS
BEGIN
    SELECT * FROM Person
END
GO


-- Procedure for GetAllPerson
CREATE PROCEDURE spGetAllPersonwithAge
AS
BEGIN
    SELECT PersonId, Name, DateOfBirth, dbo.fnCalculateAge(DateOfBirth) AS Age FROM Person
END
GO

-- Procedure for GetPersonById
CREATE PROCEDURE spGetPersonById
    @PersonId INT
AS
BEGIN
    SELECT * FROM Person WHERE PersonId = @PersonId
END
GO

-- Procedure for Insert
CREATE PROCEDURE spCreatePerson
    @Name NVARCHAR(50),
    @DateOfBirth Date
AS
BEGIN
    INSERT INTO Person(Name, DateOfBirth)
    VALUES (@Name, @DateOfBirth)
END
GO

-- Procedure for Update
CREATE PROCEDURE spUpdatePerson
    @PersonId INT,
    @Name NVARCHAR(50),
    @DateOfBirth Date
AS
BEGIN
    UPDATE Person
    SET Name = @Name, DateOfBirth = @DateOfBirth
    WHERE PersonId = @PersonId
END
GO

-- Procedure for Delete
CREATE PROCEDURE spDeletePerson
    @PersonId INT
AS
BEGIN
    DELETE FROM Person WHERE PersonId = @PersonId
END
GO

--BackupTable
Create table tblBackup
(
PersonId int primary key,
[Name] Varchar(50) not null,
DateOfBirth Date not null
)
go
select * from tblBackup
go

--Trigger
CREATE TRIGGER trg_AfterDelete
ON Person
FOR DELETE
AS
BEGIN
    INSERT INTO tblBackup
    SELECT * FROM deleted
END
GO

-- SP get all from backup table
CREATE PROCEDURE spGetAllBackup
AS
BEGIN
    SELECT * FROM tblBackup
END
GO


--Create PersonAudit table for capture info:
CREATE TABLE PersonAudit
(
  Id int identity(1,1) primary key,
  AuditData nvarchar(1000)
)
GO

--Example for AFTER TRIGGER for INSERT event on Person table:
CREATE TRIGGER tr_Person_ForInsert
ON Person
FOR INSERT
AS
BEGIN
 Declare @Id int
 Select @Id = PersonId from inserted
 
 insert into PersonAudit 
 values('New Person with Id  = ' + Cast(@Id as nvarchar(5)) + ' is added at ' + cast(Getdate() as nvarchar(20)))
END
GO

--Example for AFTER TRIGGER for DELETE event on Person table:
CREATE TRIGGER tr_Person_ForDelete
ON Person
FOR DELETE
AS
BEGIN
 Declare @Id int
 Select @Id = PersonId from deleted
 
 insert into PersonAudit 
 values('An existing Person with Id  = ' + Cast(@Id as nvarchar(5)) + ' is deleted at ' + Cast(Getdate() as nvarchar(20)))
END
GO

--Create AFTER UPDATE trigger for UPDATE event on Person table:
CREATE TRIGGER tr_player_ForUpdate
on Person
for Update
as
Begin
 Declare @Id int
 Select @Id = PersonId from inserted
 
 insert into PersonAudit 
 values('An existing Person with Id  = ' + Cast(@Id as nvarchar(5)) + ' is updated at ' + Cast(Getdate() as nvarchar(20)))
End
GO

select * from PersonAudit
go

-- Procedure for GetAllPersonAudit
CREATE PROCEDURE spGetAllPersonAudit
AS
BEGIN
    SELECT * FROM PersonAudit
END
GO