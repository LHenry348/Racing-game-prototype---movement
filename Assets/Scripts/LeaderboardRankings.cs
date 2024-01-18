using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRankings : MonoBehaviour
{
    public Text positionText;
    public Text carText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPositionText(string newPosition)
    {
        positionText.text = newPosition;
    }

    public void SetNameText(string newCar)
    {
        carText.text = newCar;
    }
}
