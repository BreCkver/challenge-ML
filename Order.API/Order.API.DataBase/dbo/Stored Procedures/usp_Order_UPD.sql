--=============================================================================================================================================            
-- Author: <JAIME REYES VEREA>            
-- Date:  <2021-12-06>          
-- Desc: <Updated Order>       
--=============================================================================================================================================            
CREATE PROCEDURE [dbo].[usp_Order_UPD]      
@pi_OrderId			INT,
@pc_Name			VARCHAR(100),
@pi_OrderStatusId	INT
AS                                                                          
BEGIN      

 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;               
 SET NOCOUNT ON;

	UPDATE Orders
	SET 
		Name = COALESCE(@pc_Name, Name),
		OrderStatusId = COALESCE(@pi_OrderStatusId, OrderStatusId),
		UpdatedDate = GETDATE()
	WHERE
		Identifier	= @pi_OrderId		
		      
 SET TRANSACTION ISOLATION LEVEL READ COMMITTED;               
 SET NOCOUNT OFF;              
END