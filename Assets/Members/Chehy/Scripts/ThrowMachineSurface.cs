public class ThrowMachineSurface : PlayerSurface
{
    public override PlayerState State => PlayerState.OnThrowMachine;
    public override bool CanThrow => true;
}
