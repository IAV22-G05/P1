using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCM.IAV.Movimiento;


public class ControlRata : MonoBehaviour
{

    Merodear merodear;
    Seguir seguir;
    // Start is called before the first frame update
    void Start()
    {
        merodear = GetComponent<Merodear>();
        seguir = GetComponent<Seguir>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            merodear.enabled = false;
            seguir.enabled = true;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            merodear.enabled = true;
            seguir.enabled = false;
        }
    }
}
