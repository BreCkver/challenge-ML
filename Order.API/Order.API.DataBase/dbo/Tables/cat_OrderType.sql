CREATE TABLE [dbo].[cat_OrderType]
(
	 [Identifier]		INT NOT NULL
	,[Name]			    VARCHAR(100)
	,[IsActive]			BIT
	,[CreatedDate]		DATETIME
	,[UpdatedDate]		DATETIME
	CONSTRAINT [PK_OrderType] PRIMARY KEY ([Identifier] ASC)
)