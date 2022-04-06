using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BreakInfinity;
using System;
using System.IO;

public class ErlangC : MonoBehaviour
{
 /*   [Header("Inputs")]
    [SerializeField] TextMeshProUGUI noffCallsTM;
    [SerializeField] TextMeshProUGUI periodTM;
    [SerializeField] TextMeshProUGUI AHTTM;
    [SerializeField] TextMeshProUGUI SLTM;
    [SerializeField] TextMeshProUGUI TATTM;
    [SerializeField] TextMeshProUGUI maxOTM;
    [SerializeField] TextMeshProUGUI shrinkageTM; */
    [Header("InputsDeneme")]
    [SerializeField] double period;
    [SerializeField] double AHT;
    [SerializeField] double SL;
    [SerializeField] double SLUpperLimit;
    [SerializeField] double lowSL;
    [SerializeField] double lowSLUpperLimit;
    [SerializeField] double lowSLLowerBound;
    [SerializeField] double lowSLUpperBound;   
    [SerializeField] float busySLLowerBound;
    [SerializeField] float busySLUpperBound;
    [SerializeField] double TAT;
    [SerializeField] double maxO;
    [SerializeField] double shrinkage;
    [Header("Shifts")]
    [SerializeField] public List<double> hourCalls = new List<double>();
    public List<double> quarterCalls = new List<double>();
    [SerializeField] List<BigDouble> quarterSL = new List<BigDouble>();


    //[Header("Starting Agents")]
    //[SerializeField] public List<double> tryAgents = new List<double>();
  //  BigDouble e = 2.7182818284590452353602874713526624977572470936999595749669676277240766303535475945713821785251664274274663919320030599218174135966290435729003342;
 
    int quarter;
    int hours;
    public  List<double> shiftOne = new List<double>();
    public List<double> shiftTwo = new List<double>();
    public List<double> shiftThree= new List<double>();
    //math variables
    BigDouble A;
    double N;
    BigDouble Y;
    BigDouble result;
    bool firstN = true;
    bool highLevel=false;
    double M;
    bool dailySLUpdater = false;
    bool lessThanTarget;
    int addHour;
    //shift decider bools
    //double checkDiff;
    //bool firstTry = true;
    //bool secondGroup = false;
    //bool secondIsEmpty = true;
    //bool thirdGroup = false;

    //quarter operators
    double denominator, firstRate, mid1Rate, mid2Rate, lastRate,firstQuarter,secondQuarter,thirdQuarter,forthQuarter;
    int quarterCounter;

    void Start()
    {
        string path = Application.dataPath + "/Log.txt";
        

        int am;
        for (hours = 1; hours < hourCalls.Count-1; hours++)
        {
             denominator = hourCalls[hours-1] * 3 + hourCalls[hours] * 6 + hourCalls[hours + 1] * 3;
             firstRate = (2 * hourCalls[hours-1] + hourCalls[hours]) / denominator;
             mid1Rate = (hourCalls[hours-1] + 2 * hourCalls[hours]) / denominator;
             mid2Rate = (2 * hourCalls[hours] + hourCalls[hours + 1]) / denominator;
             lastRate = (2 * hourCalls[hours + 1] + hourCalls[hours]) / denominator;
            
            QuarterCalls();
          //  ErlangCalculation(hours); 
        }

        for (quarter = 0; quarter < quarterCalls.Count; quarter++)
        {
            ErlangCalculation(quarter);
        }

        //for (am = 0; am < shiftOne.Count; am++)
        //{
        //    Debug.Log("Shift 1 " + "hour: " + am + "quarter: " + am%4+1 + ": " + shiftOne[am]);
        //}
        //for (int bam = 0; bam < shiftTwo.Count; bam++)
        //{
        //    Debug.Log("Shift 2 " + "hour: " + am + "quarter: " + am % 4+1 + ": " + shiftTwo[bam]);                                     SHIFT WRITER
        //    am++;
        //}
        //for (int cam = 0; cam < shiftThree.Count; cam++)
        //{
        //    Debug.Log("Shift 3 " + "hour: " + am + "quarter: " + am % 4+1 + ": " + shiftThree[cam]);
        //    am++;
        //}

        do
        {
            DailyChecker();
        } while (lessThanTarget);

        bool logger = true;

        for (am = 0; am < shiftOne.Count; am++)
        {
            
            Debug.Log(shiftOne[am]);
            if (logger)
            {
                File.WriteAllText(path, shiftOne[am].ToString() + "\n");
                logger = false;
            }
            else
                File.AppendAllText(path, shiftOne[am].ToString() + "\n");
        }
        //for (int bam = 0; bam < shiftTwo.Count; bam++)
        //{
        //    Debug.Log(shiftTwo[bam]);
        //    File.AppendAllText(path, shiftTwo[bam].ToString() + "\n");                               SEPARATED SHIFT WRITER
            
        //}
        //for (int cam = 0; cam < shiftThree.Count; cam++) 
        //{
        //    Debug.Log(shiftThree[cam]);
        //    File.AppendAllText(path, shiftThree[cam].ToString() + "\n");
           
        //}
    }

    private void DailyChecker()
    {
        BigDouble sumOfSL = 0;
        BigDouble numOfCalls = 0;
        for(int i = 0; i < quarterSL.Count; i++)
        {
            sumOfSL += quarterSL[i] * quarterCalls[i];
            numOfCalls += quarterCalls[i];

        }
        BigDouble dailySL=sumOfSL / numOfCalls;
        Debug.Log("dailySL " + dailySL);
        if (dailySL < SL)
        {
            lessThanTarget = true;
            addHour = (int)Math.Round(UnityEngine.Random.Range(busySLLowerBound, busySLUpperBound));
            dailySLUpdater = true;
            Debug.Log("adder " + addHour);
            ErlangCalculation(addHour);
            Debug.Log("I did it");
            DailyChecker();
        }
        else
        {
            lessThanTarget = false;
        }
    }

    private void QuarterCalls()
    {
       
                firstQuarter=hourCalls[hours] * firstRate;
                quarterCalls.Add(firstQuarter);
       
                secondQuarter = hourCalls[hours] * mid1Rate;
                quarterCalls.Add(secondQuarter);
        
                thirdQuarter = hourCalls[hours] * mid2Rate;
                quarterCalls.Add(thirdQuarter);
         
                forthQuarter = hourCalls[hours] * lastRate;
                quarterCalls.Add(forthQuarter);
        
    }

    public void ErlangCalculation(int hours)
    {

        BigDouble callsPerHour = quarterCalls[hours] * 60 / period; 
        A = AHT * callsPerHour / 3600;
            if (dailySLUpdater)
            {
            Debug.Log("old N " + shiftOne[hours]);
            N = Math.Ceiling(shiftOne[hours]++);
            Debug.Log("New N " + N);
            }
            else if (firstN)
            {
                N = Mathf.Ceil((float)A) + 1;
                firstN = false;
                //Debug.Log("N first" + N);
            }
            else if (highLevel)
            {
                highLevel = false;
                N--;
                //Debug.Log("N " + N);
            }
            else N++;
        

        BigDouble ApowerN = 1;
        for (int i = 1; i <= N; i++)
        {
            ApowerN = ApowerN * A;
        }

        BigDouble factorielN = 1;
        for (int i = 1; i <= N; i++)
        {
            factorielN *= i;
        }

        BigDouble X = ApowerN*N/ ((N - A) * factorielN);
                                
        //Debug.Log("X " + X);
       

        result = X / (X + SummationFormula(N, A));
        //Debug.Log("result " + result);
        ServiceLevelCalculation();
      

    }

    //public BigDouble SummationFormula(double N, BigDouble A)
    //{
    //    for (int i = 0; i < N; i++)
    //    {
    //        BigDouble Apoweri = 1;
    //        for (int u = 0; u < i; u++)
    //        {
    //            Apoweri = Apoweri * A;
    //        }
    //        BigDouble factoriali = 1;
    //        for (int o = 0; o <= i; o++)
    //        {
    //            if (o == 0) { factoriali = 1; }
    //            else factoriali *= o;
    //        }

    //        Y += Apoweri / factoriali;

    //    }
    //    Debug.Log("Utkusuz" + Y);
    //    //Debug.Log(Y);
    //    return Y;
    //}

    public BigDouble SummationFormula(double N, BigDouble A)
    {
        BigDouble deneme = 1;
        for (int i = 0; i < N; i++)
        {
            BigDouble Apoweri = 1;
            BigDouble factoriali = 1;

            for (int o = 0; o <= i; o++)
            {
                if (o == 0 || o == 1) { Apoweri = A; }
                else Apoweri = Apoweri * A;
                if (o == 0) { factoriali = 1; }
                else factoriali *= o;
                deneme += Apoweri / factoriali;
            }

           
        }
        //Debug.Log("Utkulu" + deneme);
        //Debug.Log(Y);
        return deneme;
    }

    public void ServiceLevelCalculation()
    {

         float ex = Mathf.Exp((float)(-(N - A) * (TAT / AHT)));
        //double ex = Power(-(N - A) * (TAT / AHT));
        

            //Debug.Log("ex " + ex);
            BigDouble currentSL =1 - (result * ex);        
        //Debug.Log("currentSL " + currentSL);
        // BigDouble currentSL = Mathf.Round((float)(1 - (result * ex))*100f)*0.01f;

        BigDouble currentOccupancy = A / N;
 
            if (quarter >= lowSLLowerBound && quarter <= lowSLUpperBound)
        {
            if (currentSL < lowSL || currentOccupancy > maxO)
            {

           //     Debug.Log("Number of agents " + N + "Current Occupancy " + currentOccupancy);
                ErlangCalculation(quarter);
            }
            else if (currentSL > lowSLUpperLimit)
            {
                highLevel = true;
                ErlangCalculation(quarter);
            }
            else
            {
                M = N / (1 - shrinkage);
                firstN = true;
                shiftOne.Add(M);
                quarterSL.Add(currentSL);
            }
        }
        else
        {
            if (currentSL < SL || currentOccupancy > maxO)
            {
                
                // Debug.Log("Number of agents " + N + "Current Occupancy " + currentOccupancy);

                ErlangCalculation(quarter);

            }
            else if (currentSL > SLUpperLimit)
            {
                highLevel = true;
                ErlangCalculation(quarter);
            }
            else if (dailySLUpdater)
            {
                M = N / (1 - shrinkage);
                shiftOne[addHour]=M;
                quarterSL[addHour]=currentSL;
            }
            else
            {
                //Debug.Log("of " + N);
                firstN = true;
                M = N / (1 - shrinkage);
                shiftOne.Add(M);
                quarterSL.Add(currentSL);
                //Debug.Log("SOn SL " + currentSL+ "Son Ocp " + currentOccupancy);
                //Debug.Log("raw gerekli agent " + N);
                //Debug.Log("Gerekli agents with shrinkage " + N / (1 - shrinkage));
                //BigDouble ASA = result * AHT/(N - A);
                //Debug.Log("average answer time " + ASA);
                //var imAns = 1 - result;
                //Debug.Log("Calls answered immediately " + imAns);



                //if (firstTry || Mathf.Abs((float)checkDiff - (float)M) < checkDiff * 0.5 && !secondGroup)                         SHIFTING AND RESULTS
                //{
                //    shiftOne.Add(M);
                //    firstTry = false;
                //}
                //else if (secondIsEmpty)
                //{
                //    secondGroup = true;
                //    secondIsEmpty = false;
                //    shiftTwo.Add(M);

                //}
                //else if (Mathf.Abs((float)checkDiff - (float)M) < checkDiff * 0.5 && !thirdGroup)
                //{
                //    shiftTwo.Add(M);
                //}
                //else
                //{
                //    thirdGroup = true;

                //    shiftThree.Add(M);

                //}
                //checkDiff = M;
            }
        }
    }


    //POWER FORMULA

    //public BigDouble Power(BigDouble raiseToPower)
    //{
    //    BigDouble result = 0;
    //    if (raiseToPower < 0)
    //    {
    //        raiseToPower *= -1;
    //        result = 1 / e;
    //        for (int i = 1; i < raiseToPower; i++)
    //        {
    //            result /= e;
    //        }
    //    }
    //    else
    //    {
    //        result = e;
    //        for (int i = 0; i <= raiseToPower; i++)
    //        {
    //            result *= e;
    //        }
    //    }
    //    return result;
    //}










    /*
    public double NumberofCalls()
    {
        double noffCalls = double.Parse(noffCallsTM.text);
        return noffCalls;
    }

    public double Period()
    {
        double period = double.Parse(periodTM.text);
        return period;
    }
    public double AverageHandlingTime()
    {
        double AHT = double.Parse(AHTTM.text);
        return AHT;
    }
    public double ServiceLevel()
    {
        double SL = double.Parse(SLTM.text);
        return SL;
    }
    public double TargetAnswerTime()
    {
        double TAT = double.Parse(TATTM.text);
        return TAT;
    }
    public double MaximumOccupancy()
    {
        double maxO = double.Parse(maxOTM.text);
        return maxO;
    }
    public double Shrinkage()
    {
        double shrinkage = double.Parse(shrinkageTM.text);
        return shrinkage;
    }
    */

}
