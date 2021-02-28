using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitBox : MonoBehaviour,IDamageable {
    public ParticleSystem m_DieEffect;
    public int m_ReloadTime = 1;
    public ParticleSystem m_HitEffect;
    public float health = 100;
    public Animator m_animator;



    public void OnDamage(float damageAmout)
    {
        health -= damageAmout;
        
        if (health <= 0)
        {
            
            m_DieEffect.Play();
            
            //gameObject.SetActive(false);
            pointt.Onpoint(1);
            m_animator.SetTrigger("Two Handed Sword Death");

          
            Destroy(gameObject, 5);

        }
        m_HitEffect.Play();


        m_animator.SetTrigger("Firing Rifle");

    }

   


    private IEnumerator diecaunt()
    {
        

        yield return new WaitForSeconds(m_ReloadTime);

        
        
    }
}
