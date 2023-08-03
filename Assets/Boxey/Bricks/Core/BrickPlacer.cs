using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boxey.Bricks.Core {
    public class BrickPlacer : MonoBehaviour {
        private List<Brick> m_bricks = new List<Brick>(); // List of all bricks placed
        private float m_height;
        private Vector3 m_brickStart;
        private Vector3 m_brickEnd;

        [SerializeField] private bool useGrid;
        [SerializeField] private Camera playerCamera;

        private void Update() {
            var r = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Input.GetKeyDown(KeyCode.Q)) m_height--;
            if (Input.GetKeyDown(KeyCode.E)) m_height++;
            if (Physics.Raycast(r, out var hit)) {
                var position = hit.point;
                if (useGrid) position = new Vector3(Mathf.RoundToInt(hit.point.x),Mathf.RoundToInt(hit.point.y),Mathf.RoundToInt(hit.point.z));
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    m_brickStart = position;
                }
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    var height = position.y + m_height;
                    if (useGrid) height = Mathf.RoundToInt(height)
                    m_brickEnd = new Vector3(position.x, height, position.z);
                }
                if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    var height = position.y + m_height;
                    if (useGrid) height = Mathf.RoundToInt(height)
                    m_brickEnd = new Vector3(position.x, height, position.z);
                    PlaceBrick(m_brickStart, m_brickEnd, height);
                }
            }
        }

        private void PlaceBrick(Vector3 start, Vector3 end, float height) {
            
        }

        private void OnDrawGizmos() {
            if (!Application.isPlaying) return;
            //Get Positions
            var start = m_brickStart;
            var end = m_brickEnd;
            var position0 = new Vector3(start.x, end.y, start.z); // Below
            var position1 = new Vector3(start.x, start.y, end.z); // Corner 1
            var position2 = new Vector3(end.x, start.y, start.z); // Corner 2
            var position3 = new Vector3(end.x, start.y, end.z); // Above
            var position4 = new Vector3(end.x, end.y, start.z); // Corner 3
            var position5 = new Vector3(start.x, end.y, end.z); // Corner 3
            //Draw
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(start, .1f);
            Gizmos.DrawSphere(end, .1f);
            Gizmos.DrawSphere(position0, .1f);
            Gizmos.DrawSphere(position1, .1f);
            Gizmos.DrawSphere(position2, .1f);
            Gizmos.DrawSphere(position3, .1f);
            Gizmos.DrawSphere(position4, .1f);
            Gizmos.DrawSphere(position5, .1f);
            //Draw Lines
            Gizmos.DrawLine(start, position0);
            Gizmos.DrawLine(start, position1);
            Gizmos.DrawLine(start, position2);
            
            Gizmos.DrawLine(position3, position1);
            Gizmos.DrawLine(position3, position2);
            Gizmos.DrawLine(position2, position4);
            Gizmos.DrawLine(position1, position5);
            Gizmos.DrawLine(position0, position4);
            Gizmos.DrawLine(position0, position5);

            Gizmos.DrawLine(end, position3);
            Gizmos.DrawLine(end, position4);
            Gizmos.DrawLine(end, position5);
        }
    }
}
