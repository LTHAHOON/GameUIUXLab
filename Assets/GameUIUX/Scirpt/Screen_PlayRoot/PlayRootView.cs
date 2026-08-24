using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayRootView : UIView
{
    private Slider _healthSlider;
    private TMP_Text _messageText;
    private TMP_Text _scoreText;

    public PlayRootView(CanvasDocument canvasDocument) : base(canvasDocument)
    {
        _healthSlider = canvasDocument.GetUI<Slider>("Health_Slider");
        _messageText = canvasDocument.GetUI<TMP_Text>("Message_Text");
        _scoreText = canvasDocument.GetUI<TMP_Text>("Score_Text");
    }

    public Slider HealthSlider => _healthSlider;
    public TMP_Text MessageText => _messageText;
    public TMP_Text ScoreText => _scoreText;
}
