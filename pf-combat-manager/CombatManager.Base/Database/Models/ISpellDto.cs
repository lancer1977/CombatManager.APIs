namespace CombatManager.Database.Models;

public interface ISpellDto
{
    public string castingtime { get; set; }
        
    string costlycomponents { get; set; }
    string Name{get;set;}
    string school{get;set;}
    string subschool{get;set;}
    string descriptor{get;set;}
    string spellLevel{get;set;}
    string castingTime{get;set;}
    string components{get;set;}
    string costlyComponents{get;set;}
    string range{get;set;}
    string targets{get;set;}
    string effect{get;set;}
    string dismissible{get;set;}
    string area{get;set;}
    string duration{get;set;}
    string shapeable{get;set;}
    string savingThrow{get;set;}
    string spellResistence{get;set;}
    string description{get;set;}
    string descriptionFormated{get;set;}
    string source{get;set;}
    string fullText{get;set;}
    string verbal{get;set;}
    string somatic{get;set;}
    string material{get;set;}
    string focus{get;set;}
    string divineFocus{get;set;}
    string sor{get;set;}
    string wiz{get;set;}
    string cleric{get;set;}
    string druid{get;set;}
    string ranger{get;set;}
    string bard{get;set;}
    string paladin{get;set;}
    string alchemist{get;set;}
    string summoner{get;set;}
    string witch{get;set;}
    string inquisitor{get;set;}
    string oracle{get;set;}
    string antipaladin{get;set;}
    string assassin{get;set;}
    string adept{get;set;}
    string redMantisAssassin{get;set;}
    string magus{get;set;}
    string url{get;set;}
    string slaLevel{get;set;}
    string preparationTime{get;set;}
    bool duplicated{get;set;}
    string acid{get;set;}
    string air{get;set;}
    string chaotic{get;set;}
    string cold{get;set;}
    string curse{get;set;}
    string darkness{get;set;}
    string death{get;set;}
    string disease{get;set;}
    string earth{get;set;}
    string electricity{get;set;}
    string emotion{get;set;}
    string evil{get;set;}
    string fear{get;set;}
    string fire{get;set;}
    string force{get;set;}
    string good{get;set;}
    string language{get;set;}
    string lawful{get;set;}
    string light{get;set;}
    string mindAffecting{get;set;}
    string pain{get;set;}
    string poison{get;set;}
    string shadow{get;set;}
    string sonic{get;set;}
    string water{get;set;}

    //annotation fields

    //bonus annotation
    ConditionBonus bonus{get;set;}
    //treasure table annotations
    string potionWeight{get;set;}
    string divineScrollWeight{get;set;}
    string arcaneScrollWeight{get;set;}
    string wandWeight{get;set;}
    string potionLevel{get;set;}
    string potionCost{get;set;}
    string arcaneScrollLevel{get;set;}
    string arcaneScrollCost{get;set;}
    string divineScrollLevel{get;set;}
    string divineScrollCost{get;set;}
    string wandLevel{get;set;}
    string wandCost{get;set;}

}