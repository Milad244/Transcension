using System;
using System.Collections.Generic;

[Serializable]
public class DialogueOption
{
  public string text;
  public int nextId;
}

[Serializable]
public class DialogueNode
{
  public int id;
  public string dialogue;
  public List<DialogueOption> options;
}

[Serializable]
public class DialogueTree
{
  public string key;
  public List<DialogueNode> dialogueTree;
}

[Serializable]
public class DialogueContainer
{
  public List<DialogueTree> Dialogue;
}
