using Constants;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : ISceneUI
{
    [SerializeField]
    private Slider _healthSlider = default;
    [SerializeField]
    private Text _healthText = default;
    [SerializeField]
    private GameObject _pausePanel = default;
    [SerializeField]
    private Button _continueGameButton = default;
    [SerializeField]
    private Button _returnTitleButton = default;
    [SerializeField]
    private Slider _bgmSlider = default;
    [SerializeField]
    private Slider _seSlider = default;

    private PlayerHealth _healthData = default;

    public SceneName SceneName => SceneName.InGame;

    public IEnumerator Initialize()
    {
        _healthData = Object.FindObjectOfType<PlayerController>().Health;
        yield return null;

        if (_healthSlider != null)
        {
            _healthSlider.maxValue = _healthData.HP.MaxLife;
            _healthSlider.value = _healthData.HP.Life;
            _healthSlider.onValueChanged.AddListener(ReceiveDamage);
        }
        if (_healthText != null) { _healthText.text = $"{_healthData.HP.Life} / {_healthData.HP.MaxLife}"; }
        yield return null;

        _continueGameButton.onClick.AddListener(() => GameManager.Instance.InGameUpdate.Resume());
        _returnTitleButton.onClick.AddListener(() => SceneLoader.FadeLoad(SceneName.Title));
        _bgmSlider.onValueChanged.AddListener(AudioManager.Instance.VolumeSettingBGM);
        _seSlider.onValueChanged.AddListener(AudioManager.Instance.VolumeSettingSE);
        yield return null;

        _pausePanel.SetActive(false);
        Consts.Log("Finish Initialized UI System");
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_pausePanel.activeSelf)
        {
            _pausePanel.SetActive(true);
        }
    }

    public void Pause() => _pausePanel.SetActive(true);

    public void Resume() => _pausePanel.SetActive(false);

    private void ReceiveDamage(float value)
    {
        _healthSlider.value -= value;
    }
}
