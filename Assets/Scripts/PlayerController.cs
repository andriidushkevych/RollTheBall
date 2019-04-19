using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using QuantumTek.MenuSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float targetTime;
    public int pickUpsToWin;
    public Text countText;
    public Text winText;
    public Text timerText;
    


    private Rigidbody rb;
    private int count;
    private GameObject[] pickUps;
    private GameObject[] pauseObjects;
    private new Renderer renderer;
    private float halfTime, thirdTime;


    void Start()
    {
        Time.timeScale = 1.0f;
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        count = 0;
        halfTime = targetTime / 2;
        thirdTime = targetTime / 3;
        SetCountText();
        winText.text = "";
        pickUps = GameObject.FindGameObjectsWithTag("Pick Up");
        pauseObjects = GameObject.FindGameObjectsWithTag("Pause");
        PickUpsUpdate();
        PauseGameShow(false);        
    }

    void PauseGameShow(bool pause)
    {
        foreach (var po in pauseObjects)
        {
            po.SetActive(pause);
        }
    }

    void FixedUpdate()
    {   
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
        if (rb.position.z < -11 || rb.position.z > 11 || rb.position.x < -11 || rb.position.x > 11 )
        {
            timerEnded();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            PickUpsUpdate();
            count++;
            SetCountText();            
        }
    }

    void SetCountText()
    {
        countText.text = "Score: " + count.ToString() + "/" + pickUpsToWin.ToString();
        if (count >= pickUpsToWin)
        {
            winText.color = Color.green;
            winText.text = "You win";
            Time.timeScale = 0;
            PauseGameShow(true);
        }
    }

    void PickUpsUpdate()
    {
        foreach (var item in pickUps)
        {
            item.SetActive(false);
        }
        if (count < pickUpsToWin - 1)
        {
            Color randomColor = new Color(
                  Random.Range(0f, 1f),
                  Random.Range(0f, 1f),
                  Random.Range(0f, 1f)
              );
            renderer.material.color = randomColor;
            pickUps[count].SetActive(true);
        }        
    }

    

    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                PauseGameShow(true);
            }
            else
            {
                PauseGameShow(false);
                Time.timeScale = 1;
            }
        }

        targetTime -= Time.deltaTime;
        SetTimerText();
        if (targetTime <= halfTime)
        {
            timerText.color = Color.yellow;
            if (targetTime <= thirdTime)
            {
                timerText.color = Color.red;
            }
            if (targetTime <= 0.0f)
            {
                SetCountText();
                timerEnded();
            }
        }        
    }

    void SetTimerText()
    {
        timerText.text = "Remaining time: " + targetTime.ToString("0.0");
    }

    void timerEnded()
    {
        winText.color = Color.red;
        winText.text = "You lost!";
        Time.timeScale = 0;
        targetTime = 0.0f;
        SetTimerText();
        PauseGameShow(true);
    }
}
