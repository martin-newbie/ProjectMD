

public class Timer
{
    public float t;
    public float time;
    public string key;

    public Timer(float _time, string _key = "")
    {
        time = _time;
        key = _key;
        t = 0;
    }
}