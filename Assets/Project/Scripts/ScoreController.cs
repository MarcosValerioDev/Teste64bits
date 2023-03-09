using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem;
using GameInterfaces;

public class ScoreController : MonoBehaviour, IScore
{
    [SerializeField] Score score;

    public void SumScore()
    {
        score.SumScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        score.Start();
    }


}
