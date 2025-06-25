using UnityEngine;
using TMPro;

public enum DrawMode
{
    Line,
    Source,
    Sink
}

public class DrawModeSelector : MonoBehaviour
{
    public TMP_Dropdown modeDropdown; // assign in Inspector
    public DrawMode CurrentMode { get; private set; } = DrawMode.Line;

    private void Awake()
    {
        if (modeDropdown != null)
        {
            modeDropdown.onValueChanged.AddListener(OnDropdownChanged);
            OnDropdownChanged(modeDropdown.value); // initialize
        }
    }

    private void OnDropdownChanged(int index)
    {
        CurrentMode = (DrawMode)index;
    }
}
