using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject craftUI;
    public UI_ItemToolTip itemToolTip;
    public UI_CraftWindow craftWindow;
    void Start()
    {
        SwitchTo(null);
        itemToolTip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyToo(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyToo(craftUI);
    }
    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.CompareTag("Menu"))
            {
                child.SetActive(false);
            }
            if (_menu != null)
            {
                _menu.SetActive(true);
            }
        }

    }
    public void SwitchWithKeyToo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }
        SwitchTo(_menu);
    }
}
