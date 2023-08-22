# Basic Store - Repositorio .NET 6 MVC
¡Bienvenido al repositorio de Basic Store! Este repositorio contiene un proyecto .NET 6 MVC que sirve como una tienda de comercio electrónico básica. El proyecto está construido utilizando SQL Server 2012 como base de datos y ofrece varios módulos para gestionar clientes, artículos y facturas. Además, incluye autenticación de usuarios, manejo de sesiones con cookies y la capacidad de registrar nuevos usuarios.
## Demostración
Puede ver la demostración ingresando a: 
https://basicstore.azurewebsites.net/

### Credenciales**
**User:** Admin
**Password:** password123

## Resumen del Proyecto
El proyecto "Basic Store" tiene la siguiente estructura:

- Módulo de Clientes: Este módulo permite realizar operaciones CRUD en clientes para la tienda. 
Puedes crear, leer, actualizar y eliminar registros de clientes.

- Módulo de Artículos: El módulo de Artículos permite operaciones CRUD para gestionar los artículos en la tienda. 
Puedes agregar, ver, editar y eliminar información de los artículos.

- Módulo de Factura: Este módulo te permite crear e imprimir facturas. 
Ofrece la funcionalidad para generar facturas e imprimir cada una de ellas.

- Autenticación de Usuarios: El proyecto incluye un sistema de inicio de sesión que permite a los usuarios acceder con sus credenciales. 
Utiliza cookies para gestionar las sesiones de los usuarios y mejorar la experiencia de usuario.

- Registro de Usuarios: La aplicación admite el registro de nuevos usuarios, permitiendo que se inscriban y obtengan acceso al sistema.

## Empezando
Para comenzar con el proyecto "Basic Store", sigue estos pasos:

- Clona el repositorio en tu máquina local usando la consola de git y ejecutando:
git clone https://github.com/FernandoAjset/BasicStore.git

- Asegúrate de tener instalado el SDK .NET 6 y SQL Server 2012 (o posterior).

- Crea una nueva base de datos en SQL Server para almacenar los datos del proyecto, el repositorio incluye el script "Script_creacion_BD.sql" con todo lo necesario para crear la base de datos.

- Actualiza la cadena de conexión de la base de datos en los archivos de configuración del proyecto para que apunten a tu base de datos SQL Server.
El archivo por defecto del proyecto que maneja la conexión es el "appsettings.json"

### Uso de la aplicación
Una vez que la aplicación esté en funcionamiento, puedes acceder a varios módulos y funciones:
- Autenticación de Usuarios: Utiliza la página de inicio de sesión para acceder con tus credenciales de usuario. 
El sistema te autenticará y establecerá una sesión utilizando cookies.
Importante: por defecto en la base de datos se inserta un usuario de prueba que es
User: "Admin", Contraseña: "password123". Puedes usar este usuario para ingresar por primera vez.

- Módulo de Clientes: Ve a la sección de clientes para gestionar los registros de clientes. Puedes agregar nuevos clientes, ver sus detalles, editar registros existentes y eliminar clientes.

- Módulo de Artículos: Visita la sección de artículos para gestionar los artículos de la tienda. Puedes agregar nuevos artículos, ver sus detalles, actualizar información y eliminar artículos.

- Módulo de Factura: Dirígete a la sección de facturas para crear e imprimir facturas. Genera nuevas facturas, ve sus detalles e imprime cada una según sea necesario.



- Registro de Usuarios: Puedes registrar un nuevo usuario. Proporciona la información requerida para crear una nueva cuenta de usuario.

## Contacto
Si tienes alguna pregunta o necesitas ayuda adicional o te interera mi trabajo puedes contactarme por:

### Email
Dirección: fernando_ajset@hotmail.com

### Redes sociales:
- LinkedIn: https://www.linkedin.com/in/edgar-fernando-ajset-nimacach%C3%A9-3a52951ba/
- Facebook: https://www.facebook.com/hackferajset
- Instagram: https://www.instagram.com/fer_ajset/

## Donaciones
Si el proyecto es util para tí y puedes aportar en mi desarrollo, tu donación es muy valiosa.

Pues hacerla aquí: https://paypal.me/EAjset?locale.x=es_XC

¡Gracias por utilizar el repositorio "Basic Store"! Espero que te resulte útil y disfrutes trabajando con el proyecto. ¡Feliz codificación!
