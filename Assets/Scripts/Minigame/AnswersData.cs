using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnswersData : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI infoTextObject;
    [SerializeField] Image toggle;

    [Header("Textures")]
    [SerializeField] Sprite uncheckedToggle;
    [SerializeField] Sprite checkedToggle;

    [Header("References")]
    [SerializeField] GameEvents events;

    [Header("Answer Colors")]
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color correctColor = Color.green;
    [SerializeField] Color incorrectColor = Color.red;

    private RectTransform _rect;

    public RectTransform Rect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            }

            return _rect;
        }
    }

    private int _answerIndex = -1;

    public int AnswerIndex
    {
        get { return _answerIndex; }
    }

    private bool Checked = false;

    public void UpdateData(string info, int index)
    {
        infoTextObject.text = info;
        _answerIndex = index;
        Reset();
    }

    public void Reset()
    {
        Checked = false;
        toggle.color = normalColor;
        UpdateUI();
    }

    public void SwitchState()
    {
        Checked = !Checked;
        UpdateUI();

        if (events.UpdateAnswerUI != null)
        {
            events.UpdateAnswerUI(this);
        }
    }

    public void SetCorrect()
    {
        toggle.color = correctColor;
    }

    public void SetIncorrect()
    {
        toggle.color = incorrectColor;
    }

    void UpdateUI()
    {
        toggle.sprite = Checked ? checkedToggle : uncheckedToggle;
    }
}