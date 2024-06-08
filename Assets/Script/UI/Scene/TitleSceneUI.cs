using System.Collections;
using UnityEngine;

public class TitleSceneUI : ISceneUI
{
    private bool _isLoad = false;

    public SceneName SceneName => SceneName.Title;

    public IEnumerator Initialize()
    {
        Fade.Instance.StartFadeIn(() => AudioManager.Instance.PlayBGM(BGMType.TitleBGM));
        yield return GameStartInputWait();
    }

    private IEnumerator GameStartInputWait()
    {
        yield return new WaitUntil(() => Input.anyKeyDown && !_isLoad);

        if (_isLoad) { yield break; }

        _isLoad = true;
        AudioManager.Instance.PlaySE(SEType.Decide);
        yield return AudioManager.Instance.SEPlayingWait();

        SceneLoader.FadeLoad(SceneName.InGame);
        yield return null;
    }
}
