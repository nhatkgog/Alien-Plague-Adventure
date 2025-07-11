using TMPro;
using UnityEngine;

public class Coin : Entity
{
    public static float missionCoinAmount = 0f;
    public TMP_Text missionCoinText;

    private float coinValue = 0f;
    private Animator animator;
    private Rigidbody2D rb;
    private bool hasLanded = false;

    [Header("SFX")]
    [SerializeField] private AudioClip collectClip;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coinValue = Random.Range(100, 201);

        if (rb != null)
            rb.gravityScale = 1f;

        Debug.Log($"[Coin] Spawned with value: {coinValue}");
    }



    void Update()
    {
        if (!hasLanded && IsGroundDetected())
        {
            hasLanded = true;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            missionCoinAmount += coinValue;

            Debug.Log($"Gained coin: {coinValue}");
            Debug.Log($"Total mission coins: {missionCoinAmount}");

            if (missionCoinText != null)
            {
                missionCoinText.text = missionCoinAmount.ToString();
            }
            else
            {
                Debug.LogWarning("MissionCoinText is null");
            }
            SFXManager.Instance.PlayOneShot(collectClip);
            Destroy(gameObject);
        }

    }


    public static float GetMissionTotal() => missionCoinAmount;
    public static void ResetMissionTotal() => missionCoinAmount = 0f;
}