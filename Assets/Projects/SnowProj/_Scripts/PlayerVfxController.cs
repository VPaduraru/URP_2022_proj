namespace SnowProject.Vfx
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.VFX;

    public class PlayerVfxController : MonoBehaviour
    {
        [SerializeField]
        private VisualEffect _gatheringVisualEffect;
        [SerializeField]
        private Transform _gatheringVisualEffectPosObj;

        private void Update()
        {
            _gatheringVisualEffect.SetVector3("ToPosition", transform.position);
            _gatheringVisualEffect.transform.position = _gatheringVisualEffectPosObj.position;
        }
    }

}
