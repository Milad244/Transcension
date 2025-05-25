using System;
using System.Collections.Generic;

/// <summary>
/// Represents a dialogue choice for the user with the next id representing the next dialogueNode.
/// </summary>
[Serializable]
public class DialogueOption
{
  public string text;
  public int nextId;
}

/// <summary>
/// Represents a list of user dialogue choices to respond to a mind dialogue identified by an id.
/// </summary>
[Serializable]
public class DialogueNode
{
  public int id;
  public string dialogue;
  public List<DialogueOption> options;
}

/// <summary>
/// Represents a complete dialogue sequence identified by a key.
/// </summary>
[Serializable]
public class DialogueTree
{
  public string key;
  public List<DialogueNode> dialogueTree;
}

/// <summary>
/// Represents all the dialogue sequences.
/// </summary>
[Serializable]
public class DialogueContainer
{
  public List<DialogueTree> Dialogue;
}
