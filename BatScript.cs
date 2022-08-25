using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using player = PlayerPath;
using follow = FollowThePath;
using levelData = LevelScore;
using levelManager = LevelManager;
public class BatScript : MonoBehaviour
{
    Rigidbody2D m_Rigidbody;
    //for mask
    public GameObject mask;
    public float timer = 0f;
    public float growTime = 3f;
    public float maxSize = 10f;
    public bool isMaxSize = false;
    public int previosLevel;
    public List<AudioClip> mouthSounds = new List<AudioClip>();
   
    //Spriote renderer for dmg
    SpriteRenderer spriteRenderer;
    private Animator animator;
    public int count = 0;
    public static  Vector3 test2;
    public bool check = false;
    private GameObject pointer;
    public class Bat
    {
        //flag for victory
        public static bool isWon = false;
        //flag for hitting wall
        public static bool isHit = false;
        //flag for coming short
        public static bool isShort  = false;
        
    }
    public void Start()
    {
       
        m_Rigidbody = GetComponent<Rigidbody2D>();
        Bat.isShort = false;
        Bat.isWon = false;
        Bat.isHit = false;
        pointer = GameObject.FindGameObjectWithTag("Pointer");
        pointer.SetActive(false);
        mask = this.gameObject.transform.GetChild(0).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        GameManager.instance.levelScore = 0;
        previosLevel = GameManager.instance.previousLevel;
        //populate audio list with audio  10 sounds hard coded 
        for(int i = 1; i < 10; i++)
        {
            mouthSounds.Add((AudioClip)Resources.Load("Sounds/moth-b-"+i));
        }
    }

   

    public void Update()
    {
        if (Bat.isShort)
        {
           
            var currentPos = this.transform.position;

            this.transform.position = Vector2.MoveTowards(currentPos,follow.lastLocation, 2 * Time.deltaTime);
           
        }
        if(player.canMove)
        {
            animator.Play("bat-fly-w");
     
            if (check == false)
            {
           
                follow.hasnotmoved = true;
           
                StartCoroutine(waitForInput());
                check = true;
            }
            
           
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {

        
        if (col.gameObject.tag == "Wall")
        {
            stopped = true;
            Bat.isHit = true;
            follow.isMove = false;    
            BatLost();
        }
       
       
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Moth")
        {
      
            transform.GetComponent<AudioSource>().clip = mouthSounds[count];
            transform.GetComponent<AudioSource>().Play();
            count++;

        }

        if (col.gameObject.tag == "Victory" )
        {
            
            Bat.isWon = true;
            follow.isMove = false;
            // Prompt Game manager for next level or retry
            GameManager.instance.NextLevel();

            //SAVE DATA FROM LEVELSCORE HERE
            // DATA IS IN LEVEL MANAGER
 
            int beforeScoreSet = PlayerPrefs.GetInt("score" + levelManager.currentLevel);
           
            if(count > beforeScoreSet)
            { 
                levelData.SaveLevelData(levelManager.currentLevel, levelManager.mothCount);
                if (levelManager.currentLevel != previosLevel)
                {
                    int tScore = PlayerPrefs.GetInt("totalScore");
                    int levelScore = PlayerPrefs.GetInt("score" + levelManager.currentLevel);
               
                    levelData.SaveTotalScore(levelManager.currentLevel, levelManager.mothCount + tScore);
                }
            }
           
                
         
            
              
           
            this.gameObject.active = false;
        }
        
    }
    private void BatLost()
    {
        
        stopped = true;
        StartCoroutine(batDeathSounds());
        player.canMove = false;
        count = 0;
        animator.Play("bat-collide");
        GameManager.instance.GameOver();
        player.isDead = true;
       
        Bat.isShort = false;
    }
    IEnumerator batDeathSounds()
    {
        AudioClip hitWall = Resources.Load<AudioClip>("Sounds/hit-wall-a");
        AudioClip fail = Resources.Load<AudioClip>("Sounds/fail-a");
        transform.GetComponent<AudioSource>().clip = hitWall;
        transform.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(hitWall.length);
        transform.GetComponent<AudioSource>().clip = fail;
        transform.GetComponent<AudioSource>().Play();
    }

    private bool stopped = false;
    IEnumerator  waitForInput()
    {
        if (stopped)
        {
            yield break;
        }
        yield return new WaitForSeconds(2);

        if (follow.waypoints.Length == 0)
        {
          
            pointer.SetActive(true);
        }
    }

}
