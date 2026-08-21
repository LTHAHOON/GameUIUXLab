using UnityEngine;
using UnityEngine.UI;

public class TitleView : UIView
{
    private Button _playButton;
    private Button _settingButton;
    private Button _exitButton;

    public TitleView(CanvasDocument canvasDocument) : base(canvasDocument)
    {
        _playButton = canvasDocument.GetUI<Button>("Play_Button");
        _settingButton = canvasDocument.GetUI<Button>("Setting_Button");
        _exitButton = canvasDocument.GetUI<Button>("Exit_Button");
    }

    public Button PlayButton => _playButton;
    public Button SettingButton => _settingButton;
    public Button ExitButton => _exitButton;


}
