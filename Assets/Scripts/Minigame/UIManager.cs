using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct UIManagerParameters
{
    [Header("Answers Options")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBGColor;
    public Color CorrectBGColor { get { return correctBGColor; } }

    [SerializeField] Color incorrectBGColor;
    public Color IncorrectBGColor { get { return incorrectBGColor; } }

    [SerializeField] Color finalBGColor;
    public Color FinalBGColor { get { return finalBGColor; } }
}

[Serializable]
public struct UIElements
{
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswersContentArea { get { return answersContentArea; } }

    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }

    [SerializeField] Image questionImage;
    public Image QuestionImage { get { return questionImage; } }

    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }

    [Space]

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

}

public class UIManager : MonoBehaviour
{
    public enum ResolutionScreenType { Correct, Incorrect, Finish }

    [Header("References")]
    [SerializeField] GameEvents events;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] AnswersData answerPrefab;

    [SerializeField] UIElements uiElements;

    [Header("Finish Screen")]
    [SerializeField] GameObject finishPanel;
    [SerializeField] TextMeshProUGUI finalScoreText;

    [Space]
    [SerializeField] UIManagerParameters mParameters;

    List<AnswersData> currentAnswers = new List<AnswersData>();
    int resStateParaHash = 0;

    IEnumerator IE_DisplayTimedResolution = null;

    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreenUI += DisplayResolution;
        events.ScoreUpdatedUI += UpdateScoreUI;
    }
    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreenUI -= DisplayResolution;
        events.ScoreUpdatedUI -= UpdateScoreUI;
    }
    void Start()
    {
        UpdateScoreUI();
        resStateParaHash = Animator.StringToHash("ScreenState");
    }
    void UpdateQuestionUI(Question question)
    {
        uiElements.QuestionInfoTextObject.text = question.Info;
        uiElements.QuestionImage.sprite = question.QuestionImage;

        uiElements.QuestionImage.color = Color.black;

        CreateAnswers(question);
    }
    public void ResetQuestionImageColor()
    {
        uiElements.QuestionImage.color = Color.white;
    }

    void DisplayResolution(ResolutionScreenType type, int score)
    {

        if (type == ResolutionScreenType.Finish)
        {
            uiElements.MainCanvasGroup.blocksRaycasts = false;

            finishPanel.SetActive(true);
            finalScoreText.text = "Final Score: " + score;

            return;
        }

        uiElements.MainCanvasGroup.blocksRaycasts = false;

        if (IE_DisplayTimedResolution != null)
        {
            StopCoroutine(IE_DisplayTimedResolution);
        }

        IE_DisplayTimedResolution = DisplayTimedResolution();
        StartCoroutine(IE_DisplayTimedResolution);
    }

    IEnumerator DisplayTimedResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);

        uiElements.MainCanvasGroup.blocksRaycasts = true;
    }
    IEnumerator CalculateScore()
    {
        var scoreValue = 0;

        while (scoreValue < events.CurrentFinalScore)
        {
            scoreValue++;

            yield return null;
        }
    }
    void CreateAnswers(Question question)
    {
        EraseAnswers();

        float offset = 0 - mParameters.Margins;

        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswersData newAnswer = Instantiate(answerPrefab, uiElements.AnswersContentArea);
            newAnswer.UpdateData(question.Answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= newAnswer.Rect.sizeDelta.y + mParameters.Margins;
            uiElements.AnswersContentArea.sizeDelta = new Vector2(uiElements.AnswersContentArea.sizeDelta.x, offset * -1);

            currentAnswers.Add(newAnswer);
        }
    }
    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            Destroy(answer.gameObject);
        }
        currentAnswers.Clear();
    }
    void UpdateScoreUI()
    {
        uiElements.ScoreText.text = "Score: " + events.CurrentFinalScore;
    }
}