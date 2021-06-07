using UnityEngine;

#pragma warning disable 0649

public class MyClasses_FieldOfView_CharacterController : MonoBehaviour
{
    #region ----- Variable -----

    [SerializeField]
    private float _moveSpeed = 5;
    [SerializeField]
    private Rigidbody _rigidbody;

    #endregion

    #region ----- Implement MonoBehaviour -----

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
        mousePosition.y = transform.position.y;
        transform.LookAt(mousePosition);
        _rigidbody.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * _moveSpeed;
    }

    #endregion
}