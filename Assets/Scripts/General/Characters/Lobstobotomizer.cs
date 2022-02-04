using System.Collections.Generic;

public class Lobstobotomizer : Boss
{
    protected override BossData SetBossData()
    {
        return new BossData(
            "Lobstobotomizer",
            100f,
            1,
            1f,
            Phase.Player,
            3
            ); 
    }
}
