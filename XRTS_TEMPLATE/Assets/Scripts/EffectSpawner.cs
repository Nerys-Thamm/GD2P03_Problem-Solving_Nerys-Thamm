using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> Effects;

    public void SpawnEffect(int _iEffectIndex)
    {
        if (_iEffectIndex < 0 || _iEffectIndex > Effects.Count)
        {
            Debug.Log("INVALID EFFECT INDEX GIVEN FOR " + this.gameObject.name);
            return;
        }
        else
        {
            Instantiate(Effects[_iEffectIndex], this.gameObject.transform.position, Quaternion.Inverse(gameObject.transform.rotation));
        }
    }
}