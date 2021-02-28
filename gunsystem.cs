using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gunsystem : MonoBehaviour {
   
    public AudioSource audioSource;
    
    public Animator m_animator;// 총의 애니메이터
    public Transform m_FireTransform;//총구 위치 트랜스폼
    public ParticleSystem m_ShellEjectEffect;//탄피 배출 효과 재생기
    public ParticleSystem m_MuzzleFlashEffect; //총구 화염 효과 재생기
    public AudioClip m_Shotclip;//발사소리
    public AudioClip m_nonamo;//
    public AudioSource m_GunAudioPlayer;//총 소리 재생기
    public AudioClip m_Reloadclip;//발사소리
  
   

    public GameObject m_ImpactPrefab;//피탄 장소에 생성할 이펙트

    public Text m_AmmoTEXT; // 남은 탄호나수 UI

    public int m_MaxAmmo = 12;
    public float m_TimeBetFire = 0.3f;//발사지연

    public float m_Damage = 100;
    public float m_ReloadTime = 2.0f;
    public float m_FireDistance = 100f;


    private enum State { ready, Empty ,Reloading };
    private State m_CurrentState = State.Empty; // 현재 총의 상태
    private float m_LastFireTime; // 총을 마지막으로 발사한 시점
    private int m_CurrentAmmo = 12;// 탄창에 남은 현재 탄약 개수

    private int layerMask;

    void Start () {
        m_CurrentState = State.ready;
        m_LastFireTime = 0;

       

        UpdateUI(); //ui를 갱신



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


            m_LastFireTime = Time.time;//마지막으로 총을 쏜 시점이 현재 시점으로 갱신
                Shot();
                
        }
    }
    private void Shot()
    {
        //실제 발사;
        RaycastHit hit;// 충돌정보 컨테이너 
        Vector3 hitPosition = m_FireTransform.position + m_FireTransform.forward * m_FireDistance; //총알 맞는곳 ; 총구위치 + 총구 위치로 앞쪽 방향 * 사정거리
        
        
        m_CurrentAmmo--;//남은 탄환수 -1

        if (m_CurrentAmmo <= 0)//탄환수가 0 이면 
        {
            m_CurrentState = State.Empty;
            m_GunAudioPlayer.clip = m_Shotclip;
            // m_animator.SetTrigger("");
        }

        if (m_GunAudioPlayer.clip != m_Shotclip)//
        {
            m_GunAudioPlayer.clip = m_Shotclip;//총 발사 소리 장전
        }
        m_GunAudioPlayer.Play();// 총격 소리 재생
        m_animator.SetTrigger("shot");//fire 트리거 당김
        m_MuzzleFlashEffect.Play();//총구화염
        m_ShellEjectEffect.Play();//탄피

        



        //레이케스트 (시작지점 , 방향, 총돌정보 컨테이너 , 사정거리)
        if ( Physics.Raycast(m_FireTransform.position,m_FireTransform.forward, out hit, Mathf.Infinity, layerMask))
         {

            Debug.Log("Hit Green " + hit.collider.gameObject.name);
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if(target != null)
            {
                target.OnDamage(m_Damage );
            }

            //충돌위치 가져오기
            hitPosition = hit.point;
            StartCoroutine(ShotEffect(hitPosition));
           
            //파탄효과
            GameObject decal  = Instantiate(m_ImpactPrefab, hitPosition, Quaternion.LookRotation(hit.normal));

            decal.transform.SetParent(hit.collider.transform);
        }
       //발사이펙트 재생 시작
       
    }
    //발사 이펙트를 재생하고 총알 궤적을 잠시 그렸다가 끄는 함수 
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
       

        yield return new WaitForSeconds(0.2f);
        
       
        
    }
    //총의 탄약 리플레쉬
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
