using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public InputField ParticipantID;
    public Dropdown ExperimentType;
    public InputField ExperimenterInitials;
    public Dropdown DateM;
    public Dropdown DateD;
    public Dropdown DateY;

	public Dropdown CMStart;
	public Dropdown CMLandmark;

    public GameObject FirstPanel;
    public GameObject APPanel;
    public GameObject SPTPanel;
	public GameObject TPPanel;
	public GameObject CMPanel;

    private void Start()
    {
        // Comment out later, for testing only
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("ExperimentType"))) {
            FirstPanel.SetActive(false);
            if (PlayerPrefs.GetString("ExperimentType") == "AP") APPanel.SetActive(true);
            else if (PlayerPrefs.GetString("ExperimentType") == "SPT") SPTPanel.SetActive(true);
            else if (PlayerPrefs.GetString("ExperimentType") == "TP") TPPanel.SetActive(true);
			else if (PlayerPrefs.GetString("ExperimentType") == "CM") CMPanel.SetActive(true);
        }
    }

    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
        if (Input.GetKey(KeyCode.E) && FirstPanel.activeSelf == false) SceneManager.LoadScene("Menu");
    }

    public void ClickStart() {
        // check if button clicked:  Debug.Log("Start clicked!");
        // to get text field:  Debug.Log(ParticipantID.text);
        // to take screengrabs:  ScreenCapture.CaptureScreenshot(@"C:\Users\james\Desktop\ss.png");

        // Debug.Log(ExperimentType);
        // Debug.Log(ExperimentType.captionText);
        Debug.Log(ExperimentType.captionText.text);

        PlayerPrefs.SetString("ParticipantID", ParticipantID.text);
        PlayerPrefs.SetString("ExperimentType", ExperimentType.captionText.text);
        PlayerPrefs.SetString("ExperimenterInitials", ExperimenterInitials.text);
        PlayerPrefs.SetString("Date", DateM.captionText.text + "-" + DateD.captionText.text + "-" + DateY.captionText.text);
        SetDir();

        if (ExperimentType.captionText.text == "AP") APPanel.SetActive(true);
        else if (ExperimentType.captionText.text == "SPT") SPTPanel.SetActive(true);
		else if (ExperimentType.captionText.text == "TP") TPPanel.SetActive(true);
		else if (ExperimentType.captionText.text == "CM") CMPanel.SetActive(true);
    }

    static public void SetDir()
    {
        string newDir = System.IO.Directory.GetCurrentDirectory() + "\\" + PlayerPrefs.GetString("ParticipantID") + "_" + PlayerPrefs.GetString("ExperimentType") + "_" + PlayerPrefs.GetString("Date");
        System.IO.Directory.CreateDirectory(newDir);
        PlayerPrefs.SetString("dir", newDir);
    }

    public void JoystickPractice() {
        PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.SetInt ("Arrows", 0);
        SceneManager.LoadScene("Joystick Practice");
    }

    public void VisuomotorExpertise()
    {
        PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.SetInt ("Arrows", 0);
        SceneManager.LoadScene("Visuomotor Expertise Maze");
    }

    public void TaskPractice_WithArrows()
    {
        PlayerPrefs.SetString("Direction", "F");
        PlayerPrefs.SetInt("Arrows", 1);
        SceneManager.LoadScene("Task Practice");
    }

    public void TaskPractice_NoArrows()
    {
        PlayerPrefs.SetString("Direction", "F");
        PlayerPrefs.SetInt("Arrows", 0);
        SceneManager.LoadScene("Task Practice");
    }

    public void MazeA_NoArrows_Fwd()
    {
        PlayerPrefs.SetInt("Arrows", 0);
        PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.DeleteKey ("Intro");
        SceneManager.LoadScene("Experimental Maze A");
    }

    public void MazeA_NoArrows_Rev()
    {
        PlayerPrefs.SetInt("Arrows", 0);
        PlayerPrefs.SetString("Direction", "R");
		PlayerPrefs.DeleteKey ("Intro");
        SceneManager.LoadScene("Experimental Maze A");
    }

    public void MazeB_NoArrows_Fwd()
    {
        PlayerPrefs.SetInt("Arrows", 0);
        PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.DeleteKey ("Intro");
        SceneManager.LoadScene("Experimental Maze B");
    }

    public void MazeB_NoArrows_Rev()
    {
        PlayerPrefs.SetInt("Arrows", 0);
        PlayerPrefs.SetString("Direction", "R");
		PlayerPrefs.DeleteKey ("Intro");
        SceneManager.LoadScene("Experimental Maze B");
    }


    // LOAD AP SCENES

    public void AP_MazeA_WithArrows_Fwd()
    {
        PlayerPrefs.SetInt("Arrows", 1);
        PlayerPrefs.SetString("Direction", "F");
        SceneManager.LoadScene("Experimental Maze A");
    }

    public void AP_MazeB_WithArrows_Fwd()
    {
        PlayerPrefs.SetInt("Arrows", 1);
        PlayerPrefs.SetString("Direction", "F");
        SceneManager.LoadScene("Experimental Maze B");
    }


    // LOAD SPT SCENES

	public void SPT_TaskPractice_WithArrows_Rotate()
	{
		PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.SetInt("Arrows", 1);
		PlayerPrefs.SetString("RotateOrPause", "Rotate");
		SceneManager.LoadScene("Task Practice");
	}

	public void SPT_TaskPractice_WithArrows_Pause()
	{
		PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.SetInt("Arrows", 1);
		PlayerPrefs.SetString("RotateOrPause", "Pause");
		SceneManager.LoadScene("Task Practice");
	}

    public void SPT_MazeA_WithArrows_Rotate () {
        PlayerPrefs.SetInt("Arrows", 1);
        PlayerPrefs.SetString("Direction", "F");
        PlayerPrefs.SetString("RotateOrPause", "Rotate");
        SceneManager.LoadScene("Experimental Maze A");
    }

    public void SPT_MazeA_WithArrows_Pause()
    {
        PlayerPrefs.SetInt("Arrows", 1);
        PlayerPrefs.SetString("Direction", "F");
        PlayerPrefs.SetString("RotateOrPause", "Pause");
        SceneManager.LoadScene("Experimental Maze A");
    }

    public void SPT_MazeB_WithArrows_Rotate()
    {
        PlayerPrefs.SetInt("Arrows", 1);
        PlayerPrefs.SetString("Direction", "F");
        PlayerPrefs.SetString("RotateOrPause", "Rotate");
        SceneManager.LoadScene("Experimental Maze B");
    }

    public void SPT_MazeB_WithArrows_Pause()
    {
        PlayerPrefs.SetInt("Arrows", 1);
        PlayerPrefs.SetString("Direction", "F");
        PlayerPrefs.SetString("RotateOrPause", "Pause");
        SceneManager.LoadScene("Experimental Maze B");
    }


    // LOAD TP SCENES

	public void TP_TaskPractice_NoArrows_1Pic()
	{
		PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.SetInt("Arrows", 0);
		PlayerPrefs.SetString("Intro", "Single");
		SceneManager.LoadScene("Task Practice");
	}

	public void TP_TaskPractice_NoArrows_8Pics()
	{
		PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.SetInt("Arrows", 0);
		PlayerPrefs.SetString("Intro", "Multi");
		SceneManager.LoadScene("Task Practice");
	}

    public void TP_MazeA_NoArrows_1Pic() {
        PlayerPrefs.SetInt("Arrows", 0);
        PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.SetString("Intro", "Single");
        SceneManager.LoadScene("Experimental Maze A");
    }

	public void TP_MazeA_NoArrows_8Pics()
    {
        PlayerPrefs.SetInt("Arrows", 0);
        PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.SetString("Intro", "Multi");
        SceneManager.LoadScene("Experimental Maze A");
    }

	public void TP_MazeB_NoArrows_1Pic()
    {
        PlayerPrefs.SetInt("Arrows", 0);
        PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.SetString("Intro", "Single");
        SceneManager.LoadScene("Experimental Maze B");
    }

	public void TP_MazeB_NoArrows_8Pics()
    {
        PlayerPrefs.SetInt("Arrows", 0);
        PlayerPrefs.SetString("Direction", "F");
        PlayerPrefs.SetString("Intro", "Multi");
        SceneManager.LoadScene("Experimental Maze B");
    }

	public void CM_TaskPractice_StudyExplore() {
		PlayerPrefs.SetInt("Arrows", 0);
		PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.DeleteKey ("CMLandmark");
		SceneManager.LoadScene ("Task Practice");
	}

	public void CM_TaskPractice_Test() {
		PlayerPrefs.SetInt("Arrows", 0);
		PlayerPrefs.SetString("Direction", "F");
		PlayerPrefs.SetString("CMLandmark", "true");
		SceneManager.LoadScene ("Task Practice");
	}

	public void CM_StudyExplore() {
		PlayerPrefs.SetInt("Arrows", 0);
		PlayerPrefs.SetString("CMStart", CMStart.captionText.text);
		PlayerPrefs.DeleteKey ("CMLandmark");
		SceneManager.LoadScene ("Experimental Maze CM");
	}

	public void CM_TestTrial() {
		PlayerPrefs.SetInt("Arrows", 0);
		PlayerPrefs.SetString("CMStart", CMStart.captionText.text);
		PlayerPrefs.SetString("CMLandmark", CMLandmark.captionText.text);
		SceneManager.LoadScene ("Experimental Maze CM");
	}

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}