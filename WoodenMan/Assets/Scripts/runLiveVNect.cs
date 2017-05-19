using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class runLiveVNect : runLive
{
    private List<int> m_ValidJointIdx;

    override public void Start()
    {
        m_isMoveFloor = true;

        // Parse header
        string Line = "# <REAL WORLD METERS (x, y ,z)>: root_tx, root_ty, root_tz, root_rz, root_ry, root_rx, spine_3_ry, spine_3_rz, spine_3_rx, spine_4_ry, spine_4_rz, spine_4_rx, spine_2_ry, spine_2_rz, spine_2_rx, spine_1_ry, spine_1_rz, spine_1_rx, neck_1_ry, neck_1_rz, neck_1_rx, head_ee_ry, left_clavicle_ry, left_clavicle_rz, left_shoulder_rz, left_shoulder_rx, left_shoulder_ry, left_elbow_rx, left_lowarm_twist, left_hand_rx, left_hand_ry, left_ee_rx, right_clavicle_ry, right_clavicle_rz, right_shoulder_rz, right_shoulder_rx, right_shoulder_ry, right_elbow_rx, right_lowarm_twist, right_hand_rx, right_hand_ry, right_ee_rx, left_hip_rx, left_hip_rz, left_hip_ry, left_knee_rx, left_ankle_rx, left_ankle_ry, left_toes_rx, left_foot_ee, right_hip_rx, right_hip_rz, right_hip_ry, right_knee_rx, right_ankle_rx, right_ankle_ry, right_toes_rx, right_foot_ee";
        // Parse header
        // First tokenize by :
        string[] LRHead = Line.Split(':');

        // Next, parse right part of the header
        string[] FormatString = LRHead[1].Split(',');

        Debug.Log("Format joints length is " + FormatString.Length);

        // Print joints: 29
        // 5-root_rx, 8-spine_3_rx, 11-spine_4_rx, 14-spine_2_rx, 17-spine_1_rx, 20-neck_1_rx, 21-head_ee_ry, 23-left_clavicle_rz, 26-left_shoulder_ry, 27-left_elbow_rx, 28-left_lowarm_twist, 30-left_hand_ry, 31-left_ee_rx, 33-right_clavicle_rz, 36-right_shoulder_ry, 37-right_elbow_rx, 38-right_lowarm_twist, 40-right_hand_ry, 41-right_ee_rx, 44-left_hip_ry, 45-left_knee_rx, 47-left_ankle_ry, 48-left_toes_rx, 49-left_foot_ee, 52-right_hip_ry, 53-right_knee_rx, 55-right_ankle_ry, 56-right_toes_rx, 57-right_foot_ee
        int JSize = 29;
        m_ValidJointIdx = new List<int>();
        m_ValidJointIdx.Add(5);
        m_ValidJointIdx.Add(8);
        m_ValidJointIdx.Add(11);
        m_ValidJointIdx.Add(14);
        m_ValidJointIdx.Add(17);
        m_ValidJointIdx.Add(20);
        m_ValidJointIdx.Add(21);
        m_ValidJointIdx.Add(23);
        m_ValidJointIdx.Add(26);
        m_ValidJointIdx.Add(27);
        m_ValidJointIdx.Add(28);
        m_ValidJointIdx.Add(30);
        m_ValidJointIdx.Add(31);
        m_ValidJointIdx.Add(33);
        m_ValidJointIdx.Add(36);
        m_ValidJointIdx.Add(37);
        m_ValidJointIdx.Add(38);
        m_ValidJointIdx.Add(40);
        m_ValidJointIdx.Add(41);
        m_ValidJointIdx.Add(44);
        m_ValidJointIdx.Add(45);
        m_ValidJointIdx.Add(47);
        m_ValidJointIdx.Add(48);
        m_ValidJointIdx.Add(49);
        m_ValidJointIdx.Add(52);
        m_ValidJointIdx.Add(53);
        m_ValidJointIdx.Add(55);
        m_ValidJointIdx.Add(56);
        m_ValidJointIdx.Add(57);

        m_JointSpheres = new GameObject[JSize];
        for (int i = 0; i < m_JointSpheres.Length; ++i)
        {
            m_JointSpheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_WoodMatRef = Resources.Load("wood_Texture", typeof(Material)) as Material; // loads from Assests/Resources directory
            if (m_WoodMatRef != null)
            {
                m_JointSpheres[i].GetComponent<Renderer>().material = m_WoodMatRef;
                //m_JointSpheres[i].GetComponent<Renderer>().material.color = new Color(252.0f / 255.0f, 114.0f / 255.0f, 114.0f / 255.0f);
                m_JointSpheres[i].GetComponent<Renderer>().material.color = new Color(252.0f / 255.0f, 164.0f / 255.0f, 63.0f / 255.0f);
            }
            else
            {
                Debug.Log("Wood texture not assigned, will draw red.");
                m_JointSpheres[i].GetComponent<Renderer>().material.color = Color.red;
            }

            // Size of spheres
            float SphereRadius = 0.05f;
            m_JointSpheres[i].transform.localScale = new Vector3(SphereRadius, SphereRadius, SphereRadius);
        }

        // Next up create ellipsoids for bones
        int nBones = 28;
        m_Bones = new GameObject[nBones];
        for (int i = 0; i < nBones; ++i)
        {
            m_Bones[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            if (m_WoodMatRef != null)
            {
                m_Bones[i].GetComponent<Renderer>().material = m_WoodMatRef;
                //m_Bones[i].GetComponent<Renderer>().material.color = new Color(252.0f / 255.0f, 114.0f / 255.0f, 114.0f / 255.0f);
                m_Bones[i].GetComponent<Renderer>().material.color = new Color(252.0f / 255.0f, 164.0f / 255.0f, 63.0f / 255.0f);
            }
            else
            {
                Debug.Log("Wood texture not assigned, will draw red.");
                m_Bones[i].GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }

    override public void Update(string Line)
    {
        //Debug.Log(Line);

        if (Line.Length == 0)
            return;

        // Parse line
        int ParseOffset = 0; // No offset for real-time system
        string[] Tokens = Line.Split(',');
        //Debug.Log("Detected " + (Tokens.Length - ParseOffset) / 3 + " joints.");
        if ((Tokens.Length - ParseOffset) / 3 != 58)
            return;
        float LowestY = 0.0f;
        Vector3[] Joints = new Vector3[(Tokens.Length - ParseOffset) / 3];
        for (int i = 0; i < m_JointSpheres.Length; ++i)
        {
            int Idx = m_ValidJointIdx[i];
            Joints[i].x = -float.Parse(Tokens[3 * Idx + 0 + ParseOffset]) * 0.001f; // Prevent mirroring
            Joints[i].y = -float.Parse(Tokens[3 * Idx + 1 + ParseOffset]) * 0.001f;
            Joints[i].z = -float.Parse(Tokens[3 * Idx + 2 + ParseOffset]) * 0.001f; // Flip for Google VR

            if (Joints[i].y < LowestY)
                LowestY = Joints[i].y;

            //Debug.Log(Joints[i]);
            //m_JointSpheres[i].transform.position = Vector3.Lerp(m_JointSpheres[i].transform.position, Joints[i], 0.2f);
            //Joints[i] = m_JointSpheres[i].transform.position; // Hack
            m_JointSpheres[i].transform.position = Joints[i];
        }

        // Make floor stick to bottom-most joint (at index 16 or 20)
        GameObject Plane = GameObject.Find("CheckerboardPlane");
        float PlaneFootBuffer = 0.02f;
        float MoveAmount = 0.0f;
        if (Plane != null)
        {
            Vector3 OrigPos = Plane.transform.position;

            if (m_isMoveFloor)
            {
                MoveAmount = Plane.transform.position.y;
                Plane.transform.position = new Vector3(OrigPos[0], LowestY - PlaneFootBuffer, OrigPos[2]);
                MoveAmount = MoveAmount - Plane.transform.position.y;
            }

            if (m_isVRMode)
            {
                GameObject Head = GameObject.Find("Main Camera");
                if (Head != null)
                {
                    Head.transform.position = Joints[6];
                    Head.transform.rotation = GvrViewer.Controller.Head.transform.rotation;
                }
            }
            else
            {
                GameObject FollowerCamera = GameObject.Find("ExternalCamera");
                if (FollowerCamera != null)
                {
                    // Move camera with checkerboard plane
                    OrigPos = FollowerCamera.transform.position;
                    FollowerCamera.transform.position = new Vector3(OrigPos[0], Plane.transform.position.y + 1, OrigPos[2]);
                }
                else
                    Debug.Log("Empty follower camera.");
            }
        }

        if (GameObject.Find("AllBalls") != null)
        {
            int childCount = GameObject.Find("AllBalls").GetComponent<Transform>().GetChildCount();
            Transform Feet = GameObject.Find("feet").GetComponent<Transform>();
            for (int i = 0; i < childCount; ++i)
            {
                int childCnt = GameObject.Find("AllBalls").GetComponent<Transform>().GetChild(i).GetChildCount();
                for (int j = 0; j < childCnt; ++j)
                {
                    Transform Ball = GameObject.Find("AllBalls").GetComponent<Transform>().GetChild(i).GetChild(j);
                    Vector3 OldPos = Ball.transform.position;
                    if (Ball.GetComponent<Rigidbody>() != null)
                    {
                        Ball.GetComponent<Collider>().attachedRigidbody.useGravity = false;
                        Ball.GetComponent<Rigidbody>().isKinematic = true;

                        Ball.position = new Vector3(OldPos[0], OldPos[1] - MoveAmount, OldPos[2]);

                        Ball.GetComponent<Collider>().attachedRigidbody.useGravity = true;
                        Ball.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }
            }
        }



        //0-root_rx, 1-spine_3_rx
        drawEllipsoid(Joints[0], Joints[1], m_Bones[0]);
        //1-spine_3_rx, 2-spine_4_rx
        drawEllipsoid(Joints[1], Joints[2], m_Bones[1]);
        //0-root_rx, 3-spine_2_rx
        drawEllipsoid(Joints[0], Joints[3], m_Bones[2]);
        //3-spine_2_rx, 4-spine_1_rx
        drawEllipsoid(Joints[3], Joints[4], m_Bones[3]);
        //2-spine_4_rx, 5-neck_1_rx
        drawEllipsoid(Joints[2], Joints[5], m_Bones[4]);
        //5-neck_1_rx, 6-head_ee_ry
        drawEllipsoid(Joints[5], Joints[6], m_Bones[5]);

        //2-spine_4_rx, 7-left_clavicle_rz
        drawEllipsoid(Joints[2], Joints[7], m_Bones[6]);
        //7-left_clavicle_rz, 8-left_shoulder_ry
        drawEllipsoid(Joints[7], Joints[8], m_Bones[7]);
        //8-left_shoulder_ry, 9-left_elbow_rx
        drawEllipsoid(Joints[8], Joints[9], m_Bones[8]);
        //9-left_elbow_rx, 10-left_lowarm_twist
        drawEllipsoid(Joints[9], Joints[10], m_Bones[9]);
        //10-left_lowarm_twist, 11-left_hand_ry
        drawEllipsoid(Joints[10], Joints[11], m_Bones[10]);
        //11-left_hand_ry, 12-left_ee_rx
        drawEllipsoid(Joints[11], Joints[12], m_Bones[11]);

        //2-spine_4_rx, 13-right_clavicle_rz
        drawEllipsoid(Joints[2], Joints[13], m_Bones[12]);
        //13-right_clavicle_rz, 14-right_shoulder_ry
        drawEllipsoid(Joints[13], Joints[14], m_Bones[13]);
        //14-right_shoulder_ry, 15-right_elbow_rx
        drawEllipsoid(Joints[14], Joints[15], m_Bones[14]);
        //15-right_elbow_rx, 16-right_lowarm_twist
        drawEllipsoid(Joints[15], Joints[16], m_Bones[15]);
        //16-right_lowarm_twist, 17-right_hand_ry
        drawEllipsoid(Joints[16], Joints[17], m_Bones[16]);
        //17-right_hand_ry, 18-right_ee_rx
        drawEllipsoid(Joints[17], Joints[18], m_Bones[17]);

        //4-spine_1_rx, 19-left_hip_ry
        drawEllipsoid(Joints[4], Joints[19], m_Bones[18]);
        //19-left_hip_ry, 20-left_knee_rx
        drawEllipsoid(Joints[19], Joints[20], m_Bones[19]);
        //20-left_knee_rx, 21-left_ankle_ry
        drawEllipsoid(Joints[20], Joints[21], m_Bones[20]);
        //21-left_ankle_ry, 22-left_toes_rx
        drawEllipsoid(Joints[21], Joints[22], m_Bones[21]);
        //22-left_toes_rx, 23-left_foot_ee
        //drawEllipsoid(Joints[22], Joints[23], m_Bones[22]);

        //4-spine_1_rx, 24-right_hip_ry
        drawEllipsoid(Joints[4], Joints[24], m_Bones[23]);
        //24-right_hip_ry, 25-right_knee_rx
        drawEllipsoid(Joints[24], Joints[25], m_Bones[24]);
        //25-right_knee_rx, 26-right_ankle_ry
        drawEllipsoid(Joints[25], Joints[26], m_Bones[25]);
        //26-right_ankle_ry, 27-right_toes_rx
        drawEllipsoid(Joints[26], Joints[27], m_Bones[26]);
        //27-right_toes_rx, 28-right_foot_ee
        //drawEllipsoid(Joints[27], Joints[28], m_Bones[27]);

        // Disable toe sphere
        m_JointSpheres[22].GetComponent<MeshRenderer>().enabled = false;
        m_JointSpheres[23].GetComponent<MeshRenderer>().enabled = false;
        m_JointSpheres[27].GetComponent<MeshRenderer>().enabled = false;
        m_JointSpheres[28].GetComponent<MeshRenderer>().enabled = false;
        m_Bones[22].GetComponent<MeshRenderer>().enabled = false;
        m_Bones[27].GetComponent<MeshRenderer>().enabled = false;

        // Draw mesh
        RFoot.transform.rotation = Quaternion.LookRotation((Joints[23] - Joints[22]).normalized);
        RFoot.transform.rotation = Quaternion.Euler(RFoot.transform.eulerAngles + new Vector3(140, 0, 0));
        RFoot.transform.position = Joints[22];

        // Rotate z-axis to align with bone vector
        LFoot.transform.rotation = Quaternion.LookRotation((Joints[28] - Joints[27]).normalized);
        LFoot.transform.rotation = Quaternion.Euler(LFoot.transform.eulerAngles + new Vector3(140, 0, 0));
        // Position at middle
        LFoot.transform.position = Joints[27];

    }
}
