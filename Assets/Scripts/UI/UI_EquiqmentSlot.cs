public class UI_EquiqmentSlot : UI_ItemSlot
{
    public EquimentType equimentType;
    private void OnValidate()
    {
        gameObject.name = "Equeipment Slot - " + equimentType.ToString();
    }
}
