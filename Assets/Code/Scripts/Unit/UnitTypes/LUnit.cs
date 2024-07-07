using System.Collections;
using System.Collections.Generic;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units.Abilities;
using TbsFramework.Units.UnitStates;
using Random = UnityEngine.Random;
using Unit = TbsFramework.Units.Unit;

public class LUnit : Unit
{
    public event        Action<UnitDirection> OnIdle;
    public event        Action<UnitDirection> OnMove;
    public event        Action<UnitDirection> OnAttack;
    public event        Action<UnitDirection> OnDie;
    public event        Action                OnTakeDamage;
    public static event Action<LUnit>         OnAnyDisplayUnitInformation;
    public static event Action                OnAnyHideUnitInformation;
    public static event Action                OnAnyUnitClicked;
    public static event Action                OnAnyUnmarkUnit;

    [BoxGroup("Information")] [SerializeField]
    private UnitDetails _unitDetails;

    [BoxGroup("Stats")] private int _evasionFactor;

    [BoxGroup("Stats")] [SerializeField] private UnitStats _unitStats;

    [BoxGroup("Stats")] [SerializeField] private UnitFaction _unitFaction = UnitFaction.None;

    [BoxGroup("Sprites")] [SerializeField] private SpriteRenderer _markerSpriteRenderer;
    [BoxGroup("Sprites")] [SerializeField] private SpriteRenderer _maskSpriteRenderer;

    private IAttackSkill[]             _attackSkillArray;
    private IDefendSkill[]             _defendSkillArray;
    private RetaliateSkill             _retaliateSkill;
    private UnRetaliatableSkill        _unRetaliatableSkill;
    private VictoryValorSkill          _victoryValorSkill;
    private RetaliationResilienceSkill _retaliationResilienceSkill;
    private AOEHealingSkill            _aoeHealingSkill;
    private CapturerSkill              _capturerSkill;

    public Vector3 Offset;

    public bool isStructure;

    private Unit           _agressor;
    private Transform      _cachedTransform;
    private SpriteRenderer _spriteRenderer;

    private bool _isEvading;
    private bool _isMoving;
    private bool _isRetaliating;
    private int  _tempDamageReceived;

    #region Properties

    public UnitDetails UnitDetails { get => _unitDetails; protected set => _unitDetails = value; }

    private UnitDirection _currentUnitDirection = UnitDirection.Right;

    public UnitFaction UnitFaction { get => _unitFaction; set => _unitFaction = value; }

    public UnitDirection CurrentUnitDirection => _currentUnitDirection;

    public IDefendSkill[]             DefendSkillArray           => _defendSkillArray;
    public IAttackSkill[]             AttackSkillArray           => _attackSkillArray;
    public RetaliateSkill             RetaliateSkill             => _retaliateSkill;
    public UnRetaliatableSkill        UnRetaliatableSkill        => _unRetaliatableSkill;
    public RetaliationResilienceSkill RetaliationResilienceSkill => _retaliationResilienceSkill;
    public VictoryValorSkill          ValorSkill                 => _victoryValorSkill;
    public AOEHealingSkill            AoeHealingSkill            => _aoeHealingSkill;

    public bool IsEvading { get => _isEvading; set => _isEvading = value; }

    public    Transform CachedTransform => _cachedTransform;
    protected Unit      Agressor        => _agressor;

    public UnitFaction Faction { get => _unitFaction; set => _unitFaction = value; }

    public CapturerSkill CapturerSkill => _capturerSkill;

    public int EvasionFactor { get => _evasionFactor; private set => _evasionFactor = value; }

    public SpriteRenderer MaskSpriteRenderer => _maskSpriteRenderer;

    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    public UnitStats UnitStats => _unitStats;

    #endregion

    protected virtual void OnEnable()  => UITop.OnAnyEndTurnButtonClicked += OnHumanEndTurnManually;
    protected virtual void OnDisable() => UITop.OnAnyEndTurnButtonClicked -= OnHumanEndTurnManually;

    private void OnHumanEndTurnManually()
    {
        SetState(new UnitStateNormal(this));
        UnMark();
    }

    public override void Initialize()
    {
        Buffs     = new List<(Buff, int)>();
        UnitState = new UnitStateNormal(this);

        InitProperties();
        foreach (var ability in GetComponentsInChildren<Ability>())
        {
            RegisterAbility(ability);
            ability.Initialize();
        }

        _cachedTransform              =  transform;
        CachedTransform.localPosition += Offset;
    }

    public void InitProperties()
    {
        HitPoints      = _unitStats.HitPoints;
        MovementPoints = _unitStats.MovementPoints;
        ActionPoints   = _unitStats.ActionPoints;

        AttackRange            = _unitStats.AttackRange;
        AttackFactor           = _unitStats.AttackFactor;
        MovementAnimationSpeed = _unitStats.MovementAnimationSpeed;
        EvasionFactor          = _unitStats.EvasionFactor;

        TotalHitPoints      = HitPoints;
        TotalMovementPoints = MovementPoints;
        TotalActionPoints   = ActionPoints;

        _spriteRenderer             = GetComponent<SpriteRenderer>();
        _attackSkillArray           = GetComponents<IAttackSkill>();
        _defendSkillArray           = GetComponents<IDefendSkill>();
        _retaliateSkill             = GetComponent<RetaliateSkill>();
        _unRetaliatableSkill        = GetComponent<UnRetaliatableSkill>();
        _victoryValorSkill          = GetComponent<VictoryValorSkill>();
        _capturerSkill              = GetComponent<CapturerSkill>();
        _retaliationResilienceSkill = GetComponent<RetaliationResilienceSkill>();
        _aoeHealingSkill            = GetComponent<AOEHealingSkill>();
    }

    protected override int Defend(Unit other, int damage)
    {
        _agressor = other;
        int  defenceAmount                         = CalculateDefense();
        int  newDamage                             = damage - defenceAmount;
        bool isRetalationResilenceActive           = TryUseRetaliationResilence();
        if (isRetalationResilenceActive) newDamage /= 2;
        if (newDamage <= 0) newDamage              =  1;
        _tempDamageReceived = newDamage;
        return newDamage;
    }

    private bool TryUseRetaliationResilence()
    {
        if (_retaliationResilienceSkill == null) return false;
        if (Agressor is LUnit lUnit)
        {
            if (lUnit._retaliateSkill == null) return false;
            return lUnit._retaliateSkill.IsRetaliating;
        }

        return false;
    }

    private int CalculateDefense()
    {
        int defenceAmount = 0;
        for (int i = 0; i < DefendSkillArray.Length; i++)
            defenceAmount += DefendSkillArray[i].GetDefenceAmount();
        return defenceAmount;
    }

    protected void SpawnDamageText()
    {
        OnTakeDamage?.Invoke();

        if (DamageTextSpawner.Instance != null)
            DamageTextSpawner.Instance.SpawnTextGameObject(CachedTransform.localPosition,
                                                           _tempDamageReceived.ToString());

        _tempDamageReceived = 0;
    }

    protected override void DefenceActionPerformed()
    {
        SpawnDamageText();
        if (HitPoints <= 0)
        {
            if (Agressor is LUnit lUnit)
            {
                if (lUnit.ValorSkill == null) return;
                lUnit.ValorSkill.TryGetAdditionalActionPoint(this);
            }

            return;
        }

        if (CellGrid.Instance.CurrentPlayer.PlayerNumber == PlayerNumber) return;
        if (_retaliateSkill                              == null) return;
        _retaliateSkill.AggressorUnit = Agressor as LUnit;
        if (!_retaliateSkill.IsInAttackRange()) return;
        Vector3 enemyUnitPosition = Agressor.transform.localPosition;
        UpdateUnitDirection(enemyUnitPosition);
        AttackHandlerRetaliate(Agressor);
        _agressor = null;
    }

    public void AttackHandlerRetaliate(Unit unitToAttack)
    {
        _isRetaliating = true;
        AttackAction attackAction = DealDamage(unitToAttack);
        _isRetaliating = false;
        MarkAsAttacking(unitToAttack);
        unitToAttack.DefendHandler(this, attackAction.Damage);
    }

    public override void AttackHandler(Unit unitToAttack)
    {
        float attackActionCost = 1;
        LUnit enemyUnit        = unitToAttack as LUnit;
        if (enemyUnit.TryEvadeAttack(this))
        {
            if (EvadedTextSpawner.Instance != null)
                EvadedTextSpawner.Instance.SpawnTextGameObject(enemyUnit.CachedTransform.localPosition);
        }
        
        Vector3 enemyUnitPosition = unitToAttack.transform.localPosition;
        UpdateUnitDirection(enemyUnitPosition);

        if (!enemyUnit.IsEvading)
        {
            AttackAction attackAction = DealDamage(unitToAttack);
            unitToAttack.DefendHandler(this, attackAction.Damage);
            attackActionCost = attackAction.ActionCost;
        }

        MarkAsAttacking(unitToAttack);
        AttackActionPerformed(attackActionCost);
        enemyUnit.IsEvading = false;
        if (ActionPoints == 0)
            UnmarkSelection();
    }

    protected override AttackAction DealDamage(Unit unitToAttack)
    {
        var baseVal     = base.DealDamage(unitToAttack);
        int totalDamage = CalculateDamage(baseVal, unitToAttack);
        //takes into account the current units health for the damage calculation
        float hitPointsPercentage = HitPoints / TotalActionPoints;
        hitPointsPercentage += 0.4f;
        if (hitPointsPercentage > 1f) hitPointsPercentage = 1f;

        var newDmg = TotalHitPoints == 0 ? 0 : (int) Mathf.Ceil(totalDamage * ((float) HitPoints / TotalHitPoints));
        return new AttackAction(newDmg, baseVal.ActionCost);
    }

    private bool TryEvadeAttack(LUnit attacker)
    {
        if (PlayerNumber == CellGrid.Instance.CurrentPlayerNumber) return false;
        if (this is LStructure) return false;
        int     randomValue       = Random.Range(1, 100);
        LSquare thisCell          = (LSquare) Cell;
        LSquare attackerCell      = (LSquare) attacker.Cell;
        int     cellEvasionFactor = thisCell.EvasionFactor - attackerCell.HitChance;
        int     evasionChance     = _evasionFactor         + cellEvasionFactor;
        IsEvading = evasionChance >= randomValue;
        return IsEvading;
    }

    protected virtual int CalculateDamage(AttackAction baseVal, Unit unitToAttack)
    {
        float totalFactorDamage = 0;
        int   baseDamage        = baseVal.Damage;
        for (int i = 0; i < AttackSkillArray.Length; i++)
        {
            if (_isRetaliating && !AttackSkillArray[i].CanBeActivatedDuringEnemyTurn) continue;
            if (AttackSkillArray[i] is BackstabSkill backstabSkill)
                backstabSkill.UnitToAttack = unitToAttack as LUnit;
            if (AttackSkillArray[i] is SiegeBreakerSkill siegeBreakerSkill)
            {
                if (unitToAttack is LStructure lStructure)
                    siegeBreakerSkill.StructureToAttack = lStructure;
            }

            totalFactorDamage += AttackSkillArray[i].GetDamageFactor();
        }

        int factoredDamage = totalFactorDamage > 0 ? baseDamage * (int) totalFactorDamage : baseDamage;
        return factoredDamage;
    }

    public override IEnumerator Move(Cell destinationCell, IList<Cell> path)
    {
        _spriteRenderer.sortingOrder       += 10;
        _markerSpriteRenderer.sortingOrder += 10;
        MaskSpriteRenderer.sortingOrder    += 10;
        IsMoving                           =  true;
        yield return base.Move(destinationCell, path);
    }

    protected override IEnumerator MovementAnimation(IList<Cell> path)
    {
        float movementAnimationSpeed = MovementAnimationSpeed;

        if (GameSettings.Instance != null && CellGrid.Instance.CurrentPlayer is AIPlayer)
            movementAnimationSpeed *= GameSettings.Instance.Preferences.AISpeed;

        for (int i = path.Count - 1; i >= 0; i--)
        {
            var currentCell = path[i];
            Vector3 destination_pos = new Vector3(currentCell.transform.localPosition.x,
                                                  currentCell.transform.localPosition.y,
                                                  CachedTransform.localPosition.z);

            UpdateUnitDirection(destination_pos);
            OnMove?.Invoke(CurrentUnitDirection);

            while (transform.localPosition != destination_pos)
            {
                CachedTransform.localPosition = Vector3.MoveTowards(CachedTransform.localPosition,
                                                                    destination_pos,
                                                                    Time.deltaTime * movementAnimationSpeed);
                yield return 0;
            }
        }

        OnMoveFinished();
    }

    private void UpdateUnitDirection(Vector3 destination)
    {
        Vector3 direction = (CachedTransform.localPosition - destination).normalized;
        direction.x           = Mathf.RoundToInt(direction.x);
        direction.y           = Mathf.RoundToInt(direction.y);
        direction.z           = Mathf.RoundToInt(direction.z);
        _currentUnitDirection = GetMovementDirection(direction);
        FlipSpriteRenderer();
    }

    protected override void OnMoveFinished()
    {
        _spriteRenderer.sortingOrder       -= 10;
        _markerSpriteRenderer.sortingOrder -= 10;
        MaskSpriteRenderer.sortingOrder    -= 10;
        OnIdle?.Invoke(CurrentUnitDirection);
        IsMoving = false;
        base.OnMoveFinished();
    }

    private UnitDirection GetMovementDirection(Vector3 moveDirection)
    {
        UnitDirection unitDirection             = UnitDirection.Right;
        if (moveDirection.x > 0f) unitDirection = UnitDirection.Left;
        if (moveDirection.x < 0f) unitDirection = UnitDirection.Right;
        if (moveDirection.y > 0f) unitDirection = UnitDirection.Down;
        if (moveDirection.y < 0f) unitDirection = UnitDirection.Up;
        return unitDirection;
    }

    private void FlipSpriteRenderer() => MaskSpriteRenderer.flipX = CurrentUnitDirection == UnitDirection.Left;

    protected override void OnDestroyed()
    {
        Cell.IsTaken = false;
        Cell.CurrentUnits.Remove(this);
        OnDie?.Invoke(CurrentUnitDirection);
        MarkAsDestroyed();
    }

    public override bool IsCellTraversable(Cell cell)
    {
        return base.IsCellTraversable(cell) || (cell.CurrentUnits.Count > 0 &&
                                                !cell.CurrentUnits.Exists(u =>
                                                                              !((LUnit) u).isStructure &&
                                                                              u.PlayerNumber != PlayerNumber));
    }

    public override void SetColor(Color color)
    {
        if (_markerSpriteRenderer != null)
            _markerSpriteRenderer.color = color;
    }

    #region MouseEvents

    public override void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            HandleMouseDown();
    }

    public void HandleMouseDown()
    {
        OnAnyUnitClicked?.Invoke();
        base.OnMouseDown();
        if (PlayerNumber == CellGrid.Instance.CurrentPlayerNumber)
        {
            if (ObjectHolder.Instance != null)
            {
                if (ObjectHolder.Instance.CurrSelectedUnit != null)
                {
                    if (ObjectHolder.Instance.CurrSelectedUnit.IsMoving) return;
                    ObjectHolder.Instance.CurrSelectedUnit.UnmarkSelection();
                }

                ObjectHolder.Instance.CurrSelectedUnit = this;
            }

            if (UnitHighlighterAggregator != null)
                UnitHighlighterAggregator.MarkAsFriendlyCursorFn?.ForEach(o => o.Apply(this, null));
        }
    }

    protected virtual void DisplayUnitInformation() { OnAnyDisplayUnitInformation?.Invoke(this); }

    protected virtual void HideUnitInformation() => OnAnyHideUnitInformation?.Invoke();

    protected override void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        base.OnMouseEnter();
        DisplayUnitInformation();
        Cell.MarkAsHighlighted();

        if (ObjectHolder.Instance != null && PathPainter.Instance != null && Cell.IsMarkedReachable)
            PathPainter.Instance.DeletePath();

        if (ObjectHolder.Instance.CurrSelectedUnit != null)
        {
            if (SelectionCursorController.Instance != null)
                SelectionCursorController.Instance.DespawnCursor();
            return;
        }

        if (SelectionCursorController.Instance != null)
            SelectionCursorController.Instance.ShowCursorAtCellPosition(Cell);
    }

    protected void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        MarkAsTargetedEnemyFn();
        if (ObjectHolder.Instance != null && ObjectHolder.Instance.CurrentEnemyMarkedUnit != null &&
            ObjectHolder.Instance.CurrentEnemyMarkedUnit.Equals(this))
            HideUnitInformation();
    }

    protected override void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        base.OnMouseExit();
        HideUnitInformation();
        if (ObjectHolder.Instance != null)
        {
            if (ObjectHolder.Instance.CurrentEnemyMarkedUnit == this)
            {
                UnmarkSelection();
                ObjectHolder.Instance.CurrentEnemyMarkedUnit = null;
            }

            if ((Cell.IsMarkedReachable) && ObjectHolder.Instance.CurrSelectedUnit != null) return;
        }

        Cell.UnMark();
    }

    #endregion

    #region Marks

    public virtual void MarkAsTargetedEnemyFn()
    {
        if (IsUnitReachable)
        {
            if (UnitHighlighterAggregator != null)
                UnitHighlighterAggregator.MarkAsEnemyTargetedEnemyFn?.ForEach(o => o.Apply(this, null));
            ObjectHolder.Instance.CurrentEnemyMarkedUnit = this;
            if (ObjectHolder.Instance != null && PathPainter.Instance != null)
                PathPainter.Instance.DeletePath();
        }
    }

    public virtual void UnmarkSelection()
    {
        if (UnitHighlighterAggregator != null)
        {
            OnAnyUnmarkUnit?.Invoke();
            UnitHighlighterAggregator.MarkEnemyCursorDisabledFn?.ForEach(o => o.Apply(this, null));
        }
    }

    public override void MarkAsAttacking(Unit target)
    {
        OnAttack?.Invoke(CurrentUnitDirection);
        base.MarkAsAttacking(target);

        if (target is LUnit lUnit)
        {
            if (lUnit.IsEvading)
                lUnit.MarkAsEvading(this);

            lUnit.UnmarkSelection();
        }
    }

    public virtual void MarkAsEvading(Unit aggressor)
    {
        if (UnitHighlighterAggregator == null) return;
        UnitHighlighterAggregator.MarkAsEvadingFn?.ForEach(o => o.Apply(this, aggressor));
    }

    protected override void AttackActionPerformed(float actionCost)
    {
        base.AttackActionPerformed(actionCost);
        if (ActionPoints == 0)
            SetState(new UnitStateMarkedAsFinished(this));
    }

    public override void MarkAsFriendly()
    {
        if (ActionPoints == 0) return;
        base.MarkAsFriendly();
    }

    #endregion
}