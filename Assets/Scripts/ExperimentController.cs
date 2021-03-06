using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour {

    [SerializeField] private InputField ParticipantIDInput;
	[SerializeField] private Dropdown ExperimentTypeDropdown;
    [SerializeField] private Dropdown SessionDropdown;
    [SerializeField] private InputField ExperimenterInitialsInput;
	[SerializeField] private Dropdown StartLocationCMDropdown;
	[SerializeField] private Dropdown LandmarkGoalCMDropdown;
	[SerializeField] private GameObject FirstPanel;
	[SerializeField] private GameObject APPanel;
	[SerializeField] private GameObject SPTPanel;
	[SerializeField] private GameObject TPPanel;
	[SerializeField] private GameObject CMPanel;

	public static ExperimentSettings _expInstance;

    private void Start()
    {
		_expInstance = ExperimentSettings.GetInstance ();
		_expInstance.MazeSettings = new MazeSettings ();

		if (!string.IsNullOrEmpty(_expInstance.ExperimenterInitials))  // if this already exists then we're coming out of a maze
			OpenSubmenu ();
		else {
			PopulateDropdown ();
		}
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Escape)) 
			Application.Quit();
        if (Input.GetKey(KeyCode.E) && FirstPanel.activeSelf == false)
			SceneManager.LoadScene(0);
    }

	public void PopulateDropdown() {
		string[] ExperimentTypeEnumNames = Enum.GetNames(typeof(ExperimentTypeEnum));
		List<string> ExperimentTypes = new List<string> (ExperimentTypeEnumNames);
		ExperimentTypeDropdown.AddOptions (ExperimentTypes);
	}

	public void EnterExperimentInfo() {
		_expInstance.ParticipantID = ParticipantIDInput.text;
		_expInstance.ExperimenterInitials = ExperimenterInitialsInput.text;
		_expInstance.ExperimentType = (ExperimentTypeEnum)ExperimentTypeDropdown.value;
        _expInstance.Session = (int)SessionDropdown.value + 1;
        Debug.Log(_expInstance.Session);
        _expInstance.Date = DateTime.Now;
		SetDir (_expInstance);
		OpenSubmenu();
    }

	void OpenSubmenu() {
		// Cursor is disabled coming out of maze scenes
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		// QUESTION: Could this be simplified? Still have to line up the right menu w/ the experiment type selected.
		FirstPanel.SetActive (false);
        if (ExperimentSettings.IsAP()) APPanel.SetActive(true);
        else if (ExperimentSettings.IsSPT()) SPTPanel.SetActive(true);
        else if (ExperimentSettings.IsTP()) TPPanel.SetActive(true);
        else if (ExperimentSettings.IsCM()) CMPanel.SetActive(true);
	}

	public void SetDir(ExperimentSettings _expInstance)
    {
		string currentDir = System.IO.Directory.GetCurrentDirectory ();
        string newDir = "reapg" + _expInstance.ParticipantID +
            "_" + _expInstance.ExperimentType +
            "_Sess" + _expInstance.Session.ToString();
        _expInstance.FileDir = currentDir + "\\" + newDir + "_" + DateTime.Now.ToString("ddHHmm");
		System.IO.Directory.CreateDirectory(_expInstance.FileDir);
        _expInstance.FileName = _expInstance.FileDir + "\\" + newDir;
    }

    public void EnableSPTStudyConditions() {
        if (_expInstance.ExperimentType == ExperimentTypeEnum.ExpSPT_PSE) _expInstance.MazeSettings.Pause = true;
        else if (_expInstance.ExperimentType == ExperimentTypeEnum.ExpSPT_ROT) _expInstance.MazeSettings.Rotate = true;
    }

    public void EnableTPStudyConditions() {
        if (_expInstance.ExperimentType == ExperimentTypeEnum.ExpTP_SEQ) _expInstance.MazeSettings.MultiIntro = true;
        else if (_expInstance.ExperimentType == ExperimentTypeEnum.ExpTP_SIM) _expInstance.MazeSettings.SingleIntro = true;
    }

    public void EnableCMStudyConditions() { }


    public void EnableArrows () { _expInstance.MazeSettings.Arrows = true; }
	public void EnableReverse () { _expInstance.MazeSettings.Reverse = true; }
	public void SelectStartCM () { _expInstance.MazeSettings.StartLocationCM = StartLocationCMDropdown.captionText.text; }
	public void SelectLandmarkCM () { _expInstance.MazeSettings.LandmarkGoalCM = LandmarkGoalCMDropdown.captionText.text; }



	public void LoadMazeJoystickPractice () {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.JP;
        SceneManager.LoadScene(1);
    }

	public void LoadMazeVisuomotorExpertise () {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.VME;
		SceneManager.LoadScene(2);
    }

	public void LoadMazeTaskPractice () {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.TP;
		SceneManager.LoadScene(3);
	}

	public void LoadMazeA() {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.A;
		SceneManager.LoadScene(4);
	}

	public void LoadMazeB() {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.B;
		SceneManager.LoadScene(5);
	}

	public void LoadMazeCM() {
		_expInstance.MazeSettings.MazeName = MazeNameEnum.CM;
		SceneManager.LoadScene(6);
	}
}