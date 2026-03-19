/*
 *  DBTableDesc.cs
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

namespace CombatManager
{
    public class DbTableDesc
    {
        private string _name;
        private List<DbFieldDesc> _fields;
        private bool _primary;
        private Type _type;

        private List<DbFieldDesc> _subtableFields;
        private List<DbFieldDesc> _valueFields;


        public DbTableDesc()
        {
            _fields = new List<DbFieldDesc>();
        }

        public DbTableDesc(string name, Type type)
        {
            _name = name;
            _fields = new List<DbFieldDesc>();
            _type = type;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
            }
        }
        public List<DbFieldDesc> Fields
        {
            get => _fields;
            set
            {
                if (_fields != value)
                {
                    _fields = value;
                    _valueFields = null;
                    _subtableFields = null;
                }
            }
        }
        public bool Primary
        {
            get => _primary;
            set
            {
                if (_primary != value)
                {
                    _primary = value;
                }
            }
        }
        public Type Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                }
            }
        }
        public IEnumerable<DbFieldDesc> SubtableFields
        {
            get
            {
                if (_subtableFields == null)
                {
                    SortFields();
                }
                return _subtableFields;
            }
        }
        public IEnumerable<DbFieldDesc> ValueFields
        {
            get
            {
                if (_valueFields == null)
                {
                    SortFields();
                }
                return _valueFields;
            }
        }

        private void SortFields()
        {
            _subtableFields = new List<DbFieldDesc>();
            _valueFields = new List<DbFieldDesc>();
            foreach (DbFieldDesc field in _fields)
            {
                if (field.Subtable != null)
                {
                    _subtableFields.Add(field);
                }
                else
                {
                    _valueFields.Add(field);
                }
            }
        }
    }

    public class DbFieldDesc
    {
        public DbFieldDesc()
        {
        }

        public DbFieldDesc(string name, string type, bool nullable, PropertyInfo info)
        {
            _name = GetUsableName(name);
            _originalName = name;
            _type = type;
            _nullable = nullable;
            _info = info;
        }

        private string _name;
        private string _originalName;
        private string _type;
        private DbTableDesc _subtable;
        private bool _nullable;
        private PropertyInfo _info;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
            }
        }
        public string OriginalName => _originalName;

        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                }
            }
        }
        public bool Nullable
        {
            get => _nullable;
            set
            {
                if (_nullable != value)
                {
                    _nullable = value;
                }
            }
        }
        public DbTableDesc Subtable
        {
            get => _subtable;
            set => _subtable = value;
        }
        public PropertyInfo Info
        {
            get => _info;
            set => _info = value;
        }


        public static string GetUsableName(string name)
        {
            if (string.Compare(name, "Group", true) == 0)
            {
                return "_" + name;
            }
            if (string.Compare(name, "Limit", true) == 0)
            {
                return "_" + name;
            }
            return name;

        }

    }

    public interface IDbLoadable
    {
        int DbLoaderId
        {
            get;
            set;
        }

    }
    

}
