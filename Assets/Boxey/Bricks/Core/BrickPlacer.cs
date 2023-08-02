using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boxey.Bricks.Core {
    public class BrickPlacer : MonoBehaviour {
        private List<Brick> m_bricks; // List of all bricks placed
        private float m_height;
        private Vector3 m_brickStart;
        private Vector3 m_brickEnd;

        [SerializeField] private Camera playerCamera;

        private void Update() {
            var r = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Input.GetKeyDown(KeyCode.Q)) m_height--;
            if (Input.GetKeyDown(KeyCode.E)) m_height++;
            if (Physics.Raycast(r, out var hit)) {
                var position = new Vector3(Mathf.RoundToInt(hit.point.x),Mathf.RoundToInt(hit.point.y),Mathf.RoundToInt(hit.point.z));
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    m_brickStart = position;
                }
                if (Input.GetKey(KeyCode.Mouse0)) {
                    m_brickEnd = new Vector3(position.x, position.y + m_height, position.z);
                }
                if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    m_brickEnd = new Vector3(position.x, position.y + m_height, position.z);
                    //PlaceBrick(m_brickStart, m_brickEnd, height);
                }
            }
        }

        private void PlaceBrick(Vector3 start, Vector3 end, float height) {
            //get start point
            //get end point
            //draw debug mesh
            //create the brick from the 2 points
            //add Bricks to all brick
        }

        private void OnDrawGizmos() {
            if (!Application.isPlaying) return;
            //Get Positions
            var below = new Vector3(m_brickStart.x, m_brickEnd.y, m_brickStart.z); // Below
            var position1 = new Vector3(m_brickStart.x, m_brickStart.y, m_brickEnd.z); // Corner 1
            var position2 = new Vector3(m_brickEnd.x, m_brickStart.y, m_brickStart.z); // Corner 2
            var above = new Vector3(m_brickEnd.x, m_brickStart.y, m_brickEnd.z); // Above
            var position4 = new Vector3(m_brickEnd.x, m_brickEnd.y, m_brickStart.z); // Corner 3
            var position5 = new Vector3(m_brickStart.x, m_brickEnd.y, m_brickEnd.z); // Corner 3
            //Draw
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(m_brickStart, .1f);
            Gizmos.DrawSphere(m_brickEnd, .1f);
            Gizmos.DrawSphere(below, .1f);
            Gizmos.DrawSphere(position1, .1f);
            Gizmos.DrawSphere(position2, .1f);
            Gizmos.DrawSphere(above, .1f);
            Gizmos.DrawSphere(position4, .1f);
            Gizmos.DrawSphere(position5, .1f);
            //Draw Lines
            Gizmos.DrawLine(m_brickStart, below);
            Gizmos.DrawLine(m_brickStart, position1);
            Gizmos.DrawLine(m_brickStart, position2);
            
            Gizmos.DrawLine(above, position1);
            Gizmos.DrawLine(above, position2);
            Gizmos.DrawLine(position2, position4);
            Gizmos.DrawLine(position1, position5);
            Gizmos.DrawLine(below, position4);
            Gizmos.DrawLine(below, position5);

            Gizmos.DrawLine(m_brickEnd, above);
            Gizmos.DrawLine(m_brickEnd, position4);
            Gizmos.DrawLine(m_brickEnd, position5);
        }
    }
}
