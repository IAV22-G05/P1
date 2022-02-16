using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCM.IAV.Movimiento;


//Maneja por input el cambio de comportamientos de la rata
public class ControlRata : MonoBehaviour
{

    Merodear merodear;
    Seguir seguir;
    LlegadaDinamica llegada;
    // Start is called before the first frame update
    void Start()
    {
        merodear = GetComponent<Merodear>();
        seguir = GetComponent<Seguir>();
        llegada = GetComponent<LlegadaDinamica>();
    }

    // Update is called once per frame
    void Update()
    {
        //Si se da al espacio se desactiva el merodeo y se activa seguimiento y llegada
        if(Input.GetKeyDown(KeyCode.Space))
        {
            merodear.enabled = false;
            llegada.enabled = true;
            seguir.enabled = true;
        }
        //Si se levanta el espacio se resetea
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            merodear.enabled = true;
            llegada.enabled = false;
            seguir.enabled = false;
        }
    }
}
