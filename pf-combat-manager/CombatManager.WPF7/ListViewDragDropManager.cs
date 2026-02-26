/*
 *  ListViewDragDropManager.cs
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

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CombatManager.WPF7
{

	/// <summary>
	/// Manages the dragging and dropping of ListBoxItems in a ListView.
	/// The ItemType type parameter indicates the type of the objects in
	/// the ListView's items source.  The ListView's ItemsSource must be 
	/// set to an instance of ObservableCollection of ItemType, or an 
	/// Exception will be thrown.
	/// </summary>
	/// <typeparam name="ItemType">The type of the ListView's items.</typeparam>
	public class ListViewDragDropManager<ItemType> where ItemType : class
	{

		bool canInitiateDrag;
		DragAdorner dragAdorner;
		double dragAdornerOpacity;
		int indexToSelect;
		bool isDragInProgress;
		ItemType itemUnderDragCursor;
		ListBox listView;
		Point ptMouseDown;
		bool showDragAdorner;

		/// <summary>
		/// Initializes a new instance of ListViewDragManager.
		/// </summary>
		public ListViewDragDropManager()
		{
			canInitiateDrag = false;
			dragAdornerOpacity = 0.7;
			indexToSelect = -1;
			showDragAdorner = false;
		}

		/// <summary>
		/// Initializes a new instance of ListViewDragManager.
		/// </summary>
		/// <param name="listView"></param>
        public ListViewDragDropManager(ListBox listView)
			: this()
		{
			ListView = listView;
		}

		/// <summary>
		/// Initializes a new instance of ListViewDragManager.
		/// </summary>
		/// <param name="listView"></param>
		/// <param name="dragAdornerOpacity"></param>
        public ListViewDragDropManager(ListBox listView, double dragAdornerOpacity)
			: this( listView )
		{
			DragAdornerOpacity = dragAdornerOpacity;
		}

		/// <summary>
		/// Initializes a new instance of ListViewDragManager.
		/// </summary>
		/// <param name="listView"></param>
		/// <param name="showDragAdorner"></param>
        public ListViewDragDropManager(ListBox listView, bool showDragAdorner)
			: this( listView )
		{
			ShowDragAdorner = showDragAdorner;
		}


		/// <summary>
		/// Gets/sets the opacity of the drag adorner.  This property has no
		/// effect if ShowDragAdorner is false. The default value is 0.7
		/// </summary>
		public double DragAdornerOpacity
		{
			get { return dragAdornerOpacity; }
			set
			{
				if( IsDragInProgress )
					throw new InvalidOperationException( "Cannot set the DragAdornerOpacity property during a drag operation." );

				if( value < 0.0 || value > 1.0 )
					throw new ArgumentOutOfRangeException( "DragAdornerOpacity", value, "Must be between 0 and 1." );

				dragAdornerOpacity = value;
			}
		}

		/// <summary>
		/// Returns true if there is currently a drag operation being managed.
		/// </summary>
		public bool IsDragInProgress
		{
			get { return isDragInProgress; }
			private set { isDragInProgress = value; }
		}


		/// <summary>
		/// Gets/sets the ListView whose dragging is managed.  This property
		/// can be set to null, to prevent drag management from occuring.  If
		/// the ListView's AllowDrop property is false, it will be set to true.
		/// </summary>
        public ListBox ListView
		{
			get { return listView; }
			set
			{
				if( IsDragInProgress )
					throw new InvalidOperationException( "Cannot set the ListView property during a drag operation." );

				if( listView != null )
				{
					listView.PreviewMouseLeftButtonDown -= listView_PreviewMouseLeftButtonDown;
					listView.PreviewMouseMove -= listView_PreviewMouseMove;
					listView.DragOver -= listView_DragOver;
					listView.DragLeave -= listView_DragLeave;
					listView.DragEnter -= listView_DragEnter;
					listView.Drop -= listView_Drop;

				}

				listView = value;

				if( listView != null )
				{
					if( !listView.AllowDrop )
						listView.AllowDrop = true;


					listView.PreviewMouseLeftButtonDown += listView_PreviewMouseLeftButtonDown;
					listView.PreviewMouseMove += listView_PreviewMouseMove;
					listView.DragOver += listView_DragOver;
					listView.DragLeave += listView_DragLeave;
					listView.DragEnter += listView_DragEnter;
					listView.Drop += listView_Drop;

				}
			}
		}

		/// <summary>
		/// Raised when a drop occurs.  By default the dropped item will be moved
		/// to the target index.  Handle this event if relocating the dropped item
		/// requires custom behavior.  Note, if this event is handled the default
		/// item dropping logic will not occur.
		/// </summary>
		public event EventHandler<ProcessDropEventArgs<ItemType>> ProcessDrop;


        /// <summary>
        /// Raised when a drop occurs.  By default the dropped item will be moved
        /// to the target index.  Handle this event if relocating the dropped item
        /// requires custom behavior.  Note, if this event is handled the default
        /// item dropping logic will not occur.
        /// </summary>
        public event EventHandler ManagerDragOver;


		/// <summary>
		/// Gets/sets whether a visual representation of the ListBoxItem being dragged
		/// follows the mouse cursor during a drag operation.  The default value is true.
		/// </summary>
		public bool ShowDragAdorner
		{
			get { return showDragAdorner; }
			set
			{
				if( IsDragInProgress )
					throw new InvalidOperationException( "Cannot set the ShowDragAdorner property during a drag operation." );

				showDragAdorner = value;
			}
		}

		void listView_PreviewMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
		{
			if( IsMouseOverScrollbar )
			{
				// 4/13/2007 - Set the flag to false when cursor is over scrollbar.
				canInitiateDrag = false;
				return;
			}

			var index = IndexUnderDragCursor;
			canInitiateDrag = index > -1;

			if( canInitiateDrag )
			{
				// Remember the location and index of the ListBoxItem the user clicked on for later.
				ptMouseDown = MouseUtilities.GetMousePosition( listView );
				indexToSelect = index;
			}
			else
			{
				ptMouseDown = new Point( -10000, -10000 );
				indexToSelect = -1;
			}
		}

		void listView_PreviewMouseMove( object sender, MouseEventArgs e )
		{
			if( !CanStartDragOperation )
				return;

			// Select the item the user clicked on.
            if (listView.SelectedIndex != indexToSelect)
            {
                //this.listView.SelectedIndex = this.indexToSelect;
            }

			// If the item at the selected index is null, there's nothing
			// we can do, so just return;
			if( listView.SelectedItem == null )
				return;

			var itemToDrag = GetListBoxItem( listView.SelectedIndex );
			if( itemToDrag == null )
				return;

			var adornerLayer = ShowDragAdornerResolved ? InitializeAdornerLayer( itemToDrag ) : null;

			InitializeDragOperation( itemToDrag );
            try
            {
                PerformDragOperation();
                FinishDragOperation(itemToDrag, adornerLayer);
            }
            catch (InvalidOperationException)
            {

            }
		}


		void listView_DragOver( object sender, DragEventArgs e )
		{
			e.Effects = DragDropEffects.Move;

			if( ShowDragAdornerResolved )
				UpdateDragAdornerLocation();

			// Update the item which is known to be currently under the drag cursor.
			var index = IndexUnderDragCursor;
			ItemUnderDragCursor = index < 0 ? null : ListView.Items[index] as ItemType;

            if (ManagerDragOver != null)
            {
                ManagerDragOver(this, new EventArgs());
            }
		}


		void listView_DragLeave( object sender, DragEventArgs e )
		{
			if( !IsMouseOver( listView ) )
			{
				if( ItemUnderDragCursor != null )
					ItemUnderDragCursor = null;

				if( dragAdorner != null )
					dragAdorner.Visibility = Visibility.Collapsed;
			}
		}


		void listView_DragEnter( object sender, DragEventArgs e )
		{
			if( dragAdorner != null && dragAdorner.Visibility != Visibility.Visible )
			{
				// Update the location of the adorner and then show it.				
				UpdateDragAdornerLocation();
				dragAdorner.Visibility = Visibility.Visible;
			}
		}
	

		void listView_Drop( object sender, DragEventArgs e )
		{
			if( ItemUnderDragCursor != null )
				ItemUnderDragCursor = null;

			e.Effects = DragDropEffects.None;

			if( !e.Data.GetDataPresent( typeof( ItemType ) ) )
				return;

			// Get the data object which was dropped.
			var data = e.Data.GetData( typeof( ItemType ) ) as ItemType;
			if( data == null )
				return;

            IList<ItemType> itemsSource = null;
			// Get the ObservableCollection<ItemType> which contains the dropped data object.
            if (listView.ItemsSource.GetType() == typeof(ListCollectionView))
            {
                itemsSource = ((ListCollectionView)listView.ItemsSource).SourceCollection as IList<ItemType>;
            }
            else
            {
                itemsSource = listView.ItemsSource as IList<ItemType>;
            }

			if( itemsSource == null )
				throw new Exception(
					"A ListView managed by ListViewDragManager must have its ItemsSource set to an ObservableCollection<ItemType>." );

			var oldIndex = itemsSource.IndexOf( data );
			var newIndex = IndexUnderDragCursor;

			if( newIndex < 0 )
			{
				// The drag started somewhere else, and our ListView is empty
				// so make the new item the first in the list.
				if( itemsSource.Count == 0 )
					newIndex = 0;

				// The drag started somewhere else, but our ListView has items
				// so make the new item the last in the list.
				else if( oldIndex < 0 )
					newIndex = itemsSource.Count;

				// The user is trying to drop an item from our ListView into
				// our ListView, but the mouse is not over an item, so don't
				// let them drop it.
				else
                {
                    newIndex = itemsSource.Count;
                };
			}

			if( ProcessDrop != null )
			{
				// Let the client code process the drop.
				var args = new ProcessDropEventArgs<ItemType>( itemsSource, data, oldIndex, newIndex, e.AllowedEffects );
				ProcessDrop( this, args );
				e.Effects = args.Effects;
			}
			else
			{
				// Move the dragged data object from it's original index to the
				// new index (according to where the mouse cursor is).  If it was
				// not previously in the ListBox, then insert the item.
                if (oldIndex > -1)
                {
                    itemsSource.RemoveAt(oldIndex);
                    itemsSource.Insert(newIndex, data);
                }
                else
                    itemsSource.Insert(newIndex, data);

				// Set the Effects property so that the call to DoDragDrop will return 'Move'.
				e.Effects = DragDropEffects.Move;
			}
		}

		bool CanStartDragOperation
		{
			get
			{
                if (Mouse.LeftButton != MouseButtonState.Pressed)
                {
                    return false;
                }
                if (!canInitiateDrag)
                {
                    return false;
                }

                if (indexToSelect == -1)
                {
                    return false;
                }

                if (!HasCursorLeftDragThreshold)
                {
                    return false;
                }

				return true;
			}
		}

		void FinishDragOperation( ListBoxItem draggedItem, AdornerLayer adornerLayer )
		{
			// Let the ListBoxItem know that it is not being dragged anymore.
			ListBoxItemDragState.SetIsBeingDragged( draggedItem, false );

			IsDragInProgress = false;

			if( ItemUnderDragCursor != null )
				ItemUnderDragCursor = null;

			// Remove the drag adorner from the adorner layer.
			if( adornerLayer != null )
			{
				adornerLayer.Remove( dragAdorner );
				dragAdorner = null;
			}
		}

		ListBoxItem GetListBoxItem( int index )
		{
			if( listView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated )
				return null;

			return listView.ItemContainerGenerator.ContainerFromIndex( index ) as ListBoxItem;
		}

		ListBoxItem GetListBoxItem( ItemType dataItem )
		{
			if( listView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated )
				return null;

			return listView.ItemContainerGenerator.ContainerFromItem( dataItem ) as ListBoxItem;
		}

		bool HasCursorLeftDragThreshold
		{
			get
			{
                if (indexToSelect < 0)
                {
                    return false;
                }

				var item = GetListBoxItem( indexToSelect );

                if (item == null)
                {
                    return false;
                }

				var bounds = VisualTreeHelper.GetDescendantBounds( item );
				var ptInItem = listView.TranslatePoint( ptMouseDown, item );

				// In case the cursor is at the very top or bottom of the ListBoxItem
				// we want to make the vertical threshold very small so that dragging
				// over an adjacent item does not select it.
				var topOffset = Math.Abs( ptInItem.Y );
				var btmOffset = Math.Abs( bounds.Height - ptInItem.Y );
				var vertOffset = Math.Min( topOffset, btmOffset );

				var width = SystemParameters.MinimumHorizontalDragDistance * 2;
				var height = Math.Min( SystemParameters.MinimumVerticalDragDistance, vertOffset ) * 2;
				var szThreshold = new Size( width, height );

				var rect = new Rect( ptMouseDown, szThreshold );
				rect.Offset( szThreshold.Width / -2, szThreshold.Height / -2 );
				var ptInListView = MouseUtilities.GetMousePosition( listView );
				return !rect.Contains( ptInListView );
			}
		}


		/// <summary>
		/// Returns the index of the ListBoxItem underneath the
		/// drag cursor, or -1 if the cursor is not over an item.
		/// </summary>
		public int IndexUnderDragCursor
		{
			get
			{
				var index = -1;
				for( var i = 0; i < listView.Items.Count; ++i )
				{
					var item = GetListBoxItem( i );
					if( IsMouseOver( item ) )
					{
						index = i;
						break;
					}
				}
				return index;
			}
		}


		AdornerLayer InitializeAdornerLayer( ListBoxItem itemToDrag )
		{
			// Create a brush which will paint the ListBoxItem onto
			// a visual in the adorner layer.
			var brush = new VisualBrush( itemToDrag );

			// Create an element which displays the source item while it is dragged.
			dragAdorner = new DragAdorner( listView, itemToDrag.RenderSize, brush )
            {
                // Set the drag adorner's opacity.		
                Opacity = DragAdornerOpacity
            };

            var layer = AdornerLayer.GetAdornerLayer( listView );
			layer.Add( dragAdorner );

			// Save the location of the cursor when the left mouse button was pressed.
			ptMouseDown = MouseUtilities.GetMousePosition( listView );

			return layer;
		}


		void InitializeDragOperation( ListBoxItem itemToDrag )
		{
			// Set some flags used during the drag operation.
			IsDragInProgress = true;
			canInitiateDrag = false;

			// Let the ListBoxItem know that it is being dragged.
			ListBoxItemDragState.SetIsBeingDragged( itemToDrag, true );
		}


		bool IsMouseOver( Visual target )
		{
			// We need to use MouseUtilities to figure out the cursor
			// coordinates because, during a drag-drop operation, the WPF
			// mechanisms for getting the coordinates behave strangely.
            if (target == null)
            {
                return false;
            }
			var bounds = VisualTreeHelper.GetDescendantBounds( target );
			var mousePos = MouseUtilities.GetMousePosition( target );
			return bounds.Contains( mousePos );
		}


		/// <summary>
		/// Returns true if the mouse cursor is over a scrollbar in the ListView.
		/// </summary>
		bool IsMouseOverScrollbar
		{
			get
			{
				var ptMouse = MouseUtilities.GetMousePosition( listView );
				var res = VisualTreeHelper.HitTest( listView, ptMouse );
				if( res == null )
					return false;

				var depObj = res.VisualHit;
				while( depObj != null )
				{
					if( depObj is ScrollBar  || depObj is TextBox)
						return true;

					// VisualTreeHelper works with objects of type Visual or Visual3D.
					// If the current object is not derived from Visual or Visual3D,
					// then use the LogicalTreeHelper to find the parent element.
					if( depObj is Visual || depObj is System.Windows.Media.Media3D.Visual3D )
						depObj = VisualTreeHelper.GetParent( depObj );
					else
						depObj = LogicalTreeHelper.GetParent( depObj );
				}

				return false;
			}
		}


		public ItemType ItemUnderDragCursor
		{
			get { return itemUnderDragCursor; }
			set
			{
				if( itemUnderDragCursor == value )
					return;

				// The first pass handles the previous item under the cursor.
				// The second pass handles the new one.
				for( var i = 0; i < 2; ++i )
				{
					if( i == 1 )
						itemUnderDragCursor = value;

					if( itemUnderDragCursor != null )
					{
						var ListBoxItem = GetListBoxItem( itemUnderDragCursor );
						if( ListBoxItem != null )
							ListBoxItemDragState.SetIsUnderDragCursor( ListBoxItem, i == 1 );
					}
				}
			}
		}

		void PerformDragOperation()
		{
			var selectedItem = listView.SelectedItem as ItemType;
			var allowedEffects = DragDropEffects.Move | DragDropEffects.Move | DragDropEffects.Link;
			if( DragDrop.DoDragDrop( listView, selectedItem, allowedEffects ) != DragDropEffects.None )
			{
				// The item was dropped into a new location,
				// so make it the new selected item.
				listView.SelectedItem = selectedItem;
			}
		}


		bool ShowDragAdornerResolved
		{
			get { return ShowDragAdorner && DragAdornerOpacity > 0.0; }
		}

		void UpdateDragAdornerLocation()
		{
			if( dragAdorner != null )
			{
				var ptCursor = MouseUtilities.GetMousePosition( ListView );

				var left = ptCursor.X - ptMouseDown.X;

				// 4/13/2007 - Made the top offset relative to the item being dragged.
				var itemBeingDragged = GetListBoxItem( indexToSelect );
				var itemLoc = itemBeingDragged.TranslatePoint( new Point( 0, 0 ), ListView );
				var top = itemLoc.Y + ptCursor.Y - ptMouseDown.Y;

				dragAdorner.SetOffsets( left, top );
			}
		}

	}
}