using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jump;
    
    Rigidbody2D rb;
    float dirX;
    bool facingRight = true;
    Vector3 localScale;
    Animator anim;
    float startX = 17;
    float startY = -1;
    bool dead;
    public static int life = 5;
    AudioSource audioSource;
    bool isMoving;
    
    string pathX, pathY, pathIsDead, pathLife;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        pathX = Application.dataPath + "/Resources/CharacterPosX.txt";
        pathY = Application.dataPath + "/Resources/CharacterPosY.txt";
        pathIsDead = Application.dataPath + "/Resources/CharacterIsDead.txt";
        pathLife = Application.dataPath + "/Resources/CharacterLife.txt";

        LoadIsDead();
        
        if(!dead)
            ResetLife();

        if (dead)
        {
            LoadLife();
            
            if (life < 1)
            {
                Save(17, -1);
                ResetLife();

                SceneManager.LoadScene("GameOver");
            }

            Load();
        }

        rb.transform.position = new Vector3(startX, startY);
    }

    void Update()
    {
        if (transform.position.y < -15)
        {
            if(transform.position.x > 41 && transform.position.x < 50)
                Save(41, -1);
            else if(transform.position.x > 102 && transform.position.x < 119)
                Save(102, -1);
            else if(transform.position.x > 170 && transform.position.x < 180)
                Save(170, 34);
            else if(transform.position.x > 186 && transform.position.x < 206)
                Save(186, 34);
            else if(transform.position.x > 316 && transform.position.x < 357)
                Save(316, 72);
            else
                Save(startX, startY);
        }

        if (transform.position.x > 555)
        {
            LevelPassed();
        }

        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (rb.velocity.x != 0)
            isMoving = true;
        else
            isMoving = false;
        if (isMoving && rb.velocity.y == 0)
        {
            if(!audioSource.isPlaying)
                audioSource.Play();
        }
        else
            audioSource.Stop();

        if (Input.GetButtonDown("Jump") && rb.velocity.y > -0.1 && rb.velocity.y < 0.1)
        {
            SoundManager.PlaySound("jump");
            rb.AddForce(Vector2.up * jump);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        
        if (Mathf.Abs(dirX) > 0 && rb.velocity.y > -0.1 && rb.velocity.y < 0.1)
        {
            anim.SetBool("isRunning", true);

        }

        else
            anim.SetBool("isRunning", false);

        if (rb.velocity.y > -0.1 && rb.velocity.y < 0.1)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }

        if (rb.velocity.y > 0.1)
            anim.SetBool("isJumping", true);


        if (rb.velocity.y < -0.1)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }
    }

    void LevelPassed()
    {
        Save(17, -1);
        ResetLife();

        SceneManager.LoadScene("LevelPassed");
    }

    void Die()
    {
        SaveIsDead(true);
        
        SaveLife();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Save(float x, float y)
    {
        StreamWriter streamWriterX = new StreamWriter(pathX, false);
        streamWriterX.Write(x.ToString());
        streamWriterX.Close();
        
        StreamWriter streamWriterY = new StreamWriter(pathY, false);
        streamWriterY.Write(y.ToString());
        streamWriterY.Close();

        Die();
    }

    void SaveIsDead(bool dead)
    {
        StreamWriter streamWriterIsDead = new StreamWriter(pathIsDead, false);
        streamWriterIsDead.Write(dead.ToString());
        streamWriterIsDead.Close();
    }

    void SaveLife()
    {
        LoadLife();

        life--;
        
        StreamWriter streamWriter = new StreamWriter(pathLife, false);
        streamWriter.Write(life.ToString());
        streamWriter.Close();
    }
    
    void ResetLife()
    {
        life = 5;
        
        StreamWriter streamWriter = new StreamWriter(pathLife, false);
        streamWriter.Write(life.ToString());
        streamWriter.Close();
    }

    void Load()
    {
        try
        {
            StreamReader streamReaderX = new StreamReader(pathX);
            string x = streamReaderX.ReadLine();
            streamReaderX.Close();
            if(x != null)
                startX = float.Parse(x);

            StreamReader streamReaderY = new StreamReader(pathY);
            string y = streamReaderY.ReadLine();
            streamReaderY.Close();
            if(y != null)
                startY = float.Parse(y);
        }
        catch (Exception e)
        {
            startX = 17;
            startY = -1;
        }
    }
    
    void LoadIsDead()
    {
        try
        {
            StreamReader streamReaderIsDead = new StreamReader(pathIsDead);
            string d = streamReaderIsDead.ReadLine();
            streamReaderIsDead.Close();
            if(d != null)
                dead = bool.Parse(d);
        
            SaveIsDead(false);
        }
        catch (Exception e)
        {
            dead = false;
            SaveIsDead(false);
        }
    }

    void LoadLife()
    {
        try
        {
            StreamReader streamReader = new StreamReader(pathLife);
            string l = streamReader.ReadLine();
            streamReader.Close();
            if (l != null)
                life = int.Parse(l);
        }
        catch (Exception e)
        {
            life = 5;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    void LateUpdate()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            if (other.contacts[0].normal.y <= 0.5)
                Save(enemy.killX, enemy.killY);
        }

        WoodSpike woodSpike = other.gameObject.GetComponent<WoodSpike>();

        if (woodSpike != null)
        {
            if (other.contacts[0].normal.y > 0.5)
                Save(woodSpike.killX,woodSpike.killY);
        }
        
        MetalSpike metalSpike = other.gameObject.GetComponent<MetalSpike>();
        
        if (metalSpike != null)
        {
            if (other.contacts[0].normal.y > 0.5)
                Save(metalSpike.killX,metalSpike.killY);
        }

        Metal metal = other.gameObject.GetComponent<Metal>();

        if (metal != null)
        {
            if (other.contacts[0].normal.y < -0.5)
                Save(metal.killX,metal.killY);
        }
    }
}
