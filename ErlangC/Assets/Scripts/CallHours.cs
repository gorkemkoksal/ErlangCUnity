using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallHours : MonoBehaviour
{
 
    [Header("Shifts")]
    [SerializeField] double zero;
    [SerializeField] double one;
    [SerializeField] double two;
    [SerializeField] double three;
    [SerializeField] double four;
    [SerializeField] double five;
    [SerializeField] double six;
    [SerializeField] double seven;
    [SerializeField] double eight;
    [SerializeField] double nine;
    [SerializeField] double ten;
    [SerializeField] double eleven;
    [SerializeField] double twelve;
    [SerializeField] double thirteen;
    [SerializeField] double fourteen;
    [SerializeField] double fifteen;
    [SerializeField] double sixteen;
    [SerializeField] double seventeen;
    [SerializeField] double eighteen;
    [SerializeField] double nineteen;
    [SerializeField] double twenty;
    [SerializeField] double twentyOne;
    [SerializeField] double twentyTwo;
    [SerializeField] double twentyThree;
    List<double> hourCalls;
    // Start is called before the first frame update
    void Start()
    {
        List<double> hourCalls = new List<double>();
        List<double> shiftOne = new List<double>();

        hourCalls.Add(zero);
        hourCalls.Add(one);
        hourCalls.Add(two);
        hourCalls.Add(three);
        hourCalls.Add(four);
        hourCalls.Add(five);
        hourCalls.Add(six);
        hourCalls.Add(seven);
        hourCalls.Add(eight);
        hourCalls.Add(nine);
        hourCalls.Add(ten);
        hourCalls.Add(eleven);
        hourCalls.Add(twelve);
        hourCalls.Add(thirteen);
        hourCalls.Add(fourteen);
        hourCalls.Add(fifteen);
        hourCalls.Add(sixteen);
        hourCalls.Add(seventeen);
        hourCalls.Add(eighteen);
        hourCalls.Add(nineteen);
        hourCalls.Add(twenty);
        hourCalls.Add(twentyOne);
        hourCalls.Add(twentyTwo);
        hourCalls.Add(twentyThree);

        ReturnLength();

    }
    public int ReturnLength()
    {
        return hourCalls.Count;
        Debug.Log(hourCalls.Count);
    }
    public double ReturnHourCalls(int hours)
    {
       
        return hourCalls[hours];
    }
}
