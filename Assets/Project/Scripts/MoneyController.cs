using GameInterfaces;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour, IMoneyController
{
    [SerializeField] Text textmoney;
    [SerializeField] int payment;
    // Start is called before the first frame update
    public void Start()
    {
        if (PlayerPrefs.HasKey("money")) textmoney.text = PlayerPrefs.GetInt("money").ToString();
    }

    public void Sum()
    {
        textmoney.text = GameManager.instance.SumMoney().ToString();
    }

    public void Sub()
    {
        textmoney.text = GameManager.instance.SubMoney(payment).ToString();
    }
}
