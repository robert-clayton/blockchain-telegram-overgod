using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OvergodIdle.Telegram
{
    /// <summary>
    /// MonoBehaviour wrapper to interact with the Yes TMA Bridge without the bridge needing to be a MonoBehaviour.
    /// - Exposes a static username value and change event
    /// - Provides a public callback method that JS (or the bridge) can invoke via SendMessage
    /// - Bootstraps itself into the scene automatically and persists across scenes
    /// </summary>
    public sealed class YesTMABridgeRunner : MonoBehaviour
    {
        public static event Action<string> OnUserNameChanged;

        public static string CurrentUserName { get; private set; }

        public static bool TryGetUserName(out string userName)
        {
            userName = CurrentUserName;
            return !string.IsNullOrEmpty(userName);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (FindObjectOfType<YesTMABridgeRunner>() != null)
            {
                return;
            }

            var go = new GameObject("YesTMABridgeRunner");
            DontDestroyOnLoad(go);
            go.AddComponent<YesTMABridgeRunner>();
        }

        private void Awake()
        {
            // Optional: attempt a best-effort initialization if the bridge exposes obvious static init methods.
            TryCallBridgeInit();
        }

        /// <summary>
        /// Intended to be called from JS or another C# bridge layer via:
        /// unityInstance.SendMessage("YesTMABridgeRunner", "HandleTelegramUserNameFromJS", userName)
        /// </summary>
        /// <param name="userName">Telegram username string</param>
        public void HandleTelegramUserNameFromJS(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return;
            }

            if (CurrentUserName == userName)
            {
                return;
            }

            CurrentUserName = userName;
            OnUserNameChanged?.Invoke(CurrentUserName);
        }

        private static void TryCallBridgeInit()
        {
            try
            {
                // Look for a type literally named "YesTMABridge" in any loaded assembly.
                var bridgeType = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a =>
                    {
                        try { return a.GetTypes(); }
                        catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null); }
                        catch { return Array.Empty<Type>(); }
                    })
                    .FirstOrDefault(t => t != null && t.Name == "YesTMABridge");

                if (bridgeType == null)
                {
                    return;
                }

                // Try invoking a common static init method name if present.
                var candidateMethodNames = new[] { "Initialize", "Init", "Bootstrap" };
                foreach (var methodName in candidateMethodNames)
                {
                    var method = bridgeType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    if (method != null && method.GetParameters().Length == 0)
                    {
                        method.Invoke(null, null);
                        break;
                    }
                }
            }
            catch
            {
                // Best-effort only; ignore any reflection or invocation errors.
            }
        }
    }
}


