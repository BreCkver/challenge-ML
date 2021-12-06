CREATE TABLE OrderDetail
(
	 [Identifier]			INT IDENTITY(1,1) NOT NULL
	,[OrderIdentifier]		INT NOT NULL
	,[ExternalIdentifier]	VARCHAR(50)
	,[Description]			VARCHAR(500)
	,[KeyWords]				VARCHAR(30)
	,[Title]				VARCHAR(50)
	,[Author]				VARCHAR(200)
	,[Publisher]			VARCHAR(100)
	,[OrderStatusId]		INT
	,[CreatedDate]			DATETIME
	,[UpdatedDate]			DATETIME	
	CONSTRAINT [PK_OrderDetail] PRIMARY KEY ([Identifier] ASC)
)