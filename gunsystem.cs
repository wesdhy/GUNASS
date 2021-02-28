using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gunsystem : MonoBehaviour {
   
    public AudioSource audioSource;
    
    public Animator m_animator;
    public Transform m_FireTransform;
    public ParticleSystem m_ShellEjectEffect;
    public ParticleSystem m_MuzzleFlashEffect; 
    public AudioClip m_Shotclip;
    public AudioClip m_nonamo;
    public AudioSource m_GunAudioPlayer;
    public AudioClip m_Reloadclip;
  
   

    public GameObject m_ImpactPrefab;

    public Text m_AmmoTEXT; 

    public int m_MaxAmmo = 12;
    public float m_TimeBetFire = 0.3f;

    public float m_Damage = 100;
    public float m_ReloadTime = 2.0f;
    public float m_FireDistance = 100f;


    private enum State { ready, Empty ,Reloading };
    private State m_CurrentState = State.Empty; 
    private float m_LastFireTime; 
    private int m_CurrentAmmo = 999;

    private int layerMask;

    void Start () {
        m_CurrentState = State.ready;
        m_LastFireTime = 0;

       

        UpdateUI(); 



        layerMask = 1 << 9;
    }
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
           

            fire();
             UpdateUI();

            


        }

        else if (OVRInput.Get(OVRInput.Button.One))
        {
            Reload();

            

        }
    }

    public void fire()
    {

        if (m_CurrentState == State.ready && Time.time >= m_LastFireTime + m_TimeBetFire)
        {


            m_LastFireTime = Time.time;
                Shot();
                
        }
    }
    private void Shot()
    {
        
        RaycastHit hit;// 충돌정보 컨테이너 
        Vector3 hitPosition = m_FireTransform.position + m_FireTransform.forward * m_FireDistance; 총구위치 + 총구 위치로 앞쪽 방향 * 사정거리
        
        
        m_CurrentAmmo--;

        if (m_CurrentAmmo <= 0)
        {
            m_CurrentState = State.Empty;
            m_GunAudioPlayer.clip = m_Shotclip;
            // m_animator.SetTrigger("");
        }

        if (m_GunAudioPlayer.clip != m_Shotclip)//
        {
            m_GunAudioPlayer.clip = m_Shotclip;
        }
        m_GunAudioPlayer.Play();
        m_animator.SetTrigger("shot");
        m_MuzzleFlashEffect.Play();
        m_ShellEjectEffect.Play();

        



        //(시작지점 , 방향, 총돌정보 컨테이너 , 사정거리)
        if ( Physics.Raycast(m_FireTransform.position,m_FireTransform.forward, out hit, Mathf.Infinity, layerMask))
         {

            Debug.Log("Hit Green " + hit.collider.gameObject.name);
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target != null)
            {
                target.OnDamage(m_Damage );
            }

            
            hitPosition = hit.point;
            StartCoroutine(ShotEffect(hitPosition));
           
            
            GameObject decal  = Instantiate(m_ImpactPrefab, hitPosition, Quaternion.LookRotation(hit.normal));

            decal.transform.SetParent(hit.collider.transform);
        }
       
       
    }
   
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
       

        yield return new WaitForSeconds(0.2f);
        
       
        
    }
    
    private void UpdateUI()
    {
        if (m_CurrentState == State.Empty)
        {
            m_AmmoTEXT.text = "EMPTY";
        }

        else if (m_CurrentState == State.Reloading)
        {
            m_AmmoTEXT.text = "RELOADING";
        }

        else
        {
            m_AmmoTEXT.text = m_CurrentAmmo.ToString();
        }
    }

    public void Reload()
    {
        if(m_CurrentState != State.Reloading)
        {
            StartCoroutine(ReloadRoutin());
        }
    }

    private IEnumerator ReloadRoutin()
    {
        m_animator.SetTrigger("rolo");
        m_CurrentState = State.Reloading;//재장전 상태로 전환


        m_GunAudioPlayer.clip = m_Reloadclip;

        m_GunAudioPlayer.Play();

        UpdateUI();

        yield return new WaitForSeconds(m_ReloadTime);

        m_CurrentAmmo = m_MaxAmmo;
        m_CurrentState = State.ready;
        UpdateUI();
    }
}
