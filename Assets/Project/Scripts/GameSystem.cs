using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using GameInterfaces;
using System.Collections;

namespace GameSystem
{
    [Serializable]
    public class EffectInertia
    {
        [SerializeField] float timeRot;
        [SerializeField] float axes;
        [SerializeField] float maxRot;
        [SerializeField] float angleReturn;
        [SerializeField] bool loopControl;

        public float EixoRot(float axesJoy)
        {
            if (axesJoy < 0f) axes = Mathf.Lerp(axes, maxRot, timeRot * Time.deltaTime);
            else if (axesJoy > 0f) axes = Mathf.Lerp(axes, -maxRot, timeRot * Time.deltaTime);
            else axes = Mathf.Lerp(axes, 0f, timeRot * Time.deltaTime);
          
            return axes;
        }

        public float EixoRotZ(float axesJoy)
        {

            if (axesJoy != 0f)
            {
                axes = Mathf.Lerp(axes, -maxRot, timeRot * Time.deltaTime);
                loopControl = true;
                angleReturn = 13f;
            }
            else
            {
                if (loopControl)
                {
                    axes = Mathf.Lerp(axes, angleReturn, 0.8f * Time.deltaTime);
                    if (axes > 10f) angleReturn = -7f;
                    if (axes < -6f && angleReturn == -7f) loopControl = false;
                }
                else axes = Mathf.Lerp(axes, 0f, timeRot * Time.deltaTime);
            }
            return axes;
        }
    }

    [Serializable]
    public class Collect
    {
        [SerializeField] EffectInertia eixoX;
        [SerializeField] EffectInertia eixoZ;
        [SerializeField] Transform transformCollect;
        [SerializeField] bool isActive;

        public Transform GetTransformCollect()
        {
            return transformCollect;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void SetActive(bool active)
        {
            transformCollect.gameObject.SetActive(active);
            isActive = active;
        }

        public void RefreshAngle(float x, float y)
        {
            transformCollect.localEulerAngles = new Vector3(eixoX.EixoRotZ(y), eixoZ.EixoRot(x), 0f);
        }
    }

    [Serializable]
    public struct PlayerMove
    {
        [SerializeField] Transform transformPl;
        [SerializeField] CharacterController characterController;
        [SerializeField] FixedJoystick fixedJoyHorizontal;
        [SerializeField] float horizontal;
        [SerializeField] float vertical;
        [SerializeField] float verticalRunning;
        [SerializeField] bool IsRunning;
        [SerializeField] Vector3 dir;
        [SerializeField] float gravit;
        [SerializeField] float velocityGiro;
        [SerializeField] float velocity;
        [SerializeField] float yRot;
        [SerializeField] bool isPc;

        public bool Walking()
        {
            return IsRunning;
        }

        public float Horizontal()
        {
            return horizontal;
        }

        public float Vertical()
        {
            return vertical;
        }

        public float VerticalRunning()
        {
            return verticalRunning;
        }

        public void SetIsRunning(bool state)
        {
            IsRunning = state;
        }

        private void RefreshAxes()
        {
            if (isPc)
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
                IsRunning = Input.GetKey(KeyCode.Z)?true:false;
            }
            else
            {
                horizontal = fixedJoyHorizontal.Horizontal;
                vertical = fixedJoyHorizontal.Vertical;
                verticalRunning = IsRunning ? 1f : 0f;
            }
        }

        void MovePlayer()
        {
            if(IsRunning) dir = new Vector3(0f, 0f, 1f);
            else dir = new Vector3(0f, 0f, 0f);
            dir = transformPl.TransformDirection(dir);
            dir *= velocity;
            dir.y -= gravit * Time.deltaTime;
            characterController.Move(dir * Time.deltaTime);
        }

        void RotationPlayer()
        {
            if (horizontal!=0 && vertical!=0)
            {
                Vector3 dirRot = new Vector3(horizontal, 0f, vertical);
                transformPl.rotation = Quaternion.Lerp(transformPl.rotation, Quaternion.FromToRotation(Vector3.right, dirRot), velocityGiro * Time.deltaTime);
            }
        }

        void Move(bool isAttack)
        {
            if (!isAttack) MovePlayer();
            RotationPlayer();
        }

        public void FUpdate(bool isAttack)
        {
            RefreshAxes();
            Move(isAttack);
        }
    }

    [Serializable]
    public class MessageGame
    {
        [SerializeField] Canvas canvasBuyStack;
        [SerializeField] Canvas canvasMessage;
        [SerializeField] Text textMessage;

        public void SendMessage(string msg, bool buy = false)
        {
            if (buy) canvasBuyStack.enabled = false;
            textMessage.text = msg;
            canvasMessage.enabled = true;
        }

      
    }

    [Serializable]
    public class PlayerInertialEffecgt
    {
        [SerializeField] List<Collect> collect;
        [SerializeField] bool boxMoney;
        IMoneyController imoneyController;

        public void SetBoxMoney(bool boxMoney)
        {
            this.boxMoney = boxMoney;
        }

        public void EnabledCollect(string name)
        {
            bool inst=false;
            if (GameManager.instance.AvailableStack())
            {
                for (int i=0; i<collect.Count; i++)
                    {
                        if (!collect[i].IsActive())
                        {
                            collect[i].SetActive(true);
                            GameObject obj = Resources.Load<GameObject>(name);
                            Debug.Log("Name "+name);
                            Transform t = collect[i].GetTransformCollect();
                            GameManager.instance.AddCurrentStack();
                            GameObject.Instantiate(obj, t.position, t.rotation, t);
                        inst = true;
                        }
                    if (inst) break;
                }
            }
            else
            {
                GameManager.instance.SendMessage("Exceeded stack limit");
            }
        }

        public void Start()
        {
            imoneyController = GameObject.FindAnyObjectByType<MoneyController>();
        }
        
        public void ClearCollect()
        {
            boxMoney = true;
            for (int i=0; i<collect.Count; i++)
            {
                if (collect[i].IsActive())
                {
                    //Money controlle adiciona money
                    imoneyController.Sum();
                    collect[i].SetActive(false);
                    GameManager.instance.SubtractCurrentStack();
                    GameObject.Destroy(collect[i].GetTransformCollect().GetChild(1).gameObject);
                }
                else break;
            }
            boxMoney = false;
        }
        
        public void FUpdate(float horizontal,float vertical)
        {
            foreach (Collect c in collect) if (c.IsActive()) c.RefreshAngle(horizontal, vertical);
        }
    }

    [Serializable]
    public struct PlayerCam
    {
        [SerializeField] Transform cameraTransform;
        [SerializeField] float timeSmooth;

        public void Start()
        {
            cameraTransform = Camera.main.transform.parent;
        }

        public void RefreshPosition(Vector3 positionPlayer)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, positionPlayer, timeSmooth * Time.deltaTime);
        }
    }

    [Serializable]
    public class Score
    {
        [SerializeField] Text score;
        [SerializeField] Text scoreRecord;
        [SerializeField] int currentPoints;
        [SerializeField] int recordPoints;
        [SerializeField] int pointSum;
        IFactoryObject factoryObject;

        public void Start()
        {
            if (PlayerPrefs.HasKey("recordPoints"))
            {
                recordPoints = PlayerPrefs.GetInt("recordPoints");
                scoreRecord.text = recordPoints.ToString();
            }
            else PlayerPrefs.SetInt("recordPoints", 0);
            //factoryObject = GameManager.instance;
        }
        
        public void SumScore()
        {
            currentPoints += pointSum;

            if (PlayerPrefs.HasKey("recordPoints") && currentPoints > recordPoints)
            {
                //Instantiate particle recording
               // factoryObject.CreateObject("particlesRecordPoints");
                recordPoints = currentPoints;
                PlayerPrefs.SetInt("recordPoints", recordPoints);
                scoreRecord.text = recordPoints.ToString();
            }
            score.text = currentPoints.ToString();
            PlayerPrefs.SetInt("currentPoints", currentPoints);
        }


    }

    [Serializable]
    public class AnimatorController
    {
        [SerializeField] Animator animator;
        [SerializeField] bool currentStateRunning;

        public void SetAnimator(bool running, bool isAttack)
        {
            if (currentStateRunning!=running && !isAttack)
            {
                animator.SetBool("run", running);
                currentStateRunning = running;
            }
        }

        public void SetPunch()
        {
            animator.SetTrigger("punch");
        }
    }

    [Serializable]
    public class PoolingPeople : IPoolingPeople
    {
        [SerializeField] GameObject peopleObject;
        [SerializeField] Animator animator;
        [SerializeField] Rigidbody[] rigidBodys;
        [SerializeField] Collider[] colliders;
        IScore score;
        IPoolingManager poolingManager;

        public void Start()
        {
            peopleObject.transform.position = new Vector3(UnityEngine.Random.RandomRange(-23f, 23f), 0f, UnityEngine.Random.RandomRange(-23f, 23f));
            if (score == null) score = GameObject.FindAnyObjectByType<ScoreController>();
            if (poolingManager == null) poolingManager =GameObject.FindAnyObjectByType<GameManager>();
            poolingManager.AddObject(this);
            rigidBodys = peopleObject.GetComponentsInChildren<Rigidbody>();
            colliders = peopleObject.GetComponentsInChildren<Collider>();

            foreach (Rigidbody rb in rigidBodys) rb.isKinematic = true;
            foreach (Collider c in colliders) c.enabled = false;
        }

        public void Ragdoll()
        {
            score.SumScore();
            animator.enabled = false;
            for(int i=1;  i < colliders.Length; i++) colliders[i].enabled = true;
            foreach (Rigidbody rb in rigidBodys) rb.isKinematic = false;
            Debug.Log("RagDoll");
        }

        public void DisableObject()
        {
            poolingManager.RemoveObject(this);
            peopleObject.SetActive(false);
        }

        public void EnabledObject()
        {
            peopleObject.transform.position = new Vector3(UnityEngine.Random.RandomRange(-23f, 23f), 0f, UnityEngine.Random.RandomRange(-23f, 23f));
            poolingManager.AddObject(this);
            peopleObject.SetActive(true);
            animator.enabled = true;
            peopleObject.transform.GetComponent<CharacterController>().enabled = true;
            Vector3 center = peopleObject.transform.GetComponent<CharacterController>().center;
            center.z = 0.15f;
            peopleObject.transform.GetComponent<CharacterController>().center = center;
            for (int i = 1; i < colliders.Length; i++) colliders[i].enabled = false;
            foreach (Rigidbody rb in rigidBodys) rb.isKinematic = true;
        }
    }

}
