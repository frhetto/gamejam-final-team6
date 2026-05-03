using UnityEngine;

public class O2System : MonoBehaviour
{
    public static O2System Instance { get; private set; }

    [SerializeField] float maxO2 = 100f;
    [SerializeField] float drainPerSecond = 5f;

    float currentO2;
    bool isDead;

    public float Normalized => currentO2 / maxO2;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        currentO2 = maxO2;
    }

    void Update()
    {
        if (isDead) return;

        currentO2 -= drainPerSecond * Time.deltaTime;
        currentO2 = Mathf.Max(currentO2, 0f);

        HUDManager.Instance?.UpdateO2(Normalized);

        if (currentO2 <= 0f)
            Die();
    }

    // Call this from canister pickup
    public void Refill(float amount)
    {
        currentO2 = Mathf.Min(currentO2 + amount, maxO2);
    }

    void Die()
    {
        isDead = true;
        HUDManager.Instance?.ShowLose();
    }
}
