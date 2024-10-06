using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    [field: SerializeField] public PlayerController playerController { get; protected set; }

    public Enemy PREFAB_DEBUG;
    #region Movement Settings
    [field:Space][field: Header("Movement Settings")]
    [field: SerializeField] public BoidsSettings PlayerMovementSettings { get; protected set; }
    [field: SerializeField] public BoidsSettings EnemyMovementSettings { get; protected set; }
    [field: SerializeField] public BoidsSettings ProjectileMovementSettings { get; protected set; }
    #endregion

    private Enemy[] enemies;

    private void Awake()
    {
        enemies = new Enemy[20];
        for (i = 0; i < 20; i++)
        {
            float angle = i * Mathf.PI * 2 / 20;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 10f;
            enemies[i] = (Instantiate(PREFAB_DEBUG, position, Quaternion.identity));
        }
    }
    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        MoveEnemies(enemies, Time.fixedDeltaTime);
        MovePlayer(playerController,enemies, Time.fixedDeltaTime);
    }

    #region Move
    #region Distributed variables (useless optimization)
    IMovable _temp;
    float _tempDistance;
    int _tempAffectedUnits;
    Vector2 _tempOtherToCurrent;
    Vector2 _tempAvoidanceVelocity,_tempAlignVelocity, _tempCohesionVelocity, _tempFollowVelocity,_test;
    int i,j,_tempUnitsCount;
    #endregion
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MoveEnemies<T>(T[] units, in float delta) where T : IMovable
    {
        _tempUnitsCount = units.Length;
        for (i = 0; i < _tempUnitsCount; i++)
        {
            _temp = units[i];
            _temp.ApplyNextVelocityAndMove(delta);
            if (_temp.Health <= 0) continue;
            _tempAffectedUnits = 0;
            _tempAvoidanceVelocity = Vector2.zero;
            _tempAlignVelocity = Vector2.zero;
            _tempCohesionVelocity = Vector2.zero;
            _tempFollowVelocity = ((Vector2.Distance(playerController.Position, _temp.Position)>1f)?(playerController.Position - _temp.Position): Vector2.zero) * EnemyMovementSettings.FollowWeight;
            _test = ((Vector2.Distance(playerController.Position, _temp.Position) <= (playerController.Radius + _temp.Radius)) ? (_temp.Position - playerController.Position) * EnemyMovementSettings.PlayerAvoidanceWeight : Vector2.zero); 
            for (j = 0; j < _tempUnitsCount; j++)
            {
                if (i == j) continue;

                _tempOtherToCurrent = _temp.Position - units[j].Position;
                _tempDistance = _tempOtherToCurrent.magnitude;
                if (_tempDistance >= EnemyMovementSettings.AffectRange) continue;

                _tempAlignVelocity += units[j].Velocity;
                _tempAvoidanceVelocity += _tempOtherToCurrent.normalized / Mathf.Max(_tempDistance - _temp.Radius - units[j].Radius, 0.01f);
                _tempCohesionVelocity += units[j].Position;
                _tempAffectedUnits++;

            }

            if (_tempAffectedUnits > 0)
            {
                _tempAvoidanceVelocity /= _tempAffectedUnits;
                _tempAvoidanceVelocity = _tempAvoidanceVelocity * EnemyMovementSettings.SeparationWeight;

                _tempAlignVelocity /= _tempAffectedUnits;
                _tempAlignVelocity = _tempAlignVelocity * EnemyMovementSettings.AlignmentWeight;

                _tempCohesionVelocity = ((_tempCohesionVelocity / _tempAffectedUnits) - _temp.Position) * EnemyMovementSettings.CohesionWeight;
            }

            _temp.NextVelocity = Vector2.Lerp(_temp.NextVelocity , Vector2.ClampMagnitude((_tempFollowVelocity + _test +  _tempAvoidanceVelocity + _tempAlignVelocity + _tempCohesionVelocity),2f),0.3f
                );
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void MovePlayer(PlayerController player, in IMovable[] enemies,in float delta)
    {
        player.ApplyNextVelocityAndMove(delta);
        _tempUnitsCount = enemies.Length;
        if (player.Health <= 0) return;
        _tempAvoidanceVelocity = Vector2.zero;
        _tempAffectedUnits = 0;
        _tempFollowVelocity = player.Speed * InputManager.Current.InputDirection;
        for (j = 0; j < _tempUnitsCount; j++)
        {
            _tempOtherToCurrent = player.Position - enemies[j].Position;
            _tempDistance = _tempOtherToCurrent.magnitude;
            if (_tempDistance >= PlayerMovementSettings.AffectRange) continue;
            _tempAvoidanceVelocity += _tempOtherToCurrent.normalized / Mathf.Max(_tempDistance - _temp.Radius - enemies[j].Radius, 0.01f);
            _tempAffectedUnits++;

        }

        if (_tempAffectedUnits > 0)
        {
            _tempAvoidanceVelocity /= _tempAffectedUnits;
            _tempAvoidanceVelocity = _tempAvoidanceVelocity.normalized * PlayerMovementSettings.SeparationWeight;
        }

        player.NextVelocity = (_tempFollowVelocity + _tempAvoidanceVelocity).normalized;
    }

    //Move Projectiles
    #endregion
}


[Serializable]
public struct BoidsSettings
{
    [Range(1f,20f)] public float AffectRange;
    [Space]
    [Range(0f,2f)] public float FollowWeight;
    [Range(0f,2f)] public float SeparationWeight;
    [Range(0f,2f)] public float AlignmentWeight;
    [Range(0f,2f)] public float CohesionWeight;
    [Range(0f,4f)] public float PlayerAvoidanceWeight;
}