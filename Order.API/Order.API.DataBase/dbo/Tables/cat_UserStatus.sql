CREATE TABLE [dbo].[cat_UserStatus]
(
	 [Identifier]		INT NOT NULL
	,[Name]			    VARCHAR(100)
	,[IsActive]			BIT
	,[CreatedDate]		DATETIME
	,[UpdatedDate]		DATETIME
	CONSTRAINT [PK_UserStatus] PRIMARY KEY ([Identifier] ASC)
)
GO
