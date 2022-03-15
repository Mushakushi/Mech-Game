using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private const float dodgeDistance = 0.7f;
    private PhaseManager phaseManager;
    private List<AttackPattern> topAttackPatterns;
    private List<AttackPattern> sideAttackPatterns;
    private Vector2 currentPlayerPos;
    private static Camera currentCamera;
    private GameObject genericAttack;
    public int group;
    public float speed = 700f;

    private void Start()
    {
        genericAttack = FileUtility.LoadFile<GameObject>($"{FileUtility.prefabsPath}/Generic Attack");
    }

    public void Initialize(int group)
    {
        phaseManager = PhaseManagerAccess.GetManager(group);
        currentPlayerPos = phaseManager.player.transform.position;
        currentCamera = phaseManager.camera;
        CreateAttackPatterns();
    }
    /// <summary>
    /// Creates projectile based on AttackProjectileAsset values
    /// </summary>
    /// <param name="projectile"></param>
    public void SpawnProjectile(AttackProjectileAsset projectile)
    {
        AttackPattern attackPattern = ChooseAttackPattern(projectile.spawnDirections);
        List<Vector2> spawnPositions = attackPattern.spawnPositions;
        foreach (Vector2 spawnPos in spawnPositions)
        {
            GameObject newAttack = Instantiate(genericAttack, spawnPos, Quaternion.identity, phaseManager.transform);
            newAttack.GetComponent<GenericAttack>().SetValues(projectile, spawnPos, attackPattern.destination, speed);
        }
    }
    /// <summary>
    /// Sets pattern of projectiles
    /// </summary>
    /// <param name="attackDirections"></param>
    /// <returns></returns>
    private AttackPattern ChooseAttackPattern(AttackDirections attackDirections)
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

        List<AttackPattern> availablePatterns = new List<AttackPattern>();

        if (attackDirections.top)
            availablePatterns.AddRange(topAttackPatterns);
        if (attackDirections.sides)
            availablePatterns.AddRange(sideAttackPatterns);

        if (availablePatterns.Count > 0)
        {
            int chosenPattern = Random.Range(0, availablePatterns.Count);
            return availablePatterns[chosenPattern];
        }
        else
        {
            throw new System.Exception("No attack types selected!");
        }
    }

    #region ATTACK PATTERNS

    private void CreateAttackPatterns()
    {
        Vector2 playerPos = currentPlayerPos;

        Vector2 abovePlayer = new Vector2(playerPos.x, GetPosRelCamera(new Vector2(0f, 1.2f)).y);
        Vector2 leftPlayer = new Vector2(GetPosRelCamera(new Vector2(-0.01f, 0f)).x, playerPos.y);
        Vector2 rightPlayer = new Vector2(GetPosRelCamera(new Vector2(1.01f, 0f)).x, playerPos.y);

        List<AttackPattern> topPatterns = new List<AttackPattern>();

        topPatterns.Add(new AttackPattern(new List<Vector2> { abovePlayer + AttackPos.TopLeft, abovePlayer + AttackPos.TopRight }, AttackDestination.Down));
        topPatterns.Add(new AttackPattern(new List<Vector2> { abovePlayer + AttackPos.TopCenter }, AttackDestination.Down));
        topPatterns.Add(new AttackPattern(new List<Vector2> { abovePlayer + AttackPos.TopLeft, abovePlayer + AttackPos.TopCenter }, AttackDestination.Down));
        topPatterns.Add(new AttackPattern(new List<Vector2> { abovePlayer + AttackPos.TopCenter, abovePlayer + AttackPos.TopRight }, AttackDestination.Down));

        topAttackPatterns = topPatterns;

        List<AttackPattern> sidePatterns = new List<AttackPattern>();

        sidePatterns.Add(new AttackPattern(new List<Vector2> { leftPlayer }, AttackDestination.Right));
        sidePatterns.Add(new AttackPattern(new List<Vector2> { rightPlayer }, AttackDestination.Left));

        sideAttackPatterns = sidePatterns;
    }

    public static Vector3 GetPosRelCamera(Vector3 pos)
    {
        return currentCamera.ViewportToWorldPoint(pos);
    }

    private struct AttackPattern
    {
        public List<Vector2> spawnPositions;
        public AttackDestination destination;

        public AttackPattern(List<Vector2> spawnPositions, AttackDestination destination)
        {
            this.spawnPositions = spawnPositions;
            this.destination = destination;
        }
    }

    private static class AttackPos
    {
        public static readonly Vector2 TopLeft = new Vector3(-dodgeDistance, 0);
        public static readonly Vector2 TopCenter = new Vector3(0, 0);
        public static readonly Vector2 TopRight = new Vector3(dodgeDistance, 0);
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
