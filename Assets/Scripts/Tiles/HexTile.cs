using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HexTile : MonoBehaviour
{
    public float radius = 0.25f;
    public LayerMask biomeNodeLayerMask;
    
    public GameObject nodesParent;
    public List<Node> nodes;
    public List<GameObject> biomeNodes;

    private GameManager _gameManager;
    private AudioManager _audioManager;

    public GameObject popupScorePrefab;

    public MeshRenderer tileRenderer;
    private List<Material> _materials = new List<Material>();

    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _gameManager = GameManager.Instance;
        
        _materials.AddRange(tileRenderer.materials);
        if (!nodesParent.activeSelf)
        {
            ChangeMaterialsTransparency(0.6f);
        }
    }

    private void ChangeMaterialsTransparency(float newAlpha)
    {
        foreach (Material material in _materials)
        {
            if (newAlpha < 1)
            {
                ShaderUtils.ChangeRenderMode(material, ShaderUtils.BlendMode.Transparent);
            }
            else
            {
                ShaderUtils.ChangeRenderMode(material, ShaderUtils.BlendMode.Opaque);
            }
            material.color = new Color(material.color.r, material.color.g, material.color.b, newAlpha);
        }
    }

    public bool PlaceTile()
    {
        if (CheckBiomes() == false)
        {
            return false;
        }
        
        ChangeMaterialsTransparency(1f);
        nodesParent.SetActive(true);
        foreach (Node node in nodes)
        {
            node.CheckOverlappingTileCollision();
        }
        
        PlayPlaceEffect();
        return true;
    }

    private bool CheckBiomes()
    {
        int score = 1;
        foreach (GameObject node in biomeNodes)
        {
            Collider[] colliders = Physics.OverlapSphere(node.transform.position, radius, biomeNodeLayerMask);

            if (colliders.Length == 2)
            {
                BiomeNode biomeNode1 = colliders[0].GetComponent<BiomeNode>();
                BiomeNode biomeNode2 = colliders[1].GetComponent<BiomeNode>();
                if (biomeNode1.type == biomeNode2.type)
                {
                    Debug.Log("Corresponding " + biomeNode1.type + " biome: " + colliders[0].gameObject.name + " " + colliders[1].gameObject.name);
                    score += 2;
                } else if (biomeNode1.requirement || biomeNode2.requirement)
                {
                    Debug.LogWarning("Incompatible tiles");
                    return false;
                }
            }
        }

        Vector3 popupPosition = Camera.main.WorldToScreenPoint(transform.position);
        GameObject scorePopup = Instantiate(popupScorePrefab, transform);
        TextMeshProUGUI text = scorePopup.GetComponentInChildren<TextMeshProUGUI>();
        text.transform.position = popupPosition;
        text.text = "+" + score;
        _gameManager.IncreaseScore(score);
        _gameManager.DecreaseTiles(1);
        return true;
    }

    private void PlayPlaceEffect()
    {
        int index = Random.Range(1, 4);
        _audioManager.PlayEffect("place"+index);
    }
}
