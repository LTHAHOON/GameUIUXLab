using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UIUXTestEditorWindow : EditorWindow
{
    [MenuItem("Tool/UIUXTestWindow")]
    public static void CreateWindow()
    {
        GetWindow<UIUXTestEditorWindow>(title: "UIUXTestWindow");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        StyleSheet toastTestStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GameUIUX/Prefab/HUD/ToastTestStyleSheet.uss");
        if (toastTestStyleSheet != null)
        {
            root.styleSheets.Add(toastTestStyleSheet);
        }
        VisualElement tests = new();
        root.Add(tests);

        #region  Toast 메시지 테스트
        TextField toastMessageText = new("보낼 토스트 메시지");
        toastMessageText.AddToClassList("toast-message-test-text");
        if (toastMessageText.text == string.Empty)
        {
            toastMessageText.value = "This is ToastMessage";
        }

        VisualElement toastMessageTestBtns = new();
        toastMessageTestBtns.style.flexDirection = FlexDirection.Row;
        toastMessageTestBtns.style.alignSelf = Align.Center;
        Button toastMessageTestBtn = new()
        {
            text = "토스트 메시지 보내기",
        };
        Button toastMessageClearBtn = new()
        {
            text = "토스트 메시지 정리하기",
        };

        toastMessageTestBtn.AddToClassList("toast-message-test-button");
        toastMessageTestBtn.clicked += () => OnClickToastMessageTest(toastMessageText.text);
        toastMessageClearBtn.AddToClassList("toast-message-test-button");
        toastMessageClearBtn.clicked += () => OnClickToastMessageTestClear();

        tests.Add(toastMessageText);
        tests.Add(toastMessageTestBtns);
        toastMessageTestBtns.Add(toastMessageTestBtn);
        toastMessageTestBtns.Add(toastMessageClearBtn);
        #endregion

        VisualElement empty_01 = new();
        tests.Add(empty_01);

        VisualElement healthModifyTest = new();
        HelpBox guideBox = new("최대 체력: 100", HelpBoxMessageType.Info);

        FloatField currentHealth = new("현재 체력", 100);

        Button healthModifyTestBtn = new();
        healthModifyTestBtn.text = "체력 수정";
        healthModifyTestBtn.AddToClassList("toast-message-test-button");
        healthModifyTestBtn.clicked += () => OnClickHealthModifyTest(currentHealth.value);

        tests.Add(healthModifyTest);
        healthModifyTest.Add(currentHealth);
        healthModifyTest.Add(guideBox);
        healthModifyTest.Add(healthModifyTestBtn);
    }

    public void OnClickToastMessageTest(string toastMessageText)
    {
        PlayRootPresenter playRootPresenter = FindAnyObjectByType<PlayRootPresenter>();
        playRootPresenter.ShowToastMessage(toastMessageText);
    }
    public void OnClickToastMessageTestClear()
    {
        PlayRootPresenter playRootPresenter = FindAnyObjectByType<PlayRootPresenter>();
        playRootPresenter.ClearToastMessages();
    }

    public void OnClickHealthModifyTest(float currentHealth)
    {
        currentHealth = Math.Clamp(currentHealth, 0f, 100f);
        PlayRootPresenter playRootPresenter = FindAnyObjectByType<PlayRootPresenter>();
        playRootPresenter.SetHealthSliderValue(currentHealth);
    }
}
