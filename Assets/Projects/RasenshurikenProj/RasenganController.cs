using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasenganController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    private int _layerInt;
    [SerializeField]
    private Camera _cam;
    private Vector3 _lastPosition;
    private Vector3 _velocity;
    private void Awake()
    {
        _layerInt = LayerMask.NameToLayer("Ground");
    }
    // Update is called once per frame
    void Update()
    {
        _lastPosition = transform.position;
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            transform.position = Vector3.Lerp(transform.position, hit.point, _speed * Time.deltaTime);
        }
        _velocity = _lastPosition - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_velocity), _speed * 5f * Time.deltaTime);

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
