using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ripple_Text : MonoBehaviour
{
    public TMP_Text textComponent;
    public float speed = 2f;
    public float height = 5f;
    public float delayBetweenCharacters = 0.2f;
    public float delayAfterCompletion = 0.5f; // Delay before restarting the animation

    private float timeElapsed = 0f;
    private bool isAnimating = true;

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        while (true)
        {
            if (isAnimating)
            {
                textComponent.ForceMeshUpdate();
                TMP_TextInfo textInfo = textComponent.textInfo;

                timeElapsed += Time.deltaTime;

                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                    if (!charInfo.isVisible)
                    {
                        continue;
                    }

                    Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                    float animationOffset = Mathf.Clamp01((timeElapsed - (i * delayBetweenCharacters)) / delayBetweenCharacters);
                    float movement = Mathf.Sin(animationOffset * Mathf.PI) * height;

                    for (int j = 0; j < 4; j++)
                    {
                        Vector3 orig = verts[charInfo.vertexIndex + j];
                        verts[charInfo.vertexIndex + j] = orig + new Vector3(0, movement, 0);
                    }
                }

                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                    meshInfo.mesh.vertices = meshInfo.vertices;
                    textComponent.UpdateGeometry(meshInfo.mesh, i);
                }

                if (timeElapsed > (textInfo.characterCount * delayBetweenCharacters) + delayBetweenCharacters)
                {
                    timeElapsed = 0f;
                    isAnimating = false;
                    yield return new WaitForSeconds(delayAfterCompletion);
                    isAnimating = true;
                }
            }
            yield return null;
        }
    }
}
