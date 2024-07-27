using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using TMPro;

public class RaceResults : MonoBehaviourPunCallbacks
{
    public static RaceResults Instance;

    public TMP_Text resultsText;
    public GameObject resultsPanel;

    private Dictionary<Player, float> raceTimes = new Dictionary<Player, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckResults()
    {
        raceTimes.Clear();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("HasFinished", out object hasFinishedObj) && (bool)hasFinishedObj)
            {
                if (player.CustomProperties.TryGetValue("RaceTime", out object raceTimeObj))
                {
                    raceTimes.Add(player, (float)raceTimeObj);
                }
            }
            else
            {
                raceTimes.Add(player, -1f);  // Indicate that the player hasn't finished
            }
        }

        DisplayResults();
    }

    private void DisplayResults()
    {
        string results = "Race Results:\n";
        resultsText.text = results;
        resultsPanel.SetActive(true);

        foreach (var entry in raceTimes)
        {
            string playerName = entry.Key.NickName;
            float raceTime = entry.Value;
            string resultTime;

            if (raceTime >= 0)
            {
                string minutes = Mathf.Floor(raceTime / 60).ToString("00");
                string seconds = (raceTime % 60).ToString("00.00");
                resultTime = minutes + ":" + seconds;
            }
            else
            {
                resultTime = "--";
            }

            results += playerName + ": " + resultTime + "\n";
        }

        resultsText.text = results;
        resultsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("HasFinished"))
        {
            Debug.Log("Player property updated: " + targetPlayer.NickName);
            CheckResults();
        }
    }
}