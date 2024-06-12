using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float jumpForce = 5f; // Jump force
    public float metroFreaky = 75;
    private bool isGrounded;

    private Rigidbody rb;

    void Start()
    {
        OSCHandler.Instance.Init();
        Application.runInBackground = true;
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/stop_DSP", 1);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/oscplayseq", 1);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/seqmetro", metroFreaky);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Up/Down

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/footstep", 1);
        }  
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/footstep", 0);
        } 
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/jump", 1);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        } 
        if (collision.gameObject.CompareTag("Box"))
        {
            metroFreaky += 75;
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/seqmetro", metroFreaky);
        }

        if (collision.gameObject.CompareTag("Box2"))
        {
            metroFreaky -= 75;
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/seqmetro", metroFreaky);
        }
        if (collision.gameObject.CompareTag("Cube"))
        {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/thunder", 1);
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/trigger", 1);
        }
      
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Cube"))
        {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/thunder", 0);
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/trigger", 0);
        }
    }

    [InitializeOnLoad]
    public static class PlayModeStateListener
    {
        static PlayModeStateListener()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/stop_DSP", 0);
                Debug.Log("Exiting play mode");
            }
        }
    }
}
