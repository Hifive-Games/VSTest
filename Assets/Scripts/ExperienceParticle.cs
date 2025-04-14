using UnityEngine;
public class ExperienceParticle : MonoBehaviour
{
    public int experience = 1;
    public bool isTweening = false;

    public void GetExperience()
    {
        GameEvents.OnExperienceGathered.Invoke(experience);
        ObjectPooler.Instance.ReturnObject(gameObject);
    }

    void OnEnable()
    {
    }

    public void DetermineScaleAndColor()
    {
        Color color = Color.white;
        float scale = 1f;
        switch (experience)
        {
            case < 10:
                color = Color.green;
                scale = 0.5f;
                break;
            case >= 10 and <= 20:
                color = Color.yellow;
                scale = 1f;
                break;
            case > 30 and <= 50:
                color = Color.red;
                scale = 1.5f;
                break;
            case > 50 and <= 100:
                color = Color.blue;
                scale = 2f;
                break;
        }
        transform.localScale = new Vector3(scale, scale, scale);
        GetComponentInChildren<Renderer>().material.color = color;
    }

    void OnDisable()
    {
        ResetScaleAndColor();
    }

    private void ResetScaleAndColor()
    {
        transform.localScale = Vector3.one;
        GetComponentInChildren<Renderer>().material.color = Color.white;
    }
}