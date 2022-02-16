using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UnityEngine;


//Maneja el cambio de comportamientos del perro
//En resumen, el perro tiene una lista dinamica de ratas cercanas
//Cuando hay suficientes ratas, el perro empieza a huir
public class PerpepcionPerro : MonoBehaviour
{
    //Numero maximo de ratas hasta que el perro empieza a huir
    [SerializeField]
    int maxRats;

    //Numero de ratas que han entrado en rango
    int ratsInRange = 0;

    //Lista de ratas en rango
    List<GameObject> rats;

    Seguir seguir;
    Huir huir;
    LlegadaDinamica llegada;

    // Start is called before the first frame update
    void Start()
    {
        seguir = gameObject.GetComponent<Seguir>();
        huir = gameObject.GetComponent<Huir>();
        llegada = gameObject.GetComponent<LlegadaDinamica>();
        rats = new List<GameObject>();
    }

    //Cuando una rata entra al rango
    private void OnTriggerEnter(Collider other)
    {
        //Comprobamos que sea una rata (es el unico objeto con componente Merodeo)
        GameObject o = other.gameObject;
        if (o.GetComponent<Merodear>())
        {
            //Añadimos la rata a la lista
            rats.Add(o);
            ratsInRange++;

            //Si hay x ratas en rango ya se empieza a ir
            if (ratsInRange > maxRats)
            {
                //Elige la rata mas cercana para huir de ella
                huir.objetivo = minDistance();

                //Desactiva el resto de comportamientos
                seguir.enabled = false;
                llegada.enabled = false;
            }            
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

    //Cuando una rata sale de rango
    private void OnTriggerExit(Collider other)
    {
        //Comprobamos que el objeto sea una rata
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
