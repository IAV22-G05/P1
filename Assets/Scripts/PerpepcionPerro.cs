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
    List<GameObject> rats;

    // Start is called before the first frame update
    void Start()
    {
        seguir = gameObject.GetComponent<Seguir>();
        huir = gameObject.GetComponent<Huir>();
        rats = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject o = other.gameObject;
        if (o.GetComponent<Merodear>())
        {
            //if (ratsInRange == 0)
            //    huir.objetivo = o;

            rats.Add(o);
            ratsInRange++;
            Debug.Log("Entra " + ratsInRange);
            if (ratsInRange > maxRats)
            {
                huir.objetivo = rats[0];
                seguir.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject o = other.gameObject;
        if (o.GetComponent<Merodear>())
        {
            ratsInRange--;
            Debug.Log("Sale " + ratsInRange);

            if (huir.objetivo && ratsInRange == 0)
            {
                huir.objetivo = null;
                seguir.enabled = true;
            }

            int i = 0;
            bool found = false;
            while(!found && i < rats.Count)
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
