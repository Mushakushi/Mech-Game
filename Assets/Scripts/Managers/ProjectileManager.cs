using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private const float dodgeDistance = 0.7f;
    private static PhaseManager phaseManager;
    private List<AttackPattern> attackPatterns = CreateAttackPatterns();
    private GameObject genericAttack = (GameObject) Resources.Load("Prefabs/Generic Attack");

    private void Awake()
    {
        phaseManager = gameObject.GetComponent<PhaseManager>();
    }

    public void SpawnProjectile(AttackProjectileAsset projectile)
    {
        AttackPattern attackPattern = ChooseAttackPattern(projectile.spawnDirections);
        List<Vector3> spawnPositions = attackPattern.spawnPositions;
        foreach (Vector3 pos in spawnPositions)
        {
            GameObject newAttack = Instantiate(genericAttack, pos, Quaternion.identity);
            newAttack.GetComponent<GenericAttack>().SetValues(projectile, attackPattern.destination)
        }
    }

    private AttackPattern ChooseAttackPattern(AttackDirections attackDirections) // none of this is a good way to do this i bet
    {
        // possible attack positions:
        // 
        // #   #
        //   #
        // # #
        //   # #
        // 
        // left side
        // right side

        int numAttackPositions = 0;

        if (attackDirections.top)
            numAttackPositions += 4;
        if (attackDirections.sides)
            numAttackPositions += 2;

        int chosenPos = (numAttackPositions - Random.Range(0, numAttackPositions - 1));
        return attackPatterns[chosenPos];
    }

    #region ATTACK PATTERNS

    private static List<AttackPattern> CreateAttackPatterns()
    {
        Vector3 playerPos = GetPlayerPosition();

        Vector3 abovePlayer = new Vector3(playerPos.x, 0f, 0f) + GetPosRelCamera(new Vector3(0f, 1.2f));
        Vector3 leftPlayer = new Vector3(0f, playerPos.y, 0f) + GetPosRelCamera(new Vector3(-1.2f, 0f));
        Vector3 rightPlayer = new Vector3(0f, playerPos.y, 0f) + GetPosRelCamera(new Vector3(1.2f, 0f));

        List<AttackPattern> attackPatterns = new List<AttackPattern>();

        attackPatterns.Add(new AttackPattern(new List<Vector3>(), AttackDestination.None));
        attackPatterns.Add(new AttackPattern(new List<Vector3> { abovePlayer + AttackPos.TopLeft, abovePlayer + AttackPos.TopRight }, AttackDestination.Down));
        attackPatterns.Add(new AttackPattern(new List<Vector3> { abovePlayer + AttackPos.TopCenter }, AttackDestination.Down));
        attackPatterns.Add(new AttackPattern(new List<Vector3> { abovePlayer + AttackPos.TopLeft, abovePlayer + AttackPos.TopCenter }, AttackDestination.Down));
        attackPatterns.Add(new AttackPattern(new List<Vector3> { abovePlayer + AttackPos.TopCenter, abovePlayer + AttackPos.TopRight }, AttackDestination.Down));

        attackPatterns.Add(new AttackPattern(new List<Vector3> { leftPlayer }, AttackDestination.Right));
        attackPatterns.Add(new AttackPattern(new List<Vector3> { rightPlayer }, AttackDestination.Left));

        return attackPatterns;
    }

    private static Vector3 GetPlayerPosition()
    {
        return phaseManager.player.gameObject.transform.position;
    }

    private static Vector3 GetPosRelCamera(Vector3 pos)
    {
        return phaseManager.camera.ViewportToWorldPoint(pos);
    }

    private struct AttackPattern
    {
        public List<Vector3> spawnPositions;
        public AttackDestination destination;

        public AttackPattern(List<Vector3> spawnPositions, AttackDestination destination)
        {
            this.spawnPositions = spawnPositions;
            this.destination = destination;
        }
    }

    private static class AttackPos
    {
        public static Vector3 TopLeft = new Vector3(-dodgeDistance, 0, 0);
        public static Vector3 TopCenter = new Vector3(0, 0, 0);
        public static Vector3 TopRight = new Vector3(dodgeDistance, 0, 0);
    }

    #endregion
}

public enum AttackDestination
{
    Down,
    Right,
    Left,
    None
}
