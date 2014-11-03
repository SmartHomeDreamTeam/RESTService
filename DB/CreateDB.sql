IF EXISTS(SELECT name FROM sysobjects WHERE name = N'Session' AND xtype='U')
	Drop table Session

IF EXISTS(SELECT name FROM sysobjects WHERE name = N'UserInfor' AND xtype='U')
	Drop table UserInfor


CREATE TABLE UserInfor
(
	UserInforID	varchar(100)			NOT NULL,
	Pin		varchar(100)				NOT NULL
)

ALTER TABLE UserInfor
ADD CONSTRAINT PK_UserInfo_UserInfoID PRIMARY KEY CLUSTERED(UserInforID)


CREATE TABLE Session
(
	SessionID	uniqueidentifier		NOT NULL,
	UserInforID		varchar(100)		NOT NULL,
	SecretKey	varchar(100)			NULL,
	CreatedDateTime datetime			NOT NULL,
	CreatedBy varchar(100)				NULL
)

ALTER TABLE Session
ADD CONSTRAINT PK_Session_SessionID PRIMARY KEY CLUSTERED(SessionID)

ALTER TABLE Session
ADD CONSTRAINT FK_Session_UserInforID_UserInfor_UserInforID
FOREIGN KEY (UserInforID)
REFERENCES UserInfor(UserInforID)


INSERT INTO UserInfor(UserInforID, Pin)
	VALUES('userid', '1234')

INSERT INTO UserInfor(UserInforID, Pin)
	VALUES('Tim', '133I132msf%')