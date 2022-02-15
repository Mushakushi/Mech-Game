using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private const float dodgeDistance = 0.7f;
    private static PhaseManager phaseManager;
    private List<AttackPattern> attackPatterns;
    private static Vector2 currentPlayerPos;
    private static Camera currentCamera;
    private GameObject genericAttack;
    public int group;

    private void Awake()
    {
        
    }

    private void Start()
    {
        genericAttack = (GameObject) Resources.Load("Prefabs/Generic Attack");
    }

    public void Initialize(int group)
    {
        phaseManager = PhaseManagerAccess.GetManager(group);
        currentPlayerPos = phaseManager.player.transform.position;
        currentCamera = phaseManager.camera;
        attackPatterns = CreateAttackPatterns();
    }

    public void SpawnProjectile(AttackProjectileAsset projectile)
    {
        AttackPattern attackPattern = ChooseAttackPattern(projectile.spawnDirections);
        List<Vector3> spawnPositions = attackPattern.spawnPositions;
        foreach (Vector3 pos in spawnPositions)
        {
            GameObject newAttack = Instantiate(genericAttack, pos, Quaternion.identity, phaseManager.transform);
            newAttack.transform.position = pos;
            newAttack.GetComponent<GenericAttack>().SetValues(projectile, attackPattern.destination, 0.3f);
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
        Vector2 playerPos = currentPlayerPos;

        Vector3 abovePlayer = new Vector3(playerPos.x, GetPosRelCamera(new Vector3(0f, 1.2f)).y);
        Vector3 leftPlayer = new Vector3(GetPosRelCamera(new Vector3(-1.2f, 0f)).x, playerPos.y, 0f);
        Vector3 rightPlayer = new Vector3(GetPosRelCamera(new Vector3(1.2f, 0f)).x, playerPos.y, 0f);

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

    private static Vector3 GetPosRelCamera(Vector3 pos)
    {
        return currentCamera.ViewportToWorldPoint(pos);
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
