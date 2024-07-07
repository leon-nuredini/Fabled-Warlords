using System;
using TbsFramework.Units;

public interface ICapturable
{
    public event Action<LUnit> OnCaptured;
    public event Action        OnAbandoned;
    void Capture(LUnit aggressor);
}
