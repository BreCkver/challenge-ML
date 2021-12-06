--=============================================================================================================================================            
-- Author: <JAIME REYES VEREA>            
-- Date:  <2021-12-06>          
-- Desc: <GET Order>       
--=============================================================================================================================================            
CREATE PROCEDURE [dbo].[usp_Order_GETL]      
@pi_UserId			INT
AS                                                                          
BEGIN      

 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;               
 SET NOCOUNT ON;

	SELECT 
		 Identifier
		,UserIdentifier
		,Name
		,OrderTypeId
		,OrderStatusId
		,CreatedDate
	FROM 
		Orders
	WHERE
		UserIdentifier	= @pi_UserId		
		      
 SET TRANSACTION ISOLATION LEVEL READ COMMITTED;               
 SET NOCOUNT OFF;              
END