using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    
    private CharacterController controller;
    private Animator animator;
    private Vector2 inputVector;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>(); 
    }

    // Yeni Input System'deki PlayerInput componenti üzerinden "Send Messages" ile çağrılır
    void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    void Update()
    {
        MovePlayer();
        UpdateAnimations();
    }

    void MovePlayer()
    {
        // 3D uzayda (X ve Z ekseni) hareket yönü
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        
        // Eğer hareket ediyorsak karakteri o yöne döndür
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Yerçekimini basitçe ekleyelim (Karakterin havada kalmaması için)
        moveDirection.y = -9.81f; 

        // Karakteri hareket ettir
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    void UpdateAnimations()
    {
        if (animator != null)
        {
            // InputVector'un büyüklüğü (0 ile 1 arası) hızı verir.
            float speed = inputVector.magnitude;
            animator.SetFloat("Speed", speed);
        }
    }
}
