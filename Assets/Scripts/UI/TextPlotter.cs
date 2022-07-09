using System.Text;
using UnityEngine;

/// <summary>
/// Component which takes text then plots it character by character to a TMPro.Text component
/// </summary>
[RequireComponent(typeof(TMPro.TMP_Text))]
public class TextPlotter : MonoBehaviour
{
    /// <summary>
    /// Text to plot
    /// </summary>
    [TextArea(5, 10)]
    public string _text;

    /// <summary>
    /// Delay between the plotted characters
    /// </summary>
    public float _delay = 0.05f;

    /// <summary>
    /// Time since the last plot
    /// </summary>
    private float _lastPlot = 0;

    /// <summary>
    /// Component displaying the text
    /// </summary>
    private TMPro.TMP_Text _textOutput;

    /// <summary>
    /// Current text index
    /// </summary>
    private int _index;

    /// <summary>
    /// Utility string builder
    /// </summary>
    private readonly StringBuilder _builder = new StringBuilder();

    void Start()
    {
        _textOutput = GetComponent<TMPro.TMP_Text>();
        _textOutput.text = "";
        _index = 0;
    }

    void Update()
    {
        if (Time.time - _lastPlot > _delay && _index < _text.Length)
        {
            _builder.Append(_text[_index]);
            _textOutput.text = _builder.ToString();
            _lastPlot = Time.time;
            _index++;
        }
    }

    /// <summary>
    /// If the user is fed up with waiting, a click will immediately plot all text.
    /// </summary>
    public void OnClick()
    {
        _textOutput.text = _text;
        _index = _text.Length;
    }
}
