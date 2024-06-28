using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectSceneUI : ISceneUI
{
    [SerializeField]
    private Button _sampleScene = default;

    public SceneName SceneName => SceneName.StageSelect;

    public IEnumerator Initialize()
    {
        _sampleScene.onClick.AddListener(() => SceneLoader.FadeLoad(SceneName.InGame));
        Fade.Instance.StartFadeIn();

        yield return null;
    }

    public void OnUpdate() { }
}
