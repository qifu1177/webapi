USE ApartmentDB
GO

CREATE TABLE Modules(
	ModuleId int IDENTITY(1,1) NOT NULL,
	ModuleName [nvarchar](50) NOT NULL,
 CONSTRAINT PK_Module PRIMARY KEY CLUSTERED 
(
	ModuleId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE AppRole(
	RoleId nvarchar(50) NOT NULL,
	RoleDesc nvarchar(50) NOT NULL,
 CONSTRAINT PK_AppRole PRIMARY KEY CLUSTERED 
(
	RoleId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE AppUser(
	UserId nvarchar(50) NOT NULL,
	Email nvarchar(100) NOT NULL,
	Password nvarchar(50) NOT NULL,
	UserName nvarchar(50) NOT NULL,
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
	RoleId nvarchar(50) NOT NULL,
 CONSTRAINT PK_AppUser PRIMARY KEY CLUSTERED 
(
	UserId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE AppUser ADD  DEFAULT (getutcdate()) FOR CreateTs
GO

ALTER TABLE AppUser ADD  DEFAULT (getutcdate()) FOR UpdateTs
GO

CREATE TABLE AppSession(
	SessionId nvarchar(50) NOT NULL,
	UserId nvarchar(50) NOT NULL,
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
 CONSTRAINT PK_AppSession PRIMARY KEY CLUSTERED 
(
	SessionId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE AppSession ADD  DEFAULT (getutcdate()) FOR CreateTs
GO

ALTER TABLE AppSession ADD  DEFAULT (getutcdate()) FOR UpdateTs
GO

CREATE TABLE RoleRight(
	RoleRightId int IDENTITY(1,1) NOT NULL,
	RoleId nvarchar(50) NOT NULL,
	ModuleId int NOT NULL,
	RoleRight char(8) NOT NULL,
 CONSTRAINT PK_RoleRight PRIMARY KEY CLUSTERED 
(
	RoleRightId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE Things(
	ThingId int IDENTITY(1,1) NOT NULL,
	UserId nvarchar(50) NOT NULL,
	Name nvarchar(max) NULL,
	Quantity int NOT NULL,
	ImagePath nvarchar(max) NULL,
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
 CONSTRAINT PK_Things PRIMARY KEY CLUSTERED 
(
	ThingId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE Things ADD  DEFAULT (getutcdate()) FOR CreateTs
GO

ALTER TABLE Things ADD  DEFAULT (getutcdate()) FOR UpdateTs
GO

CREATE TABLE Rooms(
	RoomId int IDENTITY(1,1) NOT NULL,
	Name nvarchar(max) NULL,	
	ImagePath nvarchar(max) NULL,
	UserId nvarchar(50) NOT NULL,
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
 CONSTRAINT PK_Rooms PRIMARY KEY CLUSTERED 
(
	RoomId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE Rooms ADD  DEFAULT (getutcdate()) FOR CreateTs
GO

ALTER TABLE Rooms ADD  DEFAULT (getutcdate()) FOR UpdateTs
GO

CREATE TABLE Cupboards(
	CupboardId int IDENTITY(1,1) NOT NULL,
	Name nvarchar(max) NULL,
	Wide int NOT NULL,
	Height int NOT NULL,
	RoomId int NOT NULL,
	ImagePath nvarchar(max) NULL,
	UserId nvarchar(50) NOT NULL,
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
 CONSTRAINT PK_Cupboards PRIMARY KEY CLUSTERED 
(
	CupboardId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE Cupboards ADD  DEFAULT (getutcdate()) FOR CreateTs
GO

ALTER TABLE Cupboards ADD  DEFAULT (getutcdate()) FOR UpdateTs
GO

CREATE TABLE Grids(
	GridId int IDENTITY(1,1) NOT NULL,
	CupboardId int NOT NULL,
	PositionX int NOT NULL,
	PositionY int NOT NULL,
	ImagePath nvarchar(max) NULL,
	UserId nvarchar(50) NOT NULL,
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
 CONSTRAINT PK_Grids PRIMARY KEY CLUSTERED 
(
	GridId ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE Grids ADD  DEFAULT (getutcdate()) FOR CreateTs
GO

ALTER TABLE Grids ADD  DEFAULT (getutcdate()) FOR UpdateTs
GO

CREATE TABLE GridThrings(
	GridId int NOT NULL,
	ThingId int NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE AppUser  WITH CHECK ADD  CONSTRAINT FK_AppUser_AppRole FOREIGN KEY(RoleId)
REFERENCES AppRole (RoleId)
ON DELETE CASCADE
GO

ALTER TABLE AppUser CHECK CONSTRAINT FK_AppUser_AppRole
GO

ALTER TABLE AppSession  WITH CHECK ADD  CONSTRAINT FK_AppSession_AppUser FOREIGN KEY(UserId)
REFERENCES AppUser (UserId)
ON DELETE CASCADE
GO

ALTER TABLE AppSession CHECK CONSTRAINT FK_AppSession_AppUser
GO

ALTER TABLE RoleRight  WITH CHECK ADD  CONSTRAINT FK_RoleRight_AppRole FOREIGN KEY(RoleId)
REFERENCES AppRole (RoleId)
ON DELETE CASCADE
GO

ALTER TABLE RoleRight CHECK CONSTRAINT FK_RoleRight_AppRole
GO

ALTER TABLE RoleRight  WITH CHECK ADD  CONSTRAINT FK_RoleRight_Tabelle FOREIGN KEY(ModuleId)
REFERENCES Modules (ModuleId)
ON DELETE CASCADE
GO

ALTER TABLE RoleRight CHECK CONSTRAINT FK_RoleRight_Tabelle
GO

ALTER TABLE Things  WITH CHECK ADD  CONSTRAINT FK_Things_AppUser FOREIGN KEY(UserId)
REFERENCES AppUser (UserId)
ON DELETE CASCADE
GO

ALTER TABLE Things CHECK CONSTRAINT FK_Things_AppUser
GO

ALTER TABLE Rooms  WITH CHECK ADD  CONSTRAINT FK_Rooms_AppUser FOREIGN KEY(UserId)
REFERENCES AppUser (UserId)
ON DELETE CASCADE
GO

ALTER TABLE Rooms CHECK CONSTRAINT FK_Rooms_AppUser
GO

ALTER TABLE Cupboards  WITH CHECK ADD  CONSTRAINT FK_Cupboards_AppUser FOREIGN KEY(UserId)
REFERENCES AppUser (UserId)
ON DELETE CASCADE
GO

ALTER TABLE Cupboards CHECK CONSTRAINT FK_Cupboards_AppUser
GO

ALTER TABLE Grids  WITH CHECK ADD  CONSTRAINT FK_Grids_AppUser FOREIGN KEY(UserId)
REFERENCES AppUser (UserId)
ON DELETE CASCADE
GO

ALTER TABLE Grids CHECK CONSTRAINT FK_Grids_AppUser
GO

ALTER TABLE GridThrings  WITH CHECK ADD  CONSTRAINT FK_GridThrings_Things FOREIGN KEY(ThingId)
REFERENCES Things (ThingId)
ON DELETE CASCADE
GO

ALTER TABLE GridThrings CHECK CONSTRAINT FK_GridThrings_Things
GO

ALTER TABLE GridThrings  WITH CHECK ADD  CONSTRAINT FK_GridThrings_Grids FOREIGN KEY(GridId)
REFERENCES Grids (GridId)
GO

ALTER TABLE GridThrings CHECK CONSTRAINT FK_GridThrings_Grids
GO

ALTER TABLE Grids  WITH CHECK ADD  CONSTRAINT FK_Grids_Cupboards_CupboardId FOREIGN KEY(CupboardId)
REFERENCES Cupboards (CupboardId)
GO

ALTER TABLE Grids CHECK CONSTRAINT FK_Grids_Cupboards_CupboardId
GO

ALTER TABLE Cupboards  WITH CHECK ADD  CONSTRAINT FK_Cupboards_Rooms_RoomId FOREIGN KEY(RoomId)
REFERENCES Rooms (RoomId)
GO

ALTER TABLE Cupboards CHECK CONSTRAINT FK_Cupboards_Rooms_RoomId
GO



