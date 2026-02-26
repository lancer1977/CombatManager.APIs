using EmbedIO.WebApi;
using System.Threading.Tasks;
using CombatManager.Api.Core.Data;
using CombatManager.Api.Core.Request;
using CombatManager.Utilities;
using CombatManager.ViewModels;
using EmbedIO;
using EmbedIO.Routing; 

#pragma warning disable 618

namespace CombatManager.LocalService
{
 

    public class LocalCombatManagerServiceController : WebApiController//, ILocalCombatManagerService
    {
        CombatState _state;
        LocalCombatManagerService.ActionCallback _actionCallback;
        Action _saveCallback;
        LocalCombatManagerService _service;

        public LocalCombatManagerServiceController(IHttpContext context, CombatState state, LocalCombatManagerService service, LocalCombatManagerService.ActionCallback actionCallback, Action saveCallback)
            
        {
            this._state = state;
            this._service = service;
            this._actionCallback = actionCallback;
            this._saveCallback = saveCallback;
        }

        private class ResultHandler
        {
            public ResultHandler()
            {
                Code = System.Net.HttpStatusCode.BadRequest;
            }

            public object Data { get; set; }
            public bool Failed { get; set; }

            public System.Net.HttpStatusCode Code { get; set; }

        }

        public Character.HpMode HpMode { get; set; }

        public string Passcode { get; set; }

        [Route(HttpVerbs.Get, "/combat/state")]
        public async Task<object> GetCombatState()
        {
            return await TakeAction((res) =>
            {
                res.Data = _state.ToRemote();
            });
        }

        [Route(HttpVerbs.Get, "/combat/next")]
        public async Task<object> CombatNext()
        {
            return await TakeAction((res) =>
            {
                _state.MoveNext();
                _saveCallback();
                res.Data = _state.ToRemote();
            });
        }

        [Route(HttpVerbs.Get, "/combat/prev")]
        public async Task<object> CombatPrev()
        {

            return await TakeAction((res) =>
                {
                    _state.MovePrevious();
                    _saveCallback();
                    res.Data = _state.ToRemote();
                });
        }

        [Route(HttpVerbs.Get, "/combat/rollinit")]
        public async Task<object> CombatRollInit()
        {
            return await TakeAction((res) =>
                {
                    _state.RollInitiative();
                    _state.SortCombatList();
                    _saveCallback();
                    res.Data = _state.ToRemote();
                }
                );

        }

        [Route(HttpVerbs.Get, "/character/details/{charid}")]
        public async Task<object> GetCharacterDetails(string charid)
        {

            return await TakeCharacterAction(charid, (res, ch) =>
            {
                res.Data = ch.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/combat/moveupcharacter/{charid}")]
        public async Task<object> MoveCharacterUp(string charid)
        {

            return await TakeCharacterAction(charid, (res, ch) =>
            {

                _state.MoveUpCharacter(ch);
                _saveCallback();
                res.Data = _state.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/combat/movedowncharacter/{charid}")]
        public async Task<object> MoveDownCharacter(string charid)
        {

            return await TakeCharacterAction(charid, (res, ch) =>
            {

                _state.MoveDownCharacter(ch);
                _saveCallback();
                res.Data = _state.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/combat/deletecharacter/{charid}")]
        public async Task<object> DeleteCharacter(string charid)
        {

            return await TakeCharacterAction(charid, (res, ch) =>
            {

                _state.RemoveCharacter(ch);
                _saveCallback();
                res.Data = _state.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/combat/ready/{charid}")]
        public async Task<object> ReadyCharacter(string charid)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                ch.IsReadying = true;
                _saveCallback();
                res.Data = _state.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/combat/unready/{charid}")]
        public async Task<object> UnreadyCharacter(string charid)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                ch.IsReadying = false;
                _saveCallback();
                res.Data = _state.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/combat/delay/{charid}")]
        public async Task<object> DelayCharacter(string charid)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                ch.IsDelaying = true;
                _saveCallback();
                res.Data = _state.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/combat/undelay/{charid}")]
        public async Task<object> UndelayCharacter(string charid)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                ch.IsDelaying = false;
                _saveCallback();
                res.Data = _state.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/combat/actnow/{charid}")]
        public async Task<object> CharacterActNow(string charid)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                _state.CharacterActNow(ch);
                _saveCallback();
                res.Data = _state.ToRemote();

            });
        }


        [Route(HttpVerbs.Get, "/character/changehp/{charid}/{amount}")]
        public async Task<object> ChangeHp(string charid, int amount)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                ch.Adjuster.Hp += amount;
                _saveCallback();
                res.Data = ch.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/character/changemaxhp/{charid}/{amount}")]
        public async Task<object> ChangeMaxHp(string charid, int amount)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                ch.MaxHp += amount;
                _saveCallback();
                res.Data = ch.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/character/changetemporaryhp/{charid}/{amount}")]
        public async Task<object> ChangeTemporaryHp(string charid, int amount)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                ch.Adjuster.TemporaryHp += amount;
                _saveCallback();
                res.Data = ch.ToRemote();

            });
        }


        [Route(HttpVerbs.Get, "/character/changenonlethaldamage/{charid}/{amount}")]
        public async Task<object> ChangeNonlethalDamage(string charid, int amount)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                ch.Adjuster.NonlethalDamage += amount;
                _saveCallback();
                res.Data = ch.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/character/hide/{charid}/{state}")]
        public async Task<object> HideCharacter(string charid, bool state)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                ch.IsHidden = state;
                _saveCallback();
                res.Data = ch.ToRemote();

            });
        }

        [Route(HttpVerbs.Get, "/character/idle/{charid}/{state}")]
        public async Task<object> IdleCharacter(string charid, bool state)
        {
            return await TakeCharacterAction(charid, (res, ch) =>
            {
                ch.IsIdle = state;
                _saveCallback();
                res.Data = ch.ToRemote();

            });
        }


        [Route(HttpVerbs.Post, "/character/addcondition")]
        public async Task<object> AddCondition()
        {
            return await TakeCharacterPostAction<AddConditionRequest>((res, data, ch) =>
            {
                Condition c = Condition.ByName(data.Name);
                if (c == null)
                {
                    res.Failed = true;
                    return;
                }
                ActiveCondition ac = new ActiveCondition();
                ac.Condition = c;
                ac.InitiativeCount = _state.CurrentInitiativeCount;
                ac.Turns = data.Turns;
                ch.Monster.AddCondition(ac);
                _saveCallback();

                res.Data = ch.ToRemote();

            });
        }


        [Route(HttpVerbs.Post, "/character/removecondition")]
        public async Task<object> RemoveCondition()
        {
            return await TakeCharacterPostAction<RemoveConditionRequest>((res, data, ch) =>
            {
                ch.RemoveConditionByName(data.Name);
                _saveCallback();

                res.Data = ch.ToRemote();

            });
        }

        [Route(HttpVerbs.Post, "/monster/list")]
        public async Task<object> ListMonsters()
        {
            return await TakePostAction<MonsterListRequest>((res, data) =>
            {
                int? maxCr =  Monster.TryGetCrChartInt(data.MaxCR);
                int? minCr = Monster.TryGetCrChartInt(data.MinCR);
                res.Data= LocalRemoteConverter.CreateRemoteMonsterList(m =>
                {
                    int? crInt = m.IntCr;


                    try
                    {
                        return (data.Name.IsEmptyOrNull() || m.Name.ToUpper().Contains(data.Name.ToUpper()))
                        && (data.IsCustom == null || m.IsCustom == data.IsCustom)
                        && (data.IsNPC == null || m.Npc == data.IsNPC)
                        && (crInt.IsNullOrBetweenInclusive(minCr, maxCr)
                        );
                    }
                    catch (Exception ex)
                    {
                        if (ex != null)
                        {

                        }
                        return false;
                    }
                    
                });

            });
        }

        [Route(HttpVerbs.Post, "/monster/get")]
        public async Task<object> GetMonster()
        {

            return await TakePostAction<MonsterRequest>((res, data) =>
            {
                 res.Data = Monster.ById(data.IsCustom, data.ID).ToRemote();
                 
            });
        }

        [Route(HttpVerbs.Post, "/feat/get")]
        public async Task<object> GetFeat()
        {
            return await TakePostAction<FeatRequest>((res, data) =>
            {
                res.Data = Feat.ById(data.IsCustom, data.ID).ToRemote();

            });
        }

        [Route(HttpVerbs.Post, "/feat/fromlist")]
        public async Task<object> GetFeats()
        {
            return await TakePostAction<FeatsRequest>((res, data) =>
            {
                RemoteFeatList list = new RemoteFeatList();
                list.Feats = new List<RemoteFeat>();
                if (data.Feats != null)
                {
                    foreach (var fr in data.Feats)
                    {
                        Feat f = Feat.ById(fr.IsCustom, fr.ID);
                        list.Feats.Add(f.ToRemote());
                    }
                }
                res.Data = list;
            });
        }


        [Route(HttpVerbs.Post, "/feat/list")]
        public async Task<object> ListFeats()
        {
            return await TakePostAction<FeatListRequest>((res, data) =>
            {
               

                res.Data = LocalRemoteConverter.CreateRemoteFeatList(m =>
                {
                    try
                    {
                        return CmStringUtilities.ContainsOrNullIgnoreCase(m.Name, data.Name)
                        && (data.IsCustom == null || m.IsCustom == data.IsCustom)
                        && (data.Type == null || m.Types.Contains(data.Type));
                    }
                    catch (Exception ex)
                    {
                        if (ex != null)
                        {

                        }
                        return false;
                    }

                });

            });
        }

        [Route(HttpVerbs.Post, "/spell/get")]
        public async Task<object> GetSpell()
        {
            return await TakePostAction<SpellRequest>((res, data) =>
            {
                res.Data = Spell.ById(data.IsCustom, data.ID).ToRemote();

            });
        }

        [Route(HttpVerbs.Post, "/spell/fromlist")]
        public async Task<object> GetSpells()
        {
            return await TakePostAction<SpellsRequest>((res, data) =>
            {
                RemoteSpellList list = new RemoteSpellList();
                list.Spells = new List<RemoteSpell>();
                if (data.Spells != null)
                {
                    foreach (var sr in data.Spells)
                    {
                        Spell s = Spell.ById(sr.IsCustom, sr.ID);
                        list.Spells.Add(s.ToRemote());
                    }
                }
                res.Data = list;
            });
        }

        [Route(HttpVerbs.Post, "/spell/list")]
        public async Task<object> ListSpells()
        {
            return await TakePostAction<SpellListRequest>((res, data) =>
            {


                res.Data = LocalRemoteConverter.CreateRemoteSpellList(m =>
                {
                    try
                    {
                        return CmStringUtilities.ContainsOrNullIgnoreCase(m.Name, data.Name)
                        && (data.IsCustom == null || m.IsCustom == data.IsCustom)
                        && (data.School == null || m.School.Equals(data.School, StringComparison.CurrentCultureIgnoreCase))
                        && CmStringUtilities.ContainsOrNullIgnoreCase(m.Subschool, data.Subschool);
                    }
                    catch (Exception ex)
                    {
                        if (ex != null)
                        {

                        }
                        return false;
                    }

                });

            });
        }

        [Route(HttpVerbs.Post, "/magicitem/get")]
        public async Task<object> GetMagicItem()
        {
            return await TakePostAction<MagicItemRequest>((res, data) =>
            {
                res.Data = MagicItem.ById(data.IsCustom, data.ID).ToRemote();

            });
        }

        [Route(HttpVerbs.Post, "/magicitem/fromlist")]
        public async Task<object> GetMagicItems()
        {
            return await TakePostAction<MagicItemsRequest>((res, data) =>
            {
                RemoteMagicItemList list = new RemoteMagicItemList();
                list.MagicItems = new List<RemoteMagicItem>();
                if (data.MagicItems != null)
                {
                    foreach (var mir in data.MagicItems)
                    {
                        MagicItem mi = MagicItem.ById(mir.IsCustom, mir.ID);
                        list.MagicItems.Add(mi.ToRemote());
                    }
                }
                res.Data = list;
            });
        }

        [Route(HttpVerbs.Post, "/magicitem/list")]
        public async Task<object> ListMagicItems()
        {
            return await TakePostAction<MagicItemListRequest>((res, data) =>
            {


                res.Data = LocalRemoteConverter.CreateRemoteMagicItemList(m =>
                {

                    try
                    {
                        return m.Name.ContainsOrNullIgnoreCase(data.Name)
                        && (data.IsCustom == null || m.IsCustom == data.IsCustom)
                        && m.Slot.ContainsOrNullIgnoreCase(data.Slot)
                        && m.Group.ContainsOrNullIgnoreCase(data.Group)
                        && (data.MinCL == null || m.Cl >= data.MinCL.Value)
                        && (data.MaxCL == null || m.Cl <= data.MaxCL.Value);
                    }
                    catch (Exception ex)
                    {
                        if (ex != null)
                        {

                        }
                        return false;
                    }

                });

            });
        }


        [Route(HttpVerbs.Get, "/monster/getregular/{id}")]
        public async Task<object> GetRegularMonster(int id)
        {
            return await TakeAction( (res) =>
            {
                res.Data = Monster.ByDetailsId(id).ToRemote();
            });
        }

        [Route(HttpVerbs.Get, "/ui/bringtofront")]
        public async Task<object> BringToFront()
        {

            return await Precheck(async () =>
            {
                _service.TakeUiAction(LocalCombatManagerService.UiAction.BringToFront);
                return await Task.FromResult(new { res = true });
            });
        }

        [Route(HttpVerbs.Get, "/ui/minimize")]
        public async Task<object> Minimize()
        {

            return await Precheck(async () =>
            {
                _service.TakeUiAction(LocalCombatManagerService.UiAction.Minimize);
                return await Task.FromResult(new { res = true });
            });
        }

        [Route(HttpVerbs.Get, "/ui/goto/{place}")]
        public async Task<object> UiGoto(string place)
        {

            return await Precheck(async () =>
            {
                _service.TakeUiAction(LocalCombatManagerService.UiAction.Goto, place);
                return await Task.FromResult(new { res = true });
            });
        }

        [Route(HttpVerbs.Get, "/ui/showcombatlist")]
        public async Task<object> ShowCombatList()
        {

            return await Precheck(async () =>
            {
                _service.TakeUiAction(LocalCombatManagerService.UiAction.ShowCombatListWindow);
                return await Task.FromResult(new { res = true });
            });
        }


        [Route(HttpVerbs.Get, "/ui/hidecombatlist")]
        public async Task<object> HideCombatList()
        {

            return await Precheck(async () =>
            {
                _service.TakeUiAction(LocalCombatManagerService.UiAction.HideCombatListWindow);
                return await Task.FromResult(new { res = true });
            });
        }



        [Route(HttpVerbs.Get, "/monster/getcustom/{id}")]
        public async Task<object> GetCustomMonster(int id)
        {
            return await TakeAction((res) =>
            {
                res.Data = Monster.ByDbLoaderId(id).ToRemote();
            });
        }

        [Route(HttpVerbs.Post, "/monster/fromlist")]
        public async Task<object> GetMonsters()
        {
            return await TakePostAction<MonstersRequest>((res, data) =>
            {
                RemoteMonsterList list = new RemoteMonsterList();
                list.Monsters = new List<RemoteMonster>();
                if (data.Monsters != null)
                {
                    foreach (var mr in data.Monsters)
                    {
                        Monster m = Monster.ById(mr.IsCustom, mr.ID);
                        list.Monsters.Add(m.ToRemote());
                    }
                }
                res.Data = list;
            });
        }

        [Route(HttpVerbs.Post, "/monster/add")]
        public async Task<object> AddMonster()
        {
            return await TakePostAction<MonsterAddRequest>((res, data) =>
            {

                foreach (var mr in data.Monsters)
                {
                    Monster m = Monster.ById(mr.IsCustom, mr.ID);
                    if (m != null)
                    {
                        Character ch = _state.AddMonster(m, HpMode, data.IsMonster);
                        if (!data.Name.IsEmptyOrNull())
                        {
                            ch.Name = data.Name;
                        }
                        res.Data = ch.ToRemote();

                    }
                }
                _saveCallback();
            });
        }

        

        private async Task<object> Precheck(Func<Task<object>> function)
        {
            if (!Passcode.IsEmptyOrNull())
            {
                if (!HttpContext.HasRequestHeader("passcode"))
                {
                    throw HttpException.Forbidden();                }
                else
                {
                    string matchcode = HttpContext.RequestHeader("Passcode");
                    if (matchcode != Passcode)
                    {
                        throw HttpException.Forbidden();
                    }
                }
            }
            return await function();
        }


        private async Task<object> TakeAction(Action<ResultHandler> resAction)
        {
            return await Precheck(async () =>
            {
                try
                {
                    ResultHandler res = new ResultHandler();


                    _actionCallback(() =>
                    {
                        try
                        {
                            resAction(res);
                        }
                        catch (Exception)
                        {
                            res.Failed = true;
                        }

                    });
                    if (res.Failed)
                    {
                        throw new HttpException(res.Code);
                    }
                    else
                    {
                        return await Task.FromResult(res.Data);
                    }
                }
                catch (Exception ex)
                {
                    throw new HttpException(System.Net.HttpStatusCode.InternalServerError, ex.ToString());
                }
            });
        }

        private async Task<object> TakeCharacterAction(string charid, Action<ResultHandler, Character> handler)
        {
            return await TakeAction((res) =>
            {
                Guid id;

                Character ch = null;

                if (charid == "current")

                {
                    id = Guid.Empty;
                }
                else
                {
                    if (!Guid.TryParse(charid, out id))
                    {
                        res.Failed = true;
                        return;
                    }
                }


                if (id == Guid.Empty)
                {
                    ch = _state.CurrentCharacter;
                }
                else
                {
                    ch = _state.GetCharacterById(id);
                }
                if (ch != null)
                {
                    try
                    {
                        handler(res, ch);
                    }
                    catch
                    {
                        res.Failed = true;
                    }
                }
                else
                {
                    res.Failed = true;
                }

            });
        }

        private async Task<object> TakePostAction<T>(Action<ResultHandler, T> resAction) where T : class
        {

            return await Precheck(async () =>
            {
                try
                {
                    ResultHandler res = new ResultHandler();
                    T data = await HttpContext.GetRequestDataAsync<T>();

                    if (data == null)
                    {
                        res.Failed = true;
                    }
                    else
                    {

                        _actionCallback(() =>
                        {
                            try
                            {
                                resAction(res, data);
                            }
                            catch (Exception)
                            {
                                res.Failed = true;
                            }

                        });
                    }
                    if (res.Failed)
                    {
                        throw new HttpException(res.Code, new ArgumentException().ToString());
                    }
                    else
                    {
                        return res.Data;
                    }

                }
                catch (Exception ex)
                {
                    throw new HttpException(System.Net.HttpStatusCode.InternalServerError, ex.ToString());
                }
            });
        }

        private async Task<object> TakeCharacterPostAction<T>(Action<ResultHandler, T, Character> resAction) where T : CharacterRequest
        {
            return await TakePostAction<T>((res, data) =>
            {

                Character ch = _state.GetCharacterById(data.ID);
                if (ch != null)
                {
                    try
                    {
                        resAction(res, data, ch);
                    }
                    catch
                    {
                        res.Failed = true;
                    }
                }
                else
                {
                    res.Failed = true;
                }

            });
        }

       

    }

}
