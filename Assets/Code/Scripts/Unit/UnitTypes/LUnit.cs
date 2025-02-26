using System.Collections;
using System.Collections.Generic;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units.Abilities;
using TbsFramework.Units.UnitStates;
using Random = UnityEngine.Random;
using Unit = TbsFramework.Units.Unit;

public class LUnit : Unit
{
    public event Action<UnitDirection> OnIdle;
    public event Action<UnitDirection> OnMove;
    public event Action<UnitDirection> OnAttack;
    public event Action<UnitDirection> OnDie;
    public event Action<UnitDirection> OnGetHit;
    public event Action OnTakeDamage;
    public static event Action<LUnit> OnAnyDisplayUnitInformation;
    public static event Action OnAnyHideUnitInformation;
    public static event Action OnAnyUnitClicked;
    public event Action OnAnyUnmarkUnit;
    public event Action OnStartedMoving;
    public event Action<MovementEventArgs, bool> OnMovementFinished;

    [BoxGroup("Information")] [SerializeField]
    private UnitDetails _unitDetails;

    [BoxGroup("Stats")] private int _evasionFactor;

    [BoxGroup("Stats")] [SerializeField] private UnitStats _unitStats;

    [BoxGroup("Stats")] [SerializeField] private UnitFaction _unitFaction = UnitFaction.None;

    [BoxGroup("Countering")] [SerializeField]
    private UnitClassCounter _unitClassCounter;

    [BoxGroup("Sprites")] [SerializeField] private SpriteRenderer _markerSpriteRenderer;
    [BoxGroup("Sprites")] [SerializeField] private SpriteRenderer _maskSpriteRenderer;

    private IAttackSkill[] _attackSkillArray;
    private IDefendSkill[] _defendSkillArray;
    private RetaliateSkill _retaliateSkill;
    private UnRetaliatableSkill _unRetaliatableSkill;
    private VictoryValorSkill _victoryValorSkill;
    private RetaliationResilienceSkill _retaliationResilienceSkill;
    private AOEHealingSkill _aoeHealingSkill;
    private CapturerSkill _capturerSkill;
    private StatusEffectsController _statusEffectsController;
    private PrisonerAbility _prisonerAbility;
    private CounterVisualAction _counterVisualAction;
    private UndoMovementAction _undoMovementAction;

    public Vector3 Offset;

    public bool isStructure;

    private Unit _agressor;
    private Transform _cachedTransform;
    private SpriteRenderer _spriteRenderer;

    private bool _isEvading;
    private bool _isMoving;
    private bool _isRetaliating;
    private int _tempDamageReceived;

    #region Properties

    public UnitDetails UnitDetails
    {
        get => _unitDetails;
        set => _unitDetails = value;
    }

    private UnitDirection _currentUnitDirection = UnitDirection.Right;

    public UnitFaction UnitFaction
    {
        get => _unitFaction;
        set => _unitFaction = value;
    }

    public UnitDirection CurrentUnitDirection => _currentUnitDirection;

    public IDefendSkill[] DefendSkillArray => _defendSkillArray;
    public IAttackSkill[] AttackSkillArray => _attackSkillArray;
    public RetaliateSkill RetaliateSkill => _retaliateSkill;
    public UnRetaliatableSkill UnRetaliatableSkill => _unRetaliatableSkill;
    public RetaliationResilienceSkill RetaliationResilienceSkill => _retaliationResilienceSkill;
    public VictoryValorSkill ValorSkill => _victoryValorSkill;
    public AOEHealingSkill AoeHealingSkill => _aoeHealingSkill;

    public bool IsEvading
    {
        get => _isEvading;
        set => _isEvading = value;
    }

    public Transform CachedTransform => _cachedTransform;

    protected Unit Agressor
    {
        get => _agressor;
        set => _agressor = value;
    }

    public UnitFaction Faction
    {
        get => _unitFaction;
        set => _unitFaction = value;
    }

    public CapturerSkill CapturerSkill => _capturerSkill;

    public int EvasionFactor
    {
        get => _evasionFactor;
        private set => _evasionFactor = value;
    }

    public SpriteRenderer MaskSpriteRenderer => _maskSpriteRenderer;
    public SpriteRenderer MarkerSpriteRenderer => _markerSpriteRenderer;

    public bool IsMoving
    {
        get => _isMoving;
        set => _isMoving = value;
    }

    public UnitStats UnitStats
    {
        get => _unitStats;
        set { _unitStats = value; }
    }

    protected int TempDamageReceived
    {
        get => _tempDamageReceived;
        set => _tempDamageReceived = value;
    }

    public bool IsRetaliating => _isRetaliating;

    protected SpriteRenderer UnitSpriteRenderer => _spriteRenderer;
    public StatusEffectsController StatusEffectsController => _statusEffectsController;
    public UnitClassCounter UnitClassCounter => _unitClassCounter;
    public PrisonerAbility PrisonerAbility => _prisonerAbility;
    public UndoMovementAction UndoMovementAction => _undoMovementAction;

    #endregion

    protected virtual void OnEnable() => UITop.OnAnyEndTurnButtonClicked += OnHumanEndTurnManually;
    protected virtual void OnDisable() => UITop.OnAnyEndTurnButtonClicked -= OnHumanEndTurnManually;

    private void OnHumanEndTurnManually()
    {
        SetState(new UnitStateNormal(this));
        UnMark();
    }

    public override void Initialize()
    {
        Buffs = new List<(Buff, int)>();
        UnitState = new UnitStateNormal(this);

        InitProperties();
        foreach (var ability in GetComponentsInChildren<Ability>())
        {
            RegisterAbility(ability);
            ability.Initialize();
        }

        _cachedTransform = transform;
        CachedTransform.localPosition += Offset;
    }

    public virtual void InitProperties()
    {
        UpdateUnitStats();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _attackSkillArray = GetComponents<IAttackSkill>();
        _defendSkillArray = GetComponents<IDefendSkill>();
        _retaliateSkill = GetComponent<RetaliateSkill>();
        _unRetaliatableSkill = GetComponent<UnRetaliatableSkill>();
        _victoryValorSkill = GetComponent<VictoryValorSkill>();
        _capturerSkill = GetComponent<CapturerSkill>();
        _retaliationResilienceSkill = GetComponent<RetaliationResilienceSkill>();
        _aoeHealingSkill = GetComponent<AOEHealingSkill>();
        _statusEffectsController = GetComponent<StatusEffectsController>();
        _prisonerAbility = GetComponent<PrisonerAbility>();
        _counterVisualAction = GetComponent<CounterVisualAction>();
        _undoMovementAction = GetComponent<UndoMovementAction>();
    }

    public void UpdateUnitStats()
    {
        HitPoints = _unitStats.HitPoints;
        MovementPoints = _unitStats.MovementPoints;
        ActionPoints = _unitStats.ActionPoints;

        AttackRange = _unitStats.AttackRange;
        AttackFactor = _unitStats.AttackFactor;
        MovementAnimationSpeed = _unitStats.MovementAnimationSpeed;
        EvasionFactor = _unitStats.EvasionFactor;

        TotalHitPoints = HitPoints;
        TotalMovementPoints = MovementPoints;
        TotalActionPoints = ActionPoints;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();

        if (StatusEffectsController == null) return;

        if (StatusEffectsController.IsStatusApplied<Stun>())
        {
            ActionPoints = 0;
            MovementPoints = 0;
            SetState(new UnitStateMarkedAsFinished(this));
        }
    }

    public override void OnTurnEnd()
    {
        if (StatusEffectsController.IsStatusApplied<Poison>())
        {
            Poison poisonStatusEffect = StatusEffectsController.GetStatus<Poison>();
            int damageTaken = Mathf.RoundToInt(TotalHitPoints * poisonStatusEffect.damageFactor);
            HitPoints -= damageTaken;
            _tempDamageReceived = damageTaken;
            DefenceActionPerformedFromStatusEffect();
            if (HitPoints <= 0)
                OnDestroyed();
        }

        base.OnTurnEnd();
    }

    protected override int Defend(Unit other, int damage)
    {
        _agressor = other;
        //int  defenceAmount                         = CalculateDefense();
        //int  newDamage                             = damage - defenceAmount;
        int newDamage = damage;

        if (StatusEffectsController != null && StatusEffectsController.IsStatusApplied<Weaken>())
        {
            float weakenedFactor = StatusEffectsController.GetStatus<Weaken>().weakenFactor;
            newDamage = Mathf.RoundToInt(newDamage + (newDamage * weakenedFactor));
        }

        bool isRetalationResilenceActive = TryUseRetaliationResilence();
        if (isRetalationResilenceActive) newDamage /= 2;
        if (newDamage <= 0) newDamage = 1;
        _tempDamageReceived = newDamage;
        return newDamage;
    }

    protected bool TryUseRetaliationResilence()
    {
        if (_retaliationResilienceSkill == null) return false;
        if (Agressor is LUnit lUnit)
        {
            if (lUnit._retaliateSkill == null) return false;
            return lUnit._retaliateSkill.IsRetaliating;
        }

        return false;
    }

    protected virtual float CalculateDefense()
    {
        float defenceAmount = 0;
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
        InvokeGetHitEvent();
        SpawnDamageText();
        if (HitPoints <= 0)
        {
            if (Agressor is Paladin paladin)
            {
                if (paladin.VictorsSmiteSkill != null)
                    paladin.VictorsSmiteSkill.TryGetAdditionalActionPoint(this);
            }

            if (Agressor is LUnit lUnit)
            {
                if (lUnit.ValorSkill != null)
                    lUnit.ValorSkill.TryGetAdditionalActionPoint(this);
            }

            return;
        }

        if (CellGrid.Instance.CurrentPlayer.PlayerNumber == PlayerNumber) return;
        if (StatusEffectsController != null && StatusEffectsController.IsStatusApplied<Stun>()) return;
        if (_retaliateSkill == null) return;
        _retaliateSkill.AggressorUnit = Agressor as LUnit;
        if (!_retaliateSkill.IsInAttackRange()) return;
        Vector3 enemyUnitPosition = Agressor.transform.localPosition;
        UpdateUnitDirection(enemyUnitPosition);
        AttackHandlerRetaliate(Agressor);
        _agressor = null;
    }

    protected virtual void DefenceActionPerformedFromStatusEffect()
    {
        InvokeGetHitEvent();
        SpawnDamageText();
    }

    protected void InvokeGetHitEvent() => OnGetHit?.Invoke(CurrentUnitDirection);

    public void AttackHandlerRetaliate(Unit unitToAttack)
    {
        _isRetaliating = true;
        AttackAction attackAction = DealDamage(unitToAttack);
        MarkAsAttacking(unitToAttack);
        unitToAttack.DefendHandler(this, attackAction.Damage);
        _isRetaliating = false;
    }

    public override void AttackHandler(Unit unitToAttack)
    {
        float attackActionCost = 1;
        LUnit enemyUnit = unitToAttack as LUnit;
        if (enemyUnit.TryEvadeAttack(this))
        {
            if (EvadedTextSpawner.Instance != null)
                EvadedTextSpawner.Instance.SpawnTextGameObject(enemyUnit.CachedTransform.localPosition);
        }

        Vector3 enemyUnitPosition = unitToAttack.transform.localPosition;
        UpdateUnitDirection(enemyUnitPosition);
        if (_undoMovementAction is not null)
            _undoMovementAction.UnitDirection = _currentUnitDirection;

        if (!enemyUnit.IsEvading)
        {
            bool isThisMageUnit = this is IMage;
            AttackAction attackAction = DealDamage(unitToAttack);
            if (!isThisMageUnit) ApplyDebuffsToEnemy(enemyUnit);
            unitToAttack.DefendHandler(this, attackAction.Damage);
            attackActionCost = attackAction.ActionCost;
            if (isThisMageUnit) ApplyDebuffsToEnemy(enemyUnit);
        }

        MarkAsAttacking(unitToAttack);
        AttackActionPerformed(attackActionCost);
        enemyUnit.IsEvading = false;
        if (ActionPoints == 0)
            UnmarkSelection();
    }

    protected virtual void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        //does nothing here
    }

    protected override AttackAction DealDamage(Unit unitToAttack)
    {
        var baseVal = base.DealDamage(unitToAttack);
        int totalDamage = CalculateDamage(baseVal, unitToAttack);
        if (_isRetaliating) totalDamage = totalDamage / 2;
        //takes into account the current units health for the damage calculation
        float hitPointsPercentage = (float)HitPoints / TotalHitPoints;
        hitPointsPercentage += 0.3f;
        if (hitPointsPercentage > 1f) hitPointsPercentage = 1f;

        var newDmg = TotalHitPoints == 0 ? 0 : (int)Mathf.Ceil(totalDamage * hitPointsPercentage);
        return new AttackAction(newDmg, baseVal.ActionCost);
    }

    private bool TryEvadeAttack(LUnit attacker)
    {
        if (PlayerNumber == CellGrid.Instance.CurrentPlayerNumber) return false;
        if (this is LStructure) return false;
        if (_statusEffectsController != null && _statusEffectsController.IsStatusApplied<Stun>()) return false;
        int randomValue = Random.Range(1, 100);
        int evasionChance = GetEvasionChance(attacker);
        IsEvading = evasionChance >= randomValue;
        return IsEvading;
    }

    public int GetEvasionChance(LUnit attacker)
    {
        LSquare thisCell = (LSquare)Cell;
        LSquare attackerCell = (LSquare)attacker.Cell;
        int cellEvasionFactor = thisCell.EvasionFactor - attackerCell.HitChance;
        return _evasionFactor + cellEvasionFactor;
    }

    protected virtual int CalculateDamage(AttackAction baseVal, Unit unitToAttack)
    {
        float totalFactorDamage = 0;
        int baseDamage = baseVal.Damage;

        if (StatusEffectsController != null && StatusEffectsController.IsStatusApplied<Weaken>())
        {
            float weakenedFactor = StatusEffectsController.GetStatus<Weaken>().weakenFactor;
            baseDamage = Mathf.RoundToInt(baseDamage * weakenedFactor);
        }

        if (!IsEvaluating)
        {
            for (int i = 0; i < AttackSkillArray.Length; i++)
            {
                if (_isRetaliating && !AttackSkillArray[i].CanBeActivatedDuringEnemyTurn) continue;
                if (unitToAttack is LStructure lStructure)
                {
                    if (AttackSkillArray[i] is SiegeBreakerSkill siegeBreakerSkill)
                        siegeBreakerSkill.StructureToAttack = lStructure;
                    else
                        continue;
                }

                if (AttackSkillArray[i] is BackstabSkill backstabSkill)
                    backstabSkill.UnitToAttack = unitToAttack as LUnit;

                totalFactorDamage += AttackSkillArray[i].GetDamageFactor();
            }
        }

        int factoredDamage = totalFactorDamage > 0 ? baseDamage * (int)totalFactorDamage : baseDamage;
        return factoredDamage;
    }

    private MovementEventArgs _movementEventArgs;

    public override IEnumerator Move(Cell destinationCell, IList<Cell> path)
    {
        OnStartedMoving?.Invoke();
        if (PlayerNumber == 0 && MovementUndoController.Instance is not null && _undoMovementAction is not null)
            MovementUndoController.Instance.LastMovedUnit = _undoMovementAction;
        _spriteRenderer.sortingOrder += 10;
        _markerSpriteRenderer.sortingOrder += 10;
        MaskSpriteRenderer.sortingOrder += 10;
        IsMoving = true;
        if (_undoMovementAction is not null && _undoMovementAction.DisableUndoMovement)
            _undoMovementAction.UpdateStartingCell();

        _movementEventArgs = new MovementEventArgs(Cell, destinationCell, path, this);
        yield return base.Move(destinationCell, path);
    }

    protected override IEnumerator MovementAnimation(IList<Cell> path)
    {
        float movementAnimationSpeed = MovementAnimationSpeed;

        if (GameSettings.Instance != null && CellGrid.Instance.CurrentPlayer is AIPlayer)
            movementAnimationSpeed *= GameSettings.Instance.Preferences.AISpeed;

        if (_undoMovementAction.IsUndoingMovement && path.Count > 0)
        {
            var currentCell = path[0];
            Vector3 destination_pos = new Vector3(currentCell.transform.localPosition.x,
                currentCell.transform.localPosition.y,
                CachedTransform.localPosition.z);
            CachedTransform.localPosition = destination_pos;
            OnMoveFinished();
            yield break;
        }

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
        if (_undoMovementAction != null && _undoMovementAction.IsUndoingMovement) return;
        Vector3 direction = (CachedTransform.localPosition - destination).normalized;
        direction.x = Mathf.RoundToInt(direction.x);
        direction.y = Mathf.RoundToInt(direction.y);
        direction.z = Mathf.RoundToInt(direction.z);
        _currentUnitDirection = GetMovementDirection(direction);
        FlipSpriteRenderer();
    }

    private void SetCurrentUnitDirection(UnitDirection direction)
    {
        _currentUnitDirection = direction;
        FlipSpriteRenderer();
    }

    protected override void OnMoveFinished()
    {
        bool isUndoing = false;
        _spriteRenderer.sortingOrder -= 10;
        _markerSpriteRenderer.sortingOrder -= 10;
        MaskSpriteRenderer.sortingOrder -= 10;
        OnIdle?.Invoke(CurrentUnitDirection);
        IsMoving = false;
        if (_undoMovementAction is not null)
        {
            _undoMovementAction.IsMovementPerformed = true;
            if (_undoMovementAction.IsUndoingMovement)
            {
                isUndoing = true;
                SetCurrentUnitDirection(_undoMovementAction.UnitDirection);
                _undoMovementAction.IsUndoingMovement = false;
                _undoMovementAction.IsMovementPerformed = false;
                MovementPoints = _undoMovementAction.RemainingMovePoints;
                DOVirtual.DelayedCall(.02f, () => HandleMouseDown());
            }
        }

        OnMovementFinished?.Invoke(_movementEventArgs, isUndoing);
        base.OnMoveFinished();
    }

    private UnitDirection GetMovementDirection(Vector3 moveDirection)
    {
        UnitDirection unitDirection = _currentUnitDirection;
        if (moveDirection.x > 0f) unitDirection = UnitDirection.Left;
        if (moveDirection.x < 0f) unitDirection = UnitDirection.Right;
        //Move uncomment these lines if the unit has up and down idle/walking animations
        //if (moveDirection.y > 0f) unitDirection = UnitDirection.Down;
        //if (moveDirection.y < 0f) unitDirection = UnitDirection.Up;
        return unitDirection;
    }

    private void FlipSpriteRenderer() => MaskSpriteRenderer.flipX = CurrentUnitDirection == UnitDirection.Left;

    protected override void OnDestroyed()
    {
        Cell.IsTaken = false;
        Cell.CurrentUnits.Remove(this);
        ActionPoints = 0;
        if (ObjectHolder.Instance != null && ObjectHolder.Instance.CurrSelectedUnit == this)
            ObjectHolder.Instance.CurrSelectedUnit = null;
        OnDie?.Invoke(CurrentUnitDirection);
        MarkAsDestroyed();
    }

    public override bool IsCellTraversable(Cell cell)
    {
        return base.IsCellTraversable(cell) || (cell.CurrentUnits.Count > 0 &&
                                                !cell.CurrentUnits.Exists(u =>
                                                    !((LUnit)u).isStructure &&
                                                    u.PlayerNumber != PlayerNumber));
    }

    public override void SetColor(Color color)
    {
        if (_markerSpriteRenderer != null)
            _markerSpriteRenderer.color = color;
    }

    public override bool IsUnitAttackable(Unit other, Cell otherCell, Cell sourceCell)
    {
        if (PrisonerAbility != null && PrisonerAbility.IsPrisoner) return false;
        return base.IsUnitAttackable(other, otherCell, sourceCell);
    }

    public void DisplayCounterIcon(LUnit enemyUnit)
    {
        if (_counterVisualAction is null) return;
        float damageFactor = GetClassCounterDamageFactor(enemyUnit);
        _counterVisualAction.UpdateCounterIcon(damageFactor);
    }

    public void DisableCounterIcon()
    {
        if (_counterVisualAction is null) return;
        _counterVisualAction.DisableCounterIcon();
    }

    public virtual float GetClassCounterDamageFactor(LUnit enemyUnit)
    {
        return 1f;
    }

    #region MouseEvents

    public override void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            HandleMouseDown();
    }

    public void HandleMouseDown()
    {
        if (PrisonerAbility != null && PrisonerAbility.IsPrisoner) return;
        if (_undoMovementAction is not null && _undoMovementAction.IsUndoingMovement &&
            _undoMovementAction.IsUndoMovementButtonEnabled) return;

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

    protected virtual void DisplayUnitInformation()
    {
        OnAnyDisplayUnitInformation?.Invoke(this);
    }

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