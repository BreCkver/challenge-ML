--=============================================================================================================================================            
-- Author: <JAIME REYES VEREA>            
-- Date:  <2021-12-10>          
-- Desc: <Scripts Iniciales para las pruebas integrales>       
--=============================================================================================================================================

--Password es 123 sin embargo, encriptado = 202cb962ac59075b964b07152d234b70
INSERT INTO [dbo].[Users] (UserName, PassWord, UserStatusId, CreatedDate, UpdatedDate)
VALUES('User-01', '202cb962ac59075b964b07152d234b70', 1, GETDATE(), NULL)
GO

INSERT INTO [dbo].[Orders] (UserIdentifier, Name, OrderTypeId, OrderStatusId, CreatedDate, UpdatedDate)
VALUES(150, 'Lista Navideña', 101, 2, GETDATE(), NULL)
GO

INSERT INTO [dbo].[Orders] (UserIdentifier, Name, OrderTypeId, OrderStatusId, CreatedDate, UpdatedDate)
VALUES(150, 'Lista Dia de Reyes', 101, 2, GETDATE(), NULL)
GO

INSERT INTO [dbo].[OrderDetail] (OrderIdentifier, ExternalIdentifier, [Description], KeyWords, Title, Author, Publisher, OrderStatusId,CreatedDate, UpdatedDate)
VALUES(100, 'zUhryAEACAAJ', NULL, NULL, 'La Conquista de México', 'Sofia Guadarrama', '',10,  GETDATE(), NULL)
GO

INSERT INTO [dbo].[OrderDetail] (OrderIdentifier, ExternalIdentifier, [Description], KeyWords, Title, Author, Publisher, OrderStatusId,CreatedDate, UpdatedDate)
VALUES(100, 'iHkPDQAAQBJ', NULL, NULL, 'El código Da Vinci (Nueva Edición)', 'Dan Brown', 'Planeta',10,  GETDATE(), NULL)
GO

--=============================================================================================================================================            
-- Author: <JAIME REYES VEREA>            
-- Date:  <2021-12-10>          
-- Desc: <Scripts carga de catalogos>       
--=============================================================================================================================================            
INSERT INTO [dbo].[cat_UserStatus] VALUES(1, 'Nuevo', 1, GETDATE(), NULL) 
INSERT INTO [dbo].[cat_UserStatus] VALUES(2, 'Activo', 1, GETDATE(), NULL) 
INSERT INTO [dbo].[cat_UserStatus] VALUES(3, 'Inactivo', 1, GETDATE(), NULL) 
GO

INSERT INTO [dbo].[cat_OrderStatus] VALUES(1, 'Nuevo', 1, GETDATE(), NULL) 
INSERT INTO [dbo].[cat_OrderStatus] VALUES(2, 'Activo', 1, GETDATE(), NULL) 
INSERT INTO [dbo].[cat_OrderStatus] VALUES(3, 'Inactivo', 1, GETDATE(), NULL) 
GO

INSERT INTO [dbo].[cat_ProductStatus] VALUES(10, 'Activo', 1, GETDATE(), NULL) 
INSERT INTO [dbo].[cat_ProductStatus] VALUES(11, 'Inactivo', 1, GETDATE(), NULL) 
INSERT INTO [dbo].[cat_ProductStatus] VALUES(12, 'Nuevo', 1, GETDATE(), NULL) 
GO

INSERT INTO [dbo].[cat_OrderType] VALUES(101, 'WishList', 1, GETDATE(), NULL) 
GO