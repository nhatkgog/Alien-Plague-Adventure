using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStatusPlayer : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI expText;
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

    private PlayerStatus player;

    void Start()
    {
        player = PlayerSelector.Instance.GetSelectedPlayer();

        damageUpBtn.onClick.AddListener(() => UpgradeStat("damage", +1));
        defUpBtn.onClick.AddListener(() => UpgradeStat("def", +1));
        enduranceUpBtn.onClick.AddListener(() => UpgradeStat("endurance", +1));
        damageBoomUpBtn.onClick.AddListener(() => UpgradeStat("damageBoom", +1));

        damageDownBtn.onClick.AddListener(() => UpgradeStat("damage", -1));
        defDownBtn.onClick.AddListener(() => UpgradeStat("def", -1));
        enduranceDownBtn.onClick.AddListener(() => UpgradeStat("endurance", -1));
        damageBoomDownBtn.onClick.AddListener(() => UpgradeStat("damageBoom", -1));

        UpdateUI();
    }

    void UpgradeStat(string stat, int amount)
    {
        // Nếu nâng cấp và hết điểm thì không cho nâng
        if (amount > 0 && player.statPoints <= 0)
            return;

        switch (stat)
        {
            case "damage":
                if (amount < 0 && player.damage + amount < 0) return;
                player.damage += amount;
                break;

            case "def":
                if (amount < 0 && player.def + amount < 0) return;
                player.def += amount;
                break;

            case "endurance":
                if (amount < 0 && player.endurance + amount < 0) return;
                player.endurance += amount;
                break;

            case "damageBoom":
                if (amount < 0 && player.damageBoom + amount < 0) return;
                player.damageBoom += amount;
                break;
        }

        // Cập nhật điểm stat
        player.statPoints -= amount;

        UpdateUI();
    }

    void UpdateUI()
    {
        levelText.text = "Level: " + player.level;
        expText.text = "EXP: " + player.exp;
        damageText.text = "Damage: " + player.damage;
        defText.text = "Defense: " + player.def;
        enduranceText.text = "Endurance: " + player.endurance;
        damageBoomText.text = "Boom Damage: " + player.damageBoom;
        statPointsText.text = "Stat Points: " + player.statPoints;
    }
}
