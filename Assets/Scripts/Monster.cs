using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IRestartGame
{
    public static Monster instance;

    [SerializeField] private NavMeshAgent navMeshAgent;

    [SerializeField] public Transform spawnPoint;
    [SerializeField] private Transform ChasePoint;

    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private Transform actualPatrolPoint;
    [SerializeField] private int index = 0;

    [SerializeField] private MonsterStates actualState;

    [SerializeField] private bool isChasing = false;
    [SerializeField] private bool inRange = false;

    [Space]
    [SerializeField] private float minDistance = 1;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float finalSpeed = 5f;

    [SerializeField] private float waitTimer = 0f;

    [Space]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] public AudioSource secondaryAudioSource;
    [SerializeField] public AudioSource breathingAudioSource;

    [Space]
    [SerializeField] private AudioClip step;
    [SerializeField] private AudioClip stepFinal;
    [SerializeField] private AudioClip spawn;
    [SerializeField] private AudioClip spawnFinal;
    [SerializeField] private AudioClip normal;
    [SerializeField] private AudioClip normalFinal;
    [SerializeField] private AudioClip advice;
    [SerializeField] private AudioClip growl;
    [SerializeField] private AudioClip inspire;
    [SerializeField] private AudioClip breath;

    private enum MonsterStates
    {
        Patrol,
        Chase,
        Wait,
        FinalSprint
    }

    public IEnumerator SpawnMonster(bool lastSpawn, Vector3 position, Vector3 fwd)
    {
        navMeshAgent.enabled = false;
        transform.position = position;
        transform.forward = fwd;
        navMeshAgent.enabled = true;
        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(spawn.length);

        navMeshAgent.isStopped = false;

        if (!lastSpawn)
        {
            PlayClip(spawn, false);
            ChangeState(MonsterStates.Patrol);
            navMeshAgent.SetDestination(patrolPoints[index].position);
            secondaryAudioSource.clip = normal;
        }
        else
        {
            ChangeState(MonsterStates.FinalSprint);
            secondaryAudioSource.clip = normalFinal;
            navMeshAgent.speed = 3.5f;
        }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);
    }

    private void Update()
    {
        if (!PlayerManager.instance.Dying)
        {
            ExecuteState();
            AudioUpdate();
        }
    }

    private void ChangeState(MonsterStates nextState)
    {
        switch (nextState)
        {
            case MonsterStates.Patrol:
                PlayClip(step, true);
                PlayerDetected = false;
                break;
            case MonsterStates.Chase:
                PlayClip(step, true);
                break;
            case MonsterStates.Wait:
                break;
            case MonsterStates.FinalSprint:
                PlayClip(stepFinal, true);
                break;
        }

        if (actualState == nextState) return;

        if (actualState == MonsterStates.Wait) waitTimer = 0;
        if (actualState == MonsterStates.FinalSprint)
        {
            transform.position = ChasePoint.position;
            navMeshAgent.speed = finalSpeed;
            PlayClip(spawnFinal, false);
        }

        actualState = nextState;
    }

    private void ExecuteState()
    {
        switch (actualState)
        {
            case MonsterStates.Patrol:
                Patrol();
                break;
            case MonsterStates.Chase:
                Chase();
                break;
            case MonsterStates.Wait:
                Wait();
                break;
            case MonsterStates.FinalSprint:
                FinalSprint();
                break;
        }
    }

    private void Patrol()
    {
        if (inRange) Detect();
        if ((transform.position - navMeshAgent.destination).magnitude > 0.5f) return;

        index = (index + 1 >= patrolPoints.Count) ? 0 : index + 1;

        actualPatrolPoint = patrolPoints[index];

        navMeshAgent.SetDestination(actualPatrolPoint.position);
    }

    private void Chase()
    {
        Detect();

        if (navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
            ChangeState(MonsterStates.Patrol);

        if (navMeshAgent.remainingDistance < 1)
        {
            if ((transform.position - PlayerManager.instance.Movement.transform.position).magnitude < minDistance)
            {
                //KILL PLAYER
                PlayerManager.instance.Die();
                ChangeState(MonsterStates.Patrol);
            }
            else
            {
                ChangeState(MonsterStates.Wait);
            }
        }
    }

    private void Wait()
    {
        Detect();

        waitTimer += Time.deltaTime;

        if (waitTimer > waitTime)
        {
            waitTimer = 0;

            ChangeState(MonsterStates.Patrol);
        }
    }

    private void FinalSprint()
    {
        navMeshAgent.SetDestination(PlayerManager.instance.Movement.transform.position);

        if ((transform.position - PlayerManager.instance.Movement.transform.position).magnitude < minDistance)
        {
            PlayerManager.instance.Die();
        }
    }

    bool PlayerDetected;
    private void Detect()
    {

        NavMeshPath path = new NavMeshPath();

        navMeshAgent.CalculatePath(PlayerManager.instance.Movement.transform.position, path);



        if (PlayerManager.instance.Movement.breathing && path.status == NavMeshPathStatus.PathComplete)
        {
            if (!PlayerDetected)
            {
                PlayClip(advice, false);
                PlayerDetected = true;
            }

            navMeshAgent.SetDestination(PlayerManager.instance.Movement.transform.position);

            if (actualState != MonsterStates.Chase) ChangeState(MonsterStates.Chase);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        inRange = false;
    }

    public void RestartGame()
    {
        index = 0;
        inRange = false;

        transform.position = spawnPoint.position;

        ChangeState(MonsterStates.Patrol);
    }

    List<AudioClip> clips = new List<AudioClip>();
    List<bool> onLoop = new List<bool>();
    void AudioUpdate()
    {
        if (clips.Count > 0)
        {
            if (!audioSource.isPlaying || audioSource.loop)
            {
                audioSource.clip = clips[0];
                audioSource.loop = onLoop[0];
                clips.Remove(clips[0]);
                onLoop.Remove(onLoop[0]);
                audioSource.Play();
            }
        }
    }

    public void PlayClip(AudioClip clip, bool loop)
    {
        clips.Add(clip);
        onLoop.Add(loop);
    }

}
