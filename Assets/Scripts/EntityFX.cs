using UnityEngine;
using System.Collections;

public class EntityFX : MonoBehaviour
{
    [Header("FX Settings")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        if (sr == null)
            Debug.LogWarning($"{gameObject.name}: No SpriteRenderer assigned to EntityFX!");
    }

    private void Start()
    {
        if (sr != null)
            originalMat = sr.material;
    }

    public void PlayHitFX()
    {
        if (sr != null)
            StartCoroutine(HitFX());
    }

    public IEnumerator HitFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(0.1f);
        sr.material = originalMat;
    }
}
