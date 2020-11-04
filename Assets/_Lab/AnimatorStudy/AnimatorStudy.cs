using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStudy : MonoBehaviour
{
    [SerializeField]
    private float _fixedTime = 5;

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            var animator = GetComponent<Animator>();
            var stateNameHash = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;

            GetComponent<Animator>().PlayInFixedTime(stateNameHash, 0, _fixedTime);
        }
    }
}
