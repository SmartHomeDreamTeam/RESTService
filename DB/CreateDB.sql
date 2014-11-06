IF EXISTS(SELECT name FROM sysobjects WHERE name = N'EventLog' AND xtype='U')
	Drop table EventLog

IF EXISTS(SELECT name FROM sysobjects WHERE name = N'Session' AND xtype='U')
	Drop table Session

IF EXISTS(SELECT name FROM sysobjects WHERE name = N'UserInfor' AND xtype='U')
	Drop table UserInfor


CREATE TABLE UserInfor
(
	UserInforID uniqueidentifier		NOT NULL,
	UserID		varchar(100)			NOT NULL,
	Pin			varchar(100)			NOT NULL
)

ALTER TABLE UserInfor
ADD CONSTRAINT PK_UserInfo_UserInfoID PRIMARY KEY CLUSTERED(UserInforID)


CREATE TABLE Session
(
	SessionID	uniqueidentifier		NOT NULL,
	UserInforID	uniqueidentifier		NOT NULL,
	SecretKey	varchar(100)			NULL,
	CreatedDateTime datetime			NOT NULL,
	CreatedBy	varchar(100)			NULL
)

ALTER TABLE Session
ADD CONSTRAINT PK_Session_SessionID PRIMARY KEY CLUSTERED(SessionID)

ALTER TABLE Session
ADD CONSTRAINT FK_Session_UserInforID_UserInfor_UserInforID
FOREIGN KEY (UserInforID)
REFERENCES UserInfor(UserInforID)

CREATE TABLE EventLog
(
	EventLogID uniqueidentifier			NOT NULL,
	UserInforID uniqueidentifier		NOT NULL,
	LogID		uniqueidentifier		NULL,
	Description varchar(1000)			NULL
)

ALTER TABLE EventLog
ADD CONSTRAINT PK_EventLog_EventLogID PRIMARY KEY CLUSTERED(EventLogID)

ALTER TABLE EventLog
ADD CONSTRAINT FK_EventLog_UserInforID_UserInfor_UserInforID
FOREIGN KEY (UserInforID)
REFERENCES UserInfor(UserInforID)

DECLARE @userinforID uniqueidentifier = newid()

INSERT INTO UserInfor(UserInforID, UserID, Pin)
	VALUES(@userinforID, 'userid', '1234')

INSERT INTO UserInfor(UserInforID, UserID, Pin)
	VALUES(newId(),'Tim', '133I132msf%')



INSERT INTO Session(SessionID, UserInforID, SecretKey, CreatedDateTime, CreatedBy)
	VALUES(newid(), @userinforID, '123456798', GETDATE(), 'CreatedBy1')

INSERT INTO Session(SessionID, UserInforID, SecretKey, CreatedDateTime, CreatedBy)
	VALUES(newid(), @userinforID, 'asd1322dafdsaf', GETDATE(), 'CreatedBy2')


INSERT INTO EventLog(EventLogID, UserInforID, Description)
	VALUES(newid(), @userinforID, 'Open')

INSERT INTO EventLog(EventLogID, UserInforID, Description)
	VALUES(newid(), @userinforID, 'Close')