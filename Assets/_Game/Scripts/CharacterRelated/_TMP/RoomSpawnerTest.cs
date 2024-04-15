using UnityEngine;

namespace _Game.Scripts.CharacterRelated._TMP
{
    public class RoomSpawnerTest : MonoBehaviour
    {
        [SerializeField] private Grid globalGrid;
        [SerializeField] private Grid roomGrid;
        [SerializeField] private GameObject[] roomsPrefabs;

        private void Start()
        {
            GenerateRooms();
        }

        private void GenerateRooms()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var roomPos = globalGrid.CellToWorld(new Vector3Int(x, y, 0));
                    var room = Instantiate(
                        roomsPrefabs[Random.Range(0, roomsPrefabs.Length)],
                        roomPos,
                        Quaternion.identity,
                        roomGrid.transform);
                    
                }
            }
        }
    }
}