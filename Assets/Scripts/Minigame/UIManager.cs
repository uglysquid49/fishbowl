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

    [SerializeField] Animator resolutionScreenAnimator;
    public Animator ResolutionScreenAnimator { get { return resolutionScreenAnimator; } }

    [SerializeField] Image resolutionBG;
    public Image ResolutionBG { get { return resolutionBG; } }

    [SerializeField] TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }

    [Space]

    [SerializeField] TextMeshProUGUI highscoreText;
    public TextMeshProUGUI HighScoreText { get { return highscoreText; } }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

    [SerializeField] RectTransform finishUIElements;
    public RectTransform FinishUIElements { get { return finishUIElements; } }
}

public class UIManager : MonoBehaviour
{
    public enum ResolutionScreenType { Correct, Incorrect, Finish }

    [Header("References")]
    [SerializeField] GameEvents events;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] AnswersData answerPrefab;

    [SerializeField] UIElements uiElements;

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
        CreateAnswers(question);
    }

    void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResUI(type, score);

        uiElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 2);
        uiElements.MainCanvasGroup.blocksRaycasts = false;

        if (type != ResolutionScreenType.Finish)
        {
            if (IE_DisplayTimedResolution != null)
            {
                StopCoroutine(IE_DisplayTimedResolution);
            }

            IE_DisplayTimedResolution = DisplayTimedResolution();
            StartCoroutine(IE_DisplayTimedResolution);
        }
    }

    IEnumerator DisplayTimedResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);

        uiElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 1);
        uiElements.MainCanvasGroup.blocksRaycasts = true;
    }

    void UpdateResUI(ResolutionScreenType type, int score)
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        switch (type)
        {
            case ResolutionScreenType.Correct:
                uiElements.ResolutionBG.color = mParameters.CorrectBGColor;
                uiElements.ResolutionStateInfoText.text = "CORRECT!";
                uiElements.ResolutionScoreText.text = "+" + score;
                break;

            case ResolutionScreenType.Incorrect:
                uiElements.ResolutionBG.color = mParameters.IncorrectBGColor;
                uiElements.ResolutionStateInfoText.text = "WRONG!";
                uiElements.ResolutionScoreText.text = "-" + score;
                break;

            case ResolutionScreenType.Finish:
                uiElements.ResolutionBG.color = mParameters.FinalBGColor;
                uiElements.ResolutionStateInfoText.text = "FINAL SCORE";
                StartCoroutine(CalculateScore());
                uiElements.FinishUIElements.gameObject.SetActive(true);
                uiElements.HighScoreText.gameObject.SetActive(true);
                uiElements.HighScoreText.text = "Highscore: " + highscore;
                break;
        }
    }

    IEnumerator CalculateScore()
    {
        var scoreValue = 0;

        while (scoreValue < events.CurrentFinalScore)
        {
            scoreValue++;
            uiElements.ResolutionScoreText.text = scoreValue.ToString();

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