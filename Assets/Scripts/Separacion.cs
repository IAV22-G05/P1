using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class Separacion : ComportamientoAgente
    {
        [SerializeField]
        List<GameObject> objetivos;
        [SerializeField]
        float distancia;
        [SerializeField]
        float coeficiente;
        [SerializeField]
        float aceleracionMax;

        public override Direccion GetDireccion()
        {
            Direccion total = new Direccion();
            Direccion direccion = new Direccion();
            float fuerza;
            for(int i = 0; i < objetivos.Count; ++i)
            {
                direccion.lineal = objetivos[i].transform.position - transform.position;

                if(direccion.lineal.magnitude < distancia)
                {
                    fuerza = Mathf.Min(coeficiente / (Mathf.Pow(direccion.lineal.magnitude, 2)), aceleracionMax);
                    direccion.lineal.Normalize();
                    total.lineal += fuerza * direccion.lineal;
                }
            }
            total.lineal.y = 0;
            return total;
        }
    }
}
