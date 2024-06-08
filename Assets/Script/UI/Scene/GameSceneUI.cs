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
    private Image[] _shotKindImages = new Image[3];
    [SerializeField]
    private GameObject _pausePanel = default;
    [SerializeField]
    private Button _continueGameButton = default;
    [SerializeField]
    private Button _returnTitleButton = default;

    private Text[] _shotTexts = new Text[3];
    private PlayerAttack _attackData = default;
    private PlayerHealth _healthData = default;
    private Dictionary<ClearConditionName, Canvas[]> _canvasDict = default;

    private readonly Dictionary<InitialBulletType, string> _bulletNameDict = new()
    {
        { InitialBulletType.Default, "Default" },
        { InitialBulletType.Bomb, "Bomb" },
        {InitialBulletType.ChargeBeam, "ChargeBeam" },
        { InitialBulletType.Homing, "Homing" },
        { InitialBulletType.Laser, "Laser" },
        { InitialBulletType.ShotGun, "ShotGun" },
    };

    public SceneName SceneName => SceneName.InGame;

    public IEnumerator Initialize()
    {
        _canvasDict = new();
        for (int i = 0; i < _sceneCanvases.Length; i++)
        {
            var canvasData = _sceneCanvases[i];
            if (!_canvasDict.ContainsKey(canvasData.Condition)) { _canvasDict.Add(canvasData.Condition, canvasData.Canvases); }
            else { continue; }
        }

        _continueGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.InGameUpdate.Resume();
            _pausePanel.gameObject.SetActive(false);
        });
        _returnTitleButton.onClick.AddListener(() => SceneLoader.FadeLoad(SceneName.Title));

        yield return CanvasSetting();

        var player = UnityEngine.Object.FindObjectOfType<PlayerController>();
        _attackData = player.Attack;
        for (int i = 0; i < _shotKindImages.Length; i++)
        {
            _shotTexts[i] = _shotKindImages[i].transform.GetChild(0).gameObject.GetComponent<Text>();
            _shotTexts[i].text = _attackData.InitialBullets[i].ToString();
        }
        _shotKindImages[0].color = Color.gray;

        _healthData = player.Health;
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

    private IEnumerator CanvasSetting()
    {
        try
        {
            var consitions = GameManager.Instance.ClearConditions;
            for (int i = 0; i < consitions.Length; i++)
            {
                var canvases = _canvasDict[GetClearCondition(consitions[i])];
                foreach (var canvas in canvases) { canvas.gameObject.SetActive(true); }
            }
        }
        catch (Exception)
        {
            var canvases = _canvasDict[ClearConditionName.None];
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

    public void BulletChange(int index)
    {
        for (int i = 0; i < _shotKindImages.Length; i++)
        {
            _shotKindImages[i].color = Color.white;
        }
        _shotKindImages[index].color = Color.gray;
    }
}
