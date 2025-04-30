using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDrop : MonoBehaviour
{
    [SerializeField] private Spell spell;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatHeight = 0.5f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight * Time.deltaTime, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterController _))
        {
            SpellManager.Instance.EquipSpell(spell);
            SpellDatabase.Instance.RemoveSpell(spell);
            Destroy(gameObject);
        }
    }

    public void SetSpell(Spell spell)
    {
        this.spell = spell;
    }

    public Spell GetSpell()
    {
        return spell;
    }
    
}
