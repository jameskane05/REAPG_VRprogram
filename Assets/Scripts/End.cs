using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class End : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("MazeEnd() being called due to maze completion");
        ExperimentSettings _instance = ExperimentSettings.GetInstance();
        _instance.MazeSettings.ReachedEnd = true;
        MazeController.MazeEnd();
    }
    
}
