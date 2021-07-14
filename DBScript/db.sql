USE HomeDB
GO

CREATE TABLE Modules(
	ModuleId int IDENTITY(1,1) NOT NULL,
	ModuleName [nvarchar](50) NOT NULL,
CONSTRAINT PK_Module PRIMARY KEY (ModuleId)
)
GO

CREATE TABLE AppRole(
	RoleId nvarchar(50) NOT NULL,
	RoleDesc nvarchar(50) NOT NULL,
 CONSTRAINT PK_AppRole PRIMARY KEY (RoleId)
)
GO

CREATE TABLE AppUser(
	UserId nvarchar(50) NOT NULL,
	Email nvarchar(100) NOT NULL,
	Password nvarchar(50) NOT NULL,
	UserName nvarchar(50) NOT NULL,
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
	RoleId nvarchar(50) NOT NULL,
 CONSTRAINT PK_AppUser PRIMARY KEY (UserId)
)
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
 CONSTRAINT PK_AppSession PRIMARY KEY (SessionId)
)
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
 CONSTRAINT PK_RoleRight PRIMARY KEY (RoleRightId)
)
GO

CREATE TABLE Homes(
	HomeId nvarchar(50) NOT NULL,
	Address nvarchar(100) NULL,
	ZIP nvarchar(10) NOT NULL,
	Location nvarchar(100) NULL,
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
 CONSTRAINT PK_Home PRIMARY KEY (HomeId)
)
GO
ALTER TABLE Homes ADD  DEFAULT (getutcdate()) FOR CreateTs
GO

ALTER TABLE Homes ADD  DEFAULT (getutcdate()) FOR UpdateTs
GO

CREATE TABLE AppUserHomes(
	AppUserHomeId int IDENTITY(1,1) NOT NULL,
	HomeId nvarchar(50) NOT NULL,
	UserId nvarchar(50) NOT NULL,
	CONSTRAINT PK_AppUserHomes PRIMARY KEY (AppUserHomeId)
)
GO

CREATE TABLE HomeImages(
	HomeImageId int IDENTITY(1,1) NOT NULL,
	HomeId nvarchar(50) NOT NULL,
	ImagePath nvarchar(max) NULL, 
	CONSTRAINT PK_HomeImages PRIMARY KEY (HomeImageId)
)
GO

CREATE TABLE Things(
	ThingId int IDENTITY(1,1) NOT NULL,
	HomeId nvarchar(50) NOT NULL,
	Name nvarchar(max) NULL,
	Quantity int NOT NULL,
	ImagePath nvarchar(max) NULL,
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
 CONSTRAINT PK_Thing PRIMARY KEY (ThingId)
)
GO

ALTER TABLE Things ADD  DEFAULT (getutcdate()) FOR CreateTs
GO

ALTER TABLE Things ADD  DEFAULT (getutcdate()) FOR UpdateTs
GO

CREATE TABLE Rooms(
	RoomId int IDENTITY(1,1) NOT NULL,
	Name nvarchar(max) NULL,	
	ImagePath nvarchar(max) NULL,
	HomeId nvarchar(50) NOT NULL,
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
 CONSTRAINT PK_Room PRIMARY KEY (RoomId)
)
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
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
 CONSTRAINT PK_Cupboard PRIMARY KEY (CupboardId)
)
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
	CreateTs datetime NOT NULL,
	UpdateTs datetime NOT NULL,
 CONSTRAINT PK_Grid PRIMARY KEY (GridId)
)
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

ALTER TABLE HomeImages  WITH CHECK ADD  CONSTRAINT FK_HomeImages_Home FOREIGN KEY(HomeId)
REFERENCES Homes (HomeId)
ON DELETE CASCADE
GO

ALTER TABLE HomeImages CHECK CONSTRAINT FK_HomeImages_Home
GO

ALTER TABLE Things  WITH CHECK ADD  CONSTRAINT FK_Things_Home FOREIGN KEY(HomeId)
REFERENCES Homes (HomeId)
ON DELETE CASCADE
GO

ALTER TABLE Things CHECK CONSTRAINT FK_Things_Home
GO

ALTER TABLE Rooms  WITH CHECK ADD  CONSTRAINT FK_Rooms_Home FOREIGN KEY(HomeId)
REFERENCES Homes (HomeId)
ON DELETE CASCADE
GO

ALTER TABLE Rooms CHECK CONSTRAINT FK_Rooms_Home
GO

ALTER TABLE Cupboards  WITH CHECK ADD  CONSTRAINT FK_Cupboards_Room FOREIGN KEY(RoomId)
REFERENCES Rooms (RoomId)
ON DELETE CASCADE
GO

ALTER TABLE Cupboards CHECK CONSTRAINT FK_Cupboards_Room
GO

ALTER TABLE Grids  WITH CHECK ADD  CONSTRAINT FK_Grids_Cupboards FOREIGN KEY(CupboardId)
REFERENCES Cupboards (CupboardId)
ON DELETE CASCADE
GO

ALTER TABLE Grids CHECK CONSTRAINT FK_Grids_Cupboards
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

ALTER TABLE AppUserHomes  WITH CHECK ADD  CONSTRAINT FK_AppUserHomes_Homes FOREIGN KEY(HomeId)
REFERENCES Homes (HomeId)
ON DELETE CASCADE
GO

ALTER TABLE AppUserHomes CHECK CONSTRAINT FK_AppUserHomes_Homes
GO

ALTER TABLE AppUserHomes  WITH CHECK ADD  CONSTRAINT FK_AppUserHomes_AppUsers FOREIGN KEY(UserId)
REFERENCES AppUser (UserId)
GO

ALTER TABLE AppUserHomes CHECK CONSTRAINT FK_AppUserHomes_AppUsers
GO



