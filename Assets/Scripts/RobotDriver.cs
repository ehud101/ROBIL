using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RobotDriver : NetworkBehaviour
{
   public bool manualInput = true;
   //public List<AxleInfo> axleInfos;
   public float throttleCommand, steeringCommand;
   public float maxMotorTorque;
   public float maxSteeringAngle;
   

    private float currentMotorTorque;
    private float currentSteeringAngle;


   public Vector3 com;

   private Rigidbody rb;

   void Start(){
       rb = GetComponent<Rigidbody>();
       com = new Vector3(0, -0.2f, 0);

   }


    [Client]
    public void FixedUpdate()
    {
        if (!hasAuthority){
            return;
        }
        
        rb.centerOfMass = com;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 50.0f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 1.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

    }

}

