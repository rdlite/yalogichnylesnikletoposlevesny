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

    [SerializeField] private Transform _floorParentObject, _obstaclesParentObject;
    [SerializeField] private Transform _startFloorPoint;

    [SerializeField] private int _playerStartCubeOffsetByZ = 4;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;

    private List<Vector2Int> _usedCellsIDs = new List<Vector2Int>();
    private Vector2Int _playerStartPoint;

    private void Start()
    {
        _cubesData = GetComponent<EnvironmentPrefabsData>();

        if (Player == null)
        {
            Player = FindObjectOfType<PlayerComponentsManager>().gameObject;
        }

        PlacePlayer();

        SpawnOuterObstacles();
        SpawnInnerObstacles();

        SpawnFloor();

        SetCameraStartPosition();
        SetCameraScale();

        _fieldMeshSurface.BuildNavMesh();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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

                GameObject grassCubeToInstantiate = (x + y) % 2 == 0 ? GetComponent<EnvironmentPrefabsData>().GetBrightGrassPrefab() : GetComponent<EnvironmentPrefabsData>().GetDarkGrassPrefab();

                GameObject newEnvironmentCube = Instantiate(grassCubeToInstantiate);

                SetSpawnedCubePosition(
                    newEnvironmentCube, 
                    _startFloorPoint.position + new Vector3(x * _cubesData.GetCubesScale(), 0, -y * _cubesData.GetCubesScale()), 
                    Quaternion.identity, 
                    _floorParentObject);
            }
        }
    }

    private void SpawnOuterObstacles()
    {
        GameObject stonePrefab = GetComponent<EnvironmentPrefabsData>().GetStoneObstalcePrefab();

        for (int x = 0; x < _fieldLengthX; x++)
        {
            GameObject newEnvironmentCubeUp = Instantiate(stonePrefab);

            SetSpawnedCubePosition(
                newEnvironmentCubeUp, 
                new Vector3(_startFloorPoint.position.x + x * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(1), _startFloorPoint.position.z), 
                Quaternion.identity,
                _obstaclesParentObject);

            GameObject newEnvironmentCubeDown = Instantiate(stonePrefab);

            SetSpawnedCubePosition(
                newEnvironmentCubeDown, 
                new Vector3(_startFloorPoint.position.x + x * _cubesData.GetCubesScale(), GetGroundLevelYPositionByStartPoint(1), _startFloorPoint.position.z - _fieldLengthY * _cubesData.GetCubesScale()), 
                Quaternion.identity,
                _obstaclesParentObject);

            _usedCellsIDs.Add(new Vector2Int(x, 0));
            _usedCellsIDs.Add(new Vector2Int(x, _fieldLengthY));
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

            //_usedCellsIDs.Add(new Vector2Int(0, y));
            _usedCellsIDs.Add(new Vector2Int(_fieldLengthX, y));
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

            _usedCellsIDs.Add(new Vector2Int(newCellXIndex, newCellYIndex));
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

            _usedCellsIDs.Add(new Vector2Int(newCellXIndex, newCellYIndex));
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
        Camera.transform.parent.position = new Vector3(Camera.transform.parent.position.x, Camera.transform.parent.position.y, Camera.transform.parent.position.z);
    }

    private void SetCameraScale()
    {
        Camera.GetComponent<Camera>().orthographicSize = _fieldLengthX / 1.7f;
    }

    private float GetGroundLevelYPositionByStartPoint(int groundLevel)
    {
        return _startFloorPoint.position.y + groundLevel * _cubesData.GetCubesScale();
    }
}
