using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_WEBGL
using YesTMABridge;
#endif

namespace OvergodIdle.UI
{
    /// <summary>
    /// Creates a simple Canvas/Text at runtime and displays the Telegram user's name
    /// when running as a WebGL Telegram MiniApp. In Editor/Standalone, shows a placeholder.
    /// </summary>
    public sealed class TelegramUserNameUI : MonoBehaviour
    {
        private Text uiText;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            // Avoid duplicating if scene reloads
            if (FindObjectOfType<TelegramUserNameUI>() != null)
            {
                return;
            }

            var root = new GameObject("TelegramUserNameUI");
            DontDestroyOnLoad(root);
            var ui = root.AddComponent<TelegramUserNameUI>();
            ui.InitializeUI();
        }

        private void InitializeUI()
        {
            // Canvas
            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 1f; // favor height on mobile

            // Panel
            var panelGo = new GameObject("Panel", typeof(Image));
            panelGo.transform.SetParent(canvasGo.transform, false);
            var panelImage = panelGo.GetComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.4f);
            var panelRect = panelGo.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0, 1);
            panelRect.anchorMax = new Vector2(0, 1);
            panelRect.pivot = new Vector2(0, 1);
            panelRect.anchoredPosition = new Vector2(16, -16);
            panelRect.sizeDelta = new Vector2(700, 96);

            // Text
            var textGo = new GameObject("UserName", typeof(Text));
            textGo.transform.SetParent(panelGo.transform, false);
            uiText = textGo.GetComponent<Text>();
            uiText.alignment = TextAnchor.MiddleLeft;
            uiText.color = Color.white;
            uiText.fontSize = 36;
            uiText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var textRect = textGo.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0, 0);
            textRect.anchorMax = new Vector2(1, 1);
            textRect.offsetMin = new Vector2(16, 12);
            textRect.offsetMax = new Vector2(-16, -12);

            RefreshUserLabel();
        }

        private void RefreshUserLabel()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            try
            {
                var raw = TGMiniAppGameSDKProvider.getUserInfo();
                Debug.Log($"[TelegramUserNameUI] getUserInfo raw JSON: {raw}");
                var user = TGMiniAppGameSDKProvider.GetUserInfo();
                Debug.Log($"[TelegramUserNameUI] parsed user => id:{user?.id} first:{user?.firstName} last:{user?.lastName} username:{user?.username}");
                var display = BuildDisplayName(user);
                uiText.text = string.IsNullOrEmpty(display) ? "Telegram user: <unknown>" : $"Telegram user: {display}";
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to read Telegram user info: {ex.Message}");
                uiText.text = "Telegram user: <unavailable>";
            }
#else
            uiText.text = "Telegram user: <Editor>";
#endif
        }

        // Called from JS via unityInstance.SendMessage("TelegramUserNameUI", "ForceRefreshFromJS", "")
        public void ForceRefreshFromJS(string _)
        {
            RefreshUserLabel();
        }

#if UNITY_WEBGL
        private static string BuildDisplayName(TMAUser user)
        {
            if (user == null)
            {
                return string.Empty;
            }

            // Prefer full name; fall back to username or id
            var first = user.firstName ?? string.Empty;
            var last = user.lastName ?? string.Empty;
            var full = (first + " " + last).Trim();
            if (!string.IsNullOrEmpty(full)) return full;
            if (!string.IsNullOrEmpty(user.username)) return user.username;
            return user.id != 0 ? user.id.ToString() : string.Empty;
        }
#endif
    }
}


