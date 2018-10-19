/********** SOCIAL *********/

CREATE TABLE Users (
	  id INT NOT NULL,
	  username NVARCHAR(32) UNIQUE NOT NULL,
	  pw NVARCHAR(32) NOT NULL,
	  email NVARCHAR(64) NOT NULL,
	  modulId INT NULL,
	  settingId INT,
	  PRIMARY KEY(id)
)

CREATE TABLE Friends (
	userId1 INT NOT NULL,
	userId2 INT NOT NULL,
	PRIMARY KEY(userId1, userId2)
)

CREATE TABLE Groups (
	groupId INT NOT NULL,
	userId INT NOT NULL,
	PRIMARY KEY(groupId)
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
	PRIMARY KEY(chatId)
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

CREATE TABLE SeriesActors (  /*még külön táblábal ehet tenni az actoroket de felesleges sztem*/
	seriesId INT NOT NULL,
	name NVARCHAR(64) NOT NULL,
	FOREIGN KEY(seriesId) REFERENCES Series(seriesId)
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