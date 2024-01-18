using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionHandler : MonoBehaviour
{
    LeaderboardUIHandler leaderboardUIHandler;

    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();

    [System.Obsolete]
    private void Awake()
    {
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();

        foreach (CarLapCounter lapCounters in carLapCounters)
        {
            lapCounters.OnPassCheckpoint += OnPassCheckpoint;
        }

        leaderboardUIHandler = FindFirstObjectByType<LeaderboardUIHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        leaderboardUIHandler.UpdateList(carLapCounters);
    }
    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        //Debug.Log($"Event: Car {carLapCounter.gameObject.name} passed a checkpoint");
        carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckpoint()).ToList();

        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;
        carLapCounter.SetCarPosition(carPosition);

        leaderboardUIHandler.UpdateList(carLapCounters);
    }
}
