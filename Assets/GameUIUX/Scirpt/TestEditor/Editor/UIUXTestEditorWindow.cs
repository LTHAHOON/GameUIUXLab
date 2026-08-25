using System;
using System.ComponentModel.Design.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UIUXTestEditorWindow : EditorWindow
{
    [SerializeField]
    private float _currentHealth = 100f;
    private Slider _currentHealthSlider;
    [SerializeField]
    private int _score = 0;

    private const string CurrentHealthPrefsKey = "UIUXTestEditorWindow.CurrentHealth";
    private const string ScorePrefsKey = "UIUXTestEditorWindow.Score";

    [MenuItem("Tool/UIUXTestWindow")]
    private static void CreateWindow()
    {
        EditorWindow editorWindow = GetWindow<UIUXTestEditorWindow>(title: "UIUXTestWindow");
        editorWindow.minSize = new Vector2(450f, 650f);
    }

    private void OnEnable()
    {
        _currentHealth = EditorPrefs.GetFloat(CurrentHealthPrefsKey);
        _score = EditorPrefs.GetInt(ScorePrefsKey);
    }

    private bool _isPlaying = false;
    private void Update()
    {
        if (_isPlaying)
        {
            if (!Application.isPlaying)
            {
                _currentHealthSlider.value = 100;
                _isPlaying = false;
            }
            return;
        }
        _isPlaying = Application.isPlaying;
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        StyleSheet toastTestStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GameUIUX/Scirpt/USS/ToastTestStyleSheet.uss");
        if (toastTestStyleSheet != null)
        {
            root.styleSheets.Add(toastTestStyleSheet);
        }
        VisualElement tests = new();
        root.Add(tests);

        #region Toast 메시지 테스트
        TextField toastMessageText = new("보낼 토스트 메시지");
        toastMessageText.AddToClassList("test-text");
        if (toastMessageText.text == string.Empty)
        {
            toastMessageText.value = "This is ToastMessage";
        }
        toastMessageText.style.marginTop = 50;
        VisualElement toastMessageTestBtns = new();
        toastMessageTestBtns.style.flexDirection = FlexDirection.Row;
        toastMessageTestBtns.style.alignSelf = Align.Center;
        Button toastMessageTestBtn = new()
        {
            text = "토스트 메시지 보내기",
        };
        Button toastMessageClearBtn = new()
        {
            text = "토스트 메시지 리셋하기",
        };

        toastMessageTestBtn.AddToClassList("test-button");
        toastMessageTestBtn.clicked += () => OnClickToastMessageTest(toastMessageText.text);
        toastMessageClearBtn.AddToClassList("test-button");
        toastMessageClearBtn.clicked += () => OnClickToastMessageTestReset();

        tests.Add(toastMessageText);
        tests.Add(toastMessageTestBtns);
        toastMessageTestBtns.Add(toastMessageTestBtn);
        toastMessageTestBtns.Add(toastMessageClearBtn);
        #endregion

        VisualElement empty_01 = new();
        empty_01.style.height = 60f;
        tests.Add(empty_01);

        #region 체력 설정 테스트
        VisualElement healthModifyTest = new();
        healthModifyTest.style.flexDirection = FlexDirection.Row;
        healthModifyTest.style.alignSelf = Align.Center;
        HelpBox guideBox = new("최대 체력: 100", HelpBoxMessageType.Info);

        _currentHealthSlider = new("수정할 체력", 0, 100);
        _currentHealthSlider.showInputField = true;
        _currentHealthSlider.value = _currentHealth;
        _currentHealthSlider.AddToClassList("test-text");
        Button healthModifyTestBtn = new();

        Button healthModifyTestResetBtn = new();
        healthModifyTestBtn.text = "체력 수정하기";
        healthModifyTestResetBtn.text = "체력 리셋하기";
        healthModifyTestBtn.AddToClassList("test-button");
        healthModifyTestResetBtn.AddToClassList("test-button");
        healthModifyTestBtn.clicked += () => OnClickHealthModifyTest(_currentHealthSlider.value);
        healthModifyTestResetBtn.clicked += () => OnClickHealthModifyTestReset(_currentHealthSlider);

        tests.Add(guideBox);
        tests.Add(_currentHealthSlider);
        tests.Add(healthModifyTest);
        healthModifyTest.Add(healthModifyTestBtn);
        healthModifyTest.Add(healthModifyTestResetBtn);
        #endregion

        VisualElement empty_02 = new();
        empty_02.style.height = 60f;
        tests.Add(empty_02);

        #region 스코어 설정 테스트
        VisualElement scoreTest = new();
        scoreTest.AddToClassList("score-test-row");
        IntegerField scoreDelta = new("ScoreDelta", 100);
        scoreDelta.AddToClassList("score-delta-field");
        scoreDelta.value = 10;
        Button scoreIncreaseTestBtn = new();
        scoreIncreaseTestBtn.AddToClassList("test-square-button");
        scoreIncreaseTestBtn.AddToClassList("score-step-button");
        scoreIncreaseTestBtn.clicked += () => OnClickScoreIncreaseTest(scoreDelta.value);
        scoreIncreaseTestBtn.text = "+";
        Button scoreDecreaseTestBtn = new();
        scoreDecreaseTestBtn.AddToClassList("test-square-button");
        scoreDecreaseTestBtn.AddToClassList("score-step-button");
        scoreDecreaseTestBtn.clicked += () => OnClickScoreDecreaseTest(scoreDelta.value);
        scoreDecreaseTestBtn.text = "-";
        Button scoreResetBtn = new();
        scoreResetBtn.AddToClassList("test-button");
        scoreResetBtn.AddToClassList("score-reset-button");
        scoreResetBtn.clicked += OnClickScoreReset;
        scoreResetBtn.text = "스코어 리셋하기";

        tests.Add(scoreTest);
        scoreTest.Add(scoreDelta);
        scoreTest.Add(scoreIncreaseTestBtn);
        scoreTest.Add(scoreDecreaseTestBtn);
        scoreTest.Add(scoreResetBtn);
        #endregion

    }

    private void OnClickToastMessageTest(string toastMessageText)
    {
        PlayRootPresenter playRootPresenter = FindAnyObjectByType<PlayRootPresenter>();
        playRootPresenter.ShowToastMessage(toastMessageText);
    }
    private void OnClickToastMessageTestReset()
    {
        PlayRootPresenter playRootPresenter = FindAnyObjectByType<PlayRootPresenter>();
        playRootPresenter.ResetToastMessages();
    }

    private void OnClickHealthModifyTest(float currentHealth)
    {
        currentHealth = SaveCurrentHealth(currentHealth);
        PlayRootPresenter playRootPresenter = FindAnyObjectByType<PlayRootPresenter>();
        playRootPresenter.SetHealthSliderValue(currentHealth);
    }

    private void OnClickHealthModifyTestReset(Slider currentHealth)
    {
        currentHealth.value = 100f;
        SaveCurrentHealth(100f);
        PlayRootPresenter playRootPresenter = FindAnyObjectByType<PlayRootPresenter>();
        playRootPresenter?.SetHealthSliderValue(100f);
    }

    private float SaveCurrentHealth(float currentHealth)
    {
        _currentHealth = Mathf.Clamp(currentHealth, 0f, 100f);
        EditorPrefs.SetFloat(CurrentHealthPrefsKey, _currentHealth);
        return _currentHealth;
    }

    private void OnClickScoreIncreaseTest(int scoreDelta)
    {
        PlayRootPresenter playRootPresenter = FindAnyObjectByType<PlayRootPresenter>();
        _score += scoreDelta;
        _score = Math.Max(0, _score);
        EditorPrefs.SetInt(ScorePrefsKey, _score);
        playRootPresenter.SetScoreText(_score);
    }
    private void OnClickScoreDecreaseTest(int scoreDelta)
    {
        PlayRootPresenter playRootPresenter = FindAnyObjectByType<PlayRootPresenter>();
        _score -= scoreDelta;
        _score = Math.Max(0, _score);
        EditorPrefs.SetInt(ScorePrefsKey, _score);
        playRootPresenter.SetScoreText(_score);
    }

    private void OnClickScoreReset()
    {
        PlayRootPresenter playRootPresenter = FindAnyObjectByType<PlayRootPresenter>();
        _score = 0;
        EditorPrefs.SetInt(ScorePrefsKey, _score);
        playRootPresenter.SetScoreText(_score);
    }
}
