using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExperimentTypeEnum { AP, SPT, TP, CM }
public enum MazeNameEnum { JP, VE, TP, A, B, CM }
public class MazeSettings {
	public MazeNameEnum MazeName;
	public bool Arrows = false;
	public bool Reverse = false;
	public bool Rotate = false;
	public bool Pause = false;
	public bool SingleIntro = false;
	public bool MultiIntro = false;
	public string StartLocationCM = null; // Refactor for CM-specific script?
	public string LandmarkGoalCM = null;
}

public class ExperimentSettings {
	public string ParticipantID;
	public string ExperimenterInitials;
	public DateTime Date;
	public ExperimentTypeEnum ExperimentType;
	public MazeSettings MazeSettings;
	public string FileDir;
	public string FileName;

	//singleton:	
	private static ExperimentSettings _instance;

	public static ExperimentSettings GetInstance() {
		if (_instance == null) _instance = new ExperimentSettings();
		return _instance;
	}

	private ExperimentSettings(){}
}



