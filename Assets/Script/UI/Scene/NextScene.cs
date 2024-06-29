using UnityEngine;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{
    [SerializeField]
    private SceneName _nextScene = SceneName.Title;

    private Button _button = default;

    public void Init()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => SceneLoader.FadeLoad(_nextScene));
    }
}