using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �q�ɂ���I�u�W�F�N�g���~��ɔz�u����N���X
/// </summary>
public class CircleDeployer : MonoBehaviour
{

    //���a
    [SerializeField]
    private float radius;

    //=================================================================================
    //������
    //=================================================================================

    private void Awake()
    {
        Deploy();
    }

    //Inspector�̓��e(���a)���ύX���ꂽ���Ɏ��s
    private void OnValidate()
    {
        Deploy();
    }

    //�q���~��ɔz�u����(ContextMenu�Ō��}�[�N�̏��Ƀ��j���[�ǉ�)
    [ContextMenu("Deploy")]
    private void Deploy()
    {

        //�q���擾
        List<GameObject> childList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            childList.Add(child.gameObject);
        }

        //���l�A�A���t�@�x�b�g���Ƀ\�[�g
        childList.Sort(
          (a, b) => {
              return string.Compare(a.name, b.name);
          }
        );

        //�I�u�W�F�N�g�Ԃ̊p�x��
        float angleDiff = 360f / (float)childList.Count;

        //�e�I�u�W�F�N�g���~��ɔz�u
        for (int i = 0; i < childList.Count; i++)
        {
            float angle = i * Mathf.PI * 2f / 5;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            childList[i].transform.position = newPos;
        }
    }
}
