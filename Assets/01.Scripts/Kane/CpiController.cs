using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpiController : MonoBehaviour
{

    public GameObject _fullUi_Canvas;

    // =============================
#if UNITY_EDITOR

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            _fullUi_Canvas.SetActive(!_fullUi_Canvas.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Managers._gameUi.gameObject.SetActive(!Managers._gameUi.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Time.timeScale += 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale -= 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.DeleteAll();
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }


    }

#endif

}
