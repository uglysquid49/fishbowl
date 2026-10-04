using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/new GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Question question);
    public UpdateQuestionUICallback UpdateQuestionUI;

    public delegate void UpdateAnswerUICallback(Answer pickedanswer);
    public UpdateAnswerUICallback UpdateAnswerUI;

    public delegate void DisplayResolutionScreenCallback(UIManager.ResolutionScreenType type);
    public DisplayResolutionScreenCallback DisplayResolutionScreenUI;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback ScoreUpdatedUI;

    public int CurrentFinalScore;
}
