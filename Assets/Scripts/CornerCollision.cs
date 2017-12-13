using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerCollision : MonoBehaviour {

    public int cornerNum;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.transform.position);
        MazeCtrl.cornerTransform = gameObject.transform;
        gameObject.SetActive(false);
        //Debug.Log(gameObject.ToString());
        MazeCtrl.onCorner = cornerNum;
        MazeCtrl.cornerEvent = true;
    }


}

