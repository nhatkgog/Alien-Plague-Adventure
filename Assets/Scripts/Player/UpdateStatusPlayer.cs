using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStatusPlayer : MonoBehaviour
{
    //public TextMeshProUGUI levelText;
    //public TextMeshProUGUI expText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI enduranceText;
    public TextMeshProUGUI damageBoomText;
    public TextMeshProUGUI statPointsText;

    public Button damageUpBtn;
    public Button defUpBtn;
    public Button enduranceUpBtn;
    public Button damageBoomUpBtn;

    public Button damageDownBtn;
    public Button defDownBtn;
    public Button enduranceDownBtn;
    public Button damageBoomDownBtn;

    public Button resetBtn;
    public Button saveBtn;

    private PlayerStatus player;

    private float tempDamage, tempDef, tempEndurance, tempDamageBoom;
    private int tempStatPoints;

    void Start()
    {
        player = PlayerSelector.Instance.GetSelectedPlayer();

        tempDamage = player.damage;
        tempDef = player.def;
        tempEndurance = player.endurance;
        tempDamageBoom = player.damageBoom;
        tempStatPoints = player.statPoints;

        damageUpBtn.onClick.AddListener(() => UpgradeStat("damage", +1));
        defUpBtn.onClick.AddListener(() => UpgradeStat("def", +1));
        enduranceUpBtn.onClick.AddListener(() => UpgradeStat("endurance", +1));
        damageBoomUpBtn.onClick.AddListener(() => UpgradeStat("damageBoom", +1));

        damageDownBtn.onClick.AddListener(() => UpgradeStat("damage", -1));
        defDownBtn.onClick.AddListener(() => UpgradeStat("def", -1));
        enduranceDownBtn.onClick.AddListener(() => UpgradeStat("endurance", -1));
        damageBoomDownBtn.onClick.AddListener(() => UpgradeStat("damageBoom", -1));

        resetBtn.onClick.AddListener(ResetStats);
        saveBtn.onClick.AddListener(SaveStats);

        UpdateUI();
    }

    void UpgradeStat(string stat, int amount)
    {
        if (amount > 0 && tempStatPoints <= 0) return;

        switch (stat)
        {
            case "damage":
                if (amount < 0 && tempDamage + amount < player.damage) return;
                tempDamage += amount;
                break;
            case "def":
                if (amount < 0 && tempDef + amount < player.def) return;
                tempDef += amount;
                break;
            case "endurance":
                if (amount < 0 && tempEndurance + amount < player.endurance) return;
                tempEndurance += amount;
                break;
            case "damageBoom":
                if (amount < 0 && tempDamageBoom + amount < player.damageBoom) return;
                tempDamageBoom += amount;
                break;
        }

        tempStatPoints -= amount;
        UpdateUI();
    }

    void ResetStats()
    {
        PlayerSelector.Instance.PartialResetPlayer();
        player = PlayerSelector.Instance.GetSelectedPlayer();
        tempDamage = player.damage;
        tempDef = player.def;
        tempEndurance = player.endurance;
        tempDamageBoom = player.damageBoom;
        tempStatPoints = player.statPoints;
        UpdateUI();
    }
    void SaveStats()
    {
        PlayerSelector.Instance.SetPlayerStatus(tempDef, tempEndurance, tempDamage, tempDamageBoom);
        PlayerSelector.Instance.SetStatPoint(tempStatPoints);
        Debug.Log("✅ Stats saved.");
    }

    void UpdateUI()
    {
        damageText.text = tempDamage.ToString();
        defText.text = tempDef.ToString();
        enduranceText.text = tempEndurance.ToString();
        damageBoomText.text = tempDamageBoom.ToString();
        statPointsText.text = tempStatPoints.ToString();
    }

}
