using System.Collections;
using UnityEngine;

public interface ISceneUI : ISerializableParam
{
    public SceneName SceneName { get; }

    public IEnumerator Initialize();
    public void OnUpdate() { }
}

/// <summary> インゲームのUI管理 </summary>
public class UIController : MonoBehaviour, IPause
{
    [SubclassSelector]
    [SerializeReference]
    private ISceneUI _sceneUI = default;

    private void Start()
    {
        StartCoroutine(_sceneUI.Initialize());
    }

    private void Update()
    {
        _sceneUI.OnUpdate();
    }

    public void Pause()
    {
        if (_sceneUI is GameSceneUI inGame) { inGame.Pause(); }
    }

    public void Resume()
    {
        if (_sceneUI is GameSceneUI inGame) { inGame.Resume(); }
    }

    public void BulletChange(int index)
    {
        if (_sceneUI is GameSceneUI inGame) { inGame.BulletChange(index); }
    }

    public void OnUpdateAircraft(int count = 1, bool isHeal = false)
    {
        if (_sceneUI is GameSceneUI inGame) { inGame.OnUpdateAircraft(count, isHeal); }
    }
}
