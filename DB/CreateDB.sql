Drop table Session

Create Table Session
(
	SessionID	uniqueidentifier NOT NULL,
	CreatedDateTime datetime NOT NULL,
	CreatedBy varchar(100) NULL
)

ALTER TABLe Session
ADD CONSTRAINT PK_Session_SessionID PRIMARY KEY CLUSTERED(SessionID)
