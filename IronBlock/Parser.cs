using System;
using System.Collections.Generic;

namespace IronBlock
{
  public abstract class Parser
  {
    public static XmlParser CreateXml() => new();

    protected readonly IDictionary<string, Func<IBlock>> blocks = new Dictionary<string, Func<IBlock>>();
  }

  public abstract class Parser<TParser> : Parser where TParser : Parser
  {
    public TParser AddBlock<T>(string type) where T : IBlock, new()
    {
      AddBlock(type, () => new T());

      return this as TParser;
    }

    public TParser AddBlock<T>(string type, T block) where T : IBlock
    {
      AddBlock(type, () => block);

      return this as TParser;
    }

    public TParser AddBlock(string type, Func<IBlock> blockFactory)
    {
      if (blocks.ContainsKey(type))
        blocks[type] = blockFactory;
      else
        blocks.Add(type, blockFactory);

      return this as TParser;
    }
  }
}