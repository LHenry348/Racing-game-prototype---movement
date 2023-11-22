using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLapCounter : MonoBehaviour
{
    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckpoint = 0;
    int numberOfPassedCheckpoints = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 2;
    bool isRaceCompleted = false;

    int carPosition = 0;

    LapCounterUIHandler lapCounterUIHandler;

    public event Action<CarLapCounter> OnPassCheckpoint;

    void Start()
    {
        if(CompareTag("Player"))
        {
            lapCounterUIHandler = FindFirstObjectByType<LapCounterUIHandler>();
            lapCounterUIHandler.SetLapText($"Lap {lapsCompleted + 1}/{lapsToComplete}");
        }
    }

    public void SetCarPosition(int position)
    {
        carPosition = position;
    }

    public int GetNumberOfCheckpointsPassed()
    {
        return numberOfPassedCheckpoints;
    }
    public float GetTimeAtLastCheckpoint() 
    {
        return timeAtLastPassedCheckpoint;
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("CheckPoint"))
        {
            if (isRaceCompleted)
                return;
            Checkpoint checkPoint = collider2D.GetComponent<Checkpoint>();

            if (passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;
                numberOfPassedCheckpoints++;
                timeAtLastPassedCheckpoint = Time.time;

                if (checkPoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;

                    if (lapsCompleted >= lapsToComplete)
                        isRaceCompleted = true;
                    if (!isRaceCompleted && lapCounterUIHandler != null)
                        lapCounterUIHandler.SetLapText($"Lap {lapsCompleted + 1}/{lapsToComplete}");
                }

                OnPassCheckpoint?.Invoke(this);

                if (isRaceCompleted)
                {
                    if (CompareTag("Player"))
                    {
                        GameManager.instance.OnRaceCompleted();
                    }
                }
            }
        }
    }
}
