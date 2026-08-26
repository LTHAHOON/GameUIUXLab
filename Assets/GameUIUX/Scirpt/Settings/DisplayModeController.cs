using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayModeController : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _displayModeDropDown;

    private readonly List<FullScreenMode> _displayModes = new();
    private void Awake()
    {
        _displayModeDropDown.ClearOptions();

        _displayModes.Add(FullScreenMode.ExclusiveFullScreen); //전체화면
        _displayModes.Add(FullScreenMode.FullScreenWindow); //테두리 없는 창모드
        _displayModes.Add(FullScreenMode.Windowed); //일반 창모드

        List<string> options = _displayModes.Select(mode => mode.ToString()).ToList();
        _displayModeDropDown.AddOptions(options);
    }

    public void SetDisplayMode(int currentOptionIndex)
    {
        if (currentOptionIndex < 0 || currentOptionIndex >= _displayModes.Count)
        {
            return;
        }
        Screen.fullScreenMode = _displayModes[currentOptionIndex];
    }
}
