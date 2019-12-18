using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Text maxStage;

    private void Awake()
    {
        startButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        maxStage.text = "Top level : " + PlayerPrefs.GetInt("High Stage", 1);
    }
}
