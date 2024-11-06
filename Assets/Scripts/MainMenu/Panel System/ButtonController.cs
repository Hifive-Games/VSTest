using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private PanelController targetPanel;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (targetPanel != null)
        {
            Debug.Log("Açılacak panel: " + targetPanel.name);
            PanelManager.Instance.OpenPanel(targetPanel);
        }
        else
        {
            Debug.LogError("Hedef panel atanmamış: " + gameObject.name);
        }
    }
}