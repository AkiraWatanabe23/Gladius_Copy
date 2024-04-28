using Constants;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary> インゲームのUI管理 </summary>
public class UIController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player = default;

    [SerializeField]
    private Slider _healthSlider = default;
    [SerializeField]
    private Text _healthText = default;

    public Text HealthText => _healthText;

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        if (_player == null) { _player = FindObjectOfType<PlayerController>(); }

        if (_healthSlider != null)
        {
            _healthSlider.maxValue = _player.Health.HP.MaxLife;
            _healthSlider.value = _player.Health.HP.Life;
            _healthSlider.onValueChanged.AddListener(ReceiveDamage);
        }
        if (_healthText != null) { _healthText.text = $"{_player.Health.HP.Life} / {_player.Health.HP.MaxLife}"; }

        yield return null;
        Consts.Log("Finish Initialized UI System");
    }

    private void ReceiveDamage(float value)
    {
        _healthSlider.value -= value;
    }
}
