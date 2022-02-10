using UnityEngine;

namespace UCM.IAV.Movimiento
{

    /// <summary>
    /// Clase para modelar el comportamiento de merodear por el mapa
    /// </summary>
    public class Merodear : ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        [SerializeField]
        float velocidadMax;
        [SerializeField]
        float rotacionMax;
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();

            // Obtiene la velocidad del vector de orientacion
            direccion.lineal = velocidadMax * agente.OriToVec(agente.orientacion);

            // Cambia la orientacion aleatoriamente
            direccion.angular = Random.Range(-1.0f, 1.0f) * rotacionMax;
            
            return direccion;
        }
    }
}
