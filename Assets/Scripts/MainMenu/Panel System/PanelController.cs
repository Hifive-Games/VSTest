using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    /*
        bir panel açıldığında belirli işlemler yaptırmak istersek, buradan miras almasını sağlayabiliriz. ileri de gerekli olabilir...
    */
    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
    
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    /*
    [SerializeField] private GameObject targetPanel; // Hangi panel açılacak

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        PanelManager.Instance.OpenPanel(targetPanel);
    }
    */
}