using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PolygonGeneratorDE : Singleton
{

    // '*' - Coin, '/' - Obstacle, '+' - PowerUp, ' ' - Empty
    const int NumberOfTracks = 100,
        trackZLine = 100,
        trackXLine = 3,
        bestCoin = 100,
        bestPower = 3,
        numEmpty = 40,
        bestObs = 50;
    public int WantedObs = 50, WantedCoin = 100, WantedPWR = 3;
    public float zDiff = 2.5f, xDiff = 2.5f, zMultiplier = 0;
    public int eval = 150;
    public static int CoinsToCollect;
    public GameObject GTrack, GField, Obsticle, Coin, EndTrack;
    public GameObject[] PwrUps = new GameObject[2];
    private char[] TypesOfItems = new char[] { '*', '/', ' ' };
    private List<char[]> Track;
    private List<List<char[]>> TrackPopulation, FitnessPopulation;
    // private int[] fitness = new int[NumberOfTracks];
    // private int[] fitnessNew = new int[NumberOfTracks];
    public List<char[]> trackGenereted;
    System.Random rnd;
    void Start()
    {
        trackGenereted = new List<char[]>();
        trackGenereted = GenerateBestTrack(Obsticle, Coin, PwrUps, GTrack, GField, eval, WantedObs, WantedCoin, WantedPWR, EndTrack);
        CreateNewSegment(moveplayer.segmentPart);
        Debug.Log(CoinsToCollect);
        /*TrackPopulation = new List<List<char[]>>();
        FitnessPopulation = new List<List<char[]>>();
        TrackPopulation = PopulateList(TypesOfItems, NumberOfTracks);
        for (int f = 0; f < TrackPopulation.Count; f++)
        {
            fitness[f] = FitnessFunction(TrackPopulation[f]);
        }

        FitnessPopulation = DE(TrackPopulation, 50);

        for (int k = 0; k < FitnessPopulation.Count; k++)
        {
            fitnessNew[k] = FitnessFunction(FitnessPopulation[k]);
        }

        List<char[]> bestCandidate = new List<char[]>();
        List<char[]> candidateInPlay = new List<char[]>();

        int best = fitness[0];
        foreach (var item in fitness)
        {
            if (best > item)
            {
                best = item;
            }
            Debug.Log(item);
        }
        Debug.Log("Best in nonoptimized pop: " + best);


        foreach (var fitem in fitnessNew)
        {
            if (best > fitem)
            {
                best = fitem;
            }
            Debug.Log(fitem);
        }
        Debug.Log("Best in OPTIMIZED pop: " + best);*/
    }
    public void CreateNewSegment(int segmentPart)
    {
        CreateNewSegmentFunction(Obsticle, Coin, PwrUps, GTrack, GField, trackGenereted, EndTrack, segmentPart);
    }
    public void CreateNewSegmentFunction(GameObject obsticle, GameObject coin, GameObject[] pwrUps, GameObject gTrack, GameObject gField, List<char[]> genTrack, GameObject endSeg, int segPart)
    {
        int buildFrom = 0, buildTo = 0;
        if(segPart % 2 == 0)
        {
            buildFrom = 0;
            buildTo = 50;
        }
        else
        {
            buildFrom = 50;
            buildTo = 100;
        }
        int CoinCounter = 0;
        GameObject tmpField;
        int lineCounter = 0;
        for(int i = buildFrom; i < buildTo; i++)
        {
            char[] item = genTrack[i];
            lineCounter++;
            for (int j = 0; j < item.Count(); j++)
            {
                if (j == 0)
                {
                    tmpField = Instantiate(gField, gTrack.transform);
                    tmpField.transform.localPosition = new Vector3(-xDiff, 0, zMultiplier);
                    if (item[j] == '/')
                    {
                        Instantiate(Obsticle, tmpField.transform);
                    }
                    else if (item[j] == '*')
                    {
                        Instantiate(Coin, tmpField.transform);
                        CoinCounter++;
                    }
                    else if (item[j] == '+')
                    {
                        Instantiate(PwrUps[Random.Range(0, 2)], tmpField.transform);
                    }
                }
                else if (j == 1)
                {

                    tmpField = Instantiate(gField, gTrack.transform);
                    tmpField.transform.localPosition = new Vector3(xDiff - xDiff, 0, zMultiplier);
                    if (lineCounter == 25)
                    {
                        Instantiate(endSeg, tmpField.transform);

                        tmpField.name = "EndSegment";
                    }
                    if (item[j] == '/')
                    {
                        Instantiate(Obsticle, tmpField.transform);
                    }
                    else if (item[j] == '*')
                    {
                        Instantiate(Coin, tmpField.transform);
                        CoinCounter++;
                    }
                    else if (item[j] == '+')
                    {
                        Instantiate(PwrUps[Random.Range(0, 2)], tmpField.transform);
                    }
                }
                else
                {
                    tmpField = Instantiate(gField, gTrack.transform);
                    tmpField.transform.localPosition = new Vector3(xDiff, 0, zMultiplier);
                    if (item[j] == '/')
                    {
                        Instantiate(Obsticle, tmpField.transform);

                    }
                    else if (item[j] == '*')
                    {
                         Instantiate(Coin, tmpField.transform);
                        CoinCounter++;
                    }
                    else if (item[j] == '+')
                    {
                        Instantiate(PwrUps[Random.Range(0, 2)], tmpField.transform);
                    }
                }
                
            }
            zMultiplier += zDiff;
        }
    }
    public List<char[]> GenerateBestTrack(GameObject obsticle, GameObject coin, GameObject[] pwrUps, GameObject gTrack, GameObject gField, int eval, int wantedObs, int wantedCoin, int wantedPWR, GameObject endSeg)
    {

        List<char[]> genTrack = GetBestTrack(eval, wantedObs, wantedCoin, wantedPWR);
        return genTrack;
    }
    public List<char[]> GetBestTrack(int eval, int wantedObs, int wantedCoin, int wantedPWR)
    {
        rnd = new System.Random();
        TrackPopulation = PopulateList(TypesOfItems, NumberOfTracks);
        FitnessPopulation = DE(TrackPopulation, eval, wantedObs, wantedCoin, wantedPWR);
        int[] currFitness = FitnessFunction(FitnessPopulation[0], wantedObs, wantedCoin, wantedPWR);
        int currFValue = 0, bestCurrValue = 100;
        int bestIndex = 0;
        for (int i = 0; i < FitnessPopulation.Count; i++)
        {
            currFitness = FitnessFunction(FitnessPopulation[i], wantedObs, wantedCoin, wantedPWR);
            currFValue = 0;
            string fit = "";
            for (int j = 0; j < 3; j++)
            {
                currFValue += currFitness[j];
                if (j == 0)
                    fit += "Obs: " + currFitness[j];
                else if (j == 1)
                    fit += " Coin: " + currFitness[j];
                else if (j == 2)
                    fit += " Pwr: " + currFitness[j];


            }
            if (bestCurrValue > currFValue)
            {
                bestCurrValue = currFValue;
                bestIndex = i;
            }
            //Debug.Log("Fitnes " + i + " - " + fit);
        }
        int numObs = 0, numCoin = 0, numPowerUp = 0, numEmpty = 0;
        List<char[]> tmp = FitnessPopulation[bestIndex];
        for (int i = 0; i < tmp.Count; i++)
        {
            for (int j = 0; j < trackXLine; j++)
            {
                switch (tmp[i][j])
                {
                    case '*':
                        numCoin++;
                        break;
                    case '/':
                        numObs++;
                        break;
                    case '+':
                        numPowerUp++;
                        break;
                    default:
                        numEmpty++;
                        break;
                }
            }
        }
        //Debug.Log("Best fitness: " + bestIndex + " ---- Obs: " + numObs + " Coin: " + numCoin + "PWR" + numPowerUp);
        return FitnessPopulation[bestIndex];
    }
    // Update is called once per frame
    void Update()
    {

    }
    public List<List<char[]>> PopulateList(char[] TypesOfItems, int NumberOfTracks)
    {
        TrackPopulation = new List<List<char[]>>();
        char[] xLine;
        for (int i = 0; i < NumberOfTracks; i++)
        {
            Track = new List<char[]>();
            for (int j = 0; j < trackZLine; j++)
            {
                xLine = new char[3];
                for (int l = 0; l < trackXLine; l++)
                {
                    xLine[l] = TypesOfItems[rnd.Next(0, 3)];
                }
                Track.Add(xLine);
            }
            //fitness[i] = FitnessFunction(Track);
            TrackPopulation.Add(Track);
        }
        return TrackPopulation;
    }
    public int[] FitnessFunction(List<char[]> Track, int wantedObs, int wantedCoin, int wantedPWR)
    {
        // Dictionary<char, int> DicItemSetup = new Dictionary<char, int>();

        int numObs = 0, numCoin = 0, numPowerUp = 0, numEmpty = 0;
        for (int i = 0; i < Track.Count; i++)
        {
            for (int j = 0; j < trackXLine; j++)
            {
                switch (Track[i][j])
                {
                    case '*':
                        numCoin++;
                        break;
                    case '/':
                        numObs++;
                        break;
                    case '+':
                        numPowerUp++;
                        break;
                    default:
                        numEmpty++;
                        break;
                }
            }
        }
        // DicItemSetup.Add('*', numCoin / 100);
        //  DicItemSetup.Add('/', numObs / 100);
        //DicItemSetup.Add('+', numPowerUp / 100);
        // DicItemSetup.Add(' ', numEmpty / 100);
        return new int[] { System.Math.Abs(wantedObs - numObs), System.Math.Abs(wantedCoin - numCoin), System.Math.Abs(wantedPWR - numPowerUp) };

        //return System.Math.Abs(bestObs - numObs);
    }
    public List<List<char[]>> DE(List<List<char[]>> tPop, int maxEval, int wantedObs, int wantedCoin, int wantedPWR)
    {
        List<List<char[]>> temp = new List<List<char[]>>(tPop);
        List<char[]> Original;
        List<char[]> Candidate;
        List<char[]> Track1;
        List<char[]> Track2;
        List<char[]> Track3;
        int popSize = tPop.Count;
        int eval = 0;
        float F = 0.8f;
        float CR = 0.8f;
        int D = trackZLine;
        int R = 0;
        int mutationCount = 0, crCount = 0;
        char[] itemArr = new char[3] { '*', ' ', '+' };
        int counter = 0;
        while (eval < maxEval)
        {
            for (int j = 0; j < popSize; j++)
            {
                int x = rnd.Next(0, popSize);
                int a = 0, b = 0, c = 0;

                do
                {
                    a = rnd.Next(0, popSize);
                }
                while (a == x);
                do
                {
                    b = rnd.Next(0, popSize);
                }
                while (b == a || b == x);
                do
                {
                    c = rnd.Next(0, popSize);
                }
                while (c == a || c == b || c == x);

                Original = new List<char[]>();
                Original = temp[x].Select(o => (char[])o.Clone()).ToList();
                Candidate = new List<char[]>();
                Candidate = temp[x].Select(o => (char[])o.Clone()).ToList();
                Track1 = new List<char[]>();
                Track1 = temp[a].Select(o => (char[])o.Clone()).ToList();
                Track2 = new List<char[]>();
                Track2 = temp[b].Select(o => (char[])o.Clone()).ToList();
                Track3 = new List<char[]>();
                Track3 = temp[c].Select(o => (char[])o.Clone()).ToList();
                R = rnd.Next(0, D);
                double randCR = NextDouble(0f, 1f);
                double randF = NextDouble(0f, 1f);
                if (randCR > CR)
                {
                    Candidate = new List<char[]>();
                    Candidate = CombineTrack(Track1, Track2, Track3);
                    crCount++;
                }
                if (randF > F)
                {
                    mutationCount++;
                    char[] arr = new char[3];
                    arr = Candidate[R];
                    arr[rnd.Next(0, 3)] = itemArr[rnd.Next(0, 3)];
                    Candidate[R] = arr;
                }
                //int first = FitnessFunction(Original);
                //int second = FitnessFunction(Candidate);
                int[] first = FitnessFunction(Original, wantedObs, wantedCoin, wantedPWR);
                int[] second = FitnessFunction(Candidate, wantedObs, wantedCoin, wantedPWR);
                if (first[0] > second[0] || first[1] > second[1] || first[2] > second[2])
                {
                    temp[x] = Candidate;
                    for (int k = 0; k < Candidate.Count(); k++)
                    {
                        if (Candidate[k][0] == '/' && Candidate[k][1] == '/' && Candidate[k][2] == '/')
                        {
                            Candidate[k][Random.Range(0, 3)] = itemArr[Random.Range(0, 2)];
                        }
                        if (Candidate[k][0] == '*' && Candidate[k][1] == '*')
                        {
                            Candidate[k][Random.Range(0, 2)] = itemArr[1];
                        }
                        if (Candidate[k][1] == '*' && Candidate[k][2] == '*')
                        {
                            Candidate[k][Random.Range(1, 3)] = itemArr[1];
                        }
                        if (Candidate[k][0] == '*' && Candidate[k][2] == '*')
                        {
                            if (rnd.Next(0, 1) == 0)
                                Candidate[k][0] = itemArr[1];
                            else
                                Candidate[k][2] = itemArr[1];

                        }
                    }
                }

                counter++;
            }
            eval++;
        }
        Debug.Log("Mutated: " + mutationCount + "  CR : " + crCount);
        return temp;
    }
    public double NextDouble(double min, double max)
    {
        return rnd.NextDouble() * (max - min) + min;
    }

    public List<char[]> CombineTrack(List<char[]> first, List<char[]> second, List<char[]> third)
    {
        List<char[]> CRTrack = new List<char[]>();
        List<char[]> firstPart = new List<char[]>();
        List<char[]> secondPart = new List<char[]>();
        List<char[]> thirdPart = new List<char[]>();
        firstPart = GetPartOfTrack(first, 0, 33);
        secondPart = GetPartOfTrack(second, 33, 66);
        thirdPart = GetPartOfTrack(third, 66, 100);
        for (int i = 0; i < firstPart.Count; i++)
        {
            CRTrack.Add(firstPart[i]);
        }
        for (int i = 0; i < secondPart.Count; i++)
        {
            CRTrack.Add(secondPart[i]);
        }
        for (int i = 0; i < thirdPart.Count; i++)
        {
            CRTrack.Add(thirdPart[i]);
        }
        return CRTrack;
    }
    public List<char[]> GetPartOfTrack(List<char[]> track, int from, int to)
    {
        List<char[]> part = new List<char[]>();
        for (int i = from; i < to; i++)
        {
            part.Add(track[i]);
        }
        return part;
    }
}
