using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private InputManager _inputManager;
    private AudioManager _audioManager;
    private GameManager _gameManager;
    private Camera _camera;

    public List<GameObject> possibleTiles;
    public Vector3 handPos;
    private GameObject _currentTile;
    private GameObject _lastHoveredNode;

    public Transform dummyTile;
    private GameObject _lastDummyTile;

    private void Awake()
    {
        _inputManager = InputManager.Instance;
        _audioManager = AudioManager.Instance;
        _gameManager = GameManager.Instance;
        _camera = Camera.main;
    }

    private void Start()
    {
        _audioManager.Play("mainTheme");
        PickTile();
    }

    private void PickTile()
    {
        _currentTile = null;
        _lastHoveredNode = null;
        Destroy(_lastDummyTile);
        
        if (_gameManager.state == GameManager.State.End)
        {
            return;
        }
        
        int index = Random.Range(0, possibleTiles.Count);
        GameObject pickedTile = possibleTiles[index];

        _currentTile = Instantiate(pickedTile, handPos, Quaternion.identity, transform);
        _lastDummyTile = Instantiate(pickedTile, dummyTile);
        _lastDummyTile.GetComponent<HexTile>().enabled = false;
        foreach (Transform child in _lastDummyTile.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("CameraUI");
        }
    }

    private void Update()
    {
        if (_currentTile != null)
        {
            RaycastHit raycastHit;
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out raycastHit, 200f))
            {
                if (raycastHit.transform != null)
                {
                    OnHover(raycastHit.transform.gameObject);
                }
            }
        }
    }

    private void OnHover(GameObject hoveredObject)
    {
        if (hoveredObject.CompareTag("Node"))
        {
            _lastHoveredNode = hoveredObject;
            _currentTile.transform.position = hoveredObject.transform.position;
        }
    }

    private void OnEnable()
    {
        _inputManager.OnRightMouseClick += OnRightMouseClick;
        _inputManager.OnLeftMouseClick += OnLeftMouseClick;
    }
    
    private void OnDisable()
    {
        _inputManager.OnRightMouseClick -= OnRightMouseClick;
        _inputManager.OnLeftMouseClick += OnLeftMouseClick;
    }
    
    // Rotate current player tile
    private void OnRightMouseClick(Vector2 mousePos)
    {
        Vector3 worldPos = _camera.ScreenToWorldPoint(mousePos);

        if (_currentTile != null)
        {
            _currentTile.transform.Rotate(0, 60, 0);
            _lastDummyTile.transform.Rotate(0, 60, 0);
        }
    }
    
    // Place current player tile
    private void OnLeftMouseClick(Vector2 mousePos)
    {
        Vector3 worldPos = _camera.ScreenToWorldPoint(mousePos);

        if (_currentTile != null && _lastHoveredNode != null)
        {
            if (_currentTile.GetComponent<HexTile>().PlaceTile())
            {
                Destroy(_lastHoveredNode);
                PickTile();
            }
        }
    }
}
