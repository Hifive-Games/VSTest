using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject targetPanel;

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