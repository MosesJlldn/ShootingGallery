using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour {

    // Use this for initialization
    delegate void CurrentState();
    CurrentState currentState;
    public float i;

    public Transform coolDownTransform;
    public Transform triggeredTransform;
    public GameObject target;
    public float speed = 0.1F;

    void Start () {
        i = 5;
        currentState = CoolDown;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        currentState();
	}

    public void CoolDown()
    {
        if (i > 0)
        {
            i -= Time.deltaTime;
            target.transform.localRotation = Quaternion.Lerp(target.transform.localRotation, coolDownTransform.localRotation, i);
        } else
        {
            i = 0;
            target.transform.localRotation = coolDownTransform.localRotation;
        }
    }

    public void Idle()
    {

    }

    public void Triggered()
    {
        if (i > 0)
        {
            i -= Time.deltaTime;
            target.transform.localRotation = Quaternion.Lerp(target.transform.localRotation, triggeredTransform.localRotation, i);
        }
        else
        {
            i = 0;
            target.transform.localRotation = triggeredTransform.localRotation;
        }
    }

    public void beHitted()
    {
        //if (other.tag.Equals("bullets"))
        //{
            i = 5;
            currentState = CoolDown;
            //GetComponent<Collider>().isTrigger = false;
            TargetManager.plusHittedTarget();
            //Destroy(other.gameObject, 0);
        //}
    }

    public void Trigger()
    {
        i = 5;
        currentState = Triggered;
    }
}

