using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : UIView
{
    private Button _resumeButton;
    private Button _optionButton;
    private Button _quitButton;

    public PauseMenuView(CanvasDocument canvasDocument) : base(canvasDocument)
    {
        _resumeButton = canvasDocument.GetUI<Button>("Pause_BackToGame_Button");
        _optionButton = canvasDocument.GetUI<Button>("Pause_Option_Button");
        _quitButton = canvasDocument.GetUI<Button>("Pause_Quit_Button");
    }

    public Button ResumeButton => _resumeButton;
    public Button OptionButton => _optionButton;
    public Button QuitButton => _quitButton;
}
