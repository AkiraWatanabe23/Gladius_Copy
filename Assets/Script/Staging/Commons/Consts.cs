using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    /// <summary> 定数管理ファイル </summary>
    public static class Consts
    {
        /// <summary> SEの同時再生上限 </summary>
        public const int SEPlayableLimit = 5;

        /// <summary> enumとシーン名のDictionary </summary>
        public static readonly Dictionary<SceneName, string> Scenes = new()
        {
            { SceneName.Title, "TitleScene" },
            { SceneName.StageSelect, "StageSelectScene" },
            { SceneName.InGame, "GameScene" },
            { SceneName.Result, "ResultScene" },
            { SceneName.Tanaka, "tanakaStage" },
            { SceneName.Watanabe, "New Scene" }
        };

        /// <summary> 指定したシーンのシーン名を取得する </summary>
        public static string GetSceneNameString(SceneName scene) => Scenes[scene];

        #region Console Logs
        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        public static void LogWarning(object message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }

        public static void LogError(object message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }
        #endregion
    }
}

public enum SceneName
{
    Title,
    StageSelect,
    InGame,
    Result,

    Tanaka,
    Watanabe,
}
