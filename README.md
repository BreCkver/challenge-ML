# challenge-ML
#El proyecto esta estructurado de la siguiente manera:

### Business 
  - Business          --> Mantiene la logica del negocio. 
  - Contracts         --> Define los contratos, interfaces, de las clases.
### Data
  - Data              --> Ejecuciones a la base de datos
  - Data.Agent        --> Consultas a servicios externos.
### DataBase             
  - Objetos de BD, un script inicial para permitir la ejecucion de las pruebas de integracion
### Host 
  - Order.API.Host    --> Expone los controladores del api e implementa la authentificacion 
  - Order.API.Web     --> Aplicacion cliente, consume el api
### Shared
  - Shared.Entities   --> Definicion de Entidades, declaracion de DTOS para el paso entre capas
  - Shared.Framework  --> Herramientas en comun, utilies, utilizadas en la solucion
### Test
  - Test.Integration  --> Pruebas de integracion de la funcionalidad, implementacion de la BD
  - Test.UnitTest     --> Pruebas de la funcionalidad core, se trabajan con MOCK de la BD
  
### El api, se defininen los siguientes controladores:
- User        --> Tiene la responsabilidad de registra nuevos usuarios o authentificarlos
- Order       --> Tiene la responsabilidad de registrar, actualizar y listar los wishlist de un usuario
- OrderDetail --> Tiene la responsabilidad de administrar los item de un wishList
- Book        --> Esta dedicado a la busqueda de libros en una api externa

## Ejemplos 
### La documetacion del servicio se realizo en swagger y se puede consultar en:  https://{server}:{port}/swagger,
- ![imagen](https://user-images.githubusercontent.com/59982584/145640080-38a963a8-2774-45b8-8265-c1200e9e0a99.png)


### Creacion de usuario: 
- https://{server}:{port}/api/user
{
  "PasswordConfirm": "password41",
  "UserName": "UserName-41",
  "Password": "password41",
  "StatusIdentifier": 1
}
- curl -X POST --header 'Content-Type: application/json' --header 'Accept: application/json' --header -d '{ \ 
   "PasswordConfirm": "password41", \ 
   "UserName": "UserName-10", \ 
   "Password": "password41", \ 
   "StatusIdentifier": 1 \ 
 }' 'https://{server}:{port}/api/user'


### Authentificacion de usuario: 
- https://{server}:{port}/api/user/authenticate
{
  "UserName": "User-01",
  "Password": "123"
}

- curl -X POST --header 'Content-Type: application/json' --header 'Accept: application/json' -d '{ \ 
   "UserName": "User-01", \ 
   "Password": "123" \ 
 }' 'https://{server}:{port}/api/user/authenticate'
 
 ## Creacion de WishList: 
 - https://{server}:{port}/api/order
 {
  "WishList": {
    "Status": 1,
    "Name": "WishList Navidad 2021"
  },
  "User": {
    "Identifier": 150
  }
}
 - **NOTA La Authorization se genera cuando se logea el usuario, el token tiene una vida de 10 mins**

- curl -X POST --header 'Content-Type: application/json' --header 'Accept: application/json' --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkphaW1lIiwibmJmIjoxNjM5MTE2OTI3LCJleHAiOjE2MzkxMTc1MjcsImlhdCI6MTYzOTExNjkyNywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMjciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDMyNyJ9.SzQnuGqUP2qr-8X_tDxwQ-Dzgl-oPTWiVRQdFq1H9Jk' -d '{ \ 
   "WishList": { \ 
     "Status": 1, \ 
     "Name": "WishList Navidad 2021" \ 
   }, \ 
   "User": { \ 
     "Identifier": 150 \ 
   } \ 
 }' 'https://{server}:{port}/api/order'
 
## Consulta de los WishLists de un usuario: 
- https://{server}:{port}/api/order/user?identifier=150

- curl -X GET --header 'Accept: application/json' --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkphaW1lIiwibmJmIjoxNjM5MTE2OTI3LCJleHAiOjE2MzkxMTc1MjcsImlhdCI6MTYzOTExNjkyNywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMjciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDMyNyJ9.SzQnuGqUP2qr-8X_tDxwQ-Dzgl-oPTWiVRQdFq1H9Jk' 'https://{server}:{port}/api/order/user?identifier=150'


## Actualizacion de WishLists, eliminar: 
- https://{server}:{port}/api/order/action/delete
{
  "WishList": {
    "Identifier": 157,
    "Status": 3
  },
  "User": {
    "Identifier": 150
  }
} 

- curl -X PUT --header 'Content-Type: application/json' --header 'Accept: application/json' --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkphaW1lIiwibmJmIjoxNjM5MTE2OTI3LCJleHAiOjE2MzkxMTc1MjcsImlhdCI6MTYzOTExNjkyNywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMjciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDMyNyJ9.SzQnuGqUP2qr-8X_tDxwQ-Dzgl-oPTWiVRQdFq1H9Jk' -d '{ \ 
   "WishList": { \ 
     "Identifier": 157, \ 
     "Status": 3 \ 
   }, \ 
   "User": { \ 
     "Identifier": 150 \ 
   } \ 
 }' 'https://{server}:{port}/api/order/action/delete'
 
## Agregar un libro a un WishLists: 
- https://{server}:{port}/api/order/detail
 {
  "WishList": {
    "BookList": [
      {
        "Keyword": "sample string 1",
        "Title": "El Código Da Vinci",
        "Authors": [
          "Dan Brown",
          "Juanjo Estrella"
        ],
        "Publisher": "Umbriel Editores",
        "ExternalIdentifier": "IYuHPwAACAAJ",
        "Description": "",
        "Status": 1
      }
    ],
    "Identifier": 164
  },
  "User": {
    "Identifier": 150
  }
}

- curl -X POST --header 'Content-Type: application/json' --header 'Accept: application/json' --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkphaW1lIiwibmJmIjoxNjM5MTE3Njk5LCJleHAiOjE2MzkxMTgyOTksImlhdCI6MTYzOTExNzY5OSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMjciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDMyNyJ9.qxRxuHwG-ml9R1ouF6KbWXcSJBrx7wWDb36xJ8NZD74' -d '{ \ 
   "WishList": { \ 
     "BookList": [ \ 
       { \ 
         "Keyword": "sample string 1", \ 
         "Title": "El Código Da Vinci", \ 
         "Authors": [ \ 
           "Dan Brown", \ 
           "Juanjo Estrella" \ 
         ], \ 
         "Publisher": "Umbriel Editores", \ 
         "ExternalIdentifier": "IYuHPwAACAAJ", \ 
         "Description": "", \ 
         "Status": 1 \ 
       } \ 
     ], \ 
     "Identifier": 164 \ 
   }, \ 
   "User": { \ 
     "Identifier": 150 \ 
   } \ 
 } \ 
 ' 'https://{server}:{port}/api/order/detail'
 
 ## Consultar los items de un WishLists: 
 - https://{server}:{port}/order/user/detail?orderIdentifier=164&userIdentifier=150
 
 - curl -X GET --header 'Accept: application/json' --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkphaW1lIiwibmJmIjoxNjM5MTE3Njk5LCJleHAiOjE2MzkxMTgyOTksImlhdCI6MTYzOTExNzY5OSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMjciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDMyNyJ9.qxRxuHwG-ml9R1ouF6KbWXcSJBrx7wWDb36xJ8NZD74' 'https://{server}:{port}/api/order/user/detail?orderIdentifier=164&userIdentifier=150'
 
 ## Eliminar un item de un WishLists: 
 - https://{server}:{port}/api/order/detail/action/delete
 {
  "WishList": {
    "BookList": [
      {
	"Identifier": 41,
        "Status": 11
      }
    ],
    "Identifier": 164
  },
  "User": {
    "Identifier": 150
  }
}
 
- curl -X PUT --header 'Content-Type: application/json' --header 'Accept: application/json' --header 'Authorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkphaW1lIiwibmJmIjoxNjM5MTE4MTU4LCJleHAiOjE2MzkxMTg3NTgsImlhdCI6MTYzOTExODE1OCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMjciLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDMyNyJ9.ADrOcV9HHhUVCnYrbqjg8k6fRoPcsVoNS85YzeQnEsA' -d ' \ 
 { \ 
   "WishList": { \ 
     "BookList": [ \ 
       { \ 
 	"Identifier": 41, \ 
         "Status": 11 \ 
       } \ 
     ], \ 
     "Identifier": 164 \ 
   }, \ 
   "User": { \ 
     "Identifier": 150 \ 
   } \ 
 }' 'https://{server}:{port}/api/order/detail/action/delete'
 
 
 -
 
