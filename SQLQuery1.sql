CREATE TABLE Logins (
    LoginID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeID INT UNIQUE NOT NULL, -- Ensures one login per employee
    Username NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL, -- Store hashed passwords, NOT plaintext
    LastLogin DATETIME NULL,
    FailedAttempts INT DEFAULT 0,
    IsLocked BIT DEFAULT 0, -- 0 = Active, 1 = Locked
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Logins_Employees FOREIGN KEY (EmployeeID)
    REFERENCES Employees(EmployeeID) ON DELETE CASCADE
);
GO
