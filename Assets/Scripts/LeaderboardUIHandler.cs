using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;

    LeaderboardRankings[] leaderboardRankings;

    [Obsolete]
    void Awake()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        leaderboardRankings = new LeaderboardRankings[carLapCounterArray.Length];

        for (int i = 0; i < carLapCounterArray.Length; i++)
        {
            GameObject leaderboardRankingsGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);

            leaderboardRankings[i] = leaderboardRankingsGameObject.GetComponent<LeaderboardRankings>();

            leaderboardRankings[i].SetPositionText($"{i + 1}.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        for (int i = 0; i < lapCounters.Count; i++)
        {
            leaderboardRankings[i].SetNameText(lapCounters[i].gameObject.name);
        }
    }
}
