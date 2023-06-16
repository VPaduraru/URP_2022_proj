namespace SnowProject
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;

    public class SnowRelatedUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController;
        [SerializeField]
        private TextMeshProUGUI _accumulatedSnowText;

        private void Start()
        {
            _playerController.OnCollectSnow.AddListener(SetAccumulatedSnow);
        }

        private void SetAccumulatedSnow()
        {
            _accumulatedSnowText.text = _playerController.GetAccumulatedSnow().ToString();
        }
    }

}
