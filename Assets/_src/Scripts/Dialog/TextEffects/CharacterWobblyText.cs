using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharacterWobblyText : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private Vector2 wobbleSpeed = new Vector2(1, 1);
    [SerializeField] private Vector2 wobbleOffset = new Vector2(1, 1);

    private void Update()
    {
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var characterInfo = textInfo.characterInfo[i];

            if (!characterInfo.isVisible)
                continue;

            var vertices = textInfo.meshInfo[characterInfo.materialReferenceIndex].vertices;
            int index = characterInfo.vertexIndex;

            Vector3 offset = Wobble(Time.time + i);

            for (int j = 0; j < 4; j++)
            {
                vertices[index + j] += offset;
            }
            
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }

        
    }

    private Vector3 Wobble(float time)
    {
        return new Vector3(Mathf.Sin(time * wobbleSpeed.x) * wobbleOffset.x, Mathf.Sin(time * wobbleSpeed.y) * wobbleOffset.y);
    }
}
