namespace IronBlock.Blocks.Text
{
  public class ColourPicker : IBlock
  {
    public override object Evaluate(Context context)
    {
      return this.Fields.Get("COLOUR") ?? "#000000";
    }
  }

}