
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/10/2022 01:19:44
-- Generated from EDMX file: C:\Users\user\Downloads\銘傳二手書網站20221109\銘傳二手書網站\slnShoppingCar\prjShoppingCar\Models\dbShoppingCarModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [D:\DBSHOPPINGCAR.MDF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[tMember]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tMember];
GO
IF OBJECT_ID(N'[dbo].[tOrder]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tOrder];
GO
IF OBJECT_ID(N'[dbo].[tOrderDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tOrderDetail];
GO
IF OBJECT_ID(N'[dbo].[tProduct]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tProduct];
GO
IF OBJECT_ID(N'[dbo].[tFaculty]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tFaculty];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'tMember'
CREATE TABLE [dbo].[tMember] (
    [fId] int IDENTITY(1,1) NOT NULL,
    [fUserId] nvarchar(50)  NULL,
    [fPwd] nvarchar(50)  NULL,
    [fName] nvarchar(50)  NULL,
    [fEmail] nvarchar(50)  NULL
);
GO

-- Creating table 'tOrder'
CREATE TABLE [dbo].[tOrder] (
    [fId] int IDENTITY(1,1) NOT NULL,
    [fOrderGuid] nvarchar(50)  NULL,
    [fUserId] nvarchar(50)  NULL,
    [fReceiver] nvarchar(50)  NULL,
    [fEmail] nvarchar(50)  NULL,
    [fAddress] nvarchar(50)  NULL,
    [fDate] datetime  NULL,
    [fPid] nvarchar(max)  NULL,
    [fEnd] nchar(10)  NULL
);
GO

-- Creating table 'tOrderDetail'
CREATE TABLE [dbo].[tOrderDetail] (
    [fId] int IDENTITY(1,1) NOT NULL,
    [fOrderGuid] nvarchar(50)  NULL,
    [fUserId] nvarchar(50)  NULL,
    [fPId] nvarchar(max)  NULL,
    [fName] nvarchar(max)  NULL,
    [fPrice] int  NULL,
    [fQty] int  NULL,
    [fIsApproved] nvarchar(10)  NULL,
    [fcid] nvarchar(max)  NULL,
    [flineID] nvarchar(max)  NULL
);
GO

-- Creating table 'tProduct'
CREATE TABLE [dbo].[tProduct] (
    [fId] int IDENTITY(1,1) NOT NULL,
    [fPId] nvarchar(max)  NULL,
    [fName] nvarchar(max)  NULL,
    [fPrice] int  NULL,
    [fImg] nvarchar(max)  NULL,
    [ffaculty] nvarchar(50)  NULL,
    [fdepartment] nvarchar(50)  NULL,
    [fImages] varbinary(max)  NULL,
    [fstate] int  NULL,
    [fcId] nvarchar(max)  NULL,
    [fEmail] nvarchar(50)  NULL,
    [fsgtxt] nvarchar(max)  NULL,
    [fisbn] nvarchar(50)  NULL,
    [fclass] nvarchar(50)  NULL,
    [fPrice_s] int  NULL,
    [fUId] nvarchar(50)  NULL,
    [fsell] nchar(10)  NULL,
    [fLine] nvarchar(50)  NULL
);
GO

-- Creating table 'tFaculty'
CREATE TABLE [dbo].[tFaculty] (
    [Id] int  NOT NULL,
    [faculty] nvarchar(50)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [fId] in table 'tMember'
ALTER TABLE [dbo].[tMember]
ADD CONSTRAINT [PK_tMember]
    PRIMARY KEY CLUSTERED ([fId] ASC);
GO

-- Creating primary key on [fId] in table 'tOrder'
ALTER TABLE [dbo].[tOrder]
ADD CONSTRAINT [PK_tOrder]
    PRIMARY KEY CLUSTERED ([fId] ASC);
GO

-- Creating primary key on [fId] in table 'tOrderDetail'
ALTER TABLE [dbo].[tOrderDetail]
ADD CONSTRAINT [PK_tOrderDetail]
    PRIMARY KEY CLUSTERED ([fId] ASC);
GO

-- Creating primary key on [fId] in table 'tProduct'
ALTER TABLE [dbo].[tProduct]
ADD CONSTRAINT [PK_tProduct]
    PRIMARY KEY CLUSTERED ([fId] ASC);
GO

-- Creating primary key on [Id] in table 'tFaculty'
ALTER TABLE [dbo].[tFaculty]
ADD CONSTRAINT [PK_tFaculty]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------