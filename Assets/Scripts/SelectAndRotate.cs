using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAndRotate : MonoBehaviour
{
    [SerializeField] private GameObject Selecting;
    private static GameObject instanceofSelecting;
    //Selected GameObjects
    private static GameObject closest;
    private static GameObject secondClosest;
    private static GameObject thirdClosest;

    public static bool flag = false;

    private Vector3 mouseDownPos;
    private Vector3 mouseUpPos;
    private Vector3 MouseDiff;

    private static Vector3 Temp1;
    private static Vector3 Temp2;
    private static Vector3 Temp3;

    private static GameObject TempGO;
    private static Vector3 Temp;

    private static GameObject a;
    private static GameObject b;
    private static GameObject c;

    private static bool Selected = false;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
        }
        if (flag)
        {
            SetShapeAndHexes(closest, secondClosest, thirdClosest);
            Temp1 = a.transform.position;
            Temp2 = b.transform.position;
            Temp3 = c.transform.position;
            flag = false;
        }
        if (Input.GetMouseButtonUp(0) && Selected)
        {
            mouseUpPos = Input.mousePosition;
            MouseDiff = mouseUpPos - mouseDownPos;
            //Debug.Log("mouse kalktý");
            if (MouseDiff.x > 10 && Input.mousePosition.y > Camera.main.WorldToScreenPoint(instanceofSelecting.transform.position).y)
            {
                StartCoroutine(RotateClockwise());
            }
            else if (MouseDiff.x < -10 && Input.mousePosition.y > Camera.main.WorldToScreenPoint(instanceofSelecting.transform.position).y)
            {
                StartCoroutine(RotateCounterClockwise());
            }
            else if (MouseDiff.x > 10 && Input.mousePosition.y < Camera.main.WorldToScreenPoint(instanceofSelecting.transform.position).y)
            {
                StartCoroutine(RotateCounterClockwise());
            }
            else if (MouseDiff.x < -10 && Input.mousePosition.y < Camera.main.WorldToScreenPoint(instanceofSelecting.transform.position).y)
            {
                StartCoroutine(RotateClockwise());
            }
            else if (MouseDiff.y > 10 && Input.mousePosition.x > Camera.main.WorldToScreenPoint(instanceofSelecting.transform.position).x)
            {
                StartCoroutine(RotateCounterClockwise());
            }
            else if (MouseDiff.y < -10 && Input.mousePosition.x > Camera.main.WorldToScreenPoint(instanceofSelecting.transform.position).x)
            {
                StartCoroutine(RotateClockwise());
            }
            else if (MouseDiff.y > 10 && Input.mousePosition.x < Camera.main.WorldToScreenPoint(instanceofSelecting.transform.position).x)
            {
                StartCoroutine(RotateClockwise());
            }
            else if (MouseDiff.y < -10 && Input.mousePosition.x < Camera.main.WorldToScreenPoint(instanceofSelecting.transform.position).x)
            {
                StartCoroutine(RotateCounterClockwise());
            }
        }
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject[] hexes;
            hexes = GameObject.FindGameObjectsWithTag("Hex");
            closest = null;
            secondClosest = null;
            thirdClosest = null;
            GameObject temp = null;
            Vector3 Center;
            float distance = Mathf.Infinity;

            foreach (GameObject hex in hexes)
            {
                Vector3 diff = hex.transform.position - hit.point;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    secondClosest = hex;
                    if (GetDistance(closest, hit) > GetDistance(secondClosest, hit))
                    {
                        temp = closest;
                        closest = secondClosest;
                        secondClosest = temp;
                    }
                    distance = (secondClosest == null) ? Mathf.Infinity : GetDistance(secondClosest, hit);
                }
            }
            thirdClosest = GetThirdClosest(closest, secondClosest, hexes, hit);
            Center = (closest.transform.position + secondClosest.transform.position + thirdClosest.transform.position) / 3;
            Debug.Log(closest + " " + secondClosest + " " + thirdClosest);
            if (Selecting != null)
            {
                Destroy(instanceofSelecting);
                instanceofSelecting = (GameObject)Instantiate(Selecting, new Vector3(Center.x, Center.y + 0.15f, Center.z), Quaternion.identity);
                Selected = true;
            }
            instanceofSelecting.transform.LookAt(closest.transform);
            instanceofSelecting.transform.Rotate(-8, 0, 0);
            Temp = instanceofSelecting.transform.position;
            flag = true;
        }
    }

    private float GetDistance(GameObject hex, RaycastHit hit)
    {
        return (hex == null) ? Mathf.Infinity : Vector3.Distance(hit.point, hex.transform.position);
    }

    private GameObject GetThirdClosest(GameObject hex1, GameObject hex2, GameObject[] hexes, RaycastHit hit)
    {
        float distance = Mathf.Infinity;
        GameObject thirdClosest = null;
        foreach (GameObject hex in hexes)
        {
            if (hex != hex1 && hex != hex2)
            {
                float curDistance = Vector3.Distance(hex.transform.position, hit.point) + Vector3.Distance(hex.transform.position, hex1.transform.position) + Vector3.Distance(hex.transform.position, hex2.transform.position);
                if (curDistance < distance)
                {
                    distance = curDistance;
                    thirdClosest = hex;
                }
            }
        }
        return thirdClosest;
    }

    private IEnumerator RotateClockwise()
    {
        float elapsedTime = 0f;
        float duration = 0.1f;
        a.transform.SetParent(instanceofSelecting.transform);
        b.transform.SetParent(instanceofSelecting.transform);
        c.transform.SetParent(instanceofSelecting.transform);

        while (elapsedTime < duration)
        {
            instanceofSelecting.transform.Rotate(0f, 15f * Time.deltaTime, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        a.transform.SetParent(null);
        b.transform.SetParent(null);
        c.transform.SetParent(null);

        instanceofSelecting.transform.LookAt(closest.transform.position);
        instanceofSelecting.transform.Rotate(-8, 0, 0);
        if (elapsedTime >= duration)
        {
            a.transform.position = Temp2;
            b.transform.position = Temp3;
            c.transform.position = Temp1;

            closest.transform.rotation = Quaternion.identity;
            secondClosest.transform.rotation = Quaternion.identity;
            thirdClosest.transform.rotation = Quaternion.identity;

            flag = true;
        }
    }
    private IEnumerator RotateCounterClockwise()
    {
        float elapsedTime = 0f;
        float duration = 0.1f;
        a.transform.SetParent(instanceofSelecting.transform);
        b.transform.SetParent(instanceofSelecting.transform);
        c.transform.SetParent(instanceofSelecting.transform);

        while (elapsedTime < duration)
        {
            instanceofSelecting.transform.Rotate(0f, -15f * Time.deltaTime, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        a.transform.SetParent(null);
        b.transform.SetParent(null);
        c.transform.SetParent(null);

        instanceofSelecting.transform.LookAt(closest.transform.position);
        instanceofSelecting.transform.Rotate(-8, 0, 0);

        if (elapsedTime >= duration)
        {
            a.transform.position = Temp3;
            b.transform.position = Temp1;
            c.transform.position = Temp2;

            closest.transform.rotation = Quaternion.identity;
            secondClosest.transform.rotation = Quaternion.identity;
            thirdClosest.transform.rotation = Quaternion.identity;

            flag = true;
        }
    }


    private void SetShapeAndHexes(GameObject first, GameObject second, GameObject third)
    {
        int firstXLocation = Convert.ToInt32(first.name.Split('_')[0]);
        int secondXLocation = Convert.ToInt32(second.name.Split('_')[0]);
        int thirdXLocation = Convert.ToInt32(third.name.Split('_')[0]);

        int firstYLocation = Convert.ToInt32(first.name.Split('_')[1]);
        int secondYLocation = Convert.ToInt32(second.name.Split('_')[1]);
        int thirdYLocation = Convert.ToInt32(third.name.Split('_')[1]);

        if (firstXLocation == secondXLocation)
        {
            if (firstYLocation > secondYLocation)
            {
                a = third;
                b = second;
                c = first;
            }
            else if (firstYLocation < secondYLocation)
            {
                a = third;
                b = first;
                c = second;
            }
        }

        else if (firstXLocation == thirdXLocation)
        {
            if (firstYLocation < thirdYLocation)
            {
                a = second;
                b = third;
                c = first;
            }
            else if (firstYLocation > thirdYLocation)
            {
                a = second;
                b = first;
                c = third;
            }
        }

        else if (secondXLocation == thirdXLocation)
        {
            if (thirdYLocation > secondYLocation)
            {
                a = first;
                b = second;
                c = third;
            }
            else if (thirdYLocation < secondYLocation)
            {
                a = first;
                b = third;
                c = second;
            }
        }
    }

    public static void IdSwap(GameObject[,] Hexagons, int x, int y) 
    {
        
    }
}
