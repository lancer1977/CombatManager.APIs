using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CombatManager.Api;
using CombatManager.Api.Core;
using CombatManager.Api.Core.Request;
using NUnit.Framework;

namespace TestProject1
{
    /// <summary>
    /// Expects Combat manager to be running, 1 player, 1 monster, probably have to fix the chaId based on local data.
    /// </summary>
    public class Tests
    {
        ICombatManagerService _service;
        private string chaId = "53f7b6d8-fe60-45b7-b39a-3a3141b33bb9";
        [SetUp]
        public void Setup()
        {
            _service = new CombatManagerService("http://localhost:12457");
        }
        //public async Task<string> UIGoto(string place) => await Route(HttpVerbs.Get, string.Format("/ui/goto/{place}")); 
        [Test]
        public async Task GetCombatState()
        {
            var response = await _service.GetCombatState();
            Debug.WriteLine(response);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task GetCharacterDetails()
        {
            var response = await _service.GetCharacterDetails(chaId);
            Debug.WriteLine(response);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task CombatNext()
        {
            var response = await _service.CombatNext();
            Debug.WriteLine(response);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task CombatPrev()
        {
            var response = await _service.CombatPrev();
            Debug.WriteLine(response);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task CombatRollInit()
        {
            var response = await _service.CombatRollInit();
            Debug.WriteLine(response);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task MoveCharacterUp()
        {
            var response = await _service.MoveCharacterUp(chaId);
            Debug.WriteLine(response);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task MoveDownCharacter()
        {
            var response = await _service.MoveDownCharacter(chaId);
            Assert.IsNotNull(response);
        }
        //[Test,TestIgnore]
        public async Task DeleteCharacter()
        {
            var response = await _service.DeleteCharacter(chaId);
            Assert.IsNotNull(response);
        }
        [Test]
        public async Task ReadyCharacter()
        {
            var response = await _service.ReadyCharacter(chaId);
            Assert.IsNotNull(response);
            //Assert.IsTrue(response.CombatList.First(x => x.ID == new Guid(chaId)).IsActive);

        }
        [Test]
        public async Task UnreadyCharacter()
        {
            var response = await _service.UnreadyCharacter(chaId);
            Assert.IsNotNull(response);
            Assert.IsFalse(response.CombatList.First(x => x.ID == new Guid(chaId)).IsActive);
        }
        //public async Task<CombatState> DelayCharacter(string charid) => await Route<CombatState>(HttpVerbs.Get, "/combat/delay/{0}", charid);
        [Test]
        public async Task DelayCharacter()
        {
            var response = await _service.DelayCharacter(chaId);
            Assert.IsNotNull(response);
            //Assert.IsFalse(response.CombatList.First(x => x.ID == new Guid(chaId)).IsActive);
        }

        [Test]
        public async Task UndelayCharacter()
        {
            var response = await _service.UndelayCharacter(chaId);
            Assert.IsNotNull(response);
            Assert.IsFalse(response.CombatList.First(x => x.ID == new Guid(chaId)).IsActive);
        }

        [Test]
        public async Task CharacterActNow()
        {
            var response = await _service.CharacterActNow(chaId);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task ChangeHP()
        {
            var response = await _service.ChangeHP(chaId, 10);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task ChangeMaxHP()
        {
            var response = await _service.ChangeMaxHP(chaId, 10);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task ChangeTemporaryHP()
        {
            var response = await _service.ChangeTemporaryHP(chaId, 10);
            Assert.IsNotNull(response);
        }
        [Test]
        public async Task ChangeNonlethalDamage()
        {
            var response = await _service.ChangeNonlethalDamage(chaId, 10);
            Assert.IsNotNull(response);
        }
        [TestCase(true),
        TestCase(false)]
        public async Task HideCharacter(bool state)
        {
            var response = await _service.HideCharacter(chaId, state);
            Assert.IsNotNull(response);
        }
        [TestCase(true),
         TestCase(false)]
        public async Task IdleCharacter(bool state)
        {
            var response = await _service.IdleCharacter(chaId, state);
            Assert.IsNotNull(response);
        }
        [Test]
        public async Task AddCondition()
        {
            var response = await _service.AddCondition(new AddConditionRequest()
            {
                ID = new Guid(chaId),
                Name = "Prone",
                Turns = 5
            });
            Assert.IsNotNull(response);
        }
        [Test]
        public async Task RemoveCondition()
        {
            var response = await _service.RemoveCondition(new RemoveConditionRequest()
            {
                ID = new Guid(chaId),
                Name = "Prone"
            });
            Assert.IsNotNull(response);
        }
        [Test]
        public async Task ListMonsters()
        {
            var response = await _service.ListMonsters(new MonsterListRequest()
            {
                Name = "Goblin"
            });
            Assert.IsNotNull(response.Items.Any());
        }
        [TestCase(134, ExpectedResult = "Young Copper Dragon")]
        public async Task<string> GetMonster(int id)
        {
            var response = await _service.GetMonster(new MonsterRequest()
            {
                ID = id
            });
            return response.Name;
        }
        [TestCase(134, ExpectedResult = "Self-Sufficient")]
        public async Task<string> GetFeat(int id)
        {
            var response = await _service.GetFeat(new FeatRequest()
            {
                ID = id
            });
            return response.Name;
        }

        [TestCase(134, ExpectedResult = "Self-Sufficient")]
        public async Task<string> GetFeats(int id)
        {
            var response = await _service.GetFeats(new FeatsRequest()
            {
                Feats = new System.Collections.Generic.List<FeatRequest> { new FeatRequest() { ID = id } }
            });
            return response.Feats.First().Name;
        }

        [Test]
        public async Task ListFeats()
        {
            var response = await _service.ListFeats(new FeatListRequest()
            {
                IsCustom = false,
                //Type = "Combat",
                Name = "focus",
            });
            Assert.IsTrue(response.Items.Any());
            //Assert.IsNotNull(response);
        }
        //public async Task<string> GetSpell() => await Route(HttpVerbs.Post, "/spell/get");
        [Test]
        public async Task GetSpell()
        {
            var response = await _service.GetSpell(new SpellRequest()
            {
                ID = 134
            });
            Assert.IsTrue(response.Name == "Detect Chaos");
            //Assert.IsNotNull(response);
        }
        //public async Task<string> GetSpells() => await Route(HttpVerbs.Post, "/spell/fromlist");
        [Test]
        public async Task GetSpells()
        {
            var response = await _service.GetSpells(new SpellsRequest()
            {
                Spells = new List<SpellRequest>() { new SpellRequest()
                {
                    ID = 134
                }}
            });
            Assert.IsTrue(response.Spells.First().Name == "Detect Chaos");
            //Assert.IsNotNull(response);
        }
        [Test]
        public async Task ListSpells()
        {
            var response = await _service.ListSpells(new SpellListRequest()
            {
                Name = "magic",
            });
            Assert.IsTrue(response.Items.Any());
        }

        [Test]
        public async Task GetMagicItem()
        {
            var response = await _service.GetMagicItem(new MagicItemRequest()
            {
                ID = 309
            });
            Assert.IsTrue(response.Name == "Mithral Shirt");
            //Assert.IsNotNull(response);
        }

        [Test]
        public async Task GetMagicItems()
        {
            var response = await _service.GetMagicItems(new MagicItemsRequest()
            {
                MagicItems = new List<MagicItemRequest>() { new MagicItemRequest()
                {
                    ID = 309
                }}
            });
            Assert.IsTrue(response.MagicItems.First().Name == "Mithral Shirt");

            //Assert.IsNotNull(response);
        }
        [Test]
        public async Task ListMagicItems()
        {
            var response = await _service.ListMagicItems(new MagicItemListRequest()
            {
                Name = "Portable",
            });
            Assert.IsTrue(response.Items.Any());
        }





        [Test]
        public async Task ShowCombatList()
        {
            var response = await _service.ShowCombatList();
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task BringToFront()
        {
            var response = await _service.BringToFront();
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Minimize()
        {
            var response = await _service.Minimize();
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task HideCombatList()
        {
            var response = await _service.HideCombatList();
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task GetRegularMonster()
        {
            var response = await _service.GetRegularMonster(10);
            Assert.IsNotNull(response);
        }
        [Test]
        public async Task GetCustomMonster()
        {
            var response = await _service.GetCustomMonster(10);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task GetMonsters()
        {
            var response = await _service.GetMonsters(new MonstersRequest()
            {
                Monsters = new List<MonsterRequest>
              {
                  new MonsterRequest(){ID = 10},
              }
            });
            Assert.IsNotNull(response);
        }
        //public async Task<string> AddMonster() => await Route(HttpVerbs.Post, "/monster/add");
    }
}