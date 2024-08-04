using System.Collections;
using UnityEngine;
using TMPro;

public class GradientTextEffect : MonoBehaviour
{
    [SerializeField] Gradient gradientText;
    [SerializeField] float gradientSpeed = 0.1f;

    private TMP_Text textComponent;
    private float totalTime;

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void Start()
    {
        StartCoroutine(AnimateVertexColors());
    }

    IEnumerator AnimateVertexColors()
    {
        textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = textComponent.textInfo;
        int currentCharacter = 0;

        Color32[] newVertexColors;
        Color32 c0 = textComponent.color;

        while(true)
        {
            int characterCount = textInfo.characterCount;

            if(characterCount == 0) 
            {
                yield return new WaitForSeconds(0.25f);
                continue;
            }

            int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

            newVertexColors = textInfo.meshInfo[materialIndex].colors32;

            int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

            if (textInfo.characterInfo[currentCharacter].isVisible)
            {
                float offset = (currentCharacter / characterCount);
                c0 = gradientText.Evaluate((totalTime + offset) % 1);
                totalTime += Time.deltaTime;

                newVertexColors[vertexIndex + 0] = c0;
                newVertexColors[vertexIndex + 1] = c0;
                newVertexColors[vertexIndex + 2] = c0;
                newVertexColors[vertexIndex + 3] = c0;

                textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }

            currentCharacter = (currentCharacter + 1) % characterCount;

            yield return new WaitForSeconds(gradientSpeed);
        }
    }
}
