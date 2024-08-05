using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseUnitAbilitiesPresenter : MonoBehaviour, IUnitPresenter
{
    [BoxGroup("Position")] [SerializeField]
    private RectTransform _abilityPosition;

    [BoxGroup("Position")] [SerializeField]
    private Vector2 _unitAbilityPosition;

    [BoxGroup("Position")] [SerializeField]
    private Vector2 _structureAbilityPosition;

    [BoxGroup("Delta Size")] [SerializeField]
    private float _yUnitDeltaSize;

    [BoxGroup("Delta Size")] [SerializeField]
    private float _yStructureDeltaSize;

    [BoxGroup] [SerializeField] private ScrollRect _scrollRect;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _stillStrike;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _backstab;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _unRetaliatable;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _victoryValor;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _rage;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _retaliationResilience;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _retaliate;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _siegeBreaker;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _capturer;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _villageHealing;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _taxIncome;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _recruitUnit;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _antiLarge;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _parry;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _charge;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _soar;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _victorsSmite;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _hex;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _rapidShot;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _stun;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _stoneWill;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _poison;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _thunderStrike;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _poistonHex;

    [BoxGroup("Skill UI")] [SerializeField]
    private UIAbility _regenerate;

    private void Awake()
    {
        _unitAbilityPosition.x = _abilityPosition.localPosition.x;
        _structureAbilityPosition.x = _abilityPosition.localPosition.x;
    }

    protected void UpdateUnitAbilities(LUnit lUnit)
    {
        UpdatePositionAndSize(lUnit);

        _stillStrike.gameObject.SetActive(false);
        _backstab.gameObject.SetActive(false);
        _unRetaliatable.gameObject.SetActive(false);
        _victoryValor.gameObject.SetActive(false);
        _rage.gameObject.SetActive(false);
        _retaliationResilience.gameObject.SetActive(false);
        _siegeBreaker.gameObject.SetActive(false);
        _retaliate.gameObject.SetActive(false);
        _capturer.gameObject.SetActive(false);
        _villageHealing.gameObject.SetActive(false);
        _taxIncome.gameObject.SetActive(false);
        _recruitUnit.gameObject.SetActive(false);
        _antiLarge.gameObject.SetActive(false);
        _parry.gameObject.SetActive(false);
        _charge.gameObject.SetActive(false);
        _soar.gameObject.SetActive(false);
        _victorsSmite.gameObject.SetActive(false);
        _hex.gameObject.SetActive(false);
        _rapidShot.gameObject.SetActive(false);
        _stoneWill.gameObject.SetActive(false);
        _poison.gameObject.SetActive(false);
        _thunderStrike.gameObject.SetActive(false);
        _stun.gameObject.SetActive(false);
        _poistonHex.gameObject.SetActive(false);
        _regenerate.gameObject.SetActive(false);

        for (int i = 0; i < lUnit.AttackSkillArray.Length; i++)
        {
            switch (lUnit.AttackSkillArray[i])
            {
                case BackstabSkill backstabSkill:
                    _backstab.gameObject.SetActive(true);
                    _backstab.UpdateNameAndDescription(backstabSkill);
                    break;
                case RageSkill rageSkill:
                    _rage.gameObject.SetActive(true);
                    _rage.UpdateNameAndDescription(rageSkill);
                    break;
                case SiegeBreakerSkill siegeBreakerSkill:
                    _siegeBreaker.gameObject.SetActive(true);
                    _siegeBreaker.UpdateNameAndDescription(siegeBreakerSkill);
                    break;
                case StillStrikeSkill stillStrikeSkill:
                    _stillStrike.gameObject.SetActive(true);
                    _stillStrike.UpdateNameAndDescription(stillStrikeSkill);
                    break;
                case AntiLargeSkill antiLargeSkill:
                    _antiLarge.gameObject.SetActive(true);
                    _antiLarge.UpdateNameAndDescription(antiLargeSkill);
                    break;
                case ParrySkill parrySkill:
                    _parry.gameObject.SetActive(true);
                    _parry.UpdateNameAndDescription(parrySkill);
                    break;
                case ChargeSkill chargeSkill:
                    _charge.gameObject.SetActive(true);
                    _charge.UpdateNameAndDescription(chargeSkill);
                    break;
            }
        }

        for (int i = 0; i < lUnit.DefendSkillArray.Length; i++)
        {
            switch (lUnit.DefendSkillArray[i])
            {
                case StoneWillSkill stoneWillSkill:
                    _stoneWill.gameObject.SetActive(true);
                    _stoneWill.UpdateNameAndDescription(stoneWillSkill);
                    break;
            }
        }

        UpdateAbilityText(lUnit.RetaliateSkill, _retaliate);
        UpdateAbilityText(lUnit.UnRetaliatableSkill, _unRetaliatable);
        UpdateAbilityText(lUnit.ValorSkill, _victoryValor);
        UpdateAbilityText(lUnit.RetaliationResilienceSkill, _retaliationResilience);
        UpdateAbilityText(lUnit.AoeHealingSkill, _regenerate);
        UpdateAbilityText(lUnit.CapturerSkill, _capturer);

        if (lUnit is Griffin griffin) UpdateAbilityText(griffin.SoarSkill, _soar);
        if (lUnit is Paladin paladin) UpdateAbilityText(paladin.VictorsSmiteSkill, _victorsSmite);
        if (lUnit is Monk monk) UpdateAbilityText(monk.HexSkill, _hex);
        if (lUnit is Centaur centaur) UpdateAbilityText(centaur.RapidShotSkill, _rapidShot);
        if (lUnit is Cyclop cyclop) UpdateAbilityText(cyclop.StunSkill, _stun);
        if (lUnit is Troll troll) UpdateAbilityText(troll.RegenerationSkill, _regenerate);
        if (lUnit is Leafshooter leafshooter)
        {
            UpdateAbilityText(leafshooter.RapidShotSkill, _rapidShot);
            UpdateAbilityText(leafshooter.PoisonSkill, _poison);
        }

        if (lUnit is StormElemental stormElemental)
            UpdateAbilityText(stormElemental.ThunderStrikeSkill, _thunderStrike);
        if (lUnit is Satyr satyr) UpdateAbilityText(satyr.PoisonSkill, _poison);
        if (lUnit is Pixie pixie)
        {
            UpdateAbilityText(pixie.SoarSkill, _soar);
            UpdateAbilityText(pixie.StunSkill, _stun);
        }

        if (lUnit is Druid druid) UpdateAbilityText(druid.PoisonHexSkill, _poistonHex);
        if (lUnit is Treant treant) UpdateAbilityText(treant.RootGraspSkill, _stun);

        if (lUnit is Stronghold stronghold)
        {
            UpdateAbilityText(stronghold.IncomeGenerationAbility, _taxIncome);

            if (!stronghold.IsRuined)
                UpdateAbilityText(stronghold.RecruitUnitAbility, _recruitUnit);
        }
        else if (lUnit is Barrack barrack)
        {
            UpdateAbilityText(barrack.RecruitUnitAbility, _recruitUnit);
        }

        if (lUnit is Village village)
        {
            UpdateAbilityText(village.IncomeGenerationAbility, _taxIncome);
            UpdateAbilityText(village.VillageHealingSkill, _villageHealing);
        }

        FocusScrollRect();
    }

    private void UpdatePositionAndSize(LUnit lUnit)
    {
        if (_abilityPosition == null) return;

        Vector2 size = _abilityPosition.sizeDelta;
        _abilityPosition.localPosition = _unitAbilityPosition;
        size.y = _yUnitDeltaSize;
        if (lUnit is LStructure)
        {
            _abilityPosition.localPosition = _structureAbilityPosition;
            size.y = _yStructureDeltaSize;
        }

        _abilityPosition.sizeDelta = size;
    }

    private void UpdateAbilityText(ISkill skill, UIAbility uiAbility)
    {
        if (skill != null && uiAbility.IsVisible)
        {
            uiAbility.gameObject.SetActive(true);
            uiAbility.UpdateNameAndDescription(skill);
        }
        else
        {
            uiAbility.gameObject.SetActive(false);
        }
    }

    private void FocusScrollRect()
    {
        if (_scrollRect == null) return;
        if (EventSystem.current == null) return;
        EventSystem.current.SetSelectedGameObject(_scrollRect.gameObject);
        _scrollRect.OnInitializePotentialDrag(new PointerEventData(EventSystem.current));
        _scrollRect.OnBeginDrag(new PointerEventData(EventSystem.current));
    }
}