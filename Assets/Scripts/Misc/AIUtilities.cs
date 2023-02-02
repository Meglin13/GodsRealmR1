using UnityEngine;

public static class AIUtilities
{
    //Слои в именах
    public static string CharsLayerName = "CharLayer";
    public static string EnemyLayerName = "EnemyLayer";
    public static string GroundLayerName = "GroundLayer";
    public static string InteractLayerName = "InteractLayer";
    public static string MiniMapLayerName = "MiniMapLayer";
    //Слои в цифрах
    public static int CharsLayer = LayerMask.NameToLayer(CharsLayerName);
    public static int EnemyLayer = LayerMask.NameToLayer(EnemyLayerName);
    public static int GroundLayer = LayerMask.NameToLayer(GroundLayerName);
    public static int InteractLayer = LayerMask.NameToLayer(InteractLayerName);
    public static int MiniMapLayer = LayerMask.NameToLayer(MiniMapLayerName);

    //Теги
    public static string CharsTag = "Character";
    public static string EnemyTag = "Enemy";

    public static GameObject FindNearestEntity(Transform transform, string tag)
    {
        GameObject ClosestEntity = null;
        float DistanceToClosestEntity = Mathf.Infinity;

        GameObject[] entities = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject entity in entities)
        {
            float Distance = (entity.transform.position - transform.position).sqrMagnitude;

            if (DistanceToClosestEntity > Distance)
            {
                DistanceToClosestEntity = Distance;
                ClosestEntity = entity;
            }
        }

        Color lineColor = transform.gameObject.TryGetComponent(out CharacterScript chara) ? Color.green : Color.red;
        Debug.DrawLine(transform.position, ClosestEntity.transform.position, lineColor);

        return ClosestEntity;
    }

    public static bool IsCertainEntityInRadius(GameObject gameObject, GameObject certainGameObject, float radius)
    {
        return (certainGameObject.transform.position - gameObject.transform.position).sqrMagnitude < 3 * radius;
    }
    public static GameObject FindPlayer()
    {
        GameObject Player = null;

        CharacterScript[] Characters = GameObject.FindObjectsOfType<CharacterScript>();

        foreach (var item in Characters)
        {
            if (item.IsActive)
                Player = item.gameObject;
        }
        return Player;
    }
}