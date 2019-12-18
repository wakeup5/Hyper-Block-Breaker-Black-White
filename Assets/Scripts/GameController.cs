using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Shooter shooter;

    [SerializeField]
    private BlockManager blockManager;

    [SerializeField]
    private Floor floor;

    [Header("UIs")]
    [SerializeField]
    private SpriteRenderer whiteSprite;

    [SerializeField]
    private SpriteRenderer blackSprite;

    [SerializeField]
    private TextMesh powerText;

    [SerializeField]
    private TextMesh shotCountText;

    [SerializeField]
    private TextMesh stageText;

    [SerializeField]
    private TextMesh failedText;

    [SerializeField]
    private LineRenderer lineRenderer;

    private bool hasControl = false;
    private int stage = 0;
    private int shotCount = 1;
    private int damage = 1;

    private Vector2? downPos;
    private Vector2? dragPos;
    private Vector2? dir;

    private void Start()
    {
        powerText.text = damage + " Power";
        shotCountText.text = "Shot " + shotCount;
        stageText.text = "";
        failedText.text = "";

        StartCoroutine(StateRoutine());

        Item.OnHit += Item_OnHit;
    }

    private void OnDestroy()
    {
        Item.OnHit -= Item_OnHit;
    }

    private void Item_OnHit(Item item)
    {
        switch (item.type)
        {
            case ItemType.IncreasePower:
                damage += item.value;
                powerText.text = damage + " Power";
                break;
            case ItemType.IncreaseShots:
                shotCount += item.value;
                shotCountText.text = "Shot " + shotCount;
                break;
        }
    }

    private void Update()
    {
        if (hasControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                downPos = Input.mousePosition;
            }

            if (downPos.HasValue && Input.GetMouseButton(0))
            {
                dragPos = Input.mousePosition;

                int wallsLayer = LayerMask.GetMask("Walls");
                List<Vector3> positions = new List<Vector3>();

                RaycastHit2D hit;
                Vector3 dir = (dragPos.Value - downPos.Value).normalized;
                Vector3 pos = shooter.transform.position;

                positions.Add(pos);

                for (int i = 0; i < 2; i++)
                {
                    hit = Physics2D.Raycast(pos, dir, float.MaxValue, wallsLayer);

                    pos = hit.point + hit.normal * 0.005f;
                    dir = Vector2.Reflect(dir, hit.normal);

                    positions.Add(pos);
                }

                lineRenderer.positionCount = positions.Count;
                lineRenderer.SetPositions(positions.ToArray());
            }

            if (downPos.HasValue &&
                dragPos.HasValue &&
                Input.GetMouseButtonUp(0))
            {
                lineRenderer.positionCount = 0;
                dir = (dragPos.Value - downPos.Value).normalized;
            }
        }

    }

    private IEnumerator StateRoutine()
    {
        Vector2 pos = shooter.transform.position;

        while (true)
        {
            stage++;
            stageText.text = stage + "\nStage";
            ShotType type = (ShotType)(stage % 2);

            // initialize
            downPos = null;
            dir = null;

            // Move shooter
            shooter.transform.position = pos;

            // Move and generate blocks
            yield return blockManager.MoveAndGenerate((stage / 4) + 1, stage % 17 == 8, stage % 10 == 5);

            // Game failed.
            if (blockManager.MinPosition() < 1f)
            {
                break;
            }

            // Get direction
            blackSprite.gameObject.SetActive(type == ShotType.Black);
            whiteSprite.gameObject.SetActive(type == ShotType.White);

            hasControl = true;
            yield return new WaitUntil(() => dir.HasValue);
            hasControl = false;

            // Shot and record floor
            floor.BeginHitRecord();

            yield return shooter.Shot(dir.Value, damage, shotCount, type);
            yield return new WaitUntil(() => ElectricBall.Actives.Count == 0);

            pos = (Vector3)floor.EndHitRecord() + new Vector3(0f, 0.2f, 0f);

            blackSprite.gameObject.SetActive(false);
            whiteSprite.gameObject.SetActive(false);

            yield return new WaitForSeconds(1f);
        }

        // 
        failedText.text = "You're Failed.";
    }
}
