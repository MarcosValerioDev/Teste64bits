using GameInterfaces;
using UnityEngine;

public class BoxMoneyController : MonoBehaviour
{
    IBoxMoney iboxMoney;
    [SerializeField] ParticleSystem moneyParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moneyParticle.Play();
            if (iboxMoney == null) iboxMoney = other.GetComponent< PlayerController>();
            iboxMoney.BoxMoneyOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moneyParticle.Stop();
            iboxMoney.BoxMoneyOff();
        }
    }
}
