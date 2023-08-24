using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Score score;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private Image background;

    private void Awake()
    {
        background = GetComponent<Image>();
    }
    public void UpdateDisplay()
    {
        playerNameText.text = score.playerName;
        scoreText.text = score.score.ToString();
    }

    public void ShowBackground()
    {
        Color color = background.color;
        color.a = 0.8f;
        background.color = color;
    }

    public void HideBackground()
    {
        Color color = background.color;
        color.a = 0f;
        background.color = color;
    }
}
