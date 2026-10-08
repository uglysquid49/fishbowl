using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuizGameManager : MonoBehaviour
{
    #region Variables
    private Question[] _questions = null;
    public Question[] Questions { get { return _questions; } }

    [SerializeField] GameEvents events = null;

    private List<AnswersData> PickedAnswers = new List<AnswersData>();
    private List<int> FinishedQuestions = new List<int>();

    private int currentQuestion = 0;

    private bool IsFinished
    {
        get
        {
            return FinishedQuestions.Count >= Questions.Length;
        }
    }
    #endregion

    #region Unity Methods
    void OnEnable()
    {
        events.UpdateAnswerUI += UpdateAnswers;
    }
    void OnDisable()
    {
        events.UpdateAnswerUI -= UpdateAnswers;
    }
    void Awake()
    {
        events.CurrentFinalScore = 0;
    }
    void Start()
    {
        events.StartupHighscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        LoadQuestions();

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        foreach (var question in Questions)
        {
            Debug.Log(question.Info);
        }
        Display();
    }
    #endregion

    #region Answer Methods
    public void UpdateAnswers(AnswersData newAnswer)
    {
        if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);

            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }
    void ShowAnswerResult()
    {
        List<int> correctAnswers = Questions[currentQuestion].GetCorrectAnswers();

        foreach (var answer in PickedAnswers)
        {
            if (correctAnswers.Contains(answer.AnswerIndex))
            {
                answer.SetCorrect();
            }
            else
            {
                answer.SetIncorrect();
            }
        }

        foreach (var answer in FindObjectsByType<AnswersData>(FindObjectsSortMode.None))
        {
            if (correctAnswers.Contains(answer.AnswerIndex))
            {
                answer.SetCorrect();
            }
        }
    }
    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswersData>();
    }
    #endregion

    #region Question Methods
    void Display()
    {
        EraseAnswers();

        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else
        {
            Debug.LogWarning("Something went wrong while trying to display new Question UI Data. GameEvents.UpdateQuestionUI is null.");
        }
    }
    Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();

        currentQuestion = randomIndex;

        return Questions[currentQuestion];
    }
    int GetRandomQuestionIndex()
    {
        var random = 0;

        if (FinishedQuestions.Count < Questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, Questions.Length);
            }
            while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }

        return random;
    }
    void LoadQuestions()
    {
        Object[] objs = Resources.LoadAll("Questions", typeof(Question));

        _questions = new Question[objs.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            _questions[i] = (Question)objs[i];
        }
    }
    #endregion

    #region Answer Checking
    public void Accept()
    {
        bool isCorrect = CheckAnswers();

        ShowAnswerResult();

        FinishedQuestions.Add(currentQuestion);

        UpdateScore(isCorrect ? Questions[currentQuestion].AddScore : -Questions[currentQuestion].AddScore);

        if (IsFinished)
        {
            SetHighscore();
            return;
        }

        StartCoroutine(WaitTillNextRound());
    }
    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }
    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> correctAnswers = Questions[currentQuestion].GetCorrectAnswers();
            List<int> pickedAnswers = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var missingAnswers = correctAnswers.Except(pickedAnswers).ToList();
            var wrongAnswers = pickedAnswers.Except(correctAnswers).ToList();

            return !missingAnswers.Any() && !wrongAnswers.Any();
        }
        return false;
    }
    #endregion

    #region Score Methods
    void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if (events.ScoreUpdatedUI != null)
        {
            events.ScoreUpdatedUI();
        }
    }
    void SetHighscore()
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        if (highscore < events.CurrentFinalScore)
        {
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, events.CurrentFinalScore);
        }
    }
    #endregion

    #region Next Question
    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);

        Display();
    }
    #endregion
}