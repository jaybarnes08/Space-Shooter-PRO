using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private TMP_Text _restartGameText;
    [SerializeField] private TMP_Text _playAgainText;
    [SerializeField] private TMP_Text _congratulationsText;
    [SerializeField] private TMP_Text _ammoCountText;
    [SerializeField] private Slider _thrusterSlider;
    
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _NewWaveText;

    [SerializeField] private TMP_Text _thrusterRechargeText;

    [SerializeField] GameObject _bossUI;
    [SerializeField] Slider _bossHealthSlider;
    [SerializeField] Slider _bossShieldSlider;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();

        if( _gameManager == null) {
            Debug.LogError("Game Manager is null!");
        }

        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);

    }

    public void UpdateScore(int playerScore) {
        _scoreText.text = "Score :" + playerScore;
    }

    public void UpdateLives(int currentLives) {
        _livesImage.sprite = _livesSprites[currentLives];

        if(currentLives <= 0) {
            currentLives = 0;
            GameOverSequence();
           
        }
    }

    public void UpdateWaveText(int currentWave) {
        _waveText.text = "Wave: " + currentWave;
    }

    public void UpdateAmmoCount(int ammo) {
        _ammoCountText.text = "Current Ammo: " + ammo;
    }

    public void UpdateThrusterSlider(float fuelPercentage)
    {
        _thrusterSlider.value = fuelPercentage;
    }

    public void EnableRechargeText(bool active) {
        _thrusterRechargeText.gameObject.SetActive(enabled);
    }

    void GameOverSequence() {
        _restartGameText.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(GameOverRoutine());
    }

    public void GameWonSequence()
    {
        _playAgainText.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(GameWonRoutine());
    }

    public void NewWaveSequence() {
        StartCoroutine(NewWaveRoutine());
    }

    IEnumerator GameOverRoutine() {
        while (true) {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
            
        }
        
    }

    IEnumerator NewWaveRoutine() {
        _NewWaveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        _NewWaveText.gameObject.SetActive(false);
    }

    public void EnableBossUI()
    {
        _bossUI.gameObject.SetActive(true);
    }

    public void UpdateBossHealth(float health, float shield)
    {
        _bossHealthSlider.value = health;
        _bossShieldSlider.value = shield;

    }

    IEnumerator GameWonRoutine()
    {
        while (true)
        {
            _congratulationsText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _congratulationsText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }
    }
}
