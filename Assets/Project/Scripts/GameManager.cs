using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using GameSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IFactoryObject, IPoolingManager
{
    static public GameManager instance;
    [SerializeField] List<PoolingPeople> poolingPeoplesOn = new List<PoolingPeople>();
    [SerializeField] List<PoolingPeople> poolingPeoplesOff = new List<PoolingPeople>();
    [SerializeField] int money;
    [SerializeField] int stackCapacity;
    [SerializeField] int currentStack;
    [SerializeField] int maxPeople;
    [SerializeField] bool DeleteAllPlayerPrefs;
    [SerializeField] MessageGame messageGame;
    [SerializeField] Text textCurrentStackCapacity;
    [SerializeField] Text textCurrentStackCapacityPlayGame;

    public void SendMessage(string msg)
    {
        messageGame.SendMessage(msg);
    }

    public void BuyStack()
    {
        if (money < 50)
        {
            messageGame.SendMessage("You don't have enough money", true);
        }
        else if (stackCapacity == 12)
        {
            messageGame.SendMessage("Exceeds the capacity", true);
        }
        else
        {
            SubMoney(50);
        }
    }

    public bool AvailableStack()
    {
        return currentStack < stackCapacity;
    }

    public void AddCurrentStack()
    {
        currentStack++;
    }

    public void SubtractCurrentStack()
    {
        currentStack--;
    }

    public int SumMoney()
    {
        money += 10;
        PlayerPrefs.SetInt("money", money);
        return money;
    }

    public int SubMoney(int money)
    {
        this.money -= money;
        stackCapacity ++;
        PlayerPrefs.SetInt("stackCapacity", stackCapacity);
        textCurrentStackCapacity.text = "Current stack capacity: " + stackCapacity.ToString();
        textCurrentStackCapacityPlayGame.text = "Stack capacity: " + stackCapacity;
        PlayerPrefs.SetInt("money", this.money);
        return this.money;
    }


    public void AddObject(PoolingPeople poolingPeople = null)
    {
        foreach(PoolingPeople pPeople in poolingPeoplesOff)
        {
            poolingPeoplesOn.Add(pPeople);
        }
        
        Debug.Log("AddObjectStart");
    }

    public void AddObjec()
    {
        foreach (PoolingPeople pPeople in poolingPeoplesOff)
        {
            poolingPeoplesOn.Add(pPeople);
            pPeople.EnabledObject();
        }
        poolingPeoplesOff.Clear();
        Debug.Log("AddObject");
        Invoke("AddObjec", 10f);
    }

    public void CreateObject(string objectName)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveObject(PoolingPeople poolingPeople)
    {
        poolingPeoplesOn.Remove(poolingPeople);
        poolingPeoplesOff.Add(poolingPeople);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        if (DeleteAllPlayerPrefs) PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("stackCapacity")) stackCapacity = PlayerPrefs.GetInt("stackCapacity");
        else PlayerPrefs.SetInt("stackCapacity", 3);
        if(stackCapacity == 0) PlayerPrefs.SetInt("stackCapacity", 3);
        if (PlayerPrefs.HasKey("money")) money = PlayerPrefs.GetInt("money");
        else PlayerPrefs.SetInt("money", 0);
        Invoke("AddObjec", 10f);
        textCurrentStackCapacity.text = "Current stack capacity: " + stackCapacity;
        textCurrentStackCapacityPlayGame.text = "Stack capacity: " + stackCapacity;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("ListOn: " + poolingPeoplesOn.Count);
            Debug.Log("ListOff: " + poolingPeoplesOff.Count);
        }
    }
}
