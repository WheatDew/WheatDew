using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//保存用于对话的标签信息(未经过语料库的信息)
public class CorpusCommand : MonoBehaviour
{
    public int origin;
    public int target;
    public List<string> tags;
}
