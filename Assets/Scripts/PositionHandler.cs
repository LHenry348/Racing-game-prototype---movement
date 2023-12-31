using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionHandler : MonoBehaviour
{
    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        CarLapCounter[] carLapCounterArray = FindObjectsByType<CarLapCounter>(FindObjectsSortMode.None);
        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();

        foreach (CarLapCounter lapCounters in carLapCounters)
        {
            lapCounters.OnPassCheckpoint += OnPassCheckpoint;
        }
    }
    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        //Debug.Log($"Event: Car {carLapCounter.gameObject.name} passed a checkpoint");
        carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckpoint()).ToList();

        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;
        carLapCounter.SetCarPosition(carPosition);
    }
}
