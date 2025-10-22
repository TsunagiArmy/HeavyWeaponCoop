using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    [SerializeField]
    Sprite[] idleSprites;

    [SerializeField]
    Sprite[] runSprites;

    int currentFrame = 0;

    [SerializeField]
    float targetFrameRate = 0.1f;

    float frameTimer;

    [SerializeField]
    MeshRenderer meshRenderer;

    bool isRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        frameTimer = targetFrameRate;

        //var atlasTex = SpriteUtility.GetSpriteTexture(idleSprites[0], true);
        var atlasTex = idleSprites[0].texture;

        meshRenderer.material.SetTexture("_BaseMap", atlasTex);

        //Vector2[] uvs = SpriteUtility.GetSpriteUVs(idleSprites[0], true);
        Vector2[] uvs = idleSprites[0].uv;

        Vector4 uvRect = new Vector4(uvs[3].x, uvs[3].y, uvs[0].x, uvs[0].y);

        meshRenderer.material.SetVector("_SpriteUV", uvRect);
    }

    // Update is called once per frame
    void Update()
    {
        if (frameTimer > 0)
        {
            frameTimer -= Time.deltaTime;
            return;
        }

        frameTimer = targetFrameRate;

        if (Input.GetKey(KeyCode.Space))
        {
            if (!isRunning)
            {
                isRunning = true;
                currentFrame = 0;
            }

            currentFrame = (currentFrame + 1) % runSprites.Length;

            //Vector2[] uvs = SpriteUtility.GetSpriteUVs(runSprites[currentFrame], true);
            Vector2[] uvs = runSprites[currentFrame].uv;

            Vector4 uvRect = new Vector4(uvs[3].x, uvs[3].y, uvs[0].x, uvs[0].y);

            meshRenderer.material.SetVector("_SpriteUV", uvRect);
        }
        else
        {
            if (isRunning)
            {
                isRunning = false;
                currentFrame = 0;
            }

            currentFrame = (currentFrame + 1) % idleSprites.Length;

            //Vector2[] uvs = SpriteUtility.GetSpriteUVs(idleSprites[currentFrame], true);
            Vector2[] uvs = idleSprites[currentFrame].uv;

            Vector4 uvRect = new Vector4(uvs[3].x, uvs[3].y, uvs[0].x, uvs[0].y);

            meshRenderer.material.SetVector("_SpriteUV", uvRect);
        }
    }
}
