using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] _commonEnemyPrefabs;
    [SerializeField] GameObject[] _uncommonEnemyPrefabs;
    [SerializeField] GameObject _bossPrefab;

    [SerializeField] GameObject _enemyContainer;

    [SerializeField] GameObject[] _commonPowerup;
    //[SerializeField] GameObject[] _uncommonPowerup;
    [SerializeField] GameObject[] _rarePowerup;
    
    [SerializeField] int _wave = 1;
    [SerializeField] int _finalWave = 7;
    [SerializeField] int _enemiesPerWave;
    [SerializeField] int _enemiesKilled;
    int _enemiesSpawned;

    [SerializeField] private bool _stopSpawning = false;
    bool _bossSpawned = false;

    UIManager _uiManager;

    Coroutine _powerupRoutine;
    Coroutine _enemyRoutine;

    private void Start() {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_uiManager == null) {
            Debug.LogError("UIManager is NULL");
        }

        _uiManager.UpdateWaveText(_wave);
    }

    private void Update() {
        _enemiesPerWave = _wave * 5;
        
    }

    IEnumerator SpawnEnemyRoutine() {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false) {
            int randomEnemyRarityIndex = Random.Range(0, 101);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

            if(randomEnemyRarityIndex < 76)
            {
                int randomEnemyIndex = Random.Range(0, _commonEnemyPrefabs.Length);
                GameObject newEnemy = Instantiate(_commonEnemyPrefabs[randomEnemyIndex], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else
            {
                int randomEnemyIndex = Random.Range(0, _uncommonEnemyPrefabs.Length);
                GameObject newEnemy = Instantiate(_uncommonEnemyPrefabs[randomEnemyIndex], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }


            _enemiesSpawned++;

            if (_enemiesSpawned >= _enemiesPerWave) {
                _stopSpawning = true;

                if (_wave <_finalWave)
                {
                    _enemiesSpawned = 0;
                    _wave++;
                    _uiManager.UpdateWaveText(_wave);
                    _uiManager.NewWaveSequence();
                    yield return new WaitForSeconds(6.5f);
                    _stopSpawning = false;
                    StartCoroutine(SpawnPowerupRoutine());
                    
                }
                else
                {
                    if (!_bossSpawned)
                        SpawnBoss();
                }

            }

            yield return new WaitForSeconds(2.5f);

        }
    }

    IEnumerator SpawnPowerupRoutine() {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            float rarityIndex = Random.Range(0, 100f);
            

            if(rarityIndex < 71)
            {
                int randomPowerup = Random.Range(0, _commonPowerup.Length - 1);
                Instantiate(_commonPowerup[randomPowerup], posToSpawn, Quaternion.identity);
    
            }
            else
            {
                int randomPowerup = Random.Range(0, _rarePowerup.Length - 1);
                Instantiate(_rarePowerup[randomPowerup], posToSpawn, Quaternion.identity);
                

            }
            //else
            //{
            //    int randomPowerup = Random.Range(0, _rarePowerup.Length - 1);
            //    Instantiate(_rarePowerup[randomPowerup], posToSpawn, Quaternion.identity);
                
            //}

            
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }

    }

    public void UpdateEnemiesKilled()
    {
        _enemiesKilled++;
    }

    public void OnPlayerDeath() {
        _stopSpawning = true;
    }

    public void StartSpawning() {
        _enemyRoutine = StartCoroutine(SpawnEnemyRoutine());
        _powerupRoutine = StartCoroutine(SpawnPowerupRoutine());
    }

    void SpawnBoss()
    {
        Instantiate(_bossPrefab, new Vector3(0, 9f, 0), Quaternion.identity);
        _bossSpawned = true;

        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if(player != null)
        {
            player.FightingBoss();
        }
    }
}
