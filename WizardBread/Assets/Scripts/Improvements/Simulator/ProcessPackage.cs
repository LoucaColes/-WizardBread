using System;

[Serializable]
public class ProcessPackage
{
    public Simulator.ImproveTags Tag;
    public int Level;
    public ImprovementSim.ImprovementProcess Process;
    public int OtherLevel;
    public ImprovementSim.ImprovementProcess OtherProcess;

    public ProcessPackage(Simulator.ImproveTags _tag, int _level, ImprovementSim.ImprovementProcess _process, int _otherLevel, ImprovementSim.ImprovementProcess _otherProcess)
    {
        Tag = _tag;
        Level = _level;
        Process = _process;
        OtherLevel = _otherLevel;
        OtherProcess = _otherProcess;
    }

    public bool ResolveScenario(int _levelIn, int _otherLevelIn)
    {
        if(!ResolveProcess(_levelIn, Level, Process))
        {
            return false;
        }

        return ResolveProcess(_otherLevelIn, OtherLevel, OtherProcess);
    }

    private bool ResolveProcess(int _in, int _cur, ImprovementSim.ImprovementProcess _process)
    {
        switch(_process)
        {
            case ImprovementSim.ImprovementProcess.None:
                {
                    return true;
                }
            case ImprovementSim.ImprovementProcess.Equals:
                {
                    return _in == _cur;
                }
            case ImprovementSim.ImprovementProcess.NotEquals:
                {
                    return _in != _cur;
                }
            case ImprovementSim.ImprovementProcess.Greater:
                {
                    return _in > _cur;
                }
            case ImprovementSim.ImprovementProcess.GreaterOrEquals:
                {
                    return _in >= _cur;
                }
            case ImprovementSim.ImprovementProcess.Less:
                {
                    return _in < _cur;
                }
            case ImprovementSim.ImprovementProcess.LessOrEquals:
                {
                    return _in <= _cur;
                }
            default:
                {
                    return false;
                }
        }
    }
}
