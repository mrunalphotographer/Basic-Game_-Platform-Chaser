using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IPausable
{
    private float _horizontalAxis;
    private float _verticalAxis;
    public float playerMoveSpeed;
    private Rigidbody m_Rb;
    private GameObject m_Elevator;
    float offsetElevatorY;
    public Camera followCamera;
    private Vector3 cameraPos;
    Quaternion targetRotation;
    private float speedModifier;
    public UnityEvent OnPlayerLost;

    // Start is called before the first frame update
    void Awake()
    {
        enabled = false;
        m_Rb = GetComponent<Rigidbody>();
        offsetElevatorY = 0;
        cameraPos = followCamera.transform.position - m_Rb.position;
        speedModifier = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _horizontalAxis = Input.GetAxis("Horizontal");
        _verticalAxis = Input.GetAxis("Vertical");

        if (transform.position.y <= -15.0f)
        {
            OnPlayerLost.Invoke();
        }

        Vector3 playerPos = m_Rb.position;
        
        //Player Vector Movement with Normalisation
        Vector3 movement = new Vector3(
                    _horizontalAxis,
                    0,
                    _verticalAxis
            ).normalized;

        if (movement != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        }

       

        /*//Player Rotation
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.forward, movement);

        //to get backward rotation
        if (Mathf.Approximately(Vector3.Dot( movement, Vector3.forward) ,-1.0f))
        {
            targetRotation= Quaternion.LookRotation(-Vector3.forward);
        }*/

        //Control Speed of rotation
        targetRotation = Quaternion.RotateTowards(m_Rb.rotation, targetRotation, 360 *Time.fixedDeltaTime);

        //Sticking Player with ELevator using Y axis position
        if(m_Elevator != null)
        {
             playerPos.y = m_Elevator.transform.position.y + offsetElevatorY;
            
        }
        
        //transform.Translate(movement * Time.deltaTime * playerMoveSpeed);

        //Player Movement and Rotation using Rigidbody 
        m_Rb.MovePosition(playerPos + movement * speedModifier * Time.fixedDeltaTime * playerMoveSpeed);
        m_Rb.MoveRotation(targetRotation);

        
        
    }

    private void LateUpdate()
    {
        //Following Camera to Player without parenting camera
        followCamera.transform.position = m_Rb.position + cameraPos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            //Setting hitting object is Elevator
            m_Elevator = other.gameObject;

            //Majoring Distance between Player Y position and elevator Y position after Hitting
            offsetElevatorY  = transform.position.y - m_Elevator.transform.position.y;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            //Reset Hitting object after player moved from Elevator
            m_Elevator = null;
            offsetElevatorY = 0;
           
        }

        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Powerup"))
        {
            Destroy(collision.gameObject);
            speedModifier *= 1.5f;
            StartCoroutine(PowerUpTime());
        }
        if (collision.gameObject.CompareTag("Enemy") && speedModifier > 1)
        {
           
            Vector3 awayFromPlayer = collision.transform.position - transform.position;
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            enemyRb.AddForce(awayFromPlayer * 40.0f,ForceMode.Impulse);
        }
    }

    private IEnumerator PowerUpTime()
    {
        yield return new WaitForSeconds(20.0f);
        speedModifier = 1;
    }

    public void OnGameStart()
    {
        enabled = true;
    }
}
