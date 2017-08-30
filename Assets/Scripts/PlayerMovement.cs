using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Mouse handling variavles
    private Transform cam;
    private float yRotation;
    private float xRotation;
    public float lookSensitivity = 5;
    private float currentXRotation;
    private float currentYRotation;
    private float yRotationV;
    private float xRotationV;
    public float lookSmoothnes = 0.1f;
    public float bottom = 60F;
    public float top = -60f;

    //Movement
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    //Weapon
    public GameObject bullet;
    public float bulletSpeed = 1000f;
    public float fireTime = 0.1f;
    private float fireTimeTemp = 0;

    //UI
    public GameObject menu;


    // Use this for initialization
    void Start () {
        cam = GetComponentInChildren<Camera>().transform;
	}
	
	// Update is called once per frame
	void Update () {

        //Look controller

        yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
        xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -80, 100);
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothnes);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothnes);
        if (currentXRotation > bottom)
        {
            currentXRotation = bottom;
            xRotation = bottom;
        }

        if (currentXRotation < top)
        {
            currentXRotation = top;
            xRotation = top;
        }

        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
        cam.transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);

        //Movement
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            FireWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu();
        }

        if (fireTimeTemp > 0)
        {
            fireTimeTemp -= Time.deltaTime;
        }

    }

    void MainMenu()
    {
        menu.SetActive(!menu.activeSelf);
        Debug.Log("Menu opened or closed.");
    }

    void FireWeapon()
    {
        if (fireTimeTemp <= 0)
        {
            GameObject firedBullet = Instantiate(bullet, cam.transform.position + cam.transform.forward * 1, cam.transform.rotation);
            Rigidbody bulletRigidbody = firedBullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = transform.forward * bulletSpeed;
            fireTimeTemp = fireTime;
        }

    }
}
