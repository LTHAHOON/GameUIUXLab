using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _resolutionDropDown;

    private Resolution[] _resolutions;

    private void Awake()
    {
        _resolutions = Screen.resolutions;
        _resolutionDropDown.ClearOptions();
        List<string> options = new();
        for (int i = 0; i < _resolutions.Length; ++i)
        {
            options.Add($"{_resolutions[i].width} x {_resolutions[i].height}");
        }
        _resolutionDropDown.AddOptions(options);
    }

    private void OnEnable()
    {
        int currentOptionIndex = GetCurrentResolutionIndex();
        _resolutionDropDown.value = currentOptionIndex;
    }

    public void SetResolution(int currentOptionIndex)
    {
        Resolution curResolution = _resolutions[currentOptionIndex];
        Screen.SetResolution(curResolution.width, curResolution.height, true);
        Debug.Log($"현재 해상도: {curResolution.width} x {curResolution.height}");
    }

    private int GetCurrentResolutionIndex()
    {
        for (int i = _resolutions.Length - 1; i >= 0; --i)
        {
            if (Screen.currentResolution == _resolutions[i])
            {
                return i;
            }
        }
        return 0;
    }
}
