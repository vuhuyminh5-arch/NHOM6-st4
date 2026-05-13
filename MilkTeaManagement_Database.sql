USE [master]
GO

IF DB_ID(N'MilkTeaManagement') IS NULL
BEGIN
    CREATE DATABASE [MilkTeaManagement]
END
GO

USE [MilkTeaManagement]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID(N'[dbo].[OrderDetails]', N'U') IS NOT NULL DROP TABLE [dbo].[OrderDetails]
GO
IF OBJECT_ID(N'[dbo].[Orders]', N'U') IS NOT NULL DROP TABLE [dbo].[Orders]
GO
IF OBJECT_ID(N'[dbo].[Products]', N'U') IS NOT NULL DROP TABLE [dbo].[Products]
GO
IF OBJECT_ID(N'[dbo].[Categories]', N'U') IS NOT NULL DROP TABLE [dbo].[Categories]
GO
IF OBJECT_ID(N'[dbo].[AspNetUserTokens]', N'U') IS NOT NULL DROP TABLE [dbo].[AspNetUserTokens]
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', N'U') IS NOT NULL DROP TABLE [dbo].[AspNetUserRoles]
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', N'U') IS NOT NULL DROP TABLE [dbo].[AspNetUserLogins]
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', N'U') IS NOT NULL DROP TABLE [dbo].[AspNetUserClaims]
GO
IF OBJECT_ID(N'[dbo].[AspNetRoleClaims]', N'U') IS NOT NULL DROP TABLE [dbo].[AspNetRoleClaims]
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', N'U') IS NOT NULL DROP TABLE [dbo].[AspNetUsers]
GO
IF OBJECT_ID(N'[dbo].[AspNetRoles]', N'U') IS NOT NULL DROP TABLE [dbo].[AspNetRoles]
GO

CREATE TABLE [dbo].[AspNetRoles](
    [Id] [nvarchar](450) NOT NULL,
    [Name] [nvarchar](256) NULL,
    [NormalizedName] [nvarchar](256) NULL,
    [ConcurrencyStamp] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetUsers](
    [Id] [nvarchar](450) NOT NULL,
    [UserName] [nvarchar](256) NULL,
    [NormalizedUserName] [nvarchar](256) NULL,
    [Email] [nvarchar](256) NULL,
    [NormalizedEmail] [nvarchar](256) NULL,
    [EmailConfirmed] [bit] NOT NULL,
    [PasswordHash] [nvarchar](max) NULL,
    [SecurityStamp] [nvarchar](max) NULL,
    [ConcurrencyStamp] [nvarchar](max) NULL,
    [PhoneNumber] [nvarchar](max) NULL,
    [PhoneNumberConfirmed] [bit] NOT NULL,
    [TwoFactorEnabled] [bit] NOT NULL,
    [LockoutEnd] [datetimeoffset](7) NULL,
    [LockoutEnabled] [bit] NOT NULL,
    [AccessFailedCount] [int] NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetRoleClaims](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [RoleId] [nvarchar](450) NOT NULL,
    [ClaimType] [nvarchar](max) NULL,
    [ClaimValue] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetUserClaims](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar](450) NOT NULL,
    [ClaimType] [nvarchar](max) NULL,
    [ClaimValue] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetUserLogins](
    [LoginProvider] [nvarchar](450) NOT NULL,
    [ProviderKey] [nvarchar](450) NOT NULL,
    [ProviderDisplayName] [nvarchar](max) NULL,
    [UserId] [nvarchar](450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetUserRoles](
    [UserId] [nvarchar](450) NOT NULL,
    [RoleId] [nvarchar](450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[AspNetUserTokens](
    [UserId] [nvarchar](450) NOT NULL,
    [LoginProvider] [nvarchar](450) NOT NULL,
    [Name] [nvarchar](450) NOT NULL,
    [Value] [nvarchar](max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Categories](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Categories_Name] UNIQUE ([Name])
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Products](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](200) NOT NULL,
    [Price] [decimal](18,2) NOT NULL,
    [ImageUrl] [nvarchar](max) NULL,
    [CategoryId] [int] NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Products_Price] CHECK ([Price] >= 0)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Orders](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [OrderDate] [datetime2](7) NOT NULL CONSTRAINT [DF_Orders_OrderDate] DEFAULT (GETDATE()),
    [UserId] [nvarchar](450) NULL,
    [TotalAmount] [decimal](18,2) NOT NULL,
    [FullName] [nvarchar](150) NOT NULL,
    [Address] [nvarchar](300) NOT NULL,
    [Phone] [nvarchar](20) NOT NULL,
    [Status] [nvarchar](50) NOT NULL CONSTRAINT [DF_Orders_Status] DEFAULT (N'Cho xac nhan'),
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_Orders_TotalAmount] CHECK ([TotalAmount] >= 0)
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OrderDetails](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [OrderId] [int] NOT NULL,
    [ProductId] [int] NOT NULL,
    [Quantity] [int] NOT NULL,
    [Price] [decimal](18,2) NOT NULL,
    CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_OrderDetails_Quantity] CHECK ([Quantity] > 0),
    CONSTRAINT [CK_OrderDetails_Price] CHECK ([Price] >= 0)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetRoleClaims] WITH CHECK
ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserClaims] WITH CHECK
ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserLogins] WITH CHECK
ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] WITH CHECK
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserTokens] WITH CHECK
ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Products] WITH CHECK
ADD CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[OrderDetails] WITH CHECK
ADD CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[OrderDetails] WITH CHECK
ADD CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO

CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
ON [dbo].[AspNetRoles] ([NormalizedName] ASC)
WHERE [NormalizedName] IS NOT NULL
GO

CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId]
ON [dbo].[AspNetRoleClaims] ([RoleId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId]
ON [dbo].[AspNetUserClaims] ([UserId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId]
ON [dbo].[AspNetUserLogins] ([UserId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId]
ON [dbo].[AspNetUserRoles] ([RoleId] ASC)
GO

CREATE NONCLUSTERED INDEX [EmailIndex]
ON [dbo].[AspNetUsers] ([NormalizedEmail] ASC)
GO

CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
ON [dbo].[AspNetUsers] ([NormalizedUserName] ASC)
WHERE [NormalizedUserName] IS NOT NULL
GO

CREATE NONCLUSTERED INDEX [IX_Products_CategoryId]
ON [dbo].[Products] ([CategoryId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Orders_UserId]
ON [dbo].[Orders] ([UserId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_OrderDetails_OrderId]
ON [dbo].[OrderDetails] ([OrderId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_OrderDetails_ProductId]
ON [dbo].[OrderDetails] ([ProductId] ASC)
GO

INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES
(N'role-admin', N'Admin', N'ADMIN', N'admin-role-stamp'),
(N'role-customer', N'Customer', N'CUSTOMER', N'customer-role-stamp')
GO

SET IDENTITY_INSERT [dbo].[Categories] ON
GO
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (1, N'Cà Phê')
GO
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (2, N'Trà S?a')
GO
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (3, N'Trà Trái Cây')
GO
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (4, N'Bánh')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO

SET IDENTITY_INSERT [dbo].[Products] ON
GO
INSERT [dbo].[Products] ([Id], [Name], [Price], [ImageUrl], [CategoryId]) VALUES (1, N'Ðen Ðá', 25000.00, N'/images/ca-phe-den-da.jpg', 1)
GO
INSERT [dbo].[Products] ([Id], [Name], [Price], [ImageUrl], [CategoryId]) VALUES (2, N'S?a Ðá', 30000.00, N'/images/ca-phe-sua-da.jpg', 1)
GO
INSERT [dbo].[Products] ([Id], [Name], [Price], [ImageUrl], [CategoryId]) VALUES (3, N'Trà S?a Trân Châu', 45000.00, N'/images/tra-sua-tran-chau.jpg', 2)
GO
INSERT [dbo].[Products] ([Id], [Name], [Price], [ImageUrl], [CategoryId]) VALUES (4, N'Trà Ðào Cam S?', 42000.00, N'/images/tra-dao-cam-sa.jpg', 3)
GO
INSERT [dbo].[Products] ([Id], [Name], [Price], [ImageUrl], [CategoryId]) VALUES (5, N'Matcha latte', 48000.00, N'/images/matcha-latte.jpg', 2)
GO
INSERT [dbo].[Products] ([Id], [Name], [Price], [ImageUrl], [CategoryId]) VALUES (6, N'Bánh Tiramisu', 50000.00, N'/images/banh-tiramisu.jpg', 4)
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO

SET IDENTITY_INSERT [dbo].[Orders] ON
GO
INSERT [dbo].[Orders] ([Id], [OrderDate], [UserId], [TotalAmount], [FullName], [Address], [Phone], [Status])
VALUES (1, CAST(N'2026-05-12T09:30:00.0000000' AS datetime2), NULL, 105000.00, N'Nguyen Van Minh', N'12 Nguyen Hue, Quan 1, TP.HCM', N'0901234567', N'Cho xac nhan')
GO
INSERT [dbo].[Orders] ([Id], [OrderDate], [UserId], [TotalAmount], [FullName], [Address], [Phone], [Status])
VALUES (2, CAST(N'2026-05-12T10:15:00.0000000' AS datetime2), NULL, 90000.00, N'Tran Thi Mai', N'25 Le Loi, Quan 3, TP.HCM', N'0912345678', N'Dang pha che')
GO
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO

SET IDENTITY_INSERT [dbo].[OrderDetails] ON
GO
INSERT [dbo].[OrderDetails] ([Id], [OrderId], [ProductId], [Quantity], [Price]) VALUES (1, 1, 3, 1, 45000.00)
GO
INSERT [dbo].[OrderDetails] ([Id], [OrderId], [ProductId], [Quantity], [Price]) VALUES (2, 1, 2, 2, 30000.00)
GO
INSERT [dbo].[OrderDetails] ([Id], [OrderId], [ProductId], [Quantity], [Price]) VALUES (3, 2, 4, 1, 42000.00)
GO
INSERT [dbo].[OrderDetails] ([Id], [OrderId], [ProductId], [Quantity], [Price]) VALUES (4, 2, 5, 1, 48000.00)
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
USE [MilkTeaManagement]
GO

-- C?p nh?t ?nh Cà phê
UPDATE Products SET ImageUrl = 'https://tse2.mm.bing.net/th/id/OIP.j9KMyZQMP5q8AIAaNcZKfQHaHa?r=0&rs=1&pid=ImgDetMain&o=7&rm=3' WHERE Id = 1;
UPDATE Products SET ImageUrl = 'https://thecupcafevietnam.com/wp-content/uploads/2022/11/ca-phe-sua-da.jpg' WHERE Id = 2;

-- C?p nh?t ?nh Trà s?a
UPDATE Products SET ImageUrl = 'https://tse3.mm.bing.net/th/id/OIP.thH9McA7XPKSnrTNJxHMUQHaE8?r=0&rs=1&pid=ImgDetMain&o=7&rm=3' WHERE Id = 3;
UPDATE Products SET ImageUrl = 'https://cdn.loveandlemons.com/wp-content/uploads/2023/06/iced-matcha-latte.jpg' WHERE Id = 5;

-- C?p nh?t ?nh Trà trái cây & Bánh
UPDATE Products SET ImageUrl = 'https://tse4.mm.bing.net/th/id/OIP.mRn2GFN0LCaNLKIgZqwyNQHaE8?r=0&rs=1&pid=ImgDetMain&o=7&rm=3' WHERE Id = 4;
UPDATE Products SET ImageUrl = 'https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?q=80&w=500' WHERE Id = 6;
GO


USE [MilkTeaManagement]
GO

INSERT INTO [dbo].[Products] ([Name], [Price], [ImageUrl], [CategoryId]) 
VALUES (N'Trà S?a Olong', 55000.00, 'https://th.bing.com/th/id/R.5ab81a9cee244f0fc1e7527671417538?rik=rnTl67u0q4vTyQ&pid=ImgRaw&r=0', 2)
GO
