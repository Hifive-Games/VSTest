using UnityEngine;
using UnityEngine.UI;

public class BackButtonController : MonoBehaviour
{
    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnBackButtonClick);
        }
    }

    private void OnBackButtonClick()
    {
        PanelManager.Instance.GoBack();
    }
}