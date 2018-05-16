using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExperimentTypeEnum { ExpTP_SEQ, ExpTP_SIM, ExpAP_SVY, ExpAP_RTE, ExpSPT_ROT, ExpSPT_PSE,  CM }
public enum MazeNameEnum { JP, VME, TP, A, B, CM }
public class MazeSettings {
	public MazeNameEnum MazeName;
    public bool Reverse = false;
    public bool ReachedEnd = false;

    public bool Arrows = false;
	public bool Rotate = false;
	public bool Pause = false;
	public bool SingleIntro = false;
	public bool MultiIntro = false;

	public string StartLocationCM = null; // Refactor for CM-specific script?
	public string LandmarkGoalCM = null;

    public string TrialName;
}

public class ExperimentSettings {
	public string ParticipantID;
	public string ExperimenterInitials;
	public DateTime Date;
	public ExperimentTypeEnum ExperimentType;
    public int Session = 0;
    public MazeSettings MazeSettings;
	public string FileDir;
	public string FileName;
    public IDictionary<string, int> TrialTracker = new Dictionary<string, int>();

    //singleton:	
    private static ExperimentSettings _instance;

	public static ExperimentSettings GetInstance() {
		if (_instance == null) _instance = new ExperimentSettings();
		return _instance;
	}
    
	private ExperimentSettings(){}

    public static void NameAndCountTrial() {
        _instance.MazeSettings.TrialName = _instance.MazeSettings.MazeName.ToString();
        if (ExperimentSettings.IsStudy() && !ExperimentSettings.IsPractice()) _instance.MazeSettings.TrialName += "_LearnT";

        else if (!ExperimentSettings.IsStudy() && !ExperimentSettings.IsPractice()) {
            if (_instance.MazeSettings.Reverse) _instance.MazeSettings.TrialName += "_FWDT";
            else _instance.MazeSettings.TrialName += "_REVT";
        }

        if (_instance.TrialTracker.ContainsKey(_instance.MazeSettings.TrialName)) _instance.TrialTracker[_instance.MazeSettings.TrialName] += 1;
        else _instance.TrialTracker[_instance.MazeSettings.TrialName] = 1;
    }

    public static bool IsPractice() {
        if (_instance.MazeSettings.MazeName == MazeNameEnum.JP ||
            _instance.MazeSettings.MazeName == MazeNameEnum.VME ) return true;
        else return false;
    }

    public static bool IsStudy() {
        if (_instance.MazeSettings.Arrows ||
            _instance.MazeSettings.Pause ||
            _instance.MazeSettings.Rotate ||
            _instance.MazeSettings.SingleIntro ||
            _instance.MazeSettings.MultiIntro 
       
            ) return true;
        else return false;
    }

    public static bool IsAP() {
        if (_instance.ExperimentType == ExperimentTypeEnum.ExpAP_RTE || _instance.ExperimentType == ExperimentTypeEnum.ExpAP_SVY) return true;
        else return false;
    }

    public static bool IsTP()
    {
        if (_instance.ExperimentType == ExperimentTypeEnum.ExpTP_SEQ || _instance.ExperimentType == ExperimentTypeEnum.ExpTP_SIM) return true;
        else return false;
    }

    public static bool IsSPT()
    {
        if (_instance.ExperimentType == ExperimentTypeEnum.ExpSPT_PSE || _instance.ExperimentType == ExperimentTypeEnum.ExpSPT_ROT) return true;
        else return false;
    }

    public static bool IsCM() {
        if (_instance.ExperimentType == ExperimentTypeEnum.CM) return true;
        else return false;
    }
}



