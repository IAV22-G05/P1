# INTELIGENCIA ARTIFICIAL PARA VIDEOJUEGOS - PRÁCTICA 1 - GRUPO 05

Sergio Molinero Aparicio - Andrés Carnicero Esteban

Video de pruebas: https://youtu.be/saQC4jCFhf8

En el proyecto encontramos varios elementos que deben interactuar entre sí, simulando 
el comportamiento del flautista de hamelín con unas ratas.

El primero es el que controla el jugador
-Flautista: Movimiento por el input del jugador y llamada a a las ratas con la flauta

El resto son agentes inteligentes
-Perro: Sigue al jugador con un algoritmo de persecución, parándose con un algoritmo de llegada cuando está cerca. El perro huye cuando hay demasiadas ratas.

-Ratas: Merodean cuando el jugador no está tocando la flauta y se acercan a él en formación con un algoritmo de seguimiento cuando la toca, se pararán al acercarse con un
algoritmo de llegada.

## PUNTO DE PARTIDA
Estas son las clases ya implementadas al comienzo de la práctica.

### Clase Dirección:

Representa la dirección que corrige el movimiento dinámicamente.

Consta de un float con la velocidad angular y un Vector3 con la lineal.

Variables de clase:

      float angular
      Vector3 lineal

### Clase Agente:

Contiene los datos necesarios para crear los agentes y manejar su comportamiento.
 
Tiene 3 funciones básicas que se encargan de la actualización de las velocidades, rotaciones y posiciones del propio agente.

#### -Update
aplica los movimientos de forma cinemática, actualizando velocidades lineales, angulares y orientación del agente.

#### -FixedUpdate
aplica los movimientos de forma dinámica, usando fuerzas y aceleraciones, el resultado son movimientos algo más realistas y físicos que usando Update.

#### -LateUpdate
se usa para corregir y ajustar movimientos finales, limitar máximas velocidades, restear variables para su uso en próximas iteraciones o elegir direcciones prioritarias en caso de que haya varias.

#### Funciones auxiliares
Esta clase usa una serie de funciones como setters y getters para facilitar el uso y obtencion de determinados valores.
##### -SetDireccion(Direccion direccion)
##### -SetDireccion(Direccion direccion, float peso)
##### -SetDireccion(Direccion direccion, int prioridad)
##### -Direccion GetPrioridadDireccion()
##### -Vec3 OriToVec(float orientacion)


        
        
### Clase ComportamientoAgente:

Es la clase base de todos los comportamientos que se implementarán a continuación, seguimiento, huída...

Dispone de 2 métodos principales 

#### -Update
Es un método que simplemente asigna la dirección que dicta cada comportamiento hijo. Hace elecciones en función de la prioridad.
      
#### -Direccion GetDireccion
Este método es uno virtual "Vacío" que redefine cada comportamiento hijo, es en el que se define cada algortimo de comportamiento.
      
      
      
## Comportamientos
En esta sección se describen los algoritmos que usaremos y en qué agentes se usarán.

Para resolver los diferentes problemas se han decidido usar sólo comportamientos dinámicos, para crear movimientos más fluidos y realistas.


### Seguimiento (dinámico)
Se aplica sobre el perro

    GameObject objetivo
    Agente agente
    float aceleracionMax

function GetDireccion() -> Direccion:
    
    // Se tiene en cuenta la velocidad actual del agente y se aplica sólo la aceleración necesaria
    Vector3 deltaV = targetVelocity - body.velocity
    Vector3 accel = deltaV / Time.deltaTime

    Direccion direccion = new Direccion()
    direccion.lineal *= accel

    // Podríamos meter una rotación automática en la dirección del movimiento, si quisiéramos
    direccion.rotation = 0
    return direccion

### Huida (dinámica)
Se aplica sobre el perro

Variables de clase:

    GameObject objetivo
    Agente agente
    float aceleracionMax

function GetDireccion() -> Direccion:
    
    // Se tiene en cuenta la velocidad actual del agente y se aplica sólo la aceleración necesaria
    Vector3 deltaV = body.velocity - targetVelocity
    Vector3 accel = deltaV / Time.deltaTime

    Direccion direccion = new Direccion()
    direccion.lineal *= accel

    // Podríamos meter una rotación automática en la dirección del movimiento, si quisiéramos
    direccion.rotation = 0
    return direccion
    
### Llegada (dinámica)
Se aplica sobre el perro

Variables de clase:

    GameObject objetivo
    Agente agente
    float velocidadMax
    float aceleracionMax
    float radioObjetivo
    float radioDeceleracion
    float tiempoObjetivo = 0.1
    float RapidezObjetivo    
    Vector3 VelocidadObjetivo

function GetDireccion() -> Direccion:
    
    Direccion direccion = new Direccion()

    // Obtiene la direccion al objetivo    
    direccion.lineal = objetivo.transform.position - transform.position

    // Comprueba si estamos dentro del radio
    if direccion.lineal.magnitude < radioObjetivo:
      return null
    
    // Si está fuera del radio de deceleración se mueve a velocidad máxima
    if direccion.lineal.magnitude > radioDeceleracion:
      RapidezObjetivo = velocidadMax
    
    // Si no calcula una velocidad escalada
    else 
      RapidezObjetivo = velocidadMax * direccion.lineal.magnitude / radioDeceleracion
      
    // Combinación de rapidez y direccion
    VelocidadObjetivo = direccion
    VelocidadObjetivo.normalize()
    VelocidadObjetivo *= RapidezObjetivo
    
    // Aceleración hacia la velocidad objetivo
    direccion.lineal = VelocidadObjetivo - agente.velocidad
    direccion.lineal /= tiempoObjetivo

    // Comprueba si la aceleración es demasiado rápida
    if direccion.lineal.length() > aceleracionMax:
      direccion.lineal.normalize()
      direccion.lineal *= aceleracionMax

    direccion.angular = 0
    return direccion
    
### Merodeo (Dinámico)
Se aplica sobre la manada de ratas

Variables de clase:

    Agente agente
    float velocidadMax
    float rotacionMax 

function GetDireccion() -> Direccion:
    
    Direccion direccion = new Direccion()

    // Obtiene la velocidad del vector de orientacion
    direccion.lineal = velocidadMax * agente.OriToVec(agente.orientacion)
    
    // Cambia la orientacion aleatoriamente
    direccion.angular = random() * rotacionMax

    return direccion
    
    
## COMPORTAMIENTOS AÑADIDOS 
Se han añadido los siguientes scripts para cumplir con los requisitos de la práctica

### Percepción Perro
Es un script que maneja los cambios de comportamientos del perro

El perro tiene una lista dinamica de ratas cercanas, cuando hay suficientes ratas, el perro empieza a huir.

Variables de clase

    //Numero maximo de ratas hasta que el perro empieza a huir
    int maxRats;
    //Numero de ratas que han entrado en rango
    int ratsInRange = 0;
    //Lista de ratas en rango
    List<GameObject> rats;
    
OnTriggerEnter (Cuando una rata entra en rango)
    
      //Comprobamos que sea una rata (es el unico objeto con componente Merodeo)
      if (es rata)
      {
            //Añadimos la rata a la lista
            rats.Add(rata)
            ratsInRange++

            //Si hay x ratas en rango ya se empieza a ir
            if (ratsInRange > maxRats)
            {
                  //Elige la rata mas cercana para huir de ella
                   huir objetivo = rata mas cercana -> Funcion auxiliar

                  //Desactiva el resto de comportamientos
                  desactiva seguir
                  desactiva llegada
            }            
       }
       
       
OnTriggerExit (Cuando una rata sale entra en rango)

        //Comprobamos que el objeto sea una rata
        if (es rata)
        {
            //Restamos el numero de ratas
            ratsInRange--

            //Comprobar si hay suficientes ratas para que el perro huya
            if (ratsInRange < maxRats)
            {
                quitar objetivo huir
                
                //Importante el orden de activacion porque se pueden pisar comportamientos
                activar llegada
                activar seguir
            }

            //Eliminar las ratas de la lista
            while ()
            {
               Encontrar la rata que ha salido
            }

            //La eliminamos de la lista
            rats.RemoveAt(rata que ha salido);
        }
        
### Separacion
Maneja el movimiento en formación de las ratas cuando siguen al flautista
Variables de clase
      
      List<GameObject> objetivos
      float distanciaMin
      float coeficiente
      float acelMax
      
Direccion GetDireccion()
      
      Direccion total = new Direccion
      Direccion direccion = new Direccion
      float fuerza
      
      //Recorrer la lista de objetivos y si es necesario añade distancia de separación entre las ratas
      for(objetivos)
            distancia =  rata y otra rata de la lista
            
            if(distancia es menor que el minimo)
                  empujar a esta rata el minimo entre dividir el coeficiente entre la distancia al cuadrado o la aceleración máxima
            
      return total
            
### ControlRata
Maneja los cambios de comportamientos de las ratas

Update (en cada frame)
      
      if(pulsandoEspacio)
            merodear.enabled = false
            llegada.enabled = true
            seguir.enabled = true
      else
            merodear.enabled = true
            llegada.enabled = false
            seguir.enabled = false


## RENDIMIENTO

Tras realizar pruebas con distintos números de ratas, se han obtenido estas cantidades medias de fotogramas por segundo:

10 ratas -> 400 fps
25 ratas -> 360 fps
50 ratas -> 250 fps
100 ratas -> 175 fps
1000 ratas -> 25 fps


## REFERENCIAS

Artificial Intelligence for Games (3a Edición) - Ian Millington


## RECURSOS UTILIZADOS

Modelos de:
Árbol: https://sketchfab.com/3d-models/giant-low-poly-tree-acfd2b7f80894848b56c2ac8e7e59572
Paisano: https://sketchfab.com/3d-models/low-poly-character-rigged-b2ca55ab706c4b1ba1c6475d8b3c5bfd

Audio:
https://www.youtube.com/watch?v=V10SLqij2UM&t=26s


