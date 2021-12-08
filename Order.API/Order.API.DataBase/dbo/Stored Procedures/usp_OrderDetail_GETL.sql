--=============================================================================================================================================            
-- Author: <JAIME REYES VEREA>            
-- Date:  <2021-12-06>          
-- Desc: <GET OrderDetail>       
--=============================================================================================================================================            
CREATE PROCEDURE [dbo].[usp_OrderDetail_GETL]      
@pi_OrderIdentifier		INT,
@pi_OrderStatusId		INT
AS                                                                          
BEGIN      

 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;               
 SET NOCOUNT ON;

	SELECT 
		 Identifier
		,OrderIdentifier
		,ExternalIdentifier
		,[Description]
		,KeyWords
		,Title
		,Author
		,Publisher
		,OrderStatusId
		,CreatedDate
	FROM 
		OrderDetail
	WHERE
		OrderIdentifier	= @pi_OrderIdentifier
	AND OrderStatusId	= @pi_OrderStatusId
		      
 SET TRANSACTION ISOLATION LEVEL READ COMMITTED;               
 SET NOCOUNT OFF;              
END 