using System;
using UnityEngine;
using UnityEngine.UI;
using OvergodIdle.Telegram;

namespace OvergodIdle.UI
{
    /// <summary>
    /// Creates a simple Canvas/Text at runtime and displays the Telegram user's name
    /// when running as a WebGL Telegram MiniApp. In Editor/Standalone, shows a placeholder.
    /// Note: Telegram integration is currently disabled due to removed dependencies.
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
            YesTMABridgeRunner.OnUserNameChanged += HandleUserNameChanged;
        }

        private void RefreshUserLabel()
        {
            if (YesTMABridgeRunner.TryGetUserName(out var user))
            {
                uiText.text = $"Telegram user: {user}";
            }
            else
            {
                uiText.text = "Telegram user: <unknown>";
            }
        }

        // Called from JS via unityInstance.SendMessage("TelegramUserNameUI", "ForceRefreshFromJS", "")
        public void ForceRefreshFromJS(string _)
        {
            RefreshUserLabel();
        }

        private void HandleUserNameChanged(string newUserName)
        {
            uiText.text = $"Telegram user: {newUserName}";
        }

        private void OnDestroy()
        {
            YesTMABridgeRunner.OnUserNameChanged -= HandleUserNameChanged;
        }
    }
}


