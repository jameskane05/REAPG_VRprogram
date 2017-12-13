using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class End : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MazeEnd() being called due to maze completion");
        MazeCtrl.MazeEnd();
    }
    
}
