
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WinInterop = System.Windows.Interop;

namespace CombatManager.Maps
{
    public enum GameMapActionMode
    {
        None,
        SetOrigin,
        SetFog,
        SetMarker,
        SetCorner
    }

    /// <summary>
    /// Interaction logic for GameMapDisplayWindow.xaml
    /// </summary>
    public partial class GameMapDisplayWindow : Window
    {
        GameMap map;
        GameMapActionMode mode;

        bool settingFog;
        bool settingMarkers;
        bool draggingMap;
        bool newFogState;

        Point lastPosition;

        GameMap.MarkerStyle markerStyle = GameMap.MarkerStyle.Square;
        Color markerColor = Colors.Red;
        bool eraseMode = false;

        int brushSize = 1;

        bool fullscreen;

        public delegate void MapEventDelegate(GameMap map);

        public event MapEventDelegate ShowPlayerMap;

        bool scaleTimerPlayer;
        bool scaleTimerUp;

        public GameMapDisplayWindow() : this(false)
        {
        }

        public GameMapDisplayWindow(bool playerMode)
        {
            InitializeComponent();
            if (!playerMode)
            {
                LoadActionButtonState();
            }

            UpdateActionButtons();

            this.playerMode = playerMode;
            FogOfWar.PlayerMode = playerMode;
            RotateButton.Visibility = playerMode ? Visibility.Visible : Visibility.Collapsed;

            if (playerMode)
            {
                HideGMControls();
            }

        }


        private void HideGMControls()
        {
            RootGrid.ColumnDefinitions[0].Width = new System.Windows.GridLength(0);
            NameGrid.Visibility = Visibility.Collapsed;
            GridSizeGrid.Visibility = Visibility.Collapsed;
            RegularScaleControls.Visibility = Visibility.Collapsed;
            ShowActionButtons(false);
        }

        public GameMap Map
        {
            get
            {
                return map;
            }
            set
            {
                if (map != value)
                {
                    if (map != null)
                    {
                        map.PropertyChanged -= MapPropertyChanged;
                    }
                    map = value;
                    map.PropertyChanged += MapPropertyChanged;
                    map.FogOrMarkerChanged += Map_FogOrMarkerChanged;

                    if (map != null && map.Image != null)
                    {

                        UpdateMapImage();
                        DataContext = map;

                    }
                }
            }

        }

        private void Map_FogOrMarkerChanged(GameMap map)
        {
            UpdateGridBrush();
        }

        bool playerMode;

        public bool PlayerMode
        {
            get
            {
                return playerMode;
            }
        }

        void MapPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Scale" && !playerMode)
            {
                UpdateMapImage();
            }
            else if (e.PropertyName == "TableScale" && playerMode)
            {
                UpdateMapImage();
            }
            else if (e.PropertyName == "CellSize" || e.PropertyName == "CellOrigin"
                || e.PropertyName == "ShowGrid" || e.PropertyName == "GridColor")
            {
                UpdateGridBrush();
            }
            else if (e.PropertyName == "Image")
            {
                UpdateMapImage();
            }
        }

        void UpdateMapImage()
        {

            MapImageControl.Source = map.Image;

            var size = new Size(map.Image.Width, map.Image.Height);
            size = size.Multiply(UseScale);

            MapImageControl.Width = size.Width;
            MapImageControl.Height = size.Height;
            MapGridCanvas.Width = size.Width;
            MapGridCanvas.Height = size.Height;
            FogOfWar.Width = size.Width;
            FogOfWar.Height = size.Height;


            UpdateGridBrush();
        }


        void UpdateGridBrush()
        {
            if (map != null)
            {
                if (map.ShowGrid)
                {
                    var xSize = map.CellSize.Width / map.Image.Width;
                    var ySize = map.CellSize.Height / map.Image.Height;
                    var xStart = map.CellOrigin.X / map.Image.Width;
                    var yStart = map.CellOrigin.Y / map.Image.Height;

                    var r = new RectangleGeometry(new Rect(0, 0, 100, 100));
                    var p = new Pen(new SolidColorBrush(map.GridColor), 1);
                    var gd = new GeometryDrawing(null, p, r);
                    var db = new DrawingBrush(gd);
                    db.TileMode = TileMode.Tile;
                    db.Viewport = new Rect(xStart, yStart, xSize, ySize);


                    MapGridCanvas.Background = db;
                    FogOfWar.InvalidateVisual();
                }
                else
                {
                    MapGridCanvas.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    FogOfWar.InvalidateVisual();
                }
            }
        }

        private void SetOriginButton_Click(object sender, RoutedEventArgs e)
        {
            SetMode(GameMapActionMode.SetOrigin, true);
        }


        private void SetCornerButton_Click(object sender, RoutedEventArgs e)
        {
            SetMode(GameMapActionMode.SetCorner, true);
        }

        void SetMode(GameMapActionMode newMode, bool flip)
        {
            if (newMode == mode)
            {
                if (flip)
                {
                    mode = GameMapActionMode.None;
                }
            }
            else
            {
                mode = newMode;
            }
            FogOfWar.DrawAnchor = (mode == GameMapActionMode.SetOrigin ||
                        mode == GameMapActionMode.SetCorner);
            UpdateActionButtons();
            SaveActionButtonState();
        }




        private void MapGridCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {

                var p = e.GetPosition((Canvas)sender);
                lastPosition = p;

                draggingMap = true;
            }
        }



        private void MapGridCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {

                draggingMap = false;
            }
        }



        private void MapGridCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var p = e.GetPosition((Canvas)sender);
                lastPosition = p;
                switch (mode)
                {
                    case GameMapActionMode.SetOrigin:

                        p = p.Divide(UseScale);
                        map.CellOrigin = p;
                        //UpdateGridBrush();
                        break;
                    case GameMapActionMode.SetCorner:
                        p = p.Divide(UseScale);

                        var s = map.CellOrigin.Difference(p);
                        map.CellSize = s;

                        break;
                    case GameMapActionMode.SetFog:

                        {
                            var cell = PointToCell(p);
                            if (CellOnBoard(cell))
                            {
                                var list = PointToCellArray(p, brushSize);

                                newFogState = !map[cell.X, cell.Y];
                                foreach (var c in list)
                                {
                                    if (CellOnBoard(c))
                                    {
                                        map[c.X, c.Y] = newFogState;
                                    }
                                }
                                map.FireFogOrMarkerChanged();
                                settingFog = true;
                            }
                            break;
                        }
                    case GameMapActionMode.SetMarker:
                        {
                            SetMarkers(p);
                            settingMarkers = true;

                        }
                        break;

                }
            }
            else if (e.ClickCount == 2)
            {
                if (mode != GameMapActionMode.SetCorner &&
                    mode != GameMapActionMode.SetOrigin)

                {
                    if (fullscreen)
                    {
                        ExitFullScreen();
                    }
                    else
                    {
                        EnterFullScreen();
                    }
                }
            }
        }

        private void SetMarkers(Point p)
        {
            var cell = PointToCell(p);
            if (CellOnBoard(cell))
            {
                var list = PointToCellArray(p, brushSize);

                foreach (var c in list)
                {
                    if (CellOnBoard(c))
                    {
                        if (eraseMode)
                        {
                            map.DeleteAllMarkers(c);
                        }
                        else
                        {
                            var marker = new GameMap.Marker();
                            marker.Style = markerStyle;
                            marker.Color = markerColor;
                            map.SetMarker(c, marker);
                        }
                    }
                }
                map.SaveMap(false);
                map.FireFogOrMarkerChanged();
            }
        }

        private void MapGridCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        bool rightClickDown = false;
        Point rightClickPosition;
        GameMap.MapCell rightClickCell;

        private void MapGridCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            rightClickDown = true;
            rightClickPosition = e.GetPosition((Canvas)sender);
        }

        private void MapGridCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (rightClickDown)
            {
                rightClickDown = false;

                var cell = PointToCell(rightClickPosition);

                ShowContextMenu(cell);
            }
        }

        void ShowContextMenu(GameMap.MapCell cell)
        {
            var menu = (ContextMenu)Resources["MapContextMenu"];
            if (menu.IsOpen)
            {
                return;
            }

            rightClickCell = cell;

            menu.DataContext = cell;

            var hasMarkers = map.CellHasMarkers(cell);

            menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
            menu.IsOpen = true;

            MenuItem mi;

            if (CellOnBoard(cell))
            {

                mi = (MenuItem)menu.FindLogicalNode("ToggleFogItem");
                mi.DataContext = cell;
                mi.Visibility = !playerMode ? Visibility.Visible : Visibility.Collapsed;

                mi = (MenuItem)menu.FindLogicalNode("DeleteMarkerItem");
                mi.Visibility = (hasMarkers && !playerMode) ? Visibility.Visible : Visibility.Collapsed;
                mi.DataContext = cell;


                mi = (MenuItem)menu.FindLogicalNode("NameBoxItem");
                mi.Visibility = (hasMarkers && !playerMode) ? Visibility.Visible : Visibility.Collapsed;

                if (hasMarkers)
                {
                    mi.DataContext = map.GetMarkers(cell)[0];
                }
            }
            else
            {
                menu.SetElementsVisibility(new string[] { "ToggleFogItem", "DeleteMarkerItem", "NameBoxItem" }, Visibility.Collapsed);
            }


            mi = (MenuItem)menu.FindLogicalNode("ShowControlsItem");
            mi.Visibility = controlsHidden ? Visibility.Visible : Visibility.Collapsed;

            mi = (MenuItem)menu.FindLogicalNode("HideControlsItem");
            mi.Visibility = controlsHidden ? Visibility.Collapsed : Visibility.Visible;

            mi = (MenuItem)menu.FindLogicalNode("ExitFullScreenItem");
            mi.Visibility = fullscreen ? Visibility.Visible : Visibility.Collapsed;

            mi = (MenuItem)menu.FindLogicalNode("EnterFullScreenItem");
            mi.Visibility = fullscreen ? Visibility.Collapsed : Visibility.Visible;
        }

        private void MapGridCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition((Canvas)sender);
            if (settingFog)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    settingFog = false;
                    map.SaveMap(true);
                }
                else
                {

                    var cell = PointToCell(p);

                    var list = PointToCellArray(p, brushSize);
                    var changed = false;
                    foreach (var c in list)
                    {
                        if (CellOnBoard(c))
                        {
                            if (map[c.X, c.Y] != newFogState)
                            {
                                map[c.X, c.Y] = newFogState;
                                changed = true;
                            }
                        }
                    }
                    if (changed)
                    {
                        map.FireFogOrMarkerChanged();
                    }

                }

                lastPosition = p;
            }
            else if (settingMarkers)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    settingMarkers = false;
                    map.SaveMap(true);
                }
                else
                {
                    SetMarkers(p);
                }
            }
            if (draggingMap)
            {
                if (e.MiddleButton == MouseButtonState.Released)
                {
                    draggingMap = false;
                }
                else
                {
                    var v = p - lastPosition;

                    var newVerticalOffset = MapScrollViewer.VerticalOffset - v.Y;
                    var newHorizontalOffset = MapScrollViewer.HorizontalOffset - v.X;

                    double verticalDiff = 0;
                    double horizontalDiff = 0;

                    if (newVerticalOffset < 0)
                    {
                        verticalDiff = newVerticalOffset;
                        newVerticalOffset = 0;

                    }
                    else if (newVerticalOffset > MapScrollViewer.ScrollableHeight)
                    {
                        verticalDiff = newVerticalOffset - MapScrollViewer.ScrollableHeight;
                        newVerticalOffset = MapScrollViewer.ScrollableHeight;

                    }

                    if (newHorizontalOffset < 0)
                    {
                        horizontalDiff = newHorizontalOffset;
                        newHorizontalOffset = 0;

                    }
                    else if (newHorizontalOffset > MapScrollViewer.ScrollableWidth)
                    {
                        horizontalDiff = newHorizontalOffset - MapScrollViewer.ScrollableWidth;
                        newHorizontalOffset = MapScrollViewer.ScrollableWidth;
                    }

                    MapScrollViewer.ScrollToVerticalOffset(newVerticalOffset);
                    MapScrollViewer.ScrollToHorizontalOffset(newHorizontalOffset);

                    lastPosition.X = lastPosition.X - horizontalDiff;
                    lastPosition.Y = lastPosition.Y - verticalDiff;

                }
            }



        }




        private GameMap.MapCell PointToCell(Point p)
        {
            var cell = new GameMap.MapCell();

            var v = p - UseGridOrigin;
            cell.X = (int)(v.X / UseCellSize.Width);
            cell.Y = (int)(v.Y / UseCellSize.Height);


            return cell;
        }

        private List<GameMap.MapCell> PointToCellArray(Point p, int size)
        {
            var list = new List<GameMap.MapCell>();

            if (size == 1)
            {

                list.Add(PointToCell(p));
            }
            else if (size.IsOdd())
            {


                var c = PointToCell(p);

                var minx = c.X - size / 2;
                var miny = c.Y - size / 2;
                var maxx = c.X + size / 2;
                var maxy = c.Y + size / 2;

                for (var y = miny; y <= maxy; y++)
                {
                    for (var x = minx; x <= maxx; x++)
                    {
                        list.Add(new GameMap.MapCell(x, y));
                    }
                }

            }
            else
            {
                var start = p;
                start.X = start.X - UseCellSize.Width * ((double)size) / 2.0 + UseCellSize.Width / 2.0;
                start.Y = start.Y - UseCellSize.Height * ((double)size) / 2.0 + UseCellSize.Height / 2.0;

                for (var x = 0; x < size; x++)
                {
                    for (var y = 0; y < size; y++)
                    {
                        list.Add(PointToCell(new Point(start.X + UseCellSize.Width * (double)x,
                            start.Y + UseCellSize.Width * (double)y)));
                    }
                }
            }


            return list;
        }




        private bool CellOnBoard(GameMap.MapCell cell)
        {
            return !(cell.X < 0 || cell.Y < 0
                || cell.X >= map.CellsWidth || cell.Y >= map.CellsHeight);

        }

        private void SetFogButton_Click(object sender, RoutedEventArgs e)
        {
            SetMode(GameMapActionMode.SetFog, true);
        }


        void UpdateActionButtons()
        {
            SetActionButtonState(SetOriginButton, (mode == GameMapActionMode.SetOrigin));
            SetActionButtonState(SetFogButton, (mode == GameMapActionMode.SetFog));
            SetActionButtonState(FogOptionsButton, (mode == GameMapActionMode.SetFog));
            SetActionButtonState(SetMarkerButton, (mode == GameMapActionMode.SetMarker));
            SetActionButtonState(MarkerOptionsButton, (mode == GameMapActionMode.SetMarker));
            SetActionButtonState(SetCornerButton, (mode == GameMapActionMode.SetCorner));

            BrushSizeComboBox.SelectedIndex = brushSize - 1;
            UpdateMarkerButtonImage();

        }

        private void SaveActionButtonState()
        {
            if (actionButtonStateLoaded && !playerMode)
            {
                XmlLoader<ActionButtonState>.Save(GetActionButtonState(),
                    "GameMapDisplayWindowActionButtonState.xml", true);
            }
        }

        bool actionButtonStateLoaded = false;

        private void LoadActionButtonState()
        {
            try
            {
                var state = XmlLoader<ActionButtonState>.Load(
                    "GameMapDisplayWindowActionButtonState.xml", true);
                if (state != null)
                {
                    mode = state.Mode;
                    brushSize = state.BrushSize;
                    markerColor = state.MarkerColor;
                    markerStyle = state.MarkerStyle;
                    eraseMode = state.EraseMode;
                }
            }
            catch (Exception)
            {

            }
            actionButtonStateLoaded = true;
        }

        private ActionButtonState GetActionButtonState()
        {
            return new ActionButtonState()
            {
                Mode = this.mode,
                MarkerStyle = markerStyle,
                MarkerColor = markerColor,
                BrushSize = brushSize,
                EraseMode = eraseMode,
            };
        }


        public class ActionButtonState
        {
            GameMapActionMode mode;
            GameMap.MarkerStyle markerStyle;
            Color markerColor;
            int brushSize;
            bool eraseMode;

            public GameMapActionMode Mode
            {
                get
                {
                    return mode;
                }

                set
                {
                    mode = value;
                }
            }

            public GameMap.MarkerStyle MarkerStyle
            {
                get
                {
                    return markerStyle;
                }

                set
                {
                    markerStyle = value;
                }
            }

            public Color MarkerColor
            {
                get
                {
                    return markerColor;
                }

                set
                {
                    markerColor = value;
                }
            }

            public int BrushSize
            {
                get
                {
                    return brushSize;
                }

                set
                {
                    brushSize = value;
                }
            }

            public bool EraseMode
            {
                get
                {
                    return eraseMode;
                }

                set
                {
                    eraseMode = value;
                }
            }
        }


        void ShowActionButtons(bool show)
        {
            ShowPlayerWindowButton.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            SetOriginButton.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            SetFogButton.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            FogOptionsButton.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            SetMarkerButton.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            MarkerOptionsButton.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        void UpdateMarkerButtonImage()
        {
            if (!eraseMode)
            {
                var path = new Path();
                path.Data = GetUnitMarkerStylePath(markerStyle);
                path.Fill = new SolidColorBrush(markerColor);
                path.Height = 16;
                path.Width = 16;
                path.Stretch = Stretch.Fill;
                SetMarkerButton.Content = path;
            }
            else
            {
                var image = new Image();
                image.Source = CMUIUtilities.LoadBitmapFromImagesDir("eraser-48.png");
                SetMarkerButton.Content = image;
            }

        }

        Geometry GetUnitMarkerStylePath(GameMap.MarkerStyle style)
        {

            var rect = new Rect(0, 0, 1, 1);
            return GetMarkerStylePath(rect, style);
        }

        Geometry GetMarkerStylePath(Rect rect, GameMap.MarkerStyle style)
        {
            switch (style)
            {
                case GameMap.MarkerStyle.Circle:
                    return rect.CirclePath();
                case GameMap.MarkerStyle.Square:
                    return rect.RectanglePath();
                case GameMap.MarkerStyle.Diamond:
                    return rect.DiamondPath();
                case GameMap.MarkerStyle.Target:
                    return rect.TargetPath();
                case GameMap.MarkerStyle.Star:
                    return rect.StarPath();
            }

            return null;
        }

        void SetActionButtonState(Button b, bool state)
        {
            b.Background = this.FindSolidBrush(state ? CMUIUtilities.SecondaryColorALight : CMUIUtilities.SecondaryColorADark);

        }



        private void SetMarkerButton_Click(object sender, RoutedEventArgs e)
        {

            SetMode(GameMapActionMode.SetMarker, true);
        }

        private void HideAll_Click(object sender, RoutedEventArgs e)
        {
            map.Fog.SetAll(true);

            map.SaveMap(true);
            map.FireFogOrMarkerChanged();
        }

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            map.Fog.SetAll(false);

            map.SaveMap(true);
            map.FireFogOrMarkerChanged();
        }

        private void FogOptionsButton_Click(object sender, RoutedEventArgs e)
        {

            var menu = (ContextMenu)Resources["FogOfWarContextMenu"];

            menu.PlacementTarget = FogOptionsButton;
            menu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            menu.IsOpen = true;
            SetMode(GameMapActionMode.SetFog, false);
        }

        private void MarkerOptionsButton_Click(object sender, RoutedEventArgs e)
        {

            var menu = (ContextMenu)Resources["MarkerContextMenu"];

            menu.PlacementTarget = MarkerOptionsButton;
            menu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            menu.IsOpen = true;

            var shapeMenu = (MenuItem)menu.FindLogicalNode("ShapeMenuItem");
            foreach (var c in shapeMenu.Items)
            {
                var mi = c as MenuItem;
                if (mi != null)
                {
                    var i = int.Parse(mi.Tag as String);
                    if (i >= 0)
                    {
                        var path = new Path();
                        path.Data = GetUnitMarkerStylePath((GameMap.MarkerStyle)i);
                        path.Fill = new SolidColorBrush(markerColor);
                        path.Height = 16;
                        path.Width = 16;
                        path.Stretch = Stretch.Fill;
                        mi.Icon = path;
                    }
                }
            }


            SetMode(GameMapActionMode.SetMarker, false);
        }

        private void Shape_Click(object sender, RoutedEventArgs e)
        {
            var shapeTag = (String)((FrameworkElement)sender).Tag;
            var id = int.Parse(shapeTag);

            if (id == -1)
            {
                eraseMode = true;
            }
            else
            {
                markerStyle = (GameMap.MarkerStyle)id;
                eraseMode = false;
            }
            UpdateMarkerButtonImage();

            SaveActionButtonState();
        }


        private void Color_Click(object sender, RoutedEventArgs e)
        {
            eraseMode = false;


            markerColor = ((SolidColorBrush)((MenuItem)sender).Background).Color;
            UpdateMarkerButtonImage();

            SaveActionButtonState();


        }

        private void ShowPlayerWindowButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPlayerMap?.Invoke(map);
        }

        private void ToggleFogItem_Click(object sender, RoutedEventArgs e)
        {

            var cell = (GameMap.MapCell)((FrameworkElement)sender).DataContext;
            if (CellOnBoard(cell))
            {
                map[cell.X, cell.Y] = !map[cell.X, cell.Y];
                map.FireFogOrMarkerChanged();
                map.SaveMap(true);
            }
        }

        private void DeleteMarkerItem_Click(object sender, RoutedEventArgs e)
        {

            var cell = (GameMap.MapCell)((FrameworkElement)sender).DataContext;
            if (CellOnBoard(cell))
            {
                map.DeleteAllMarkers(cell);

                map.FireFogOrMarkerChanged();
                map.SaveMap(false);
            }
        }

        private void MapScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollControl = sender as ScrollViewer;
            if (!e.Handled && sender != null)
            {
                var steps = ((double)e.Delta) / 120.0;

                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;


                var delta = e.Delta;
                if (delta != 0)
                {
                    var diff = Math.Pow(1.1, steps);

                    var mapSizeStart = new Size(MapScrollViewer.ViewportWidth, MapScrollViewer.ViewportHeight);

                    var vertOffset = MapScrollViewer.VerticalOffset;
                    var horzOffset = MapScrollViewer.HorizontalOffset;

                    UseScale = UseScale * diff;

                    var mapSizeEnd = mapSizeStart.Multiply(diff);

                    var mapSizeDiff = mapSizeEnd.Subtract(mapSizeStart).Divide(2.0);

                    var scrollOnSizeX = horzOffset * diff + mapSizeDiff.X;
                    var scrollOnSizeY = vertOffset * diff + mapSizeDiff.Y;

                    MapScrollViewer.ScrollTo(scrollOnSizeX, scrollOnSizeY);
                }
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }


        private double UseScale
        {
            get
            {
                return playerMode ? map.TableScale : map.Scale;
            }
            set
            {
                if (playerMode)
                {
                    map.TableScale = value;
                }
                else
                {
                    map.Scale = value;
                }
            }
        }

        public Size UseCellSize
        {
            get
            {
                return playerMode ? map.TableCellSize : map.ActualCellSize;
            }
        }

        public Point UseGridOrigin
        {
            get
            {
                return playerMode ? map.TableGridOrigin : map.ActualGridOrigin;
            }
        }

        private void MapGridCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void BrushSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            brushSize = BrushSizeComboBox.SelectedIndex + 1;
            SaveActionButtonState();
        }

        private void ShowGridCheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        DispatcherTimer scaleTimer;



        private void ScaleTimer_Tick(object sender, EventArgs e)
        {

            var diff = Math.Pow(1.1, this.scaleTimerUp ? 1:-1);
            ChangeScale(scaleTimerPlayer, diff);
        }

        private void ChangeScale(bool player, double diff)
        {
            if (player)
            {
                map.TableScale = map.TableScale * diff;
            }
            else
            {
                map.Scale = map.Scale * diff;
            }
        }

        private void ScaleUpButton_MouseUp(object sender, MouseButtonEventArgs e)
        {

            EndScaleTimer();
        }

        private void ScaleDownButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartScaleTimer(false, false);

        }

        private void ScaleUpButton_MouseDown(object sender, MouseButtonEventArgs e)
        {


            StartScaleTimer(true, false);

        }

        private void TableScaleDownButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartScaleTimer(false, true);

        }

        private void TableScaleUpButton_MouseDown(object sender, MouseButtonEventArgs e)
        {


            StartScaleTimer(true, true);

        }

        void StartScaleTimer(bool directionUp, bool player)
        {
            scaleTimerUp = directionUp;
            scaleTimerPlayer = player;
            EndScaleTimer();

            scaleTimer?.Stop();
            scaleTimer = null;

            scaleTimer = new DispatcherTimer();
            scaleTimer.Tick += ScaleTimer_Tick;
            scaleTimer.Interval = new TimeSpan(30000);
            scaleTimer.Start();
            var diff = Math.Pow(1.1, 1);
            ChangeScale(player, diff);
        }

        void EndScaleTimer()
        {

            scaleTimer?.Stop();
            scaleTimer = null;
        }




        private void ScaleDownButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EndScaleTimer();
        }




        bool controlsHidden;
        GridLength controlRowDefinition;
        GridLength actionColumnDefinition;

        private void ScaleGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                HideControls();
            }
        }

        private void ShowControlsItem_Click(object sender, RoutedEventArgs e)
        {
            if (controlsHidden)
            {
                showControls();
            }
        }


        private void HideControlsItem_Click(object sender, RoutedEventArgs e)
        {
            if (!controlsHidden)
            {
                HideControls();
            }
        }

        void HideControls()
        {
            controlRowDefinition = RootGrid.RowDefinitions[0].Height;
            actionColumnDefinition = RootGrid.ColumnDefinitions[0].Width;
            RootGrid.RowDefinitions[0].Height = new System.Windows.GridLength(0);
            if (!playerMode)
            {

                RootGrid.ColumnDefinitions[0].Width = new System.Windows.GridLength(0);
            }

            MapScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            MapScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;



            NameGrid.Visibility = Visibility.Collapsed;
            GridSizeGrid.Visibility = Visibility.Collapsed;
            ScaleGrid.Visibility = Visibility.Collapsed;

            ShowActionButtons(false);

            controlsHidden = true;

        }

        void showControls()
        {
            controlsHidden = false;
            RootGrid.RowDefinitions[0].Height = controlRowDefinition;
            if (!playerMode)
            {
                RootGrid.ColumnDefinitions[0].Width = actionColumnDefinition;
            }
            MapScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            MapScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            ScaleGrid.Visibility = Visibility.Visible;

            if (!playerMode)
            {
                NameGrid.Visibility = Visibility.Visible;
                GridSizeGrid.Visibility = Visibility.Visible;

                ShowActionButtons(true);
            }
        }

        private void EnterFullScreenItem_Click(object sender, RoutedEventArgs e)
        {
            EnterFullScreen();
        }

        private void ExitFullScreenItem_Click(object sender, RoutedEventArgs e)
        {
            ExitFullScreen();
        }

        private void EnterFullScreen()
        {
            WindowStyle = WindowStyle.None;
            Topmost = false;
            WindowState = WindowState.Maximized;
            fullscreen = true;


            Hide();
            Show();
        }

        private void ExitFullScreen()
        {

            WindowStyle = WindowStyle.SingleBorderWindow;
            Topmost = false;
            WindowState = WindowState.Normal;
            fullscreen = false;

            Hide();
            Show();
        }

        private void MapScrollViewerGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowContextMenu(new GameMap.MapCell(-1, -1));
        }

        double rotation = 0;

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {
            if (playerMode)
            {
                rotation += 90.0;
                var animate = new DoubleAnimation();
                animate.To = rotation;
                animate.Duration = new Duration(TimeSpan.FromMilliseconds(100.0));

                LayoutRootRenderRotateTransform.BeginAnimation(RotateTransform.AngleProperty, animate);
            }
        }

        private void DecimalBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateTextBinding(sender);
            }
        }

        private void UpdateTextBinding(object sender)
        {
            var box = (TextBox)sender;
            var be = box.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
        }

    }
        
}
