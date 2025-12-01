## Instalación del Proyecto

Este repositorio contiene tanto el backend como el frontend del sistema.

### Backend

Para ejecutar correctamente el backend, primero es necesario configurar la base de datos y luego ajustar la cadena de conexión.

#### 1. Crear la base de datos
Ejecuta el archivo `scriptDatabase.sql` que se encuentra dentro de la carpeta database, el mismo crea automáticamente la base de datos junto con sus tablas, relaciones y datos iniciales de los Seguros.

#### 2. Configurar la conexión
Edita el archivo `appsettings.json` del proyecto API del backend y actualiza con las valores correspondientes:

<img width="889" height="196" alt="image" src="https://github.com/user-attachments/assets/ae088733-f49d-48a5-8601-1b574766cb48" />

- `Server`
- `User ID`
- `Password`

Con esto ya deberiamos poder ejecutar el proyecto back abriendo la solucion principal `Chubb.Back.sln` y consumir las APIS del back.

`https://localhost:7247/swagger/index.html`



### Frontend

#### 1. Ejecucion 
Ubicados en la carpeta `Chubb.Front`, ejecutar `npm i` para la instalacion de paquetes del proyecto front.
Luego, ejecutar `ng serve` para levantar el proyecto.


#### 2. Cors

Si tenemos problemas con los CORS debemos de ir a la carpeta API del back, en el program.cs encontraremos la configuacion de los CORS

<img width="565" height="166" alt="image" src="https://github.com/user-attachments/assets/2e50ad9f-402b-420d-b24f-98aa30e93dd9" />


Podremos configurar el acceso de nuestro front en la linea "policy.WithOrigins("http://localhost:4200")", deberemos cambiar o agregar un nuevo "http://localhost:[Aqui iria el puerto en el que se levanto el front]"

Tener en cuenta que la ruta que se esta apuntando desde el front sea la correcta donde se esta levantando el back
Revisarlo en \Chubb.Front\src\environments\environment.ts, el valor de apiUrl

<img width="407" height="95" alt="image" src="https://github.com/user-attachments/assets/2fc02043-498c-43b8-a873-748de8fb31f9" />


## Instalación con Docker 
La forma más rápida de ejecutar el proyecto completo es usando Docker. Solo necesitas tener Docker Desktop instalado.

#### Prerrequisitos
 - Docker Desktop instalado
 - Cambiar la contraseña a una personal, dentro del `.env` de la carpeta principal, el valor del campo `DB_PASSWORD`

### Pasos de Instalación

#### 1. Clonar el repositorio
`git clone <url-del-repositorio>`

`cd ProyectoChubb`

#### 2. Construir las imágenes
`docker-compose build --no-cache`

Este comando tardara un poco debido a que construirá las imágenes de:

 - Backend (.NET 8 API)
 - Frontend (Angular + Nginx)
 - SQL Server (con inicialización automática de BD)

#### 3. Levantar los contenedores
`docker-compose up -d`


Este comando iniciará los 3 contenedores:

 - seguros-sqlserver (Puerto 1433)
 - seguros-backend (Puerto 5000)
 - seguros-frontend (Puerto 4200)

Con esto ya el proyecto deberia estar funcional en tu navegador `http://localhost:4200`



## Pruebas
El proyecto incluye pruebas unitarias y de integración para el backend.

#### Para ejecutar todas las Pruebas ejecuta: 
##### 1. `cd Chubb.Back`
##### 2. `dotnet test`

#### O bien puedes usar el `Explorador de pruebas` de Visual Studio 2022: 

<img width="855" height="464" alt="image" src="https://github.com/user-attachments/assets/80d33b5d-8817-4603-accf-952164f02413" />

