using UnityEngine;
using GameSystem;
using GameInterfaces;
using UnityEditor.PackageManager;
using System;

public class PlayerController : MonoBehaviour, IBoxMoney
{
    [SerializeField] PlayerMove player;
    [SerializeField] AnimatorController animatorController;
    [SerializeField] PlayerInertialEffecgt playerInertialEffecgt;
    [SerializeField] GameObject poofParticle;
    [SerializeField] float radius;
    [SerializeField] bool isAttack;


    void Start()
    {
        playerInertialEffecgt.Start();
        UIController uiController = FindAnyObjectByType<UIController>();
        uiController.SetPlayerActions(Run, Punch);
        Instantiate(poofParticle, new Vector3(transform.position.x-100f, transform.position.y + 1f, transform.position.z), Quaternion.identity);
    }

    void NotAttack()
    {
        isAttack = false;
    }

    void Update()
    {
        player.FUpdate(isAttack);
        animatorController.SetAnimator(player.Walking(), isAttack);
        playerInertialEffecgt.FUpdate(player.Horizontal(), player.VerticalRunning());
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X)) Punch();
    }

    public void Run(bool isRunning)
    {
        player.SetIsRunning(isRunning);
    }

    public void BoxMoneyOn()
    {
        playerInertialEffecgt.ClearCollect();
    }

    public void BoxMoneyOff()
    {
        playerInertialEffecgt.SetBoxMoney(false);
    }

    public void PunchCollider()
    {
        Debug.Log("Colidiu1");
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider c in colliders)
        {
            Debug.Log("Colidiu2");
            if (c.CompareTag("People"))
            {
                CharacterController chctl = c.GetComponent<CharacterController>();
                if (chctl)
                {
                    if (!c.GetComponent<PeopleController>().isDead)
                    {
                        try
                        {
                            c.GetComponent<PeopleController>().SetAction(c.name, playerInertialEffecgt.EnabledCollect);
                            float dir = Vector3.Dot(transform.forward, c.transform.forward);
                            Vector3 center = c.GetComponent<CharacterController>().center;
                            if (dir > 0f) center.z = -0.6f;
                            c.GetComponent<CharacterController>().center = center;
                        }
                        catch(Exception ex)
                        {
                            Debug.Log(ex.Message);
                        }
        
                            c.GetComponent<PeopleController>().ColletePeople();
                            Instantiate(poofParticle, new Vector3(c.transform.position.x, c.transform.position.y + 1f, c.transform.position.z), Quaternion.identity);
                        
                    }
                }
               
            }
        }
    }

    public void Punch()
    {
        if (!isAttack)
        {
            isAttack = true;
            animatorController.SetPunch();
            animatorController.SetAnimator(false, false);
            Invoke("NotAttack", 0.4f);
            Invoke("PunchCollider", 0.2f);
        }
    }
}
