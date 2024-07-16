public class Centaur : LUnit
{
    private RapidShotSkill _rapidShotSkill;

    #region Properties

    public RapidShotSkill RapidShotSkill => _rapidShotSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _rapidShotSkill = GetComponent<RapidShotSkill>();
    }

    protected override void AttackActionPerformed(float actionCost)
    {
        if (_rapidShotSkill != null) _rapidShotSkill.AddAdditionalActionPoint();
        base.AttackActionPerformed(actionCost);
    }
}