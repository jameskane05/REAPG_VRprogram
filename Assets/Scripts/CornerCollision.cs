using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerCollision : MonoBehaviour {

    public int cornerNum;

    private void OnTriggerEnter(Collider other)
    {
		ExperimentSettings _expInstance = ExperimentSettings.GetInstance ();
		if(_expInstance.MazeSettings.Rotate || _expInstance.MazeSettings.Pause){
			Debug.Log(gameObject.transform.position);
			MazeController.cornerTransform = gameObject.transform;
			gameObject.SetActive(false);
			//Debug.Log(gameObject.ToString());
			MazeController.onCorner = cornerNum;
			MazeController.cornerEvent = true;
		}

    }


}

