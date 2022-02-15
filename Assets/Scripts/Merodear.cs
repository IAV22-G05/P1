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
        

        float timeToNext = 1.0f; // Timer para cambiar de orientación 
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            // Obtiene la velocidad del vector de orientacion
            direccion.lineal = velocidadMax * agente.OriToVec(agente.orientacion);
            if (timeToNext <= 0)
            {
                // Cambia la orientacion aleatoriamente
                agente.rotacion = Random.Range(-1.0f, 1.0f) * rotacionMax;
                timeToNext = 1.0f;
            }
            else
                timeToNext -= Time.deltaTime;
            
            return direccion;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.GetComponent<BoxCollider>())
            {
                agente.rotacion -= 180.0f;
            }
        }
    }
}
