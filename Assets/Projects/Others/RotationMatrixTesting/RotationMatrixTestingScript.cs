namespace Testing
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class RotationMatrixTestingScript : MonoBehaviour
    {
        [SerializeField]
        private Material _mat;
        [SerializeField]
        private Texture2D _texture;
        [SerializeField]
        private Texture2D _transformedTexture;
        [SerializeField]
        private MeshRenderer _otherPlaneMeshRenderer;
        private Material _otherPlaneMat;

        private void Awake()
        {
            _otherPlaneMat = new Material(GetComponent<MeshRenderer>().material);
            _otherPlaneMat.mainTexture = _transformedTexture;
            _otherPlaneMeshRenderer.material = _otherPlaneMat;

            _mat = GetComponent<MeshRenderer>().material;
            _texture = (Texture2D)_mat.mainTexture;
            _transformedTexture = new Texture2D(_texture.width, _texture.height);

            for (int i = 0; i < _texture.height; i++)
            {
                for (int j = 0; j < _texture.width; j++)
                {
                    Color c = _texture.GetPixel(j, i);
                    _transformedTexture.SetPixel(j, i, c);
                }
            }
            _transformedTexture.Apply();


        }

        private void Update()
        {
            Vector2 up = transform.forward;
            Vector2 right = transform.right;
            float degrees = transform.eulerAngles.x;

            float rads = (Mathf.Sin(Time.time) + 1) / 2 * 360 * Mathf.Deg2Rad;
            Vector2 row0 = new Vector2(Mathf.Cos(rads), -Mathf.Sin(rads));
            Vector2 row1 = new Vector2(Mathf.Sin(rads), Mathf.Cos(rads));
            Vector2 pivot = new Vector2(_transformedTexture.width / 2, _transformedTexture.height / 2);

            for (int i = 0; i < _transformedTexture.height; i++)
            {
                for (int j = 0; j < _transformedTexture.width; j++)
                {
                    Vector2 offset = new Vector2(j - pivot.x, i - pivot.y);
                    int newJ = Mathf.RoundToInt(offset.x * row0.x + offset.y * row0.y) + Mathf.RoundToInt(pivot.x);
                    int newI = Mathf.RoundToInt(offset.x * row1.x + offset.y * row1.y) + Mathf.RoundToInt(pivot.y);
                    newJ = Mathf.Clamp(newJ, 0, _transformedTexture.width - 1);
                    newI = Mathf.Clamp(newI, 0, _transformedTexture.height - 1);
                    _transformedTexture.SetPixel(newJ, newI, _texture.GetPixel(j, i));

                }
            }
            _transformedTexture.Apply();
            _otherPlaneMeshRenderer.material.mainTexture = _transformedTexture;
        }
    }
}

