using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CanvasData
{
    [field: SerializeField]
    public ClearConditionName Condition { get; private set; }
    [field: SerializeField]
    public Canvas[] Canvases { get; private set; }
}

public class GameSceneUI : ISceneUI
{
    [SerializeField]
    private CanvasData[] _sceneCanvases = default;
    [SerializeField]
    private GameObject _aircraftImages = default;
    [SerializeField]
    private GameObject _lifeTexts = default;
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
    private Dictionary<ClearConditionName, Canvas[]> _canvasDict = default;

    protected Dictionary<ClearConditionName, Canvas[]> CanvasDict
    {
        get
        {
            if (_canvasDict == null)
            {
                _canvasDict = new();
                for (int i = 0; i < _sceneCanvases.Length; i++)
                {
                    var canvasData = _sceneCanvases[i];
                    if (!_canvasDict.ContainsKey(canvasData.Condition)) { _canvasDict.Add(canvasData.Condition, canvasData.Canvases); }
                    else { continue; }
                }
            }
            return _canvasDict;
        }
    }

    public SceneName SceneName => SceneName.InGame;

    public IEnumerator Initialize()
    {
        _continueGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.InGameUpdate.Resume();
            _pausePanel.gameObject.SetActive(false);
        });
        _returnTitleButton.onClick.AddListener(() => SceneLoader.FadeLoad(SceneName.Title));
        _bgmSlider.onValueChanged.AddListener(AudioManager.Instance.VolumeSettingBGM);
        _seSlider.onValueChanged.AddListener(AudioManager.Instance.VolumeSettingSE);

        yield return CanvasSetting();

        _healthData = UnityEngine.Object.FindObjectOfType<PlayerController>().Health;
        yield return null;

        if (_healthData.HealthType == HealthType.HP)
        {
            _lifeTexts.SetActive(true);
            if (_healthText != null) { _healthText.text = $"{_healthData.HP.Life} / {_healthData.HP.MaxLife}"; }

            _aircraftImages.SetActive(false);
        }
        else if (_healthData.HealthType == HealthType.RemainingAircraft)
        {
            _aircraftImages.SetActive(true);
            _lifeTexts.SetActive(false);
        }
        yield return null;

        _pausePanel.SetActive(false);
        Consts.Log("Finish Initialized UI System");
        AudioManager.Instance.PlayBGM(BGMType.InGameBGM);
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_pausePanel.activeSelf) { _pausePanel.SetActive(true); }
    }

    private IEnumerator CanvasSetting()
    {
        try
        {
            var consitions = GameManager.Instance.ClearConditions;
            for (int i = 0; i < consitions.Length; i++)
            {
                var canvases = CanvasDict[GetClearCondition(consitions[i])];
                foreach (var canvas in canvases) { canvas.gameObject.SetActive(true); }
            }
        }
        catch (Exception)
        {
            var canvases = CanvasDict[ClearConditionName.None];
            foreach (var canvas in canvases) { canvas.gameObject.SetActive(true); }
        }

        yield return null;
    }

    private ClearConditionName GetClearCondition(IClearRule clearRule)
    {
        if (clearRule is ArriveGoal) { return ClearConditionName.ArriveGoal; }
        else if (clearRule is DefeatBoss) { return ClearConditionName.DefeatBoss; }
        else if (clearRule is EnemyAnnihilated) { return ClearConditionName.EnemyAnnihilated; }
        else if (clearRule is TimeAttack) { return ClearConditionName.TimeAttack; }
        else if (clearRule is Survival) { return ClearConditionName.Survival; }
        else { return ClearConditionName.None; }
    }

    public void Pause() => _pausePanel.SetActive(true);

    public void Resume() => _pausePanel.SetActive(false);
}
