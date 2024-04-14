using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public void OpenLevel(int LevelId)
    {
        string LevelName = "Map " + LevelId;
        SceneManager.LoadScene(LevelName);
    }
}