using System.Collections.Generic;
using UnityEngine;

namespace SequentialMovementSystem
{
    public class SequentialMoveData : ScriptableObject
    {
        [SerializeField]List<Vector3> movePositions;


        public SequentialMoveData()
        {
            movePositions = new List<Vector3>();
        }
        public SequentialMoveData(List<Vector3> movePositions=null)
        {
            if (movePositions != null)
                this.movePositions = movePositions;
            else
                this.movePositions = new List<Vector3>();
        }

        public SequentialMoveData(Vector3[] movePositions = null)
        {
            this.movePositions = new List<Vector3>();
            if (movePositions != null)
                this.movePositions.AddRange(movePositions);
        }

        public void ClearData()
        {
            movePositions = new List<Vector3>();
        }

        public void AddPosition(Vector3 position)
        {
            movePositions.Add(position);
        }

        public void AddPositions(List<Vector3> positions)
        {
            movePositions.AddRange(positions);
        }

        public List<Vector3> GetPositions()
        {
            return movePositions;
        }

    }

}