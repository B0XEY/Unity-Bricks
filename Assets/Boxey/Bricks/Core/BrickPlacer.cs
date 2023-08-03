using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Boxey.Bricks.Core {
    public class BrickPlacer : MonoBehaviour {
        private List<Brick> m_bricks = new List<Brick>(); // List of all bricks placed
        private float m_height;
        private Vector3 m_brickStart;
        private Vector3 m_brickEnd;
        private Mesh m_previewMesh;
        
        [Header("Preview OBJ")]
        [SerializeField] private Transform previewObject;
        [SerializeField] private MeshFilter previewFilter;
        
        [Header("Data")]
        [SerializeField] private bool useGrid;
        [SerializeField] private float gridSize = 1f;
        [SerializeField] private Camera playerCamera;

        private void Awake() {
            m_previewMesh = new Mesh {
                name = "Preview Mesh"
            };
        }

        private void Update() {
            var r = playerCamera.ScreenPointToRay(Input.mousePosition);
            m_height += Input.GetAxisRaw("Mouse ScrollWheel");
            if (Physics.Raycast(r, out var hit)) {
                var position = hit.point;
                if (useGrid) position = SnapPositionToGrid(position);
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    m_brickStart = position;
                }
                if (Input.GetKey(KeyCode.Mouse0)) {
                    var height = position.y + m_height;
                    if (useGrid) height = SnapFloatToGrid(height);
                    m_brickEnd = new Vector3(position.x, height, position.z);
                    previewObject.position = m_brickStart;
                    CreatePreviewMesh(position);
                }
                if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    var height = position.y + m_height;
                    if (useGrid) height = SnapFloatToGrid(height);
                    m_brickEnd = new Vector3(position.x, height, position.z);
                    if (position == m_brickStart) return;
                    PlaceBrick(m_brickStart, m_brickEnd);
                }
            }
        }

        private void PlaceBrick(Vector3 start, Vector3 end) {
            m_bricks.Add(new Brick(start, end));
            m_previewMesh.Clear();
        }
        private Vector3 SnapPositionToGrid(Vector3 position) {
            var snappedX = Mathf.Round(position.x / gridSize) * gridSize;
            var snappedY = Mathf.Round(position.y / gridSize) * gridSize;
            var snappedZ = Mathf.Round(position.z / gridSize) * gridSize;
            return new Vector3(snappedX, snappedY, snappedZ);
        }
        private float SnapFloatToGrid(float value) {
            return Mathf.Round(value / gridSize) * gridSize;
        }
        private void CreatePreviewMesh(Vector3 brickEnd) {
            var start = Vector3.zero;
            var vertices = new Vector3[24] {
                // Front face
                new (start.x, start.y, start.z),
                new (brickEnd.x, start.y, start.z),
                new (start.x, brickEnd.y, start.z),
                new (brickEnd.x, brickEnd.y, start.z),
                // Back face
                new (start.x, start.y, brickEnd.z),
                new (brickEnd.x, start.y, brickEnd.z),
                new (start.x, brickEnd.y, brickEnd.z),
                new (brickEnd.x, brickEnd.y, brickEnd.z),
                // Left face
                new (start.x, start.y, start.z),
                new (start.x, brickEnd.y, start.z),
                new (start.x, start.y, brickEnd.z),
                new (start.x, brickEnd.y, brickEnd.z),
                // Right face
                new (brickEnd.x, start.y, start.z),
                new (brickEnd.x, brickEnd.y, start.z),
                new (brickEnd.x, start.y, brickEnd.z),
                new (brickEnd.x, brickEnd.y, brickEnd.z),
                // Top face
                new (start.x, brickEnd.y, start.z),
                new (brickEnd.x, brickEnd.y, start.z),
                new (start.x, brickEnd.y, brickEnd.z),
                new (brickEnd.x, brickEnd.y, brickEnd.z),
                // Bottom face
                new (start.x, start.y, start.z),
                new (brickEnd.x, start.y, start.z),
                new (start.x, start.y, brickEnd.z),
                new (brickEnd.x, start.y, brickEnd.z)
            };
            var triangles = new int[36] {
                // Front
                0, 2, 1,
                1, 2, 3,
                // Back
                5, 7, 4,
                4, 7, 6,
                // Left
                8, 10, 9,
                9, 10, 11,
                // Right
                13, 15, 14,
                13, 14, 12,
                // Top
                16, 18, 17,
                17, 18, 19,
                // Bottom
                20, 21, 22,
                22, 21, 23
            };
            var normals = new Vector3[24] {
                // Front face
                Vector3.forward,
                Vector3.forward,
                Vector3.forward,
                Vector3.forward,
                // Back face
                Vector3.back,
                Vector3.back,
                Vector3.back,
                Vector3.back,
                // Left face
                Vector3.left,
                Vector3.left,
                Vector3.left,
                Vector3.left,
                // Right face
                Vector3.right,
                Vector3.right,
                Vector3.right,
                Vector3.right,
                // Top face
                Vector3.up,
                Vector3.up,
                Vector3.up,
                Vector3.up,
                // Bottom face
                Vector3.down,
                Vector3.down,
                Vector3.down,
                Vector3.down
            };
            m_previewMesh.Clear();
            m_previewMesh.vertices = vertices;
            m_previewMesh.triangles = triangles;
            m_previewMesh.normals = normals;
            previewFilter.sharedMesh = m_previewMesh;
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
