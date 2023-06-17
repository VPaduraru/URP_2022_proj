namespace SnowProject
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SnowController : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerTransform;
        private PlayerController _playerController;
        [SerializeField]
        private Texture2D _snowPathTexture;
        [SerializeField]
        private Texture2D _snowMeltingMask;
        [SerializeField]
        private Texture2D _toolMeltingMask;
        [SerializeField]
        private MeshRenderer _snowPlaneMeshRenderer;
        [field: SerializeField]
        [Range(0.0f, 5.0f)]
        private float _playerSnowMeltingRate = 1.5f;
        private int _textureRes = 1024;

        public static SnowController Instance { get; private set; }


        private float _planeScaleX;
        private float _playerPosX01;
        private float _playerPosZ01;
        private int _texturePosX;
        private int _texturePosY;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            _playerController = _playerTransform.gameObject.GetComponent<PlayerController>();
            _snowPathTexture = new Texture2D(_textureRes, _textureRes, TextureFormat.RGBA32, false);
            for (int i = 0; i < _textureRes; i++)
            {
                for (int j = 0; j < _textureRes; j++)
                {
                    _snowPathTexture.SetPixel(j, i, Color.white);
                }
            }
            _snowPathTexture.Apply();

            _planeScaleX = _snowPlaneMeshRenderer.gameObject.transform.localScale.x;
        }

        private void Update()
        {
            CalculatePlayerPositionOnPlane();
            CalculatePlayerPositionOnTexture();

        }

        private void CalculatePlayerPositionOnPlane()
        {
            _playerPosX01 = Mathf.InverseLerp(_planeScaleX, -_planeScaleX, _playerTransform.position.x);
            _playerPosZ01 = Mathf.InverseLerp(_planeScaleX, -_planeScaleX, _playerTransform.position.z);
        }

        private void CalculatePlayerPositionOnTexture()
        {
            _texturePosX = Mathf.RoundToInt(Mathf.Lerp(0, _textureRes, _playerPosX01));
            _texturePosY = Mathf.RoundToInt(Mathf.Lerp(0, _textureRes, _playerPosZ01));
        }
        public float GetCurrentPlaneHeight()
        {
            return _snowPathTexture.GetPixel(_texturePosX, _texturePosY).r;
        }
        public float PaintOnTextureCustomMaskFromPlayerRotation(float rotationDegrees)
        {
            float totalBrightness = 0;
            float rads = -rotationDegrees * Mathf.Deg2Rad - Mathf.PI;
            rads += Random.Range(-.1f, .1f);
            int maskHalfWidth = _toolMeltingMask.width / 2;

            int xOffset = (int)(_texturePosX + _playerController.transform.forward.x * -20);
            int yOffset = (int)(_texturePosY + _playerController.transform.forward.z * -20);



            Vector2 row0 = new Vector2(Mathf.Cos(rads), -Mathf.Sin(rads));
            Vector2 row1 = new Vector2(Mathf.Sin(rads), Mathf.Cos(rads));

            for (int y = 0; y < _toolMeltingMask.height; y++)
            {
                for (int x = 0; x < _toolMeltingMask.width; x++)
                {
                    float r = _toolMeltingMask.GetPixel(x, y).r;

                    int newX = x - maskHalfWidth;
                    int newY = y;

                    // Apply rotation transformation
                    int rotatedX = Mathf.RoundToInt(newX * row0.x + newY * row0.y);
                    int rotatedY = Mathf.RoundToInt(newX * row1.x + newY * row1.y);

                    // Offset the position relative to the player
                    int finalX = rotatedX + xOffset;
                    int finalY = rotatedY + yOffset;

                    float val = _snowPathTexture.GetPixel(finalX, finalY).r;
                    float temp = Mathf.Clamp01(val);
                    temp = Mathf.RoundToInt(temp * 100000) / 100000f;
                    val -= r * Time.deltaTime * _playerSnowMeltingRate / 2;
                    val = Mathf.Clamp01(val);
                    totalBrightness += temp - val;
                    Color c = new Color(val, val, val);

                    _snowPathTexture.SetPixel(finalX, finalY, c);

                }

            }



            _snowPathTexture.Apply();
            _snowPlaneMeshRenderer.material.SetTexture("_InteractiveSnowTexture", _snowPathTexture);
            _snowPlaneMeshRenderer.material.SetVector("_PlayerPosition", new Vector4(_playerTransform.position.x, _playerTransform.position.y, _playerTransform.position.z));
            return totalBrightness;
        }


        public float PaintOnTexture()
        {
            float totalBrightness = 0;
            int halfMaskWidthHeight = _snowMeltingMask.width / 2;
            for (int i = 0; i < _snowMeltingMask.height; i++)
            {
                for (int j = 0; j < _snowMeltingMask.width; j++)
                {
                    int xPos = _texturePosX + j - halfMaskWidthHeight;
                    int yPos = _texturePosY + i - halfMaskWidthHeight;
                    float brightVal = _snowPathTexture.GetPixel(xPos, yPos).r;
                    float temp = brightVal;
                    //Debug.Log("Before: " + temp);
                    brightVal -= _snowMeltingMask.GetPixel(j, i).r * _playerSnowMeltingRate * Time.deltaTime;
                    brightVal = Mathf.Clamp01(brightVal);
                    //Debug.Log("After: " + temp);
                    float delta = temp - brightVal;
                    totalBrightness += delta;
                    Color colorToApply = new Color(brightVal, brightVal, brightVal);
                    _snowPathTexture.SetPixel(xPos, yPos, colorToApply);
                }
            }
            _snowPathTexture.Apply();
            _snowPlaneMeshRenderer.material.SetTexture("_InteractiveSnowTexture", _snowPathTexture);
            _snowPlaneMeshRenderer.material.SetVector("_PlayerPosition", new Vector4(_playerTransform.position.x, _playerTransform.position.y, _playerTransform.position.z));
            return totalBrightness;
        }
    }
}

