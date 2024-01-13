using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackSelect1 : MonoBehaviour
{
    public void Update()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Track 1");
    }

}