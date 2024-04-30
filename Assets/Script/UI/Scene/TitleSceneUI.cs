using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneUI : ISceneUI
{
    [SerializeField]
    private Button _startButton = default;

    public SceneName SceneName => SceneName.Title;

    public IEnumerator Initialize()
    {
        _startButton.onClick.AddListener(async () =>
        {
            AudioManager.Instance.PlaySE(SEType.Decide);
            await UniTask.WaitWhile(() => AudioManager.Instance.SeSource.isPlaying);

            SceneLoader.FadeLoad(SceneName.InGame);
        });
        yield return null;
        Fade.Instance.StartFadeIn(() => AudioManager.Instance.PlayBGM(BGMType.TitleBGM));
    }

    public async void OnUpdate()
    {
        if (Input.anyKeyDown)
        {
            AudioManager.Instance.PlaySE(SEType.Decide);
            await UniTask.WaitWhile(() => AudioManager.Instance.SeSource.isPlaying);

            SceneLoader.FadeLoad(SceneName.InGame);
        }
    }
}
