using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/new GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Question question);
    public UpdateQuestionUICallback UpdateQuestionUI;

    public delegate void UpdateAnswerUICallback(AnswersData pickedAnswer);
    public UpdateAnswerUICallback UpdateAnswerUI;

    public delegate void DisplayResolutionScreenCallback(UIManager.ResolutionScreenType type, int score);
    public DisplayResolutionScreenCallback DisplayResolutionScreenUI;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback ScoreUpdatedUI;

    [HideInInspector] 
    public int CurrentFinalScore;
    [HideInInspector]
    public int StartupHighscore;
}
