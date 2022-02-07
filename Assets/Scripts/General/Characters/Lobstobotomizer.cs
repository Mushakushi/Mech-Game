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
            3
            ); 
    }

    protected override void OnHealthDeplete()
    {
        base.OnHealthDeplete();
        base.OnHealthDepleteFull(); 
    }
}
