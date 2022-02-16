using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugInput : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ratas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            for(int i = 1; i < ratas.Count; ++i)
            {
                ratas[i].SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            int i = ratas.Count - 1;
            while(i >= 0)
            {
                if (ratas[i].activeSelf) {
                    ratas[i].SetActive(false);
                    break;
                }
                i--;
            }
        }
    }
}
