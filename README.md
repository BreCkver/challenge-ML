# challenge-ML
#El proyecto esta estructurado de la siguiente manera:

## Business 
  ###-Business          --> Mantiene la logica del negocio. 
  ###-Contracts         --> Define los contratos, interfaces, de las clases.
## Data
  ###-Data              --> Ejecuciones a la base de datos
  ###-Data.Agent        --> Consultas a servicios externos.
## DataBase             --> Objetos de BD, un script inicial para permitir la ejecucion de las pruebas de integracion
## Host 
  ###-Order.API.Host    --> Expone los controladores del api e implementa la authentificacion 
  ###-Order.API.Web     --> Aplicacion cliente, consume el api
## Shared
  ###-Shared.Entities   --> Definicion de Entidades, declaracion de DTOS para el paso entre capas
  ###-Shared.Framework  --> Herramientas en comun, utilies, utilizadas en la solucion
## Test
  ###-Test.Integration  --> Pruebas de integracion de la funcionalidad, implementacion de la BD
  ###-Test.UnitTest     --> Pruebas de la funcionalidad core, se trabajan con MOCK de la BD
  
## El api, se defininen los siguientes controladores:
## -> User        --> Tiene la responsabilidad de registra nuevos usuarios o authentificarlos
## -> Order       --> Tiene la responsabilidad de registrar, actualizar y listar los wishlist de un usuario
## -> OrderDetail --> Tiene la responsabilidad de administrar los item de un wishList
## -> Book        --> Esta dedicado a la busqueda de libros en una api externa

