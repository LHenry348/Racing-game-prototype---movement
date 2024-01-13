using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownUIHandler : MonoBehaviour
{
    public Text countDownText;
    public AudioSource countDown;
    public AudioSource bgMusic;

    private void Awake()
    {
        countDownText.text = "";
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDownCO());
    }
    IEnumerator CountDownCO()
    {
        yield return new WaitForSeconds(0.3f);
        countDown.Play();
        int counter = 3;
        while (true)
        {
            if (counter != 0)
                countDownText.text = counter.ToString();
            else
            {
                countDownText.text = "GO";
                GameManager.instance.OnRaceStart();
                break;
            }
            counter--;
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        bgMusic.Play();
    }
}
