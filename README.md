## Instalación del Proyecto

Este repositorio contiene tanto el backend como el frontend del sistema.

### Backend

Para ejecutar correctamente el backend, primero es necesario configurar la base de datos y luego ajustar la cadena de conexión.

#### 1. Crear la base de datos
Ejecuta el archivo `scriptDatabase.sql`, el cual crea automáticamente la base de datos junto con sus tablas, relaciones y datos iniciales.

#### 2. Configurar la conexión
Edita el archivo `appsettings.json` del proyecto backend y actualiza con las valores correspondientes:

<img width="889" height="196" alt="image" src="https://github.com/user-attachments/assets/ae088733-f49d-48a5-8601-1b574766cb48" />

- `Server`
- `User ID`
- `Password`

Con esto ya deberiamos poder consumir las APIS desde el front.

### Frontend
#### 1. Cors

Si tenemos problemas con los CORS debemos de ir a la carpeta API del back, en el program.cs encontraremos la configuacion de los CORS

<img width="565" height="166" alt="image" src="https://github.com/user-attachments/assets/2e50ad9f-402b-420d-b24f-98aa30e93dd9" />


Podremos configurar el acceso de nuestro front en la linea "policy.WithOrigins("http://localhost:4200")", deberemos cambiar o agregar un nuevo "http://localhost:[Aqui iria el puerto en el que se levanto el front]"

Tener en cuenta que la ruta que se esta apuntando desde el front sea la correcta donde se esta levantando el back
Revisarlo en \Chubb.Front\src\environments\environment.ts, el valor de apiUrl

<img width="407" height="95" alt="image" src="https://github.com/user-attachments/assets/2fc02043-498c-43b8-a873-748de8fb31f9" />

