//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class EnemyManager_old : MonoBehaviour
//{
//    private static EnemyManager_old _instance = null;

//    public int UnitCount = 300;
//    public float CircleRadius = 16f;
//    public Vector4 Weights;

//    public Enemy EnemyPrefab;

//    public Transform Target;

//    private Vector2 _targetPosition;

//    private List<Enemy> _enemies;
//    private int _activeEnemiesCount;
//    private void Awake()
//    {
//        _instance = this;
//        _enemies = new List<Enemy>();
//    }

//    private void Start()
//    {
//        for (int i = 0; i < UnitCount; i++)
//        {
//            float angle = i * Mathf.PI * 2 / UnitCount;
//            Vector3 position = new Vector3(Mathf.Cos(angle),0, Mathf.Sin(angle)) * CircleRadius; 
//            _enemies.Add(Instantiate(EnemyPrefab, position, Quaternion.identity)); 
//        }
//    }
//    void FixedUpdate()
//    {
//        _targetPosition = new Vector2(Target.position.x, Target.position.z);
//        _activeEnemiesCount = _enemies.Count();
//        for (int i = 0;i < _activeEnemiesCount; i++)
//        {
//            if (_enemies[i].Health < 0) continue;

//            _enemies[i].ApplyTargetDirAndMove(Time.fixedDeltaTime);
//            int affectedNeighbors = 0;
//            Vector2 GroupVelocity = Vector2.zero;
//            Vector2 GroupCenter = Vector2.zero;
//            Vector2 avoidanceForce = Vector2.zero;
//            for (int j = 0; j < _activeEnemiesCount; j++)
//            {
//                if (j == i) continue;
//                float dist = (_enemies[j].Position - _enemies[i].Position).magnitude;
//                if (dist > 7f) continue;
//                else if( dist > (_enemies[j].Radius + _enemies[i].Radius))
//                {
//                    //avoidanceForce += (_enemies[i].Position + _enemies[j].Position) * Time.fixedDeltaTime;
//                }
//                else
//                {
//                    //avoidanceForce += (_enemies[i].Position - _enemies[j].Position).normalized * (dist-_enemies[i].Radius);
//                    avoidanceForce += (_enemies[i].Position - _enemies[j].Position).normalized * (dist-_enemies[i].Radius);
//                }
//                GroupVelocity += _enemies[j].Direction;
//                GroupCenter += _enemies[j].Position;
//                affectedNeighbors++;
//            }
//            if(affectedNeighbors > 0)
//            {
//                GroupVelocity /= affectedNeighbors;
//                GroupCenter /= affectedNeighbors;
//            }
//            _enemies[i].TargetDirection = ((_targetPosition - _enemies[i].Position)* Weights.x + avoidanceForce * Weights.y + GroupCenter * Weights.z + GroupVelocity * Weights.w).normalized * 2;
//        }
//    }
//}
