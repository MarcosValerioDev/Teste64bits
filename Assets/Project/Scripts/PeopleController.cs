using UnityEngine;
using GameSystem;
using UnityEngine.Events;

public class PeopleController : MonoBehaviour
{
    public PoolingPeople poolingPeople;
    public UnityAction<string> collectPeople;
    public bool isDead;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        poolingPeople.Start();
        GetComponent<CharacterController>().enabled = true;
    }

    public void SetAction(string name, UnityAction<string> collectPeople)
    {
       
        this.name = name;
        this.collectPeople += collectPeople;
    }

    public void ColletePeople()
    {
        isDead = true;
        poolingPeople.Ragdoll();
        Invoke("DisableObject", 4f);
    }

    public void DisableObject()
    {
        collectPeople(name);
        poolingPeople.DisableObject();
    }

    private void OnEnable()
    {
        isDead = false;
    }

    private void OnDisable()
    {
        collectPeople = null;
    }
}
