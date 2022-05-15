
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/15/2022 13:41:04
-- Generated from EDMX file: D:\progs\ziganshinakusch\ziganshinakusch\Model\DbModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [C:\USERS\SABIROV\DOCUMENTS\NEWDBSABSAB.MDF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserBucket]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Buckets] DROP CONSTRAINT [FK_UserBucket];
GO
IF OBJECT_ID(N'[dbo].[FK_BucketGood]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GoodSet] DROP CONSTRAINT [FK_BucketGood];
GO
IF OBJECT_ID(N'[dbo].[FK_BucketOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderSet] DROP CONSTRAINT [FK_BucketOrder];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[GoodSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GoodSet];
GO
IF OBJECT_ID(N'[dbo].[Buckets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Buckets];
GO
IF OBJECT_ID(N'[dbo].[OrderSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NULL,
    [CardNumber] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [FullName] nvarchar(max)  NULL
);
GO

-- Creating table 'GoodSet'
CREATE TABLE [dbo].[GoodSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Info] nvarchar(max)  NULL,
    [Price] bigint  NOT NULL,
    [Type] nvarchar(max)  NULL,
    [BucketId] int  NULL
);
GO

-- Creating table 'Buckets'
CREATE TABLE [dbo].[Buckets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TotalPrice] bigint  NOT NULL,
    [Count] bigint  NOT NULL,
    [User_Id] int  NOT NULL
);
GO

-- Creating table 'OrderSet'
CREATE TABLE [dbo].[OrderSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] nvarchar(max)  NOT NULL,
    [BucketId] int  NULL,
    [Items] nvarchar(max)  NOT NULL,
    [BucketId1] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GoodSet'
ALTER TABLE [dbo].[GoodSet]
ADD CONSTRAINT [PK_GoodSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Buckets'
ALTER TABLE [dbo].[Buckets]
ADD CONSTRAINT [PK_Buckets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderSet'
ALTER TABLE [dbo].[OrderSet]
ADD CONSTRAINT [PK_OrderSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'Buckets'
ALTER TABLE [dbo].[Buckets]
ADD CONSTRAINT [FK_UserBucket]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserBucket'
CREATE INDEX [IX_FK_UserBucket]
ON [dbo].[Buckets]
    ([User_Id]);
GO

-- Creating foreign key on [BucketId] in table 'GoodSet'
ALTER TABLE [dbo].[GoodSet]
ADD CONSTRAINT [FK_BucketGood]
    FOREIGN KEY ([BucketId])
    REFERENCES [dbo].[Buckets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BucketGood'
CREATE INDEX [IX_FK_BucketGood]
ON [dbo].[GoodSet]
    ([BucketId]);
GO

-- Creating foreign key on [BucketId1] in table 'OrderSet'
ALTER TABLE [dbo].[OrderSet]
ADD CONSTRAINT [FK_BucketOrder]
    FOREIGN KEY ([BucketId1])
    REFERENCES [dbo].[Buckets]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BucketOrder'
CREATE INDEX [IX_FK_BucketOrder]
ON [dbo].[OrderSet]
    ([BucketId1]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------