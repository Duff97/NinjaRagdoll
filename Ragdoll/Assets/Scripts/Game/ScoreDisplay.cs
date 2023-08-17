using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public Score score;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void UpdateDisplay()
    {
        playerNameText.text = score.playerName;
        scoreText.text = score.score.ToString();
    }
}
