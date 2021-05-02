using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnvironmentPrefabsData))]
public class FieldSpawner : MonoBehaviour
{
    private EnvironmentPrefabsData _cubesData;
    [SerializeField] private NavMeshSurface _fieldMeshSurface;

    [SerializeField] private int _fieldLengthX = 4, _fieldLengthY = 20;

    [SerializeField] private int _stoneObstaclesAmount = 10, _waterObstaclesAmount;
    [SerializeField] private EnemiesToSpawnAmount _enemiesToSpawnData;

    [SerializeField] private Transform _floorParentObject, _obstaclesParentObject, _doorParent;
    [SerializeField] private Transform _startFloorPoint;

    [SerializeField] private GameObject _endLevelTrigger;

    [SerializeField] private int _playerStartCubeOffsetByZ = 4;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;

    [SerializeField] private Transform _enemiesParent;
    [SerializeField] private GameObject[] _enemiesToSpawn;

    private List<Vector2Int> _usedCellsIDs = new List<Vector2Int>();
    private Vector2Int _playerStartPoint;

    private List<Enemy> _spawnedEnemies = new List<Enemy>();

    private void Start()
    {
        _cubesData = GetComponent<EnvironmentPrefabsData>();

        if (Player == null)
        {
            Player = FindObjectOfType<PlayerComponentsManager>().gameObject;
        }

        PlacePlayer();

        SpawnDoor();

        SpawnOuterObstacles();
        SpawnInnerObstacles();

        SpawnFloor();

        SetCameraStartPosition();
        SetCameraScale();

        SpawnEnemies();

        _fieldMeshSurface.BuildNavMesh();

        StartLevelEvents.Instance.OnGameStarted += InitAllEnemies;

        EndLevelEvents.Instance.OnAllEnemiesKilled += SetActiveFalseDoorParent;
        EndLevelEvents.Instance.OnAllEnemiesKilled += SetActiveEndLevelTrigger;
        EndLevelEvents.Instance.OnAllEnemiesKilled += IterateEnemiesTSpawn;
    }

    private void SpawnDoor()
    {
        for (int x = _fieldLengthX / 2 - 1; x < _fieldLengthX / 2 + 2; x++)
        {
            // сами двери
            GameObject newEnvironmentCube = Instantiate(_cubesData.GetDoorObstaclePrefab());

            SetSpawnedCubePosition(
                newEnvironmentCube,
                new Vector3(_startFloorPoint.position.x + x * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(1), _startFloorPoint.position.z),
                Quaternion.identity,
                _doorParent);


            // пол под ним
            GameObject floorCubeUnderDoor = Instantiate(GetFloorCubeByMode(x));

            SetSpawnedCubePosition(
                floorCubeUnderDoor,
                new Vector3(_startFloorPoint.position.x + x * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(0), _startFloorPoint.position.z),
                Quaternion.identity,
                _floorParentObject);

            SetCellUsed(x, 0);
        }

        // установка триггера на двери
        _endLevelTrigger.transform.position = new Vector3(_startFloorPoint.position.x + _fieldLengthX / 2 * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(1), _startFloorPoint.position.z + _cubesData.GetCubesScale() / 2);
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < _enemiesToSpawnData.EnemiesToSpawnCount; i++)
        {
            int x = 0, y = 0;

            // установка рандомной позиции на 2/3 поля по высоте
            SetUnusedCells(ref x, ref y, _fieldLengthX, (int)(_fieldLengthY * (2f / 3f)));

            SetCellUsed(x, y);

            GameObject newEnemy = Instantiate(_enemiesToSpawn[UnityEngine.Random.Range(0, _enemiesToSpawn.Length)]);
            SetSpawnedCubePosition(newEnemy, new Vector3(
                _startFloorPoint.position.x + x * _cubesData.GetCubesScale(),
                GetGroundLevelYPositionByStartPoint(1),
                _startFloorPoint.position.z + -y * _cubesData.GetCubesScale()), Quaternion.identity, _enemiesParent);

            _spawnedEnemies.Add(newEnemy.GetComponent<Enemy>());
        }
    }

    private void InitAllEnemies()
    {
        foreach (Enemy item in _spawnedEnemies)
        {
            item.InitEnemy(Player.transform);
        }

        _spawnedEnemies.Clear();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    private void SpawnFloor()
    {
        for (int x = 0; x < _fieldLengthX; x++)
        {
            for (int y = 0; y < _fieldLengthY; y++)
            {
                if (_usedCellsIDs.Contains(new Vector2Int(x, y)))
                {
                    continue;
                }

                GameObject grassCubeToInstantiate = GetFloorCubeByMode(x + y);

                GameObject newEnvironmentCube = Instantiate(grassCubeToInstantiate);

                SetSpawnedCubePosition(
                    newEnvironmentCube, 
                    _startFloorPoint.position + new Vector3(x * _cubesData.GetCubesScale(), 0, -y * _cubesData.GetCubesScale()), 
                    Quaternion.identity, 
                    _floorParentObject);
            }
        }
    }

    private GameObject GetFloorCubeByMode(int id)
    {
        return id % 2 == 0 ? GetComponent<EnvironmentPrefabsData>().GetBrightGrassPrefab() : GetComponent<EnvironmentPrefabsData>().GetDarkGrassPrefab();
    }

    private void SpawnOuterObstacles()
    {
        GameObject stonePrefab = GetComponent<EnvironmentPrefabsData>().GetStoneObstalcePrefab();

        // спавн верхней стены с пробелом под двери
        {
            for (int x = 0; x < _fieldLengthX / 2 - 1; x++)
            {
                GameObject newEnvironmentCubeUp = Instantiate(stonePrefab);

                SetSpawnedCubePosition(
                    newEnvironmentCubeUp,
                    new Vector3(_startFloorPoint.position.x + x * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(1), _startFloorPoint.position.z),
                    Quaternion.identity,
                    _obstaclesParentObject);

                SetCellUsed(x, 0);
            }

            for (int x = _fieldLengthX / 2 + 2; x < _fieldLengthX; x++)
            {
                GameObject newEnvironmentCubeUp = Instantiate(stonePrefab);

                SetSpawnedCubePosition(
                    newEnvironmentCubeUp,
                    new Vector3(_startFloorPoint.position.x + x * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(1), _startFloorPoint.position.z),
                    Quaternion.identity,
                    _obstaclesParentObject);

                SetCellUsed(x, 0);
            }
        }

        for (int x = 0; x < _fieldLengthX; x++)
        {
            GameObject newEnvironmentCubeDown = Instantiate(stonePrefab);

            SetSpawnedCubePosition(
                newEnvironmentCubeDown, 
                new Vector3(_startFloorPoint.position.x + x * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(1), _startFloorPoint.position.z - _fieldLengthY * _cubesData.GetCubesScale()), 
                Quaternion.identity,
                _obstaclesParentObject);

            SetCellUsed(x, _fieldLengthY);
        }

        for (int y = 0; y < _fieldLengthY + 1; y++)
        {
            GameObject newEnvironmentCubeLeft = Instantiate(stonePrefab);

            SetSpawnedCubePosition(
                newEnvironmentCubeLeft,
                new Vector3(_startFloorPoint.position.x - (1 * _cubesData.GetCubesScale()), GetGroundLevelYPositionByStartPoint(1), _startFloorPoint.position.z - y * _cubesData.GetCubesScale()),
                Quaternion.identity,
                _obstaclesParentObject);

            GameObject newEnvironmentCubeRight = Instantiate(stonePrefab);

            SetSpawnedCubePosition(
                newEnvironmentCubeRight,
                new Vector3(_startFloorPoint.position.x + _fieldLengthX * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(1), _startFloorPoint.position.z - y * _cubesData.GetCubesScale()),
                Quaternion.identity,
                _obstaclesParentObject);

            SetCellUsed(_fieldLengthX, y);
        }
    }

    private void SetSpawnedCubePosition(GameObject cube, Vector3 position, Quaternion rotation, Transform parent)
    {
        cube.transform.position = position;
        cube.transform.rotation = rotation;
        cube.transform.SetParent(parent);
    }

    private void SpawnInnerObstacles()
    {
        // генерация препятствий происходит случайно, в тз указано, что поле должно быть заданного размера, что, возможно гоорит о его изменяемости
        // в archero можно заметить, что препятствия поддаются шаблонам, а значит сделаны заранее, так как размеры поля там всегда константны
        // к моей же игре, такая тактика с шаблонами, очевидно, не сработает, потому генерация и происходит случайно

        // спавн камней
        for (int i = 0; i < _stoneObstaclesAmount; i++)
        {
            int newCellXIndex = 0;
            int newCellYIndex = 0;

            SetUnusedCells(ref newCellXIndex, ref newCellYIndex);

            GameObject newEnvironmentCubeRight = Instantiate(_cubesData.GetStoneObstalcePrefab());

            SetSpawnedCubePosition(
                newEnvironmentCubeRight,
                new Vector3(_startFloorPoint.position.x + newCellXIndex * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(1), _startFloorPoint.position.z - newCellYIndex * _cubesData.GetCubesScale()),
                Quaternion.identity,
                _obstaclesParentObject);

            SetCellUsed(newCellXIndex, newCellYIndex);
        }

        // спавн воды
        for (int i = 0; i < _waterObstaclesAmount; i++)
        {
            int newCellXIndex = 0;
            int newCellYIndex = 0;

            SetUnusedCells(ref newCellXIndex, ref newCellYIndex);

            GameObject newEnvironmentCubeRight = Instantiate(_cubesData.GetWaterObstalcePrefab());

            SetSpawnedCubePosition(
                newEnvironmentCubeRight,
                new Vector3(_startFloorPoint.position.x + newCellXIndex * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(0), _startFloorPoint.position.z - newCellYIndex * _cubesData.GetCubesScale()),
                Quaternion.identity,
                _obstaclesParentObject);

            SetCellUsed(newCellXIndex, newCellYIndex);
        }
    }

    private void SetUnusedCells(ref int newCellXIndex, ref int newCellYIndex)
    {
        do
        {
            newCellXIndex = UnityEngine.Random.Range(0, _fieldLengthX);
            newCellYIndex = UnityEngine.Random.Range(0, _fieldLengthY);
        } while (_usedCellsIDs.Contains(new Vector2Int(newCellXIndex, newCellYIndex)) || new Vector2Int(newCellXIndex, newCellYIndex) == _playerStartPoint);
    }

    private void SetUnusedCells(ref int newCellXIndex, ref int newCellYIndex, int fieldbyX, int fieldbyY)
    {
        do
        {
            newCellXIndex = UnityEngine.Random.Range(0, fieldbyX);
            newCellYIndex = UnityEngine.Random.Range(0, fieldbyY);
        } while (_usedCellsIDs.Contains(new Vector2Int(newCellXIndex, newCellYIndex)) || new Vector2Int(newCellXIndex, newCellYIndex) == _playerStartPoint);
    }

    private void PlacePlayer()
    {
        float xPos = _startFloorPoint.position.x + (_fieldLengthX * _cubesData.GetCubesScale()) / 2f - _cubesData.GetCubesScale() / 2f;
        float yPos = GetGroundLevelYPositionByStartPoint(1);
        float zPos = _startFloorPoint.position.z - (_fieldLengthY - _playerStartCubeOffsetByZ) * _cubesData.GetCubesScale();

        Player.transform.position = new Vector3(xPos, yPos, zPos);

        _playerStartPoint = new Vector2Int(_fieldLengthX / 2, _fieldLengthY - _playerStartCubeOffsetByZ);
    }

    private void SetCameraStartPosition()
    {
        Camera.transform.parent.position = new Vector3(Player.transform.position.x, Camera.transform.parent.position.y, Player.transform.position.z);
    }

    private void SetCameraScale()
    {
        Camera.GetComponent<Camera>().orthographicSize = _fieldLengthX / 1.7f;
    }

    private float GetGroundLevelYPositionByStartPoint(int groundLevel)
    {
        return _startFloorPoint.position.y + groundLevel * _cubesData.GetCubesScale();
    }

    private void SetCellUsed(int x, int y)
    {
        _usedCellsIDs.Add(new Vector2Int(x, y));
    }

    public void SetActiveFalseDoorParent()
    {
        _doorParent.gameObject.SetActive(false);
    }

    public void SetActiveEndLevelTrigger()
    {
        _endLevelTrigger.SetActive(true);
    }

    public void IterateEnemiesTSpawn()
    {
        _enemiesToSpawnData.EnemiesToSpawnCount++;
    }
}