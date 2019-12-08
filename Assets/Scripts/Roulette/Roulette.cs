using System;

public class Roulette
{
    private const int radius = 1; // Because of using transform it is no use of radius value
    private readonly int sectorsCount;

    public Roulette(int sectorsCount)
    {
        this.sectorsCount = sectorsCount;
    }

    private int RandomSpinCount()
    {
        System.Random rand = new System.Random();
        return rand.Next(5, 15);
    }

    private float CircleLength(float radius)
    {
        return 2 * (float)Math.PI * radius;
    }

    private float DistanceToSector(int sector)
    {
        float sectorLength = CircleLength(radius) / sectorsCount;
        return sectorLength * sector - (UnityEngine.Random.Range(sectorLength / 50, (sectorLength / 50) * 49)); 
    }

    private float RandomRouletteDistance(int sector)
    {
        return CircleLength(radius) * RandomSpinCount() + DistanceToSector(sector);
    }

    public float Spin(int sector, bool right)
    {
        if (right)
            return -(RandomRouletteDistance(sectorsCount - sector + 1) * 180) / (radius * (float)Math.PI);

        return (RandomRouletteDistance(sector) * 180) / (radius * (float)Math.PI);
    }
}