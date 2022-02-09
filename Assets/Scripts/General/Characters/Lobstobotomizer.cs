using System.Collections.Generic;

public class Lobstobotomizer : Boss
{
    protected override BossData SetBossData()
    {
        return new BossData(
            "Lobstobotomizer",
            10f,
            1,
            1f,
            Phase.Player,
            3,
            new List<float>{ 33f, 33f, 33f }
            ); 
    }
}
