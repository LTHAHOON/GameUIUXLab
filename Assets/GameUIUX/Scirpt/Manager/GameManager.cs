using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private bool _useTargetFrame;
    [Min(30)]
    [SerializeField]
    private int _targetFrameRate;
    [SerializeField]
    private bool _debugFrame;

    private const float FRAME_INTERVAL_DELAY = 0.3f;
    private float _currentFrame;
    private float _currentFrameTime = 0f;
    private int _currentFrameCount = 0;

    private void Awake()
    {
        Instance = this;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        if (_useTargetFrame)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = _targetFrameRate;
        }
        else
        {
            Application.targetFrameRate = 0;
        }
    }

    private void Update()
    {
        if (_debugFrame)
        {
            _currentFrameTime += Time.unscaledDeltaTime;
            ++_currentFrameCount;
            if (_currentFrameTime >= FRAME_INTERVAL_DELAY)
            {
                _currentFrame = _currentFrameCount / _currentFrameTime;
                _currentFrameTime = 0f;
                _currentFrameCount = 0;
            }
        }
    }

    private void OnGUI()
    {
        if (_debugFrame)
        {
            GUI.Label(new Rect(10, 10, 100, 50), $"FPS: {_currentFrame:F2}", new()
            {
                fontSize = 25,
                normal = { textColor = Color.white },
            });
        }
    }
}
