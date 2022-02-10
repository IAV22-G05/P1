# INTELIGENCIA ARTIFICIAL PARA VIDEOJUEGOS - PRÁCTICA 1

En el proyecto encontramos varios elementos que deben interactuar entre sí, simulando 
el comportamiento del flautista de hamelín con unas ratas.

El primero es el que controla el jugador
-Flautista: Movimiento por el input del jugador y llamada a a las ratas con la flauta

El resto son agentes inteligentes
-Perro: Sigue al jugador con un algoritmo de persecución, parándose con un algoritmo de llegada cuando está cerca. El perro huye cuando hay demasiadas ratas.

-Ratas: Merodean cuando el jugador no está tocando la flauta y se acercan a él en formación con un algoritmo de seguimiento cuando la toca, se pararán al acercarse con un
algoritmo de llegada.

## Breve explicación del código implementado en el punto de partida:

### Clase Dirección:

Representa la dirección que corrige el movimiento dinámicamente.

Consta de un float con la velocidad angular y un Vector3 con la lineal.

Variables de clase:

      float angular
      Vector3 lineal

### Clase Agente:

Contiene los datos necesarios para crear los agentes y manejar su comportamiento.
    
Variables de clase:

      bool combinarPorPeso
      bool combinarPorPrioridad
      float umbralPrioridad = 0.2f
      float velocidadMax
      float rotacionMax
      float aceleracionMax
      float aceleracionAngularMax
      
      Vector3 velocidad
      float rotacion
      float orientacion
      
      Direccion direccion
      Dictionary<int, List<Direccion>> grupos

      Rigidbody cuerpoRigido
      
   function FixedUpdate() -> void:
      
      // Si el agente es cinemático no se aplica el movimiento dinámico      
      if cuerpoRigido == null:       
                return

      // Límite de aceleración máxima que acepta el agente (normalmente vendrá limitada)      
      if direccion.lineal.sqrMagnitude > aceleracionMax:      
          direccion.lineal = direccion.lineal.normalized * aceleracionMax

      // Aplicación de la fuerza
      cuerpoRigido.AddForce(direccion.lineal, ForceMode.Acceleration)

      // Límite de aceleración angular máxima que acepta el agente (normalmente vendrá limitada)
      if direccion.angular > aceleracionAngularMax:
          direccion.angular = aceleracionAngularMax

      // Rotación del objeto sobre su eje Y (hacia arriba)
      cuerpoRigido.AddTorque(transform.up * direccion.angular, ForceMode.Acceleration)

      // Limitación de la velocidad lineal 
      if cuerpoRigido.velocity.magnitude > velocidadMax:
          cuerpoRigido.velocity = cuerpoRigido.velocity.normalized * velocidadMax

      // Limitación de la velocidad angular 
      if cuerpoRigido.angularVelocity.magnitude > rotacionMax:
          cuerpoRigido.angularVelocity = cuerpoRigido.angularVelocity.normalized * rotacionMax
      if cuerpoRigido.angularVelocity.magnitude < -rotacionMax:
          cuerpoRigido.angularVelocity = cuerpoRigido.angularVelocity.normalized * -rotacionMax
    
  function Update() -> void:
   
      // Si el agente es dinámico no se aplica el movimiento cinemático   
      if cuerpoRigido != null:
              return 

      // Limitación de la velocidad lineal
      if velocidad.magnitude > velocidadMax:
            velocidad= velocidad.normalized * velocidadMax

     // Limitación de la velocidad angular
      if rotacion > rotacionMax:
          rotacion = rotacionMax
      if rotacion < -rotacionMax:
          rotacion = -rotacionMax
      
      // Se aplica el movimiento
      Vector3 desplazamiento = velocidad * Time.deltaTime
      transform.Translate(desplazamiento, Space.World)

      orientacion += rotacion * Time.deltaTime
      // Mantiene la orientación en el rango de 0 a 360 grados
      if orientacion < 0.0f:
          orientacion += 360.0f
      else if orientacion > 360.0f:
          orientacion -= 360.0f

      // Elimina la rotación, antes de rotar el objeto lo que marque la variable orientación
      transform.rotation = new Quaternion()
      transform.Rotate(Vector3.up, orientacion)
      
  function LateUpdate() -> void:
      
      // Si el agente tiene dirección prioritaria, se le aplica
      if (combinarPorPrioridad)
          direccion = GetPrioridadDireccion()
          grupos.Clear()
      
      // Si el agente es dinámico no se aplica el movimiento cinemático
      if (cuerpoRigido != null) 
          return       

      // Límite de aceleración máxima que acepta el agente (normalmente vendrá limitada)
      if direccion.lineal.sqrMagnitude > aceleracionMax
          direccion.lineal = direccion.lineal.normalized * aceleracionMax

      // Límite de aceleración angular máxima que acepta el agente (normalmente vendrá limitada)
      if (direccion.angular > aceleracionAngularMax)
          direccion.angular = aceleracionAngularMax;

      // Calcula la velocidad y rotación en función de las aceleraciones  
      velocidad += direccion.lineal * Time.deltaTime
      rotacion += direccion.angular * Time.deltaTime

      /// Encarar el agente hacia donde se dirige
      transform.LookAt(transform.position + velocidad)

      // Deja la dirección vacía para el próximo fotograma
      direccion = new Direccion()
      
  function SetDireccion(Direccion direccion) -> void:
      
      // Establece la dirección a seguir por el agente
      this.direccion = direccion
    
  function SetDireccion(Direccion direccion, float peso) -> void:
      
      // Establece la dirección a seguir por el agente según el peso
      this.direccion.lineal += (peso * direccion.lineal);
      this.direccion.angular += (peso * direccion.angular);
     
  function SetDireccion(Direccion direccion, int prioridad)
          
      // Establece la dirección a seguir por el agente según la prioridad
      if !grupos.ContainsKey(prioridad):          
          grupos.Add(prioridad, new List<Direccion>())

      grupos[prioridad].Add(direccion)
    
  function GetPrioridadDireccion() -> Direccion
      
      // Devuelve la dirección calculada por prioridad
      Direccion direccion = new Direccion()
      List<int> gIdList = new List<int>(grupos.Keys)
      gIdList.Sort()
      foreach int gid in gIdList:
          direccion = new Direccion()
          
          foreach (Direccion direccionIndividual in grupos[gid]):
              // Dentro del grupo la mezcla es por peso
              direccion.lineal += direccionIndividual.lineal
              direccion.angular += direccionIndividual.angular
          
          // Si el resultado supera un umbral, se devuelve ese resultado
          if direccion.lineal.magnitude > umbralPrioridad || Mathf.Abs(direccion.angular) > umbralPrioridad:
              return direccion
          
      return direccion
      
  function OriToVec(float orientacion) -> Vector3
          
      // Calcula el Vector3 según un valor de orientación
      Vector3 vector = Vector3.zero
      vector.x = Mathf.Sin(orientacion * Mathf.Deg2Rad)
      vector.z = Mathf.Cos(orientacion * Mathf.Deg2Rad)
      return vector.normalized
        
### Clase ComportamientoAgente:

Plantilla para los comportamientos de los agentes.

Variables de clase:

       
      float peso = 1.0f
      int prioridad = 1;
      GameObject objetivo
      Agente agente
      
  function Update() -> void
      
      // Establece la dirección que corresponde al agente
      if agente.combinarPorPeso:
          agente.SetDireccion(GetDireccion(), peso)
      else if agente.combinarPorPrioridad:
          agente.SetDireccion(GetDireccion(), prioridad)
      else:
          agente.SetDireccion(GetDireccion())

  function GetDireccion() -> Direccion
      
      return new Direccion();
      
 function RadianesAGrados(float rotacion)
      
      // Mentiene la rotación en el rango de 360 grados
      rotacion %= 360.0f;
      if Mathf.Abs(rotacion) > 180.0f:
          if rotacion < 0.0f:
              rotacion += 360.0f
          else:
              rotacion -= 360.0f
      
      return rotacion
      

 function OriToVec(float orientacion) -> Vector3
          
      // Calcula el Vector3 según un valor de orientación
      Vector3 vector = Vector3.zero
      vector.x = Mathf.Sin(orientacion * Mathf.Deg2Rad)
      vector.z = Mathf.Cos(orientacion * Mathf.Deg2Rad)
      return vector.normalized
      
## Algoritmos:

### Seguimiento (cinemático)

Variables de clase
    
    GameObject objetivo
    Agente agente
    float velocidadMax

function GetDireccion() -> Direccion:
    
    Direccion direccion = new Direccion();
    
    // Obtiene la direccion al objetivo    
    direccion.lineal = objetivo.transform.position - transform.position
    direccion.lineal.Normalize()
    direccion.lineal *= agente.aceleracionMax    

    return direccion

### Seguimiento (dinámico)

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

### Huida (cinemática)

Variables de clase:
    
    GameObject objetivo
    Agente agente
    float velocidadMax

function GetDireccion() -> Direccion:
    
    Direccion direccion = new Direccion();
    
    // Obtiene la direccion al objetivo    
    direccion.lineal = transform.position - objetivo.transform.position 
    direccion.lineal.Normalize()
    direccion.lineal *= agente.aceleracionMax    

    return direccion


### Huida (dinámica)

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
    
### Llegada (cinemática)

Variables de clase:

    GameObject objetivo
    Agente agente
    float velocidadMax
    float radio
    float tiempoObjetivo = 0.25

function GetDireccion() -> Direccion:
    
    Direccion direccion = new Direccion()

    // Obtiene la direccion al objetivo    
    direccion.lineal = objetivo.transform.position - transform.position

    // Comprueba si estamos dentro del radio
    if direccion.lineal.magnitude < radio:
      return null

    // Mueve al agente según el tiempo de deceleración
    direccion.lineal /= tiempoObjetivo

    // Si va demasiado rápido se le aplica la velocidad máxima
    if direccion.lineal.magnitude > velocidadMax:
    direccion.lineal.normalize()
    direccion.lineal *= velocidadMax

    // Orienta al agente hacia donde se mueve
    agente.transform.LookAt(agente.transform.position + direccion.lineal)

    direccion.angular = 0
    return direccion
    
    ### Llegada (cinemática)

    GameObject objetivo
    Agente agente
    float velocidadMax
    float radio
    float tiempoObjetivo = 0.25
    
### Llegada (dinámica)

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
    
### Merodeo

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
