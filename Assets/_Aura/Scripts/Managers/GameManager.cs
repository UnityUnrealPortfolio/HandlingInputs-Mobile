using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("How long to wait before resetting the scene")]
    [SerializeField] float timeBeforeReset;

    #region Singleton Setup
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<GameManager>();
                }
            }
            return _instance;
        }

    } 
    #endregion

    public void ResetGame()
    {
        StartCoroutine(WaitToReset());
    }
    IEnumerator WaitToReset()
    {
        yield return new WaitForSeconds(timeBeforeReset);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
