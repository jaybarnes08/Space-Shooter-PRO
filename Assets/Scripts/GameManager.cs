using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] bool _isGameOver = false;

    SpawnManager _spawnManager;

    private void Start() {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null) {
            Debug.LogError("SpawnManager is NULL!");
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true) {
            SceneManager.LoadScene(1); 
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public void GameOver() {
        _isGameOver = true;
    }

}
