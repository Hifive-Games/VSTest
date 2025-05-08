#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "RandomizeBuffDeBuff", menuName = "BuffDeBuffSystem/DynamicBuffDeBuffSystem/RandomizeBuffDeBuff")]
public class RandomizeBuffDeBuff : BuffDebuffSystemBaseData
{
    /*
     
     Buradaki;
     
        effectName = effect.name;
        description = effect.description;
     oyun içinde UI yazsın diye var. İstersen bunlara  effectName = "Randomize: " + effect.name
     gibi bir şey ekleyebilirsin.
     
     Scriptable içinde sürekli değişecek bilgin olsun alakasız deme
     en son hangisini random çektiyse ismi ve açıklaması o olacak
     
     Ayrıca buradaki value of debuff ın şuanlık bir etkisi yok ama ileride
     sadece random gelince daha güçlü gelsin gibi bir şey eklersek belki kullanırız
     gerçi o zaman yeni scrpitable obje olarak ekleriz. mesela bi tane daha saldırı hızı buff ama saldırı hızı daha çok olan mesela

     
     */
    
    [SerializeField] private List<BuffDebuffSystemBaseData> randomizeBuffDebuff = new List<BuffDebuffSystemBaseData>();
    [SerializeField, Min(1)] private int randomCount = 1;

    public override void ApplyBuffDeBuffSystem()
    {
        if (randomizeBuffDebuff == null || randomizeBuffDebuff.Count == 0)
        {
            Debug.LogError($"{name}: RandomizeBuffDeBuff listesi boş! Lütfen listeyi doldurun.");
            return;
        }

        int count = Mathf.Min(randomCount, randomizeBuffDebuff.Count);
        foreach (var effect in randomizeBuffDebuff.OrderBy(x => Random.value).Take(count))
        {
            effectName = effect.name;
            description = effect.description;
            effect.ApplyBuffDeBuffSystem();
        }
    }

    private void OnValidate()
    {
        if (randomizeBuffDebuff == null || randomizeBuffDebuff.Count == 0)
        {
            Debug.LogError($"{name}: RandomizeBuffDeBuff listesi boş! Lütfen listeyi doldurun.");
        }

        if (randomCount < 1)
        {
            randomCount = 1;
            Debug.LogWarning($"{name}: randomCount 1'den küçük olamaz! 1 olarak ayarlandı.");
        }
    }

#if UNITY_EDITOR
    private void ShowEditorError(string message)
    {
        // Pop-up göster
        if (!EditorApplication.isPlaying)
        {
            EditorApplication.delayCall += () =>
            {
                EditorUtility.DisplayDialog("Hata!", message, "Tamam");
            };
        }
    }
#endif
}