using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectSceneUI : ISceneUI
{
    [SerializeField]
    private NextScene _sampleScene = default;
    [SerializeField]
    private NextScene _sampleScene2 = default;
    [SerializeField]
    private NextScene _sampleScene3 = default;

    public SceneName SceneName => SceneName.StageSelect;

    public IEnumerator Initialize()
    {
        _sampleScene.Init();
        _sampleScene2.Init();
        _sampleScene3.Init();
        Fade.Instance.StartFadeIn();

        yield return null;
    }

    public void OnUpdate() { }
}
