
public abstract class Card
{

    public bool InHand { get; set; }

    public abstract void OnThrow();
    public abstract void OnGround();

}
