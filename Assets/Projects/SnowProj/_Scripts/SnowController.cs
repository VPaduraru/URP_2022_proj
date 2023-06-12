using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowController : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private Texture2D _snowPathTexture;
    [SerializeField]
    private Texture2D _snowMeltingMask;
    [SerializeField]
    private MeshRenderer _snowPlaneMeshRenderer;
    private int _textureRes = 1024;

    private void Awake()
    {
        _snowPathTexture = new Texture2D(_textureRes, _textureRes, TextureFormat.RGBA32, false);
        for (int i = 0; i < _textureRes; i++)
        {
            for (int j = 0; j < _textureRes; j++)
            {
                _snowPathTexture.SetPixel(j, i, Color.white);
            }
        }
        _snowPathTexture.Apply();


    }

    private void Update()
    {
        PaintOnTexture();
    }

    private void PaintOnTexture()
    {
        float planeScale = _snowPlaneMeshRenderer.gameObject.transform.localScale.x;
        float playerPosX01 = Mathf.InverseLerp(planeScale, -planeScale, _playerTransform.position.x);
        float playerPosZ01 = Mathf.InverseLerp(planeScale, -planeScale, _playerTransform.position.z);
        int texturePosX = Mathf.RoundToInt(Mathf.Lerp(0, _textureRes, playerPosX01));
        int texturePosY = Mathf.RoundToInt(Mathf.Lerp(0, _textureRes, playerPosZ01));


        int halfMaskWidthHeight = _snowMeltingMask.width / 2;
        for (int i = 0; i < _snowMeltingMask.height; i++)
        {
            for (int j = 0; j < _snowMeltingMask.width; j++)
            {
                int xPos = texturePosX + j - halfMaskWidthHeight;
                int yPos = texturePosY + i - halfMaskWidthHeight;
                float brightVal = _snowPathTexture.GetPixel(xPos, yPos).r;
                brightVal -= _snowMeltingMask.GetPixel(j, i).r * 1.5f * Time.deltaTime;
                Color colorToApply = new Color(brightVal, brightVal, brightVal);
                _snowPathTexture.SetPixel(xPos, yPos, colorToApply);
            }
        }

        _snowPathTexture.Apply();
        _snowPlaneMeshRenderer.material.SetTexture("_InteractiveSnowTexture", _snowPathTexture);
    }
}
