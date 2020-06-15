using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csas.Models
{
    public enum RuleSet
    {
        DandD5E,
        PathFinder
    }

    public enum Alignment
    {
        LawfulGood,
        LawfulNeutral,
        LawfulEvil,
        NeutralGood,
        TrueNeutral,
        NeutralEvil,
        ChaoticGood,
        ChaoticNeutral,
        ChaiticEvil
    }

    public enum AbilityNames
    {
        Str,
        Dex,
        Con,
        Int,
        Wis,
        Cha,
        Custom
    }

    public enum SkillNames
    {
        Athletics,
        Acrobatics,
        SleightOfHand,
        Stealth,
        Arcana,
        History,
        Investigation,
        Nature,
        Religion,
        AnimalHandling,
        Insight,
        Medicine,
        Perception,
        Survival,
        Deception,
        Intimidation,
        Performance,
        Persuasion,
        Custom
    }

    public class Character
    {
        public string PlayerName { get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public RuleSet RuleSet { get; set; }
        public Details Details { get; set; }
        public List<AbilityScore> AbilityScores { get; set; }
        public List<Skill> Skills { get; set; }
        public Peronality Peronality { get; set; }
        public List<ClassAndLevel> Classes { get; set; }
        public List<Equipment> Equipment { get; set; }
        public List<FeatureAndTraits> FeatureAndTraits { get; set; }
        public List<AttacksAndSpellCasting> AttacksAndSpellCasting { get; set; }

        public Character()
        {
            AbilityScores = new List<AbilityScore>();
            Skills = new List<Skill>();
            Details = new Details();
            Classes = new List<ClassAndLevel>();
            Peronality = new Peronality();
            Equipment = new List<Equipment>();
            FeatureAndTraits = new List<FeatureAndTraits>();
            AttacksAndSpellCasting = new List<AttacksAndSpellCasting>();
        }
    }

    public class Details
    {
        public int CharacterLevel { get; set; }
        public bool Inspiration { get; set; }
        public int ProficencyBonus { get; set; }
        public int ArmorClass { get; set; }
        public int Initiative { get; set; } // More Complex
        public string Speed { get; set; }
        public Alignment Alignment { get; set; }
        public int MaxHitPoints { get; set; }
        public int CurrentHitPoints { get; set; }
        public int TempHitPoints { get; set; }

        public int DeathSaveSuccesses { get; set; }
        public int DeathSaveFailures { get; set; }
        public string TotalHitDice { get; set; }
        public string CurrentHitDice { get; set; }
        public int ExperincePoints { get; set; }
        public int ExperincePointsNeeded { get; set; }
        public string Senses { get; set; }
        public string Size { get; set; }

        public int Perception { get; set; }
        public int PerceptionMod { get; set; }

        public string Appearance { get; set; }
        public string Background { get; set; }

    }

    public class ClassAndLevel
    {
        public string Name { get; set; }
        public int LevelEarned { get; set; }
        public List<FeatureAndTraits> EarnedFeaturesAndTraits { get; set; }
    }

    public class AttacksAndSpellCasting
    {
        public string Name { get; set; }
        public int AttackBonus { get; set; }
        public string DamageType { get; set; }
    }

    public class AbilityScore
    {
        public Guid Id { get; set; }
        public AbilityNames Name { get; set; }
        public string CustomName { get; set; }
        public int Value { get; set; }
        public int Bonus { get; set; }
        public int Save { get; set; }
        public int SaveMod { get; set; }
        public int SaveProf { get; set; }
    }

    public class Skill
    {
        public Guid Id { get; set; }
        public string SourceId { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public AbilityScore Ability { get; set; }
        public int MiscBonus { get; set; }
        public int ProfBonus { get; set; }
    }

    public class Equipment 
    {
        public string Name { get; set; }
        public  string Description { get; set; }
        public string Location { get; set; }

    }

    public class FeatureAndTraits 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<StatAdjustment> StatAdjustments { get; set; }
    }

    public class StatAdjustment
    {
        public string AdjustedStat { get; set; }
        public int AdjustedAmount { get; set; }
    }

    public class Peronality
    {
        public string Traits { get; set; }
        public string Ideals { get; set; }
        public string Bonds { get; set; }
        public string Flaws { get; set; }
    }

}
