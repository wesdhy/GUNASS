using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI2 : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] EnemyAI m_enemy = null;
   
    public Animator m_animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))   
        {

          
            m_enemy.SetTarget(other.transform);
            m_animator.SetTrigger("Firing Rifle");
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            m_enemy.RemoveTarget();
        }
    }





}
