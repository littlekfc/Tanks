using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UI.Components
{
    public class ListAdapter : MonoBehaviour
    {
        public ListItem item;

        private Type itemType;
        protected Type ItemType
        {
            get
            {
                if (itemType == null && item != null)
                    itemType = item.GetType();

                return itemType;
            }
        }

        private Transform grid;
        protected Transform Grid
        {
            get
            {
                if (grid == null)
                {
                    var grid_layout = GetComponentInChildren<GridLayoutGroup>();
                    if (grid_layout != null)
                        grid = grid_layout.transform;
                }

                return grid;
            }
        }

        private IList<ListItem> items = new List<ListItem>();
        public IList<ListItem> Items
        {
            get
            {
                return items;
            }
        }

        public void Add<T>(T data) where T : class
        {
            if (typeof(ListItem<T>) == ItemType)
            {
                var new_item = Instantiate(item) as ListItem<T>;
                new_item.Reset(data);

                new_item.transform.parent = Grid;
                Items.Add(new_item);
            }
        }

        public void Add<T>(IEnumerable<T> data) where T : class
        {
            if (typeof(ListItem<T>) == ItemType)
                foreach (var d in data)
                    Add(d);
        }

        public void Clear()
        {
            foreach (var i in Items)
                Destroy(i.gameObject);

            Items.Clear();
        }

        public void Remove(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                Destroy(Items[index]);
                Items.RemoveAt(index);
            }
        }
    }
}