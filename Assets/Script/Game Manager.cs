using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject[] players;

    private void Awake()
    {
        if(Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void CheckWinState()
    {
        int aliveCount = 0;
        foreach(GameObject player in players)
        {
            if (player.activeSelf)
            {
                aliveCount++;
            }
        }

        if(aliveCount <= 1 )
        {
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
