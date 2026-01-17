CREATE DATABASE DanaosTestDB;
GO

USE DanaosTestDB;
GO


-- 1. Create Student Table
-- Requirement: Field 'Id' as primary key and field 'Name' 
CREATE TABLE Student (
    Id INT IDENTITY(1,1), -- 'Identity' handles auto-incrementing
    Name NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_Student PRIMARY KEY (Id) -- Explicitly named
);

-- 2. Create Grades Table
-- Requirement: Student_Id, Course_Id, Course_Name, Grade 
-- Requirement: Student_Id and Course_Id as COMPOSITE KEY 
CREATE TABLE Grades (
    Student_Id INT NOT NULL,
    Course_Id INT NOT NULL,
    Course_Name NVARCHAR(100) NOT NULL,
    Grade DECIMAL(5, 2) NOT NULL, -- Decimal is best for grades (e.g. 85.50)
    
    -- Defining the Composite Primary Key
    CONSTRAINT PK_Grades PRIMARY KEY (Student_Id, Course_Id),
    
    -- Best Practice: Foreign Key to link to Student table
    CONSTRAINT FK_Grades_Student FOREIGN KEY (Student_Id) REFERENCES Student(Id)
);

-- 3. Fill the tables with test data 
INSERT INTO Student (Name) VALUES 
('Carlo Bactol'), 
('Maria Clara'), 
('Jose Rizal');

INSERT INTO Grades (Student_Id, Course_Id, Course_Name, Grade) VALUES 
(1, 101, 'Mathematics', 88.5),
(1, 102, 'Science', 92.0),
(1, 103, 'History', 85.0),
(2, 101, 'Mathematics', 95.0),
(2, 102, 'Science', 89.5),
(3, 101, 'Mathematics', 98.0),
(3, 102, 'Science', 91.0);

-- Check the data
SELECT * FROM Student;
SELECT * FROM Grades;