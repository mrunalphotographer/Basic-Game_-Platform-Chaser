using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody m_Rb;
    private GameObject m_FollowTarget;
    public float speed;
    public float radius;
    private bool m_IsRecharge;

    private void Awake()
    {
        AddCircle();
        m_Rb = GetComponent<Rigidbody>();
        m_IsRecharge = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_FollowTarget = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos = m_FollowTarget.transform.position - transform.position;
        targetPos.y = 0;
        m_Rb.AddForce(targetPos.normalized * speed);

        if(Mathf.Abs( targetPos.magnitude)<= radius && m_IsRecharge)
        {
            m_IsRecharge = false;
            m_Rb.AddForce(targetPos.normalized * speed * 1.1f,ForceMode.Impulse);
            Invoke(nameof(Recharge),2.0f);
        }

        if (transform.position.y <= -10.0f) 
        {
            Destroy(gameObject);
        }

    }
    private void Recharge()
    {
        m_IsRecharge = true;
    }

    void AddCircle()
    {
        //Setting Circle around Player at runtime
        GameObject go = new GameObject
        {
            name = "Circle"
        };
        Vector3 circlePos = Vector3.zero;
        circlePos.y = -0.49f;
        go.transform.parent = transform;
        go.transform.localPosition = circlePos;


        //GameObject Effects
        go.DrawCircle(radius, .02f);

    }

    
}
