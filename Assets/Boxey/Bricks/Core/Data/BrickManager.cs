using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Boxey.Bricks.Core {
    public class BrickManager : MonoBehaviour {
        private Dictionary<GameObject, Brick> m_bricks = new Dictionary<GameObject, Brick>(); // List of all bricks placed
        private float m_height;
        private Vector3 m_brickStart;
        private Vector3 m_brickEnd;
        private Mesh m_previewMesh;
        
        [Header("Preview OBJ")]
        [SerializeField] private Transform previewObj;
        [SerializeField] private MeshFilter previewFilter;
        
        [Header("Data")]
        [SerializeField] private bool useGrid;
        [SerializeField] private float gridSize = 1f;
        [SerializeField] private float scrollStrength = 1.5f;
        [SerializeField] private Camera playerCamera;
        [Space] 
        [SerializeField] private int numToPlace = 1;
        [SerializeField] private int length;

        private void Awake() {
            m_previewMesh = new Mesh {
                name = "Preview Mesh"
            };
        }

        private void Update() {
            length = m_bricks.Count;
            var r = playerCamera.ScreenPointToRay(Input.mousePosition);
            m_height += Input.GetAxisRaw("Mouse ScrollWheel") * scrollStrength;
            if (Physics.Raycast(r, out var hit)) {
                var position = hit.point;
                if (useGrid) position = SnapPositionToGrid(position);
                previewObj.position = position;
                CreatePreviewMesh(new float3(gridSize));
                if (Input.GetKeyDown(KeyCode.Mouse1)) {
                    DeleteBrick(hit);
                }
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    m_brickStart = position;
                    m_height = gridSize;
                }
                if (Input.GetKey(KeyCode.Mouse0)) {
                    previewObj.position = m_brickStart;
                    var height = position.y + m_height;
                    if (useGrid) height = SnapFloatToGrid(height);
                    m_brickEnd = new Vector3(position.x, height, position.z);
                    CreatePreviewMesh(m_brickEnd - m_brickStart);
                }
                if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    var height = position.y + m_height;
                    if (useGrid) height = SnapFloatToGrid(height);
                    m_brickEnd = new Vector3(position.x, height, position.z);
                    if (position == m_brickStart) {
                        m_brickEnd = new Vector3(position.x + gridSize, position.y + gridSize, position.z + gridSize);
                    }
                    PlaceBrick();
                }
            }
        }

        private void PlaceBrick() {
            for (int x = 0; x < numToPlace; x++) {
                for (int z = 0; z < numToPlace; z++) {
                    var offset = new Vector3(x * gridSize, 0, z * gridSize);
                    var brick = new Brick(m_brickStart + offset, m_brickEnd + offset);
                    m_bricks.Add(brick.BrickObject, brick);
                }
            }
            m_previewMesh.Clear();
        }
        private void DeleteBrick(RaycastHit hitInfo) {
            if (!hitInfo.transform.name.Contains("Brick")) return;
            Destroy(hitInfo.transform.gameObject);
            m_bricks.Remove(hitInfo.transform.gameObject);
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
            var end = brickEnd;
            var vertices = new Vector3[24] {
                // Front face
                new Vector3(start.x, start.y, start.z),
                new Vector3(end.x, start.y, start.z),
                new Vector3(start.x, end.y, start.z),
                new Vector3(end.x, end.y, start.z),
                // Back face
                new Vector3(start.x, start.y, end.z),
                new Vector3(end.x, start.y, end.z),
                new Vector3(start.x, end.y, end.z),
                new Vector3(end.x, end.y, end.z),
                // Left face
                new Vector3(start.x, start.y, start.z),
                new Vector3(start.x, end.y, start.z),
                new Vector3(start.x, start.y, end.z),
                new Vector3(start.x, end.y, end.z),
                // Right face
                new Vector3(end.x, start.y, start.z),
                new Vector3(end.x, end.y, start.z),
                new Vector3(end.x, start.y, end.z),
                new Vector3(end.x, end.y, end.z),
                // Top face
                new Vector3(start.x, end.y, start.z),
                new Vector3(end.x, end.y, start.z),
                new Vector3(start.x, end.y, end.z),
                new Vector3(end.x, end.y, end.z),
                // Bottom face
                new Vector3(start.x, start.y, start.z),
                new Vector3(end.x, start.y, start.z),
                new Vector3(start.x, start.y, end.z),
                new Vector3(end.x, start.y, end.z)
            };
            var triangles = BrickTables.CubicTriangles;
            var normals = BrickTables.CubicNormals;
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
