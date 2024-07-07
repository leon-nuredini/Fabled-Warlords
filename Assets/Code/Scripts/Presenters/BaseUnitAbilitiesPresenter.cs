using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseUnitAbilitiesPresenter : MonoBehaviour, IUnitPresenter
{
    [BoxGroup][SerializeField] private ScrollRect _scrollRect;
    
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _stillStrike;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _backstab;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _unRetaliatable;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _victoryValor;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _rage;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _retaliationResilience;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _shieldWall;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _alliedArmament;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _massHealing;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _retaliate;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _siegeBreaker;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _capturer;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _villageHealing;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _taxIncome;
    [BoxGroup("Skill UI")][SerializeField] private UIAbility _recruitUnit;

    protected void UpdateUnitAbilities(LUnit lUnit)
    {
        _stillStrike.gameObject.SetActive(false);
        _backstab.gameObject.SetActive(false);
        _unRetaliatable.gameObject.SetActive(false);
        _victoryValor.gameObject.SetActive(false);
        _rage.gameObject.SetActive(false);
        _retaliationResilience.gameObject.SetActive(false);
        _shieldWall.gameObject.SetActive(false);
        _alliedArmament.gameObject.SetActive(false);
        _massHealing.gameObject.SetActive(false);
        _siegeBreaker.gameObject.SetActive(false);
        _retaliate.gameObject.SetActive(false);
        _capturer.gameObject.SetActive(false);
        _villageHealing.gameObject.SetActive(false);
        _taxIncome.gameObject.SetActive(false);
        _recruitUnit.gameObject.SetActive(false);

        for (int i = 0; i < lUnit.AttackSkillArray.Length; i++)
        {
            switch (lUnit.AttackSkillArray[i])
            {
                case AlliedArmamentSkill alliedArmamentSkill:
                    _alliedArmament.gameObject.SetActive(true);
                    _alliedArmament.UpdateNameAndDescription(alliedArmamentSkill);
                    break;
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
            }
        }

        for (int i = 0; i < lUnit.DefendSkillArray.Length; i++)
        {
            switch (lUnit.DefendSkillArray[i])
            {
                case ShieldwallSkill shieldwallSkill:
                    _shieldWall.gameObject.SetActive(true);
                    _shieldWall.UpdateNameAndDescription(shieldwallSkill);
                    break;
            }
        }

        UpdateAbilityText(lUnit.RetaliateSkill,             _retaliate);
        UpdateAbilityText(lUnit.UnRetaliatableSkill,        _unRetaliatable);
        UpdateAbilityText(lUnit.ValorSkill,                 _victoryValor);
        UpdateAbilityText(lUnit.RetaliationResilienceSkill, _retaliationResilience);
        UpdateAbilityText(lUnit.AoeHealingSkill,            _massHealing);
        UpdateAbilityText(lUnit.CapturerSkill,              _capturer);

        if (lUnit is Stronghold stronghold)
        {
            UpdateAbilityText(stronghold.IncomeGenerationAbility,  _taxIncome);
            UpdateAbilityText(stronghold.RecruitUnitAbility, _recruitUnit);
        } else if (lUnit is Barrack barrack)
        {
            UpdateAbilityText(barrack.RecruitUnitAbility, _recruitUnit);
        }

        if (lUnit is Village village)
        {
            UpdateAbilityText(village.IncomeGenerationAbility, _taxIncome);
            UpdateAbilityText(village.VillageHealingSkill,  _villageHealing);
        }

        FocusScrollRect();
    }

    private void UpdateAbilityText(ISkill skill, UIAbility uiAbility)
    {
        if (skill != null)
        {
            uiAbility.gameObject.SetActive(true);
            uiAbility.UpdateNameAndDescription(skill);
        }
        else { uiAbility.gameObject.SetActive(false); }
    }

    private void FocusScrollRect()
    {
        if (_scrollRect         == null) return;
        if (EventSystem.current == null) return;
        EventSystem.current.SetSelectedGameObject(_scrollRect.gameObject);
        _scrollRect.OnInitializePotentialDrag(new PointerEventData(EventSystem.current));
        _scrollRect.OnBeginDrag(new PointerEventData(EventSystem.current));
    }
}