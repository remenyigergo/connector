/********** SOCIAL *********/

CREATE DATABASE Connector

CREATE TABLE Users (
	  id INT NOT NULL,
	  username NVARCHAR(32) UNIQUE NOT NULL,
	  pw NVARCHAR(32) NOT NULL,
	  email NVARCHAR(64) NOT NULL,
	  moduleId INT NULL,
	  settingId INT,
	  PRIMARY KEY(id)
)

CREATE TABLE Friends (
	userId1 INT NOT NULL,
	userId2 INT NOT NULL,
	PRIMARY KEY(userId1, userId2),
	FOREIGN KEY(userId1) REFERENCES Users(id),
	FOREIGN KEY(userId2) REFERENCES Users(id)
)

CREATE TABLE Groups (
	groupId INT NOT NULL,
	PRIMARY KEY(groupId),
)

CREATE TABLE GroupMembers(
	userId INT NOT NULL,
	groupId INT NOT NULL,
	FOREIGN KEY (userId) REFERENCES Users(id),
	FOREIGN KEY (groupId) REFERENCES Groups(groupId)
)

CREATE TABLE GroupMessages (
	groupId INT NOT NULL,
	Message NVARCHAR(512) NOT NULL,
	date DATETIME,
	FOREIGN KEY (groupId) REFERENCES Groups(groupId)
)

CREATE TABLE Feeds(
	userId INT NOT NULL,
	message NVARCHAR(512),
	picture NVARCHAR(512),
	taggedUserId INT NULL,
	date DATETIME,
	FOREIGN KEY (userId) REFERENCES Users(id)
)

CREATE TABLE Profile (
	userId INT NOT NULL,
	picture NVARCHAR(512),
	birthDate DATE,
	sex TINYINT,
	city NVARCHAR(64)
	FOREIGN KEY (userId) REFERENCES Users(Id)
)

CREATE TABLE Chat(
	chatId INT NOT NULL,
	userId1 INT NOT NULL,
	userId2 INT NOT NULL,
	PRIMARY KEY(chatId),
	FOREIGN KEY (userId1) REFERENCES Users(id),
	FOREIGN KEY (userId2) REFERENCES Users(id)
)

CREATE TABLE ChatMessages (
	chatId INT NOT NULL,
	message NVARCHAR(512),
	FOREIGN KEY(chatId) REFERENCES Chat(chatId)
)

CREATE TABLE Settings (
	userId INT NOT NULL,
	language INT NOT NULL,
	FOREIGN KEY (userId) REFERENCES Users(Id)
)


/********** MODULES *********/

CREATE TABLE Modules(
	moduleId INT NOT NULL,
	name NVARCHAR(32) NOT NULL,
	PRIMARY KEY (moduleId) 
)

CREATE TABLE ActivatedModules(
	userId INT NOT NULL,
	moduleId INT NOT NULL,
	FOREIGN KEY (userId) REFERENCES Users(Id),
	FOREIGN KEY (moduleId) REFERENCES Modules(moduleId)
)

/*************  SOROZAT **************/

CREATE TABLE Series(
	seriesId INT NOT NULL,
	title NVARCHAR(32) NOT NULL,
	runtime INT NOT NULL,
	rating INT NOT NULL,
	year DATE NOT NULL,
	category NVARCHAR(32) NOT NULL,
	description NVARCHAR(512) null,
	PRIMARY KEY(seriesId)
)

CREATE TABLE Actors(
	actorId INT NOT NULL,
	name NVARCHAR(128) NOT NULL,
	PRIMARY KEY(actorId)
)

CREATE TABLE SeriesCast ( 
	seriesId INT NOT NULL,
	actorId INT NOT NULL,
	PRIMARY KEY(seriesId, actorId),
	FOREIGN KEY(seriesId) REFERENCES Series(seriesId),
	FOREIGN KEY (actorId) REFERENCES Actors(actorId)
)

CREATE TABLE FavoriteSeries( /*lehet több is*/
	seriesId INT NOT NULL,
	userId INT NOT NULL,
	FOREIGN KEY (seriesId) REFERENCES Series(seriesID)
)

CREATE TABLE SeriesStarted (
	seriesId INT NOT NULL,
	userId INT NOT NULL,
	FOREIGN KEY (seriesId) REFERENCES Series(seriesId),
	FOREIGN KEY (userId) REFERENCES Users(Id)
)

CREATE TABLE AddedSeries(
	userId INT NOT NULL,
	seriesId INT NOT NULL,
	FOREIGN KEY (seriesId) REFERENCES Series(seriesId),
	FOREIGN KEY (userId) REFERENCES Users(Id)
)

CREATE TABLE Episode( 
	episodeId INT NOT NULL,
	title NVARCHAR(128) NOT NULL,
	length INT NOT NULL,
	rating FLOAT NOT NULL,
	airdate NVARCHAR(32) NOT NULL,
	PRIMARY KEY(episodeId)
)

CREATE TABLE EpisodeCast (
	episodeId INT NOT NULL,
	actorId INT NOT NULL,
	PRIMARY KEY(episodeId, actorId),
	FOREIGN KEY(episodeId) REFERENCES Episode(episodeId),
	FOREIGN KEY (actorId) REFERENCES Actors(actorId)
)


CREATE TABLE EpisodeStarted (
	seriesId INT NOT NULL,
	userId INT NOT NULL,
	timeElapsed INT NOT NULL,
	season INT NOT NULL,
	episode INT NOT NULL,
	date DATE NOT NULL,
	watchedPercentage INT NULL,
	FOREIGN KEY (seriesId) REFERENCES Series(seriesId),
	FOREIGN KEY (userId) REFERENCES Users(Id)
)

CREATE TABLE SeenEpisodes(
	userId INT NOT NULL,
	seriesId INT NOT NULL,
	season INT NOT NULL,
	episode INT NOT NULL,
	PRIMARY KEY(season,episode,userId,seriesId),
	FOREIGN KEY (seriesId) REFERENCES Series(seriesId),
	FOREIGN KEY (userId) REFERENCES Users(Id)
)

CREATE TABLE Programs(
	programId int IDENTITY(1,1),
	programName VARCHAR(128) NOT NULL,
	PRIMARY KEY(programId)
)


CREATE TABLE ProgramBaseCategories(
	categoryId INT IDENTITY(1,1),
	name NVARCHAR(32) NOT NULL,
	PRIMARY KEY(categoryId)
)
SET IDENTITY_INSERT ProgramBaseCategories ON; 

CREATE TABLE ProgramsFollowed(
	userId INT NOT NULL,
	programId INT NOT NULL,
	duration INT NOT NULL,
	since DATE NOT NULL,
	visible int not null,
	category INT NOT NULL,
	PRIMARY KEY(userId,programId),
	FOREIGN KEY(userId) REFERENCES Users(Id),
	FOREIGN KEY(programId) REFERENCES Programs(programId),
	FOREIGN KEY(category) REFERENCES ProgramBaseCategories(categoryId)
)

CREATE TABLE ProgramsFollowedUpdates(
	userId INT NOT NULL,
	programId INT NOT NULL,
	lastUpdated DATE NOT NULL,
	durationAdded INT NOT NULL,
	FOREIGN KEY(userId) REFERENCES Users(Id),
	FOREIGN KEY(programId) REFERENCES Programs(programId)
)

