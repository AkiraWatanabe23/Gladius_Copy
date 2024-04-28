using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneUI : ISceneUI
{
    [SerializeField]
    private Button _startButton = default;

    public SceneName SceneName => SceneName.Title;

    public IEnumerator Initialize()
    {
        _startButton.onClick.AddListener(() =>
        {
            SceneLoader.FadeLoad(SceneName.StageSelect);
        });
        yield return null;
        Fade.Instance.StartFadeIn();
    }
}
