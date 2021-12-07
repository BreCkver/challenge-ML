--=============================================================================================================================================            
-- Author: <JAIME REYES VEREA>            
-- Date:  <2021-12-06>          
-- Desc: <Updated Order>       
--=============================================================================================================================================            
CREATE PROCEDURE [dbo].[usp_OrderDetail_UPD]      
@pi_OrderDetailId		INT,
@pi_OrderStatusId		INT
AS                                                                          
BEGIN      

 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;               
 SET NOCOUNT ON;

	UPDATE OrderDetail
	SET 
		OrderStatusId	= COALESCE(@pi_OrderStatusId, OrderStatusId),
		UpdatedDate		= GETDATE()
	WHERE
		Identifier	= @pi_OrderDetailId		
		      
 SET TRANSACTION ISOLATION LEVEL READ COMMITTED;               
 SET NOCOUNT OFF;              
END