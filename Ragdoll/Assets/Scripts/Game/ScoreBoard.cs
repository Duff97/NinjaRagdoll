using Mirror;
using UnityEngine;

public class ScoreBoard : NetworkBehaviour
{
    private readonly SyncList<Score> scores = new SyncList<Score>();
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
        PlayerRespawn.OnPlayerRespawn += HandlePoints;
    }

    public override void OnStopServer()
    {
        PlayerRespawn.OnPlayerRespawn -= HandlePoints;
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
        foreach (var score in scores)
        {
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

    private void HandlePoints(NetworkConnectionToClient victimConn, NetworkConnectionToClient attackerConn)
    {
        if (attackerConn == null)
        {
            Debug.Log("Null attacker");
            int victimScoreIndex = scores.FindIndex(Score => Score.netId.connectionToClient == victimConn);
            Score victimScore = scores[victimScoreIndex];
            victimScore.score -= 1;
            scores[victimScoreIndex] = victimScore;
            ReorganizeScoreDownward(victimScoreIndex);
        }
        else
        {
            Debug.Log("Not Null attacker");
            int attackerScoreIndex = scores.FindIndex(Score => Score.netId.connectionToClient == attackerConn);
            Score attackerScore = scores[attackerScoreIndex];
            attackerScore.score += 1;
            scores[attackerScoreIndex] = attackerScore;
            ReorganizeScoreUpward(attackerScoreIndex);
        }
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
