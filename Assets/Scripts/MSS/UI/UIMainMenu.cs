using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public Button BtnChallenge;

    private void Start()
    {
        BtnChallenge.onClick.AddListener(OnBtnChallengeClick);
    }

    private void OnBtnChallengeClick()
    {
        SceneManager.LoadScene("Map01");
    }
}
