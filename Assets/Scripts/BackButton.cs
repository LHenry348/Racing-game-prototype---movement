using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }

    public void CharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

}