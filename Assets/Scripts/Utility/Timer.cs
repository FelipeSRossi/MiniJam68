public class Timer
{
    private float startAt;
    private float value;

    public Timer(float startAt)
    {
        this.startAt = startAt;
    }

    public void Set(float startAt)
    {
        this.value = this.startAt = startAt;
    }

    public void Reset()
    {
        this.value = this.startAt;
    }

    public void Update(float delta)
    {
        this.value -= delta;
    }

    public bool isFinished()
    {
        return this.value <= 0;
    }
}
