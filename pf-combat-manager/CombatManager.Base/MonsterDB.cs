/*
 *  MonsterDB.cs
 *
 *  Copyright (C) 2010-2012 Kyle Olson, kyle@kyleolson.com
 *
 *  This program is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU General Public License
 *  as published by the Free Software Foundation; either version 2
 *  of the License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 *
 */

using CombatManager.Interfaces;
using CombatManager.ViewModels;

namespace CombatManager
{
    public class MonsterRepository : IDisposable
    {
        
        private ISQLService<Monster> _monsterService; 

        private bool _mythicCorrected;
        
        public static event Action<Monster> MonsterUpdated;


        public MonsterRepository(ISQLService<Monster> monsterService)
        {
            _monsterService = monsterService;
        }

 

        public IEnumerable<Monster> Monsters
        {
            get
            {
                var v = _monsterService.GetAll();

                if (!_mythicCorrected)
                {
                    foreach (var monster in v)
                    {
                        if (monster.Mythic != "0" || monster.Mr != null)
                        {
                            monster.Mythic = "0";
                            monster.Mr = null;
                            UpdateMonster(monster);
                        }
                    }
                    _mythicCorrected = true;
                }

                return v;
            }
        }

        public void AddMonsterNotify(Monster monster)
        {
            AddMonster(monster);
            MonsterUpdated?.Invoke(monster);
        }


        public void AddMonster(Monster monster)
        {
            //clear conditions before adding to DB
            List<ActiveCondition> list = new List<ActiveCondition>();
            foreach (ActiveCondition ac in monster.ActiveConditions)
            {
                list.Add(ac);
            }
            foreach (ActiveCondition ac in list)
            {
                monster.RemoveCondition(ac);
            }

            _monsterService.AddItem(monster);
        }

        public void UpdateMonster(Monster monster)
        {
            _monsterService.UpdateItem(monster);
            MonsterUpdated?.Invoke(monster);
        }

        public void DeleteMonster(Monster monster)
        {
            _monsterService.DeleteItem(monster);
        }

        public void Dispose()
        {
            _monsterService.Dispose();
            _monsterService = null;
        }

       

   
    }
}
