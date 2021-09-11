using Common;
using Common.Vr.Ui;
using Common.Vr.Ui.Controls;
using System.Collections.Generic;
using UnityEngine;

public class Ui_Menu_SetPanelSizes : Ui_Menu
{
  [SerializeField] private TextMesh _currentLabel;
  [SerializeField] private TextMesh[] _labels;
  [SerializeField] private Button[] _buttons;

  public override bool Show(Button source = null)
  {
    var result = base.Show(source);
    RefreshUi();
    return result;
  }

  public void OnResetDefaults()
  {
    var sizes = App_Details.Instance.PanelSizePresets;
    var defaults = App_Details.Instance.PanelSizePresetDefaults;
    var sizeCount = App_Details.Instance.PANEL_SIZE_PRESET_COUNT;
    for (var i = 0; i < sizeCount; i++)
    {
      sizes[i] = defaults[i];
    }
    StoreSizes();
    RefreshUi();
  }

  public void OnSetSizeButton(Button source)
  {
    var sizes = App_Details.Instance.PanelSizePresets;
    var sizeCount = App_Details.Instance.PANEL_SIZE_PRESET_COUNT;
    
    // Find the index to replace
    var indexToReplace = Global.NullUint;
    for (uint i = 0; i < sizeCount; i++)
    {
      // If current size is already one of the presets, do nothing
      if (sizes[i] == _currentSize)
      {
        return;
      }
      if (_buttons[i] == source)
      {
        indexToReplace = i;
      }
    }
    if (indexToReplace == Global.NullUint)
    {
      Debug.LogError("OnSetSizeButton: invalid button caller: " + source.name);
      return;
    }

    // Replace select size (we've already confirmed sizes[indexToReplace] != currentSize)
    if (_currentSize < sizes[indexToReplace])
    {
      // Special case for replacing 0th entry (breaks regular logic below)
      if (indexToReplace == 0)
      {
        sizes[0] = _currentSize;
        StoreSizes();
        RefreshUi();
        return; // Skip remaining logic
      }
      for (int i = (int)indexToReplace - 1; i >= 0; i--) // i as int lets i < 0 (loop exit)
      {
        if (_currentSize < sizes[i])
        {
          sizes[i + 1] = sizes[i];
        }
        else
        {
          sizes[i + 1] = _currentSize;
          StoreSizes();
          RefreshUi();
          return; // Skip the post-for-loop logic, which handles if we never got here
        }
      }
      sizes[0] = _currentSize;
      StoreSizes();
      RefreshUi();
    }
    else
    {
      // Special case for replacing last entry (breaks regular logic below)
      if (indexToReplace == sizeCount - 1)
      {
        sizes[sizeCount - 1] = _currentSize;
        StoreSizes();
        RefreshUi();
        return; // Skip remaining logic
      }
      for (var i = indexToReplace + 1; i < sizeCount; i++)
      {
        if (_currentSize > sizes[i])
        {
          sizes[i - 1] = sizes[i];
        }
        else
        {
          sizes[i - 1] = _currentSize;
          StoreSizes();
          RefreshUi();
          return; // Skip the post-for-loop logic, which handles if we never got here
        }
      }
      sizes[sizeCount - 1] = _currentSize;
      StoreSizes();
      RefreshUi();
    }
  }

  private float _currentSize = float.MaxValue;
  private string _currentLabelPrefix;

  protected override void Start()
  {
    base.Start();
    var t = transform.localPosition;
    t.y = 0;
    transform.localPosition = t;
    _currentLabelPrefix = _currentLabel.text;
  }

  private void Update()
  {
    var newCurrentSize =
      Mathf.Round(App_Functions.Instance.MyScreen.transform.localScale.x * 100.0f) / 100.0f;
    if (newCurrentSize != _currentSize)
    {
      _currentSize = newCurrentSize;
      _currentLabel.text = _currentLabelPrefix + _currentSize;
    }
  }

  private void RefreshUi()
  {
    var sizes = App_Details.Instance.PanelSizePresets;
    var sizeCount = App_Details.Instance.PANEL_SIZE_PRESET_COUNT;
    for (var i = 0; i < sizeCount; i++)
    {
      _labels[i].text = sizes[i] + "";
    }
  }

  private void StoreSizes()
  {
    App_Details.Instance.StorePanelSizePresets();
  }
}
