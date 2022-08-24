using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class RatonComun_Controler : MonoBehaviour
{
    public float horizontalMove;

    public float verticalMove;


#region Hunger System
    public float maximum;

    public float current;

     public Image hunger_bar;
#endregion



#region  PlayerPropieties
    public CharacterController player;

    public float player_speed;

    private Vector3 movePlayer;

    public float gravity = 9.8f;

    public float fallVelocity;

    public float SpeedNormal;

    public float SpeedSprinting;

    public float jumpForce;


#endregion


    private Vector3 playerInput;


#region Camera
    public Camera mainCamera;

    private Vector3 camForward;

    private Vector3 camRight;
#endregion


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedSprinting = 15;
        SpeedNormal = 8;
        maximum = 100f;
        jumpForce = 3.5f;

        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);

        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        movePlayer = movePlayer * player_speed;

        player.transform.LookAt(player.transform.position + movePlayer);

        SetGravity();
        
        HungerSystem();
        
        PlayerSkills();

        GetCurrentFill();

        
        player.Move(movePlayer * Time.deltaTime);

        Debug.Log(player.velocity.magnitude);
    }

    //Funcion para Habilidades del Raton Comun
    public void PlayerSkills()
    {
        //Habilidad de Saltado
        if (player.isGrounded && Input.GetButton("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
        }

        //Habilidad de Correr
        if (Input.GetKey("left ctrl") || Input.GetKey("joystick button 10"))
        {
            player_speed = SpeedSprinting;
        }
        else
        {
            player_speed = SpeedNormal;
        }
    }

    //Funcion determinada a mover el cuerpo del jugador segun la direccion que se le indique
    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    //Funcion que agrega la gravedad comprobando si es que la entidad toca el suelo
    void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
    }

    //Inicializar en el maximo la barra de comida
    void GetCurrentFill()
    {
        float fillAmount = (float) current / (float) maximum;

        hunger_bar.fillAmount = fillAmount;
    }

    void HungerSystem()
    {
        //Obteniendo la informacion cuando el Raton este Caminando o Corriendo
        if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            if (current > 0)
            {
                current = current - 0.001f; //Decrementando la barra de estamina
            }
            else
            {
                current = 0;
            }
        }
        //Condicional de correr
        if (((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) &&(Input.GetKey("left ctrl") || Input.GetKey("joystick button 10"))))
        {
            if (current > 0)
            {
                current = current - 0.002f; //Decrementando la barra de estamina
            }
            else
            {
                current = 0;
            }
        }
        //Eventos de delimitacion de velocidad a cierto nivel de decreso en la barra de comida
        if(current < 50)
        {
            SpeedNormal = 6f;
            SpeedSprinting = 11.25f;
        }
        if(current < 25)
        {
            SpeedNormal = 4f;
            SpeedSprinting = 7.5f;
        }
    }
}
