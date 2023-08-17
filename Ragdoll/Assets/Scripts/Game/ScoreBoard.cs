using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreBoard : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleScoreChanged))] private List<Score> scores = new List<Score>();
    [SerializeField] private KeyCode scoreKey = KeyCode.Tab;
    [SerializeField] private GameObject scoreBoardPanel;
    [SerializeField] private ScoreDisplay scoreDisplayPrefab;

    private NetworkManagerNinjaRagdoll room;
    private NetworkManagerNinjaRagdoll Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerNinjaRagdoll;
        }
    }

    public void HandleScoreChanged(List<Score> oldValue, List<Score> newValue) => UpdateDisplay();

    public override void OnStartServer()
    {
        base.OnStartServer();
        int i = 0;
        foreach(var player in Room.GamePlayers)
        {
            Debug.Log("Score instancated on server");
            Score score = ScriptableObject.CreateInstance("Score") as Score;
            score.indice = i;
            score.playerName = player.displayName;
            scores.Add(score);
            
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        foreach (var score in scores)
        {
            Debug.Log("Score display updated on client");
            ScoreDisplay scoreInstance = Instantiate(scoreDisplayPrefab);
            scoreInstance.score = score;
            scoreInstance.transform.SetParent(scoreBoardPanel.transform, false);
            scoreInstance.UpdateDisplay();
        }
    }

    private void OnGUI()
    {
        scoreBoardPanel.SetActive(Input.GetKey(scoreKey));
    }
}
