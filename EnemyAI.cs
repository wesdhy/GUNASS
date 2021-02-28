using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    NavMeshAgent m_enemy = null;

    [SerializeField] Transform[] m_tfWayPoints = null; 
    int m_pointcount = 0;


    Transform m_target = null;

    public void SetTarget(Transform p_target)
    {
       
        CancelInvoke();
        m_target = p_target;
    }

    public void RemoveTarget()
    {
        m_target = null;
        InvokeRepeating("CheckWayPoint", 0f, 2f); 
    }

    void CheckWayPoint()
    {   if (m_target == null)
        {
            if (m_enemy.velocity == Vector3.zero)
            {
                m_enemy.SetDestination(m_tfWayPoints[m_pointcount++].position);
                if (m_pointcount >= m_tfWayPoints.Length) m_pointcount = 0;
            }
        }
    }
    

    // Start is called before the first frame update
    void Start() 
    {
        m_enemy = GetComponent<NavMeshAgent>();
        InvokeRepeating("CheckWayPoint", 0f, 2f);//2초마다 함수 반복
    }

    // Update is called once per frame
    void Update()
    {
        if (m_target !=     null)
        {
            m_enemy.SetDestination(m_target.position);
        }
    }
}
