using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class NewMonoBehaviourScript : MonoBehaviour
{
    private VisualElement _SettingsBackground;
    private Button _SettingsButton;
    private Button _CloseSettingsButton;
    private VisualElement _SettingsPanel;



    void Start()
    {
       var root = GetComponent<UIDocument>().rootVisualElement;
        _SettingsBackground = root.Q<VisualElement>("SettingsBackground");
        _SettingsButton = root.Q<Button>("SettingsButton");
        _CloseSettingsButton = root.Q<Button>("CloseSettingsButton");
        _SettingsPanel = root.Q<VisualElement>("SettingsPanel");


        _SettingsBackground.style.display = DisplayStyle.None;

        _SettingsButton.RegisterCallback<ClickEvent>(OnOpenButtonClicked);

        _CloseSettingsButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);
       
        


    }

    // Update is called once per frame
    private void OnOpenButtonClicked(ClickEvent evt) 
    {
        _SettingsBackground.style.display = DisplayStyle.Flex;
        _SettingsPanel.AddToClassList("SettingsPanelUp");
        _SettingsBackground.AddToClassList("SettingsBackgroundFadeIn");
    }

    private void OnCloseButtonClicked(ClickEvent evt) 
    {
        _SettingsBackground.style.display = DisplayStyle.None;
        _SettingsPanel.RemoveFromClassList("SettingsPanelUp");
        _SettingsBackground.RemoveFromClassList("SettingsBackgroundFadeIn");

    }
}
