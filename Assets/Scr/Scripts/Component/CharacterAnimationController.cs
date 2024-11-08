using System;
using Spine.Unity;
using UnityEngine;

namespace Scr.Scripts.Component
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;

        private string _moveString = "action/run";
        private string _idleString = "activity/prepare";
        private string _attack = "attack/melee/multi-attack";

        private void Awake()
        {
            skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        }



        public void SetAnimation(CharacterState characterState, bool loop)
        {
            switch (characterState)
            {
                case CharacterState.Idle:
                    skeletonAnimation.state.SetAnimation(0, _idleString, loop);
                    break;
                case CharacterState.Walk:
                    skeletonAnimation.state.SetAnimation(0, _idleString, loop);
                    break;
                case CharacterState.Attack:
                    skeletonAnimation.state.SetAnimation(0, _idleString, loop);
                    break;
            }
        }
    }

    public enum CharacterState
    {
        Idle,
        Walk,
        Attack
    }
}