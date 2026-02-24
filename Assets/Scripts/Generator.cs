using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject grassCube;
    public GameObject sandCube;
    public GameObject mountainCube;
    public GameObject waterCube;

    [Header("World")]
    public int waterLevel = 6;
    public int width = 100;
    public int length = 100;
    public int seed = 12345;

    [Header("Noise Settings")]
    public float biomeScale = 80f;   //Tamaño de biomas
    public float terrainScale = 20f; //Detalle del terreno

    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                //Valor continuo de bioma
                float biomeValue = Mathf.PerlinNoise(
                    (x + seed) / biomeScale,
                    (z + seed) / biomeScale
                );

                //Calculamos SIEMPRE todas las alturas
                int desertHeight = Mathf.RoundToInt(
                    Mathf.PerlinNoise((x + seed) / terrainScale,
                                      (z + seed) / terrainScale) * 4 + 3
                );

                int plainsHeight = Mathf.RoundToInt(
                    Mathf.PerlinNoise((x + seed) / terrainScale,
                                      (z + seed) / terrainScale) * 8 + 5
                );

                int mountainHeight = Mathf.RoundToInt(
                    Mathf.PerlinNoise((x + seed) / (terrainScale * 0.6f),
                                      (z + seed) / (terrainScale * 0.6f)) * 15 + 10
                );

                //Mezcla suave entre biomas
                float blendFactor = biomeValue * 2f;

                int height;

                if (biomeValue < 0.5f)
                {
                    //Mezcla desierto → llanura
                    height = Mathf.RoundToInt(
                        Mathf.Lerp(desertHeight, plainsHeight, blendFactor)
                    );
                }
                else
                {
                    //Mezcla llanura → montaña
                    height = Mathf.RoundToInt(
                        Mathf.Lerp(plainsHeight, mountainHeight, blendFactor - 1f)
                    );
                }

                //Generar terreno
                for (int y = 0; y < height; y++)
                {
                    GameObject block;

                    if (y == height - 1)
                    {
                        // Bloque superficial según bioma dominante
                        if (biomeValue < 0.33f)
                            block = sandCube;
                        else if (biomeValue < 0.66f)
                            block = grassCube;
                        else
                            block = mountainCube;
                    }
                    else
                    {
                        block = mountainCube;
                    }

                    Instantiate(block, new Vector3(x, y, z), Quaternion.identity);
                }

                //Generar agua si el terreno está por debajo del nivel
                if (height < waterLevel)
                {
                    for (int y = height; y < waterLevel; y++)
                    {
                        Instantiate(waterCube, new Vector3(x, y, z), Quaternion.identity);
                    }
                }
            }
        }
    }
}