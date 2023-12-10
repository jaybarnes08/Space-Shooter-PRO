using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] _commonEnemyPrefabs;
    [SerializeField] GameObject[] _uncommonEnemyPrefabs;

    [SerializeField] GameObject _enemyContainer;

    [SerializeField] GameObject[] _commonPowerup;
    [SerializeField] GameObject[] _uncommonPowerup;
    [SerializeField] GameObject[] _rarePowerup;
    
    [SerializeField] int _wave = 1;
    [SerializeField] int _enemiesPerWave;
    int _enemiesSpawned;

    [SerializeField] private bool _stopSpawning = false;

    UIManager _uiManager;

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
                int randomEnemyIndex = Random.Range(0, _commonEnemyPrefabs.Length -1);
                GameObject newEnemy = Instantiate(_commonEnemyPrefabs[randomEnemyIndex], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else
            {
                int randomEnemyIndex = Random.Range(0, _uncommonEnemyPrefabs.Length -1);
                GameObject newEnemy = Instantiate(_uncommonEnemyPrefabs[randomEnemyIndex], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }


            _enemiesSpawned++;

            if (_enemiesSpawned >= _enemiesPerWave) {
                _stopSpawning = true;
                _enemiesSpawned = 0;
                _wave++;
                _uiManager.UpdateWaveText(_wave);
                _uiManager.NewWaveSequence();
                yield return new WaitForSeconds(6.5f);
                _stopSpawning = false;
            }

            yield return new WaitForSeconds(2.5f);
        }
    }

    IEnumerator SpawnPowerupRoutine() {
        yield return new WaitForSeconds(3f);

        while (_stopSpawning == false) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            float rarityIndex = Random.Range(0, 100f);

            if(rarityIndex < 61)
            {
                int randomPowerup = Random.Range(0, _commonPowerup.Length -1);
                Instantiate(_commonPowerup[randomPowerup], posToSpawn, Quaternion.identity);
            }
            else if(rarityIndex < 91)
            {
                int randomPowerup = Random.Range(0, _uncommonPowerup.Length -1);
                Instantiate(_uncommonPowerup[randomPowerup], posToSpawn, Quaternion.identity);

            }
            else
            {
                int randomPowerup = Random.Range(0, _rarePowerup.Length -1);
                Instantiate(_rarePowerup[randomPowerup], posToSpawn, Quaternion.identity);
            }

            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }

    }

    public void OnPlayerDeath() {
        _stopSpawning = true;
    }

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void StopSpawning(bool stop) {
        _stopSpawning = stop;
    }

}
