CREATE TABLE Orders 
(
	 [Identifier]		INT IDENTITY(100,1) NOT NULL
	,[UserIdentifier]	INT NOT NULL
	,[Name]				VARCHAR(100)
	,[OrderTypeId]		INT
	,[OrderStatusId]	INT
	,[CreatedDate]		DATETIME
	,[UpdatedDate]		DATETIME
	
	CONSTRAINT [PK_Orders] PRIMARY KEY ([Identifier] ASC)
)