using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] obj1 = GameObject.FindGameObjectsWithTag("EventSystem");
        if (obj1.Length > 0)
        {
            // this is already loaded, destroy this backup!
            Destroy(gameObject);
        
            GameObject[] obj2 = GameObject.FindGameObjectsWithTag("EventSystemExtra");
            if (obj2.Length > 0)
            {
                foreach (GameObject extra in obj2) {
                    // this is already loaded, destroy these backups!
                    Destroy(extra.gameObject);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
