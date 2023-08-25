using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreBoard : NetworkBehaviour
{
    private readonly SyncList<Score> scores = new SyncList<Score>();
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

    public void HandleScoreChanged(SyncList<Score>.Operation op, int index, Score oldScore, Score newScore) => UpdateDisplay();

    public override void OnStartServer()
    {
        base.OnStartServer();
        foreach(var player in Room.GamePlayers)
        {
            Score score = new Score();
            score.playerName = player.displayName;
            score.netId = player.connectionToClient.identity;
            scores.Add(score);
            
        }
        UpdateDisplay();
        GameScore.OnPointGained += HandlePoints;
    }

    public override void OnStopServer()
    {
        GameScore.OnPointGained -= HandlePoints;
        base.OnStopServer();
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        scores.Callback += HandleScoreChanged;
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        scores.Callback -= HandleScoreChanged;
    }

    private void UpdateDisplay()
    {
        foreach(Transform child in scoreBoardPanel.transform)
        {
            Destroy(child.gameObject);
        }

        bool background = true;
        foreach (var score in scores)
        {
            ScoreDisplay scoreInstance = Instantiate(scoreDisplayPrefab);
            scoreInstance.score = score;
            scoreInstance.transform.SetParent(scoreBoardPanel.transform, false);
            scoreInstance.UpdateDisplay();

            if (background)
                scoreInstance.ShowBackground();
            else
                scoreInstance.HideBackground();

            background = !background;
        }
    }

    private void OnOpenScoreboard(InputValue inputValue)
    {
        scoreBoardPanel.SetActive(inputValue.isPressed);
    }

    private void HandlePoints(NetworkConnectionToClient playerConn, int points)
    {
        int scoreIndex = scores.FindIndex(Score => Score.netId.connectionToClient == playerConn);
        Score score = scores[scoreIndex];
        score.score += points;
        scores[scoreIndex] = score;

        if (points >= 0)
            ReorganizeScoreUpward(scoreIndex);
        else
            ReorganizeScoreDownward(scoreIndex);
    }

    private void ReorganizeScoreUpward(int i)
    {
        if (i > 0 && scores[i].score >= scores[i - 1].score)
            SwapScores(i, i - 1);
    }

    private void ReorganizeScoreDownward(int i)
    {
        if (i < scores.Count - 1 && scores[i].score < scores[i + 1].score)
            SwapScores(i, i + 1);
    }

    private void SwapScores(int i1, int i2)
    {
        Score tmpScore = scores[i1];
        scores[i1] = scores[i2];
        scores[i2] = tmpScore;
    }

    public string GetWinnerName()
    {
        return scores[0].playerName;
    }

}
