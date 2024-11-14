using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "PlayableCharacter/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public int id;
    public string characterName;
    public GameObject prefab;
    public Sprite characterImage;
    public bool isSelected = false;
}
