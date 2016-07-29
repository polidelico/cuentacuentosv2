CREATE TABLE dbo.Users (
    UserName                nvarchar(100)  NOT NULL,
    PasswordHash            binary(64)     NOT NULL,
    PasswordSalt            binary(128)    NOT NULL,
    Email                   nvarchar(max)  NOT NULL,
    Comment                 nvarchar(max)  NULL,
    IsApproved              bit            NOT NULL DEFAULT 0,
    DateCreated             datetime       NOT NULL,
    DateLastLogin           datetime       NULL,
    DateLastActivity        datetime       NULL,
    DateLastPasswordChange  datetime       NOT NULL,
    CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (UserName)
)

CREATE TABLE dbo.Roles (
    RoleName                nvarchar(100)  NOT NULL,
    CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED (RoleName)
)
CREATE TABLE dbo.RoleMemberships (
    UserName                nvarchar(100)  NOT NULL,
    RoleName                nvarchar(100)  NOT NULL,
    CONSTRAINT PK_RoleMemberships PRIMARY KEY CLUSTERED (UserName, RoleName),
    CONSTRAINT FK_RoleMemberships_Roles 
        FOREIGN KEY (RoleName) REFERENCES dbo.Roles (RoleName) 
        ON UPDATE CASCADE ON DELETE CASCADE,
)

ALTER TABLE dbo.RoleMemberships 
    ADD CONSTRAINT FK_RoleMemberships_Users 
    FOREIGN KEY (UserName) REFERENCES dbo.Users (UserName) 
    ON UPDATE CASCADE ON DELETE CASCADE