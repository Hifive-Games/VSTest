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

    public virtual void OnBackButtonClick()
    {
        PanelManager.Instance.GoBack();
    }
}