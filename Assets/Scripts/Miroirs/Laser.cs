using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] public float m_range;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(transform.position, direction, out hit, m_range))
        {
            float rayLength = hit.distance;
            Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red);
            if(hit.collider )
            {
                if (hit.collider.gameObject.layer == 0b110)
                {
                    Debug.Log("you win");
                }
                else if (hit.collider.gameObject.layer == 0b111)
                {
                    hit.collider.gameObject.GetComponent<Receiver>().ActivateChildren();
                }

                if (hit.collider.gameObject.layer == 0b11)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        Vector3 start = hit.point;
                        direction = Vector3.Reflect(direction, hit.normal);
                        
                        if (Physics.Raycast(start, direction, out hit, m_range))
                        {
                            Debug.DrawRay(start, direction * hit.distance, Color.red);
                            if (hit.collider.gameObject.layer == 0b11)
                            {
                                //Debug.Log($"bounce{i}");
                            }
                            else if (hit.collider.gameObject.layer == 0b110)
                            {
                                //Debug.Log("you win");
                                break;
                            }
                            else if (hit.collider.gameObject.layer == 0b111)
                            {
                                hit.collider.gameObject.GetComponent<Receiver>().ActivateChildren();
                                Debug.Log(hit.collider.gameObject.GetComponent<Receiver>().m_lasered);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            Debug.DrawRay(start, direction * m_range, Color.red);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * m_range, Color.green);
        }
        
    }
}
