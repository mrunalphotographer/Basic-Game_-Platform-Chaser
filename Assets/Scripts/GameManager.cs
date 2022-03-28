using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private SpawnManager m_StartGame;
    public UnityEvent onGameStart;
    // Start is called before the first frame update
    void Start()
    {
        var elevators = FindObjectsOfType<ElevatorController>();
        for(int i = 0; i< elevators.Length; i++)
        {
            onGameStart.AddListener(elevators[i].OnGameStart);
        }
    }

    // Update is called once per frame
    public void StartGame()
    {
        m_StartGame = FindObjectOfType<SpawnManager>();
        m_StartGame.StartGame();
        onGameStart.Invoke();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
