using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings (Devriye)")]
    public Transform[] waypoints; 
    public float waitTime = 3f; // Noktaya varınca bekleme süresi
    private int currentWaypointIndex = 0;
    private bool isWaiting = false; // Şu an bekliyor mu?
    private float waitTimer = 0f;
    private NavMeshAgent agent;

    [Header("Field of View (Görüş Açısı)")]
    public float viewRadius = 8f; 
    [Range(0, 360)]
    public float viewAngle = 90f; 
    public LayerMask obstacleMask; 

    [Header("Detection (Yakalanma)")]
    public float timeToCatch = 3f; 
    private float detectionTimer = 0f;
    public Image exclamationFillImage; // Kafasında dolacak olan ünlem UI'ı (World Space)
    public Image offScreenFillImage; // Ekran kenarında dolacak olan ünlem UI'ı (Screen Space)

    private Transform player;
    private bool isPlayerCaught = false;
    private Animator animator; // YENI: Animasyon yöneticisi

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Animator'u al
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[0].position);
        }
    }

    private void Update()
    {
        if (isPlayerCaught || player == null) return;

        Patrol();
        CheckFieldOfView();
        UpdateUI();
        
        // Animasyonu NavMesh'in hızına göre otomatik oynat
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;
        if (!agent.isOnNavMesh) return;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f; 
                
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
        else
        {
            if (agent.remainingDistance < 0.5f && !agent.pathPending)
            {
                isWaiting = true;
            }
        }
    }

    private void CheckFieldOfView()
    {
        bool canSeePlayer = false;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= viewRadius)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2f)
            {
                Vector3 rayOrigin = transform.position + Vector3.up * 1f; 
                Vector3 rayDir = (player.position + Vector3.up * 1f) - rayOrigin;
                
                if (!Physics.Raycast(rayOrigin, rayDir, distanceToPlayer, obstacleMask))
                {
                    canSeePlayer = true;
                }
            }
        }

        if (canSeePlayer)
        {
            detectionTimer += Time.deltaTime;
            if (detectionTimer >= timeToCatch)
            {
                CatchPlayer();
            }
        }
        else
        {
            if (detectionTimer > 0)
            {
                detectionTimer -= Time.deltaTime;
            }
        }
    }

    private void CatchPlayer()
    {
        isPlayerCaught = true;
        agent.isStopped = true; 
        
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.LevelFailed();
        }
    }

    private void UpdateUI()
    {
        float fill = Mathf.Clamp01(detectionTimer / timeToCatch);
        
        if (exclamationFillImage != null)
        {
            exclamationFillImage.fillAmount = fill;
        }

        if (offScreenFillImage != null)
        {
            offScreenFillImage.fillAmount = fill;
        }
    }
}
