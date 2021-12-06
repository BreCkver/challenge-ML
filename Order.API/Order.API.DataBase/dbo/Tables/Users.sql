CREATE TABLE Users 
(
	 [Identifier]		INT IDENTITY(150,1) NOT NULL
	,[UserName]			VARCHAR(100)
	,[PassWord]			VARCHAR(100)
	,[UserStatusId]		INT
	,[CreatedDate]		DATETIME
	,[UpdatedDate]		DATETIME
	CONSTRAINT [PK_Users] PRIMARY KEY ([Identifier] ASC)
)