using UnityEngine;

namespace UCM.IAV.Movimiento
{

    /// <summary>
    /// Clase para modelar el comportamiento de merodear por el mapa
    /// </summary>
    public class LlegadaDinamica : ComportamientoAgente
    {
        float velocidadMax;
        [SerializeField]
        float aceleracionMax;
        [SerializeField]
        float radioObjetivo;
        [SerializeField]
        float radioDeceleracion;
        [SerializeField]
        float tiempoObjetivo = 0.1f;
        [SerializeField]
        float RapidezObjetivo;
        [SerializeField]
        Vector3 VelocidadObjetivo;

        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();

            // Obtiene la direccion al objetivo    
            direccion.lineal = objetivo.transform.position - transform.position;

            // Comprueba si estamos dentro del radio
            if (direccion.lineal.magnitude < radioObjetivo)
            {
                return null;
            }

            // Si est� fuera del radio de deceleraci�n se mueve a velocidad m�xima
            if (direccion.lineal.magnitude > radioDeceleracion)
                RapidezObjetivo = velocidadMax;

            // Si no calcula una velocidad escalada
            else
                RapidezObjetivo = velocidadMax * direccion.lineal.magnitude / radioDeceleracion;

            // Combinaci�n de rapidez y direccion
            VelocidadObjetivo = direccion.lineal;
            VelocidadObjetivo.Normalize();
            VelocidadObjetivo *= RapidezObjetivo;

            // Aceleraci�n hacia la velocidad objetivo
            direccion.lineal = VelocidadObjetivo - agente.velocidad;
            direccion.lineal /= tiempoObjetivo;

            // Comprueba si la aceleraci�n es demasiado r�pida
            if (direccion.lineal.magnitude > aceleracionMax)
                direccion.lineal.Normalize();
            direccion.lineal *= aceleracionMax;

            direccion.angular = 0;
            return direccion;
        }
    }
}
