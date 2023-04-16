using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Modal.GetInstance();
    }

    private void OnDestroy()
    {
        Modal.Destroy();    
    }

    // Update is called once per frame
    void Update()
    {
        Modal.GetInstance().Update();
    }
}
