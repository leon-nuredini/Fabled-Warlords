using UnityEngine;
using UnityEngine.UI;

public class UICredits : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button     _closeButton;

    private GraphicRaycaster _graphicRaycaster;

    private void Awake()
    {
        _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        _closeButton.onClick.AddListener(CloseCreditsPanel);
        CloseCreditsPanel();
    }

    private void OnEnable()  => UIMainMenu.OnClickCreditsButton += OpenCreditsPanel;
    private void OnDisable() => UIMainMenu.OnClickCreditsButton -= OpenCreditsPanel;

    private void OpenCreditsPanel()
    {
        _graphicRaycaster.enabled = true;
        _panel.SetActive(true);
    }

    private void CloseCreditsPanel()
    {
        _graphicRaycaster.enabled = false;
        _panel.SetActive(false);
    }
}
