using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MazeCtrl : MonoBehaviour {

    public Text text;
    public GameObject player;
    public GameObject arrows;
    public GameObject startHall;
    public GameObject endHall;
    public GameObject above;
    public Transform startLoc;
    public Transform endLoc;
    static public bool hasEnded;
    public bool hasStarted;
    static public Material pathColor;
    static public int onCorner = 0;
    static public float totalTime;
    static public bool cornerEvent = false;
    static bool grabAndFaceForward = false;
    static bool rotateThreeSixty = false;
    static bool checkForFinishedRot = false;
    public Transform[] Rotations;
    public int pauseDuration = 8;
    static Scene scene;

    static bool cornerActive;
    public Sprite[] introImgs;
    static GameObject subjectInstance;

    static UnityStandardAssets.Characters.FirstPerson.FirstPersonController controller;

    private Vector3 lastPos;
    private Vector3 currentPos;
    private int picCounter;
    private bool startCorner = false;
    static private float totalDistance;
    static private float avgVelocity;
    static private List<string> path;
    static public Transform cornerTransform;
	public GameObject[] landmarks;
    
    void Start() {

        scene = SceneManager.GetActiveScene();

        // Arrow settings
		if (PlayerPrefs.GetInt ("Arrows") == 1) {
			text.text = "Please follow the arrows.\r\n" +
				"\r\n" +
				"Hit the spacebar to begin.";
			arrows.SetActive (true);
		}
		else if (PlayerPrefs.GetInt ("Arrows") == 0  && scene.name != "Joystick Practice" && scene.name != "Visuomotor Expertise Maze") {
			text.text = "Please recreate the route.\r\n" +
			"\r\n" +
			"Hit the spacebar to begin.";

			arrows.SetActive (false);
		}

		if (PlayerPrefs.HasKey ("Intro")) {
			arrows.SetActive (false);
			text.text = "Please try to remember the order and turn direction.\r\n"
				+ "\r\n" +
				"Hit the spacebar to begin.";
		} 

		if (PlayerPrefs.GetString ("ExperimentType") == "CM") {

			if (PlayerPrefs.HasKey ("CMLandmark")) {
				text.text = "Please locate the landmark.\r\n" +
				"\r\n" +
				"Hit the spacebar to begin.";
			} else if (!PlayerPrefs.HasKey ("CMLandmark") && scene.name != "Visuomotor Expertise Maze" && scene.name != "Joystick Practice") {
				text.text = "Please explore the environment.\r\n" +
				"\r\n" +
				"Hit the spacebar to begin.";
			}

			if (scene.name == "Experimental Maze CM") {
				string startName = PlayerPrefs.GetString ("CMStart");
				GameObject startLoc = GameObject.Find (startName);
				Instantiate (player, startLoc.transform);
				if (PlayerPrefs.HasKey ("CMLandmark")) {
					string landmarkName = PlayerPrefs.GetString ("CMLandmark");
					GameObject landmark = GameObject.Find (landmarkName);
					GameObject colliderObj = landmark.transform.GetChild (0).gameObject;
					colliderObj.AddComponent<End> ();
				}
			} else if (scene.name == "Task Practice") {
				Instantiate (player, startLoc.transform);
				if (PlayerPrefs.HasKey ("CMLandmark")) {
					landmarks [0].AddComponent<End> ();
					text.text = "Please locate the landmark.\r\n" +
						"\r\n" +
						"Hit the spacebar to begin.";
				} else {
					text.text = "Please explore the environment.\r\n" +
						"\r\n" +
						"Hit the spacebar to begin.";
				}

			} else if (scene.name == "Joystick Practice") {
				Instantiate (player, startLoc.transform);
			} else if (scene.name == "Visuomotor Expertise Maze") {
				Instantiate (player, startLoc.transform);
				endHall.AddComponent<End> ();
				text.text = "Please navigate to the end of the hall as quickly as possible\r\n" +
					"\r\n" +
					"Hit the spacebar to begin.";
			}
		}

		else if (PlayerPrefs.GetString ("ExperimentType") != "CM" && PlayerPrefs.HasKey ("Direction")) {
			if (PlayerPrefs.GetString ("Direction") == "F") {
				Instantiate (player, startLoc.transform);
				if (scene.name != "Joystick Practice") endHall.AddComponent<End> (); 
			}
			else if (PlayerPrefs.HasKey ("Direction") && PlayerPrefs.GetString ("Direction") == "R") {
				Instantiate (player, endLoc.transform);
				startHall.AddComponent<End> ();
			}
		}

		else
        {  // must be during dev, go w/ default
            Instantiate(player, startLoc.transform);
            if (scene.name != "Joystick Practice") endHall.AddComponent<End>();
        }

        GameObject subjectInstance = GameObject.Find("SubjectControllerWithTrail(Clone)");
        controller = subjectInstance.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        lastPos = subjectInstance.transform.position;
        currentPos = subjectInstance.transform.position;
        controller.enabled = false;
        totalDistance = 0;
        totalTime = 0;
        path = new List<string>();
        hasEnded = false;
        hasStarted = false;
        picCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && !hasEnded)
        {
            Debug.Log("E pressed, calling MazeEnd() and loading menu");
            MazeEnd();
            SceneManager.LoadScene("Menu");
        }

        else if (Input.GetKeyDown(KeyCode.E) && hasEnded)
        {
            Debug.Log("E pressed, not calling MazeEnd() and loading menu");
            SceneManager.LoadScene("Menu");
        }

        if (Input.GetKeyDown("space") && !hasStarted) {

            //if statements checking for intro section once space has been pressed
			if (PlayerPrefs.HasKey("Intro")) {
	            if (PlayerPrefs.GetString("Intro") == "Multi")
	            {
	                StartCoroutine(MultiPicIntro());
	            }
	            else if (PlayerPrefs.GetString("Intro") == "Single")
	            {
	                StartCoroutine(SinglePicIntro());
	            }
			}

            else {
                StartCoroutine(WaitAndStart());
                InvokeRepeating("TrackPathEverySecond", 2.0f, 1.0f);
                controller.enabled = true;
            }
        }

        // Spacebar pressed and start conditions set, trial has begun, do the following:
        if (hasStarted && !hasEnded) {
            subjectInstance = GameObject.Find("SubjectControllerWithTrail(Clone)");
            currentPos = subjectInstance.transform.position;
            totalDistance += Vector3.Distance(currentPos, lastPos);
            totalTime += Time.deltaTime;
            lastPos = currentPos;
        }
    }

    private void LateUpdate()
    {
        if (cornerEvent)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("RotateOrPause")))
            {
				if (PlayerPrefs.GetString ("RotateOrPause") == "Pause") {
					StartCoroutine (PauseWithOptions (pauseDuration, enableCtrlAfter: true));
				}
                if (PlayerPrefs.GetString("RotateOrPause") == "Rotate")
                {
                    controller.enabled = false;
                    StartCoroutine(PauseWithOptions(3, enableCtrlAfter: false, grabAfter: true));
                }
            }

            cornerEvent = false;
        }

        if (grabAndFaceForward) GrabAndFaceForward();
        if (rotateThreeSixty) RotateThreeSixty();
    }

    public void GrabAndFaceForward()
    {
        controller.enabled = false;
        text.text = "";

        subjectInstance.transform.position = Vector3.MoveTowards(
            subjectInstance.transform.position,
            new Vector3(cornerTransform.transform.position.x, cornerTransform.transform.position.y + 1, cornerTransform.transform.position.z),
            Time.deltaTime / 3
        );

        Rotations[onCorner].rotation = Quaternion.Euler(Rotations[onCorner].rotation.eulerAngles);

        subjectInstance.transform.rotation = Quaternion.Lerp(
            subjectInstance.transform.rotation,
            Rotations[onCorner].rotation,
            Time.deltaTime / 2
        );

        if (Mathf.Abs(Vector3.Distance(subjectInstance.transform.rotation.eulerAngles, Rotations[onCorner].rotation.eulerAngles)) <= 2f)
        {
            grabAndFaceForward = false;
            rotateThreeSixty = true;
        }
    }

    public void RotateThreeSixty()
    {
        controller.enabled = false;
        subjectInstance.transform.RotateAround(subjectInstance.transform.position, Vector3.up, 360 * Time.smoothDeltaTime / 30.0f);

        if (Mathf.RoundToInt(Mathf.Abs(subjectInstance.transform.rotation.eulerAngles.y - Rotations[onCorner].rotation.eulerAngles.y)) > 5f)
        {
            checkForFinishedRot = true;
        }
        if (checkForFinishedRot)
        {
            if (Mathf.Abs(subjectInstance.transform.rotation.eulerAngles.y - Rotations[onCorner].rotation.eulerAngles.y) < 1f)
            {
                rotateThreeSixty = false;
                controller.enabled = true;
                checkForFinishedRot = false;
            }
        }
    }

    IEnumerator WaitAndStart()
    {
        text.text = "+";
        yield return new WaitForSeconds(1);
        Image img = GameObject.Find("Panel").GetComponent<Image>();
        img.color = UnityEngine.Color.clear;
        hasStarted = true;
        text.text = "";
		InvokeRepeating("TrackPathEverySecond", 2.0f, 1.0f);
		controller.enabled = true;
    }

    IEnumerator WaitToCheckDistance() {
        yield return new WaitForSeconds(2f);

    }

    public void TrackPathEverySecond()
    {
        int second = Mathf.RoundToInt(totalTime);
        path.Add( second.ToString() + ": " + subjectInstance.transform.position);
    }

	IEnumerator PauseWithOptions(int seconds, bool enableCtrlAfter = true, bool grabAfter = false, bool rotateAfter = false, string textAfter = "")
    {
        controller.enabled = false;
        Debug.Log("Pause");
        yield return new WaitForSeconds(seconds);
        Debug.Log("Pause over");
        controller.enabled = enableCtrlAfter;
        grabAndFaceForward = grabAfter;
        rotateThreeSixty = rotateAfter;
		text.text = textAfter;
    }

    IEnumerator SinglePicIntro()
    {
        text.text = "";
        Image img = GameObject.Find("Panel").GetComponent<Image>();
        img.color = UnityEngine.Color.white;
		if (scene.name == "Task Practice") picCounter = 2;
		else picCounter = 8; // magic number for montage pic
        img.sprite = introImgs[picCounter];
		yield return new WaitForSeconds(introImgs.Length * 3);
        img.sprite = null;
        img.color = UnityEngine.Color.black;
        StartCoroutine(WaitAndStart());
    }

    IEnumerator MultiPicIntro()
    {
        text.text = "";
        Image img = GameObject.Find("Panel").GetComponent<Image>();
		while (picCounter <= introImgs.Length - 2)
        {
            img.color = UnityEngine.Color.white;
            img.sprite = introImgs[picCounter];
            yield return new WaitForSeconds(3);
            picCounter++;
        }

        img.color = UnityEngine.Color.black;
        StartCoroutine(WaitAndStart());
    }

    static public void MazeEnd()
    {
        hasEnded = true;
        controller.enabled = false;

        Text text = GameObject.Find("Text").GetComponent<Text>();
        text.text = "You have reached the end.";
        TrailRenderer trail = GameObject.Find("Trail").GetComponent<TrailRenderer>();
        Material mat = Resources.Load("TurquoiseSmooth") as Material;
        trail.material = mat;

        float avgVelocity = totalDistance / totalTime;

        string[] lines = {
            "Participant ID: " + PlayerPrefs.GetString("ParticipantID"),
            "Experiment Type: " + PlayerPrefs.GetString("ExperimentType"),
            "Experimenter Initials: " + PlayerPrefs.GetString("ExperimenterInitials"),
            "Maze: " + scene.name,
            "Date: " + PlayerPrefs.GetString("Date"),
            "Distance: " + totalDistance,
            "Time: " + totalTime,
            "Avg. Velocity: " + avgVelocity,
        };

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("dir")))
        {
            string localDir = PlayerPrefs.GetString("dir") + "\\";
            localDir += PlayerPrefs.GetString("ParticipantID") + "-" + PlayerPrefs.GetString("ExperimentType") + "-";
            localDir += scene.name + "-";

			if (!string.IsNullOrEmpty(PlayerPrefs.GetInt("Arrows").ToString()) && scene.name != "Experimental Maze CM")
            {
                if (PlayerPrefs.GetInt("Arrows") == 0) localDir += "NoArrows-";
                else localDir += "WithArrows-";
            }

			if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Direction")) && scene.name != "Experimental Maze CM")
            {
                if (PlayerPrefs.GetString("Direction") == "F") localDir += "Fwd";
                else localDir += "Rev";
            }

            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("RotateOrPause"))) localDir += "-" + PlayerPrefs.GetString("RotateOrPause");

            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Intro"))) localDir += "-" + PlayerPrefs.GetString("Intro");

			if (scene.name == "Experimental Maze CM") {
				localDir += "Start" + PlayerPrefs.GetString ("CMStart") + "-";
				if (PlayerPrefs.HasKey("CMLandmark")) {
					localDir += "Landmark" + PlayerPrefs.GetString("CMLandmark") + "-";
				}
			}

            if (!PlayerPrefs.HasKey(localDir)) PlayerPrefs.SetInt(localDir, 1);
            else PlayerPrefs.SetInt(localDir, PlayerPrefs.GetInt(localDir) + 1);
            localDir += "-Trial" + PlayerPrefs.GetInt(localDir);

            if (System.IO.Directory.Exists(localDir))
            {
                System.IO.File.WriteAllLines(localDir + "-coords.txt", path.ToArray());
                System.IO.File.WriteAllLines(localDir + ".txt", lines);
                TakePhoto(scene.name);
            }

            else
            {
                MenuScript.SetDir();
                System.IO.File.WriteAllLines(localDir + "-coords.txt", path.ToArray());
                System.IO.File.WriteAllLines(localDir + ".txt", lines);
                TakePhoto(scene.name);
            }

            PlayerPrefs.DeleteKey("Arrows");
            PlayerPrefs.DeleteKey("RotateOrPause");
            PlayerPrefs.DeleteKey("Direction");
            PlayerPrefs.DeleteKey("Rotate");
        }
    }

    static public void TakePhoto(string sceneName)
    {
        Camera cam = GameObject.Find("Overhead Cam").GetComponent<Camera>();
        RenderTexture currentRT = RenderTexture.active;
        var rTex = new RenderTexture(cam.pixelHeight, cam.pixelHeight, 16);
        cam.targetTexture = rTex;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D tex = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        tex.Apply(false);
        RenderTexture.active = currentRT;
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);
        string localDir = PlayerPrefs.GetString("dir") + "\\";
        localDir += PlayerPrefs.GetString("ParticipantID") + "-" + PlayerPrefs.GetString("ExperimentType") + "-";
        localDir += sceneName + "-";

		if (!string.IsNullOrEmpty(PlayerPrefs.GetInt("Arrows").ToString()) && scene.name != "Experimental Maze CM")
        {
            if (PlayerPrefs.GetInt("Arrows") == 0) localDir += "NoArrows-";
            else localDir += "WithArrows-";
        }

		if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Direction")) && scene.name != "Experimental Maze CM")
        {
            if (PlayerPrefs.GetString("Direction") == "F") localDir += "Fwd";
            else localDir += "Rev";
        }

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("RotateOrPause"))) localDir += "-" + PlayerPrefs.GetString("RotateOrPause");

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Intro"))) localDir += "-" + PlayerPrefs.GetString("Intro");

		if (scene.name == "Experimental Maze CM") {
			localDir += "Start" + PlayerPrefs.GetString ("CMStart") + "-";
			if (PlayerPrefs.HasKey("CMLandmark")) {
				localDir += "Landmark" + PlayerPrefs.GetString("CMLandmark") + "-";
			}
		}

        if (!PlayerPrefs.HasKey(localDir)) PlayerPrefs.SetInt(localDir, 1);
        else PlayerPrefs.SetInt(localDir, PlayerPrefs.GetInt(localDir));

        localDir += "-Trial" + PlayerPrefs.GetInt(localDir);

        System.IO.File.WriteAllBytes(localDir + "-path.png", bytes);
    }

    void OnApplicationQuit() {
        PlayerPrefs.DeleteAll();
    }

}