using System;
using System.Collections;
using UnityEngine;

namespace SequentialMovementSystem
{

    public class SequentialMoveController : MonoBehaviour
    {
        Transform moveObject;
        int currentIndex;
        SequentialMoveData sequentialMoveData;
        float tick = .1f;
        Coroutine moveCoroutine;
        Coroutine moveNextCoroutine;
        public Action OnMoveEnded;

        public void SetMoveObject(Transform moveObject)
        {
            this.moveObject = moveObject;
        }

        public void SetMoveData(SequentialMoveData sequentialMoveData)
        {
            this.sequentialMoveData = sequentialMoveData;
        }

        public void StartMovingObject()
        {
            if(moveObject==null)
                throw new ArgumentNullException("No object to move. Set object to move.");
            currentIndex = 0;
            if(sequentialMoveData.GetPositions().Count==0)
                throw new ArgumentNullException("No positions set to move.");
            moveObject.position = sequentialMoveData.GetPositions()[0];
            StartCoroutine(IEStartMoveToTarget());
        }

        public void StopMovingObject()
        {
            if (moveObject == null)
                throw new ArgumentNullException("No object to stop moving.");
            if(moveCoroutine!=null)
                StopCoroutine(moveCoroutine);
            if (moveNextCoroutine != null)
                StopCoroutine(moveNextCoroutine);
        }

        public void ResumeMovingObject()
        {
            if (moveObject == null)
                throw new ArgumentNullException("No object to move. Set object to move.");
            moveCoroutine = StartCoroutine(IEStartMoveToTarget());
        }

        IEnumerator IEStartMoveToTarget()
        {
            for (int i = currentIndex; i < sequentialMoveData.GetPositions().Count-1; i++)
            {
                moveNextCoroutine = StartCoroutine(MoveToNextTarget());
                yield return moveNextCoroutine;
            }
            OnMoveEnded?.Invoke();

        }

        IEnumerator MoveToNextTarget()
        {
            var currentPosition = moveObject.position;
            var currentRotation = moveObject.rotation;
            var targetPosition = sequentialMoveData.GetPositions()[currentIndex+1];
            var moveDirection = (targetPosition - currentPosition).normalized;
            var targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            var distance = Vector3.Distance(currentPosition, targetPosition);
            var step = tick/distance;
            float time = 0;
            while (time<1)
            {
                time += step;
                moveObject.position = Vector3.Lerp(currentPosition, targetPosition, time);
                moveObject.rotation = Quaternion.Lerp(currentRotation, targetRotation,time);
                yield return null;
            }
            currentIndex++;
        }

    }

}