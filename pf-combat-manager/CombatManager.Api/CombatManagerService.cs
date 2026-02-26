using System.Threading.Tasks;
using CombatManager.Api.Core;
using CombatManager.Api.Core.Data;
using CombatManager.Api.Core.Request; 

namespace CombatManager.Api
{
    public class CombatManagerService : ICombatManagerService
    {
        public CombatManagerService(string address)
        {
            RootAddress = address;
        }

        public CombatManagerService()
        {
        }
        public string RootAddress { get; set; }
        public string Passcode { get; set; }
        public string BaseAddress => RootAddress + "api/" ;


        private Task<T> RoutePost<T>(string address, object request)
        {
            return NetworkingExtensions.PostAsync<T>(BaseAddress,address, request);
        }

        private async Task<T> Route<T>( string partialAddress)
        { 
            return await NetworkingExtensions.GetAsync<T>(BaseAddress, partialAddress, Passcode);
        }
        private async Task<string> Route(string partialAddress)
        { 
            return await NetworkingExtensions.GetAsync(BaseAddress, partialAddress, Passcode);
        }
        public async Task<RemoteCombatState> GetCombatState() => await Route<RemoteCombatState>( "combat/state");
        public async Task<RemoteCombatState> CombatNext() => await Route<RemoteCombatState>( "combat/next");
        public async Task<RemoteCombatState> CombatPrev() => await Route<RemoteCombatState>( "combat/prev");
        public async Task<RemoteCombatState> CombatRollInit() => await Route<RemoteCombatState>( "combat/rollinit");
        public async Task<RemoteCharacter> GetCharacterDetails(string charid) => await Route<RemoteCharacter>( $"character/details/{charid}");
        public async Task<RemoteCombatState> MoveCharacterUp(string charid) => await Route<RemoteCombatState>( $"combat/moveupcharacter/{charid}");
        public async Task<RemoteCombatState> MoveDownCharacter(string charid) => await Route<RemoteCombatState>( $"combat/movedowncharacter/{charid}");
        public async Task<RemoteCombatState> DeleteCharacter(string charid) => await Route<RemoteCombatState>( $"combat/deletecharacter/{charid}");
        public async Task<RemoteCombatState> ReadyCharacter(string charid) => await Route<RemoteCombatState>( $"combat/ready/{charid}");
        public async Task<RemoteCombatState> UnreadyCharacter(string charid) => await Route<RemoteCombatState>( $"combat/unready/{charid}");
        public async Task<RemoteCombatState> DelayCharacter(string charid) => await Route<RemoteCombatState>( $"combat/delay/{charid}");
        public async Task<RemoteCombatState> UndelayCharacter(string charid) => await Route<RemoteCombatState>( $"combat/undelay/{charid}");
        public async Task<RemoteCombatState> CharacterActNow(string charid) => await Route<RemoteCombatState>( $"combat/actnow/{charid}");
        public async Task<RemoteCombatState> ChangeHP(string charid, int amount) => await Route<RemoteCombatState>( $"character/changehp/{charid}/{amount}");

        public async Task<RemoteCharacter> ChangeMaxHP(string charid, int amount) => await Route<RemoteCharacter>( $"character/changemaxhp/{charid}/{amount}");
        public async Task<RemoteCharacter> ChangeTemporaryHP(string charid, int amount) => await Route<RemoteCharacter>( $"character/changetemporaryhp/{charid}/{amount}");
        public async Task<RemoteCharacter> ChangeNonlethalDamage(string charid, int amount) => await Route<RemoteCharacter>( $"character/changenonlethaldamage/{charid}/{amount}");
        public async Task<RemoteCharacter> HideCharacter(string charid, bool state) => await Route<RemoteCharacter>( $"character/hide/{charid}/{state}");
        public async Task<RemoteCharacter> IdleCharacter(string charid, bool state) => await Route<RemoteCharacter>( $"character/idle/{charid}/{state}");
        public async Task<RemoteCharacter> AddCondition(AddConditionRequest request) => await RoutePost<RemoteCharacter>("character/addcondition", request);
        public async Task<RemoteCharacter> RemoveCondition(RemoveConditionRequest request) => await RoutePost<RemoteCharacter>("character/removecondition", request);
        public async Task<RemoteDBListing> ListMonsters(MonsterListRequest request) => await RoutePost<RemoteDBListing>("monster/list", request);
        public async Task<RemoteMonster> GetMonster(MonsterRequest request) => await RoutePost<RemoteMonster>("monster/get", request);
        public async Task<RemoteFeat> GetFeat(FeatRequest request) => await RoutePost<RemoteFeat>("feat/get", request);
        public async Task<RemoteFeatList> GetFeats(FeatsRequest request) => await RoutePost<RemoteFeatList>("feat/fromlist", request);
        public async Task<RemoteDBListing> ListFeats(FeatListRequest request) => await RoutePost<RemoteDBListing>("feat/list", request);
        public async Task<RemoteSpell> GetSpell(SpellRequest request) => await RoutePost<RemoteSpell>("spell/get", request);
        public async Task<RemoteSpellList> GetSpells(SpellsRequest request) => await RoutePost<RemoteSpellList>("spell/fromlist", request);
        public async Task<RemoteDBListing> ListSpells(SpellListRequest request) => await RoutePost<RemoteDBListing>("spell/list", request);
        public async Task<RemoteMagicItem> GetMagicItem(MagicItemRequest request) => await RoutePost<RemoteMagicItem>("magicitem/get", request);

        public async Task<RemoteMagicItemList> GetMagicItems(MagicItemsRequest request) => await RoutePost<RemoteMagicItemList>("magicitem/fromlist", request);
        public async Task<RemoteDBListing> ListMagicItems(MagicItemListRequest request) => await RoutePost<RemoteDBListing>("magicitem/list", request);
        public async Task<RemoteMonster> GetRegularMonster(int id) => await Route<RemoteMonster>( $"monster/getregular/{id}");
        public async Task<string> BringToFront() => await Route( "ui/bringtofront");
        public async Task<string> Minimize() => await Route( "ui/minimize");


        public async Task<string> UIGoto(string place) => await Route( $"ui/goto/{place}");
        public async Task<string> ShowCombatList() => await Route( "ui/showcombatlist");

        public async Task<string> HideCombatList() => await Route( "ui/hidecombatlist");
        public async Task<string> GetCustomMonster(int id) => await Route( $"monster/getcustom/{id}");
        public async Task<RemoteMonsterList> GetMonsters(MonstersRequest request) => await RoutePost<RemoteMonsterList>("monster/fromlist", request);
        public async Task<RemoteMonster> AddMonster(MonsterAddRequest request) => await RoutePost<RemoteMonster>("monster/add", request);
    }
}
