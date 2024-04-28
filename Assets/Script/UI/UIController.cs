using System.Collections;
using UnityEngine;

public interface ISceneUI : ISerializableParam
{
    public SceneName SceneName { get; }

    public IEnumerator Initialize();
}

/// <summary> インゲームのUI管理 </summary>
public class UIController : MonoBehaviour, IPause
{
    [SubclassSelector]
    [SerializeReference]
    private ISceneUI _sceneUI = default;

    public void Pause()
    {
        if (_sceneUI is GameSceneUI inGame) { inGame.Pause(); }
    }

    public void Resume()
    {
        if (_sceneUI is GameSceneUI inGame) { inGame.Resume(); }
    }

    private void Start()
    {
        StartCoroutine(_sceneUI.Initialize());
    }
}
