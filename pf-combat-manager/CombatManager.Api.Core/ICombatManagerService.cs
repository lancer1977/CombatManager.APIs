using System.Threading.Tasks;
using CombatManager.Api.Core.Data;
using CombatManager.Api.Core.Request;

namespace CombatManager.Api.Core
{
    public interface ICombatManagerService
    {
        Task<RemoteCombatState> GetCombatState();
        Task<RemoteCombatState> CombatNext();
        Task<RemoteCombatState> CombatPrev();
        Task<RemoteCombatState> CombatRollInit();
        Task<RemoteCharacter> GetCharacterDetails(string charid);
        Task<RemoteCombatState> MoveCharacterUp(string charid);
        Task<RemoteCombatState> MoveDownCharacter(string charid);
        Task<RemoteCombatState> DeleteCharacter(string charid);
        Task<RemoteCombatState> ReadyCharacter(string charid);
        Task<RemoteCombatState> UnreadyCharacter(string charid);
        Task<RemoteCombatState> DelayCharacter(string charid);
        Task<RemoteCombatState> UndelayCharacter(string charid);
        Task<RemoteCombatState> CharacterActNow(string charid);
        Task<RemoteCombatState> ChangeHP(string charid, int amount);
        Task<RemoteCharacter> ChangeMaxHP(string charid, int amount);
        Task<RemoteCharacter> ChangeTemporaryHP(string charid, int amount);
        Task<RemoteCharacter> ChangeNonlethalDamage(string charid, int amount);
        Task<RemoteCharacter> HideCharacter(string charid, bool state);
        Task<RemoteCharacter> IdleCharacter(string charid, bool state);
        Task<RemoteCharacter> AddCondition(AddConditionRequest request);
        Task<RemoteCharacter> RemoveCondition(RemoveConditionRequest request);
        Task<RemoteDBListing> ListMonsters(MonsterListRequest request);
        Task<RemoteMonster> GetMonster(MonsterRequest request);
        Task<RemoteFeat> GetFeat(FeatRequest request);
        Task<RemoteFeatList> GetFeats(FeatsRequest request);
        Task<RemoteDBListing> ListFeats(FeatListRequest request);
        Task<RemoteSpell> GetSpell(SpellRequest request);
        Task<RemoteSpellList> GetSpells(SpellsRequest request);
        Task<RemoteDBListing> ListSpells(SpellListRequest request);
        Task<RemoteMagicItem> GetMagicItem(MagicItemRequest request);
        Task<RemoteMagicItemList> GetMagicItems(MagicItemsRequest request);
        Task<RemoteDBListing> ListMagicItems(MagicItemListRequest request);
        Task<RemoteMonster> GetRegularMonster(int id);
        Task<string> BringToFront();
        Task<string> Minimize();
        Task<string> UIGoto(string place);
        Task<string> ShowCombatList();
        Task<string> HideCombatList();
        Task<string> GetCustomMonster(int id);
        Task<RemoteMonsterList> GetMonsters(MonstersRequest request);
        Task<RemoteMonster> AddMonster(MonsterAddRequest request);
    }
}