using System.Runtime.InteropServices;

namespace CombatManager.WPF7;

public class DebugTimer
{
    [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
    public static extern uint timeGetTime();

    uint _Time;
    uint _Last;
    bool _ShowTotals = true;
    string _Name;

    public DebugTimer() : this (null, true)
    {
    }

    public DebugTimer(string name)
        : this(name, true)
    {

    }

    public DebugTimer(string name, bool showTotals) : this(name, showTotals, true)
    {

    }

    public DebugTimer(string name, bool showTotals, bool showStart) 
    {
        _ShowTotals = showTotals;
        _Time = timeGetTime();
        _Last = _Time;
        _Name = name;
        if (showStart)
        {
            System.Diagnostics.Debug.WriteLine("Start Timer " + ((_Name == null) ? "" : _Name));
        }


    }

    private void Mark(string message)
    {

        var newTime = timeGetTime();
        TimeMessage(message, newTime);
        _Last = newTime;
    }

    public void MarkEvent(string text)
    {
        Mark(text);
    }

    public void MarkEventIf(string message, uint time)
    {

        var newTime = timeGetTime();

        if (newTime - _Last >= time)
        {

            TimeMessage(message, newTime);
        }
        _Last = newTime;
    }


    public void MarkEventIfTotal(string message, uint time)
    {

        var newTime = timeGetTime();

        if (newTime - _Time >= time)
        {
            TimeMessage(message, newTime);
        }
        _Last = newTime;
    }

    public void SetLastTime()
    {
        var newTime = timeGetTime();
        _Last = newTime;
    }


    private void TimeMessage(string message, uint newTime)
    {

        var output = message + ": " + (newTime - _Last);
        if (_ShowTotals)
        {
            output += " Total: " + (newTime - _Time);
        }
        System.Diagnostics.Debug.WriteLine(output);
    }


    public void MarkEvent()
    {
        Mark((_Name==null)?"":_Name);
    }

}