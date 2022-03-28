using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour, IPausable
{
    private float m_TravelledDist = 0;
    private float m_MaxDistance = 15.0f;
    private float m_Speed = 5.0f;
    private Coroutine m_Coroutine;
    private Rigidbody m_Rb;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        enabled = false;
    }
    
    void FixedUpdate()
    {
        
        //Setting Limit to Distance movement of ELevator
        if (m_TravelledDist>= m_MaxDistance)
        {
            
            //Multithread routine syntax to pause and change direction to reverse to the elevator 
            //on max distance reached after limit travelling
            //If no routine executing during movement of elevator, then its Null..
            // if movement paused then call sub routine
            if (m_Coroutine == null)
            {
                m_Coroutine = StartCoroutine(nameof(ReverseElevator));
            }

        }
        else 
        {
            // Get Distance Step using Speed and Delta Time
            float distanceStep = Time.fixedDeltaTime * m_Speed;
            
            //to measure travel distance, getting it into unsigned value
            m_TravelledDist += Mathf.Abs(distanceStep);

            Vector3 elevatorPos = m_Rb.position;
            
            //increase Elevator position by unit DistanceStep
            elevatorPos.y += distanceStep;

            //Elevator Rigidbody Movement
            m_Rb.MovePosition(elevatorPos);
        }
    }

    private IEnumerator ReverseElevator()
    {
        // waiting script
        yield return new WaitForSeconds(3.0f);
        
        //Reset Travelled Distance
        m_TravelledDist = 0;

        //Change Direction to downwards of elevator
        m_Speed *= -1 ;
        
        //reset sub routine of pausing and reversing
        m_Coroutine = null;
    }

    public void OnGameStart()
    {
        enabled = true;
        StartCoroutine(StartElevator());
    }

    private IEnumerator StartElevator()
    {
        //Waiting Elevator for 3 Seconds to move in beginning
        yield return new WaitForSeconds(3.0f);

    }


}
