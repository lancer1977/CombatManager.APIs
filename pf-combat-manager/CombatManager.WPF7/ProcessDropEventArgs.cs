using System;
using System.Collections.Generic;
using System.Windows;

namespace CombatManager.WPF7;

/// <summary>
/// Event arguments used by the ListViewDragDropManager.ProcessDrop event.
/// </summary>
/// <typeparam name="ItemType">The type of data object being dropped.</typeparam>
public class ProcessDropEventArgs<ItemType> : EventArgs where ItemType : class
{

    IList<ItemType> itemsSource;
    ItemType dataItem;
    int oldIndex;
    int newIndex;
    DragDropEffects allowedEffects = DragDropEffects.None;
    DragDropEffects effects = DragDropEffects.None;

    internal ProcessDropEventArgs(
        IList<ItemType> itemsSource, 
        ItemType dataItem, 
        int oldIndex, 
        int newIndex, 
        DragDropEffects allowedEffects )
    {
        this.itemsSource = itemsSource;
        this.dataItem = dataItem;
        this.oldIndex = oldIndex;
        this.newIndex = newIndex;
        this.allowedEffects = allowedEffects;
    }


    /// <summary>
    /// The items source of the ListView where the drop occurred.
    /// </summary>
    public IList<ItemType> ItemsSource
    {
        get { return itemsSource; }
    }

    /// <summary>
    /// The data object which was dropped.
    /// </summary>
    public ItemType DataItem
    {
        get { return dataItem; }
    }

    /// <summary>
    /// The current index of the data item being dropped, in the ItemsSource collection.
    /// </summary>
    public int OldIndex
    {
        get { return oldIndex; }
    }

    /// <summary>
    /// The target index of the data item being dropped, in the ItemsSource collection.
    /// </summary>
    public int NewIndex
    {
        get { return newIndex; }
    }

    /// <summary>
    /// The drag drop effects allowed to be performed.
    /// </summary>
    public DragDropEffects AllowedEffects
    {
        get { return allowedEffects; }
    }

    /// <summary>
    /// The drag drop effect(s) performed on the dropped item.
    /// </summary>
    public DragDropEffects Effects
    {
        get { return effects; }
        set { effects = value; }
    }

}