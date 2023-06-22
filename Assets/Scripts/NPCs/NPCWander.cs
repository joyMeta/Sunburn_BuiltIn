using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWander : MonoBehaviour
{
    public float wanderRadius;
    public float wanderTimer;
    private NavMeshAgent agent;
    private float timer;
    Animator animator;
    Vector3 targetPos;
    float thresholdDistance = 0.5f;
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    AudioSource footStepAudioSource;

    void OnEnable() {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        animator= GetComponent<Animator>();
        footStepAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        if (timer >= wanderTimer) {
            targetPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(targetPos);
            animator.SetBool("isWalking", true);
            timer = 0;
        }
        if(Vector3.Distance(transform.position, targetPos) < thresholdDistance) {
            animator.SetBool("isWalking", false);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    public void PlayFootstep() {
        footStepAudioSource.PlayOneShot(FootstepAudioClips[Random.Range(0, FootstepAudioClips.Length - 1)], 1);
    }

    public void PlayLandingSound() {
        footStepAudioSource.PlayOneShot(LandingAudioClip, 1);
    }
}
