using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UnityEngine;

public class PerpepcionPerro : MonoBehaviour
{
    [SerializeField]
    int maxRats;


    int ratsInRange = 0;
    Seguir seguir;
    Huir huir;
    LlegadaDinamica llegada;
    List<GameObject> rats;

    // Start is called before the first frame update
    void Start()
    {
        seguir = gameObject.GetComponent<Seguir>();
        huir = gameObject.GetComponent<Huir>();
        llegada = gameObject.GetComponent<LlegadaDinamica>();
        rats = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject o = other.gameObject;
        if (o.GetComponent<Merodear>())
        {

            //huir.objetivo = o;
            //seguir.enabled = false;
            //llegada.enabled = false;

            //if (ratsInRange == 0)
            //    huir.objetivo = o;

            rats.Add(o);
            ratsInRange++;
            //Debug.Log("Entra " + ratsInRange);

            //Si hay x ratas en rango ya se empieza a ir
            if (ratsInRange > maxRats)
            {
                huir.objetivo = minDistance();
                seguir.enabled = false;
                llegada.enabled = false;
            }


            //huir.objetivo = rats[0];
            
        }
    }

    //Recorremos la lista de ratas y nos quedamos con las mas cercana al perro
    private GameObject minDistance()
    {
        GameObject rat;

        rat = rats[0];
        Vector3 d1 = rat.transform.position - transform.position;
        float distance1 = d1.magnitude;

        if (rats.Count > 0)
        {
            for (int i = 1; i < rats.Count; i++)
            {
                Vector3 d2 = rats[i].transform.position - transform.position;
                float distance2 = d2.magnitude;

                if (distance1 < distance2)
                    rat = rats[i];

                distance1 = distance2;
            }
        }

        return rat;
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject o = other.gameObject;
        if (o.GetComponent<Merodear>())
        {
            //Restamos el numero de ratas
            ratsInRange--;

            //Comprobar si hay suficientes ratas para que el perro huya
            if (huir.objetivo && ratsInRange < maxRats)
            {
                Debug.Log("no hay ratas");
                huir.objetivo = null;
                llegada.enabled = true;
                seguir.enabled = true;
            }

            //Eliminar las ratas de la lista
            int i = 0;
            bool found = false;
            while (!found && i < rats.Count)
            {
                if (rats[i] == o)
                    found = true;
                else
                    i++;
            }

            rats.RemoveAt(i);
        }
    }
}
