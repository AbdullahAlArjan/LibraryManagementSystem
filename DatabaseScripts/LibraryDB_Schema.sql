-- 1️⃣ Create the Database
DROP DATABASE IF EXISTS LibraryDB;
GO

CREATE DATABASE LibraryDB;
GO

USE LibraryDB;
GO

-- ===================================================
-- 2️⃣ USERS TABLE
-- ===================================================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Librarian'))
);

-- Sample Data
INSERT INTO Users (Username, Password, Role) VALUES
('admin', '1234', 'Admin'),
('librarian1', 'abcd', 'Librarian');
GO

-- ===================================================
-- 3️⃣ MEMBERS TABLE
-- ===================================================
CREATE TABLE Members (
    MemberID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    JoinDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);

-- Sample Data
INSERT INTO Members (FullName, Email, Phone, JoinDate) VALUES
('Sara Ali', 'sara@example.com', '0791234567', '2025-01-10'),
('Omar Hasan', 'omarh@example.com', '0782345678', '2025-03-22'),
('Lina Mahmoud', 'lina.m@example.com', '0773456789', '2025-04-15'),
('Hassan Youssef', 'hassan.y@example.com', '0799876543', '2025-06-03');
GO

-- ===================================================
-- 4️⃣ BOOKS TABLE (UPDATED WITH YOUR FIELDS)
-- ===================================================
CREATE TABLE Books (
    BookID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    Author NVARCHAR(100),
    ISBN NVARCHAR(30) UNIQUE,
    PublicationDate DATE NULL,
    Publisher NVARCHAR(100),
    Category NVARCHAR(50),
    Quantity INT CHECK (Quantity >= 0),
    AvailableQuantity INT CHECK (AvailableQuantity >= 0),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Sample Data
INSERT INTO Books (Title, Author, ISBN, PublicationDate, Publisher, Category, Quantity, AvailableQuantity) VALUES
('The Great Gatsby', 'F. Scott Fitzgerald', '9780743273565', '1925-04-10', 'Charles Scribner’s Sons', 'Fiction', 5, 5),
('To Kill a Mockingbird', 'Harper Lee', '9780061120084', '1960-07-11', 'J. B. Lippincott & Co.', 'Fiction', 3, 3),
('A Brief History of Time', 'Stephen Hawking', '9780553380163', '1988-04-01', 'Bantam Books', 'Science', 4, 4),
('Clean Code', 'Robert C. Martin', '9780132350884', '2008-08-11', 'Prentice Hall', 'Programming', 6, 5),
('The Art of War', 'Sun Tzu', '9781599869773', '1910-01-01', 'Project Gutenberg', 'Philosophy', 8, 7),
('The Pragmatic Programmer', 'Andrew Hunt', '9780201616224', '1999-10-20', 'Addison-Wesley', 'Programming', 5, 5),
('1984', 'George Orwell', '9780451524935', '1949-06-08', 'Secker & Warburg', 'Fiction', 4, 3),
('Thinking, Fast and Slow', 'Daniel Kahneman', '9780374533557', '2011-10-25', 'Farrar, Straus and Giroux', 'Psychology', 3, 3),
('The Lean Startup', 'Eric Ries', '9780307887894', '2011-09-13', 'Crown Business', 'Business', 5, 5),
('Sapiens: A Brief History of Humankind', 'Yuval Noah Harari', '9780062316110', '2014-09-04', 'Harper', 'History', 4, 4);
GO

-- ===================================================
-- 5️⃣ LOANS TABLE
-- ===================================================
CREATE TABLE Loans (
    LoanID INT IDENTITY(1,1) PRIMARY KEY,
    MemberID INT NOT NULL FOREIGN KEY REFERENCES Members(MemberID),
    BookID INT NOT NULL FOREIGN KEY REFERENCES Books(BookID),
    BorrowDate DATETIME DEFAULT GETDATE(),
    DueDate DATETIME NOT NULL,
    ReturnDate DATETIME NULL,
    FineAmount DECIMAL(8,2) DEFAULT 0
);

-- Sample Data
INSERT INTO Loans (MemberID, BookID, BorrowDate, DueDate, ReturnDate, FineAmount)
VALUES
(1, 1, '2025-11-01', '2025-11-08', NULL, 0),    -- Sara borrowed "The Great Gatsby"
(2, 3, '2025-10-25', '2025-11-02', NULL, 0),    -- Omar borrowed "A Brief History of Time"
(3, 4, '2025-10-20', '2025-10-27', '2025-10-28', 2.50), -- Lina returned late
(4, 5, '2025-11-03', '2025-11-10', NULL, 0);    -- Hassan borrowed "The Art of War"
GO
