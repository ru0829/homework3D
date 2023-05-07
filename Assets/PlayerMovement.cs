using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�Ƅ��O��")]
    public float moveSpeed;
    public float jumpForce;          // ���S����
    public float jumpCooldown;       // �O��Ҫ����������������S
    public float groundDrag;         // ����Ĝp��
    public float airMultiplier;      // �ڿ��еļӳ��ٶȣ�����O����0�ʹ������w�����h�@��ֵҪС�1

    [Header("���I����")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("�����O��")]
    public Transform PlayerCamera;   // �zӰ�C

    [Header("�ذ�_�J")]
    public float playerHeight;       // �O����Ҹ߶�
    public LayerMask whatIsGround;   // �O����һ���D�����侀���Դ򵽵�
    public bool grounded;            // ����׃�����Л]�д򵽵���

    private bool readyToJump;        // �O���Ƿ�������S
    private float horizontalInput;   // ���ҷ����I�Ĕ�ֵ(-1 <= X <= +1)
    private float verticalInput;     // ���·����I�Ĕ�ֵ(-1 <= Y <= +1)

    private Vector3 moveDirection;   // �Ƅӷ���

    private Rigidbody rbFirstPerson; // ��һ�˷Q���(�z���w)�Ą��w

    private void Start()
    {
        rbFirstPerson = GetComponent<Rigidbody>();
        rbFirstPerson.freezeRotation = true;         // �i����һ�˷Q������w���D����׌�z���w�����������́y�D
        readyToJump = true;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();   // �ɜy�ٶȣ��^��͜p��

        // ���һ�l���������侀�����Д��Л]�д򵽵��棿
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        Debug.DrawRay(transform.position, new Vector3(0, -(playerHeight * 0.5f + 0.3f), 0), Color.red); // �ڜyԇ�A�Ό��侀�O����tɫ���l���������l�L�ȉ򲻉�
        // ��������ذ壬���O��һ����������(�@�������u�������ƄӵĜp�ٸ�)
        if (grounded)
            rbFirstPerson.drag = groundDrag;
        else
            rbFirstPerson.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();     // ֻҪ������Ƅӣ����h��ŵ�FixedUpdate()        
    }

    // ������ȡ��Ŀǰ��Ұ������I�������ҵĔ�ֵ���������S�О�
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // ��������O�������S���I
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // ������S�^�ᣬ�͕������O�������ƕr�g�������r�g���˲����������S
        }
    }

    private void MovePlayer()
    {
        // Ӌ���Ƅӷ���(�䌍����Ӌ��X�S�cZ�S�ɂ����������)
        moveDirection = PlayerCamera.forward * verticalInput + PlayerCamera.right * horizontalInput;

        // ����ڵ��棬�Ƅӷ�ʽ����ͨ�Ƅ�
        if (grounded)
        {
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            rbFirstPerson.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // ������ڵ��棬�t�Ƅ��ٶ�߀���Գ���һ���ڿ��еļӳ˔��֣������u������һ�������w�ĳ����w��Ч��
        else if (!grounded)
            rbFirstPerson.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    // �������ɜy�ٶȁK�p��
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z); // ȡ�ÃHX�S�cZ�S��ƽ���ٶ�

        // ���ƽ���ٶȴ���A�O�ٶ�ֵ���͌�������ٶ��޶���A�O�ٶ�ֵ
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rbFirstPerson.velocity = new Vector3(limitedVel.x, rbFirstPerson.velocity.y, limitedVel.z);
        }
    }

    // ���������S
    private void Jump()
    {
        // �����O��Y�S�ٶ�
        rbFirstPerson.velocity = new Vector3(rbFirstPerson.velocity.x, 0f, rbFirstPerson.velocity.z);
        // ���������Ƶ�һ�˷Q�����ForceMode.Impulse����׌���͵�ģʽ��һ˲�g�����������S�ĸ��X
        rbFirstPerson.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // �����������O��׃��readyToJump��true�ķ���
    private void ResetJump()
    {
        readyToJump = true;
    }
}
