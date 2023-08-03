using UnityEngine;

namespace Boxey.Bricks.Core {
    public class Brick {
        public readonly GameObject BrickObject;

        public Brick(Vector3 pos, Mesh brickMesh) {
            var position = pos;
            position.y += 0.01f;
            BrickObject = new GameObject {
                name = "Brick: " + pos,
                transform = {
                    position = position
                }
            };
            var brickFilter = BrickObject.AddComponent<MeshFilter>();
            var brickRenderer = BrickObject.AddComponent<MeshRenderer>();
            var brickCollider = BrickObject.AddComponent<MeshCollider>();
            //Mesh Rendering
            brickFilter.sharedMesh = brickMesh;
            brickCollider.sharedMesh = brickMesh;
            //Material
            brickRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            brickRenderer.sharedMaterial.SetFloat("_Smoothness", 0);
            brickRenderer.sharedMaterial.color = Random.ColorHSV();
        }
    }
}