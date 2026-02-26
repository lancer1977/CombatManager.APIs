using System.Windows;
using System.Windows.Controls;

namespace CombatManager.WPF7;

/// <summary>
/// Exposes attached properties used in conjunction with the ListViewDragDropManager class.
/// Those properties can be used to allow triggers to modify the appearance of ListBoxItems
/// in a ListView during a drag-drop operation.
/// </summary>
public static class ListBoxItemDragState
{
    /// <summary>
    /// Identifies the ListBoxItemDragState's IsBeingDragged attached property.  
    /// This field is read-only.
    /// </summary>
    public static readonly DependencyProperty IsBeingDraggedProperty =
        DependencyProperty.RegisterAttached(
            "IsBeingDragged",
            typeof( bool ),
            typeof( ListBoxItemDragState ),
            new UIPropertyMetadata( false ) );

    /// <summary>
    /// Returns true if the specified ListBoxItem is being dragged, else false.
    /// </summary>
    /// <param name="item">The ListBoxItem to check.</param>
    public static bool GetIsBeingDragged( ListBoxItem item )
    {
        return (bool)item.GetValue( IsBeingDraggedProperty );
    }

    /// <summary>
    /// Sets the IsBeingDragged attached property for the specified ListBoxItem.
    /// </summary>
    /// <param name="item">The ListBoxItem to set the property on.</param>
    /// <param name="value">Pass true if the element is being dragged, else false.</param>
    internal static void SetIsBeingDragged( ListBoxItem item, bool value )
    {
        item.SetValue( IsBeingDraggedProperty, value );
    }

    /// <summary>
    /// Identifies the ListBoxItemDragState's IsUnderDragCursor attached property.  
    /// This field is read-only.
    /// </summary>
    public static readonly DependencyProperty IsUnderDragCursorProperty =
        DependencyProperty.RegisterAttached(
            "IsUnderDragCursor",
            typeof( bool ),
            typeof( ListBoxItemDragState ),
            new UIPropertyMetadata( false ) );

    /// <summary>
    /// Returns true if the specified ListBoxItem is currently underneath the cursor 
    /// during a drag-drop operation, else false.
    /// </summary>
    /// <param name="item">The ListBoxItem to check.</param>
    public static bool GetIsUnderDragCursor( ListBoxItem item )
    {
        return (bool)item.GetValue( IsUnderDragCursorProperty );
    }

    /// <summary>
    /// Sets the IsUnderDragCursor attached property for the specified ListBoxItem.
    /// </summary>
    /// <param name="item">The ListBoxItem to set the property on.</param>
    /// <param name="value">Pass true if the element is underneath the drag cursor, else false.</param>
    internal static void SetIsUnderDragCursor( ListBoxItem item, bool value )
    {
        item.SetValue( IsUnderDragCursorProperty, value );
    }

}