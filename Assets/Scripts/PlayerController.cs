using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;
    public Text timerText;
    


    private Rigidbody rb;
    private int count;
    private GameObject[] pickUps;
    private float targetTime = 30.0f;


    void Start()
    {
        Time.timeScale = 1.0f;
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
        pickUps = GameObject.FindGameObjectsWithTag("Pick Up");
        PickUpsUpdate();
    }

    void FixedUpdate()
    {   
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
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
        countText.text = "Score: " + count.ToString() + "/10";
        if (count >= 10)
        {
            winText.text = "You win";
            //Application.LoadLevel(Application.loadedLevel);
        }
    }

    void PickUpsUpdate()
    {
        foreach (var item in pickUps)
        {
            item.SetActive(false);
        }
        pickUps[count].SetActive(true);
    }

    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        targetTime -= Time.deltaTime;
        SetTimerText();
        if (targetTime <= 20.0f)
        {
            timerText.color = Color.yellow;
            if (targetTime <= 10.0f)
            {
                timerText.color = Color.red;
            }
            if (targetTime <= 0.0f)
            {
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
        winText.text = "You lost!";
        SetCountText();
        Time.timeScale = 0;
        targetTime = 0.0f;
        SetTimerText();
    }
}
