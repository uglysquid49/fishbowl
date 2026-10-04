using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;

public class QuizGameManager : MonoBehaviour
{
    Question[] _questions = null;
    public Question[] Questions { get { return _questions; } }

    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;
    private int score;

    void Display()
    {

    }

}
