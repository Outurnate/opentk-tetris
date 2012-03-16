using OpenTK;

namespace GameFramework
{
  public abstract class GameComponent
  {
    public bool Visible
    {
      get;
      set;
    }

    public bool Enabled
    {
      get;
      set;
    }

    public GameWindow Window
    {
      get;
      private set;
    }

    public GameComponent(GameWindow window)
    {
      this.Window = window;
    }

    public void Load()
    {
      DoLoad();
    }

    public void Update(FrameEventArgs e)
    {
      if (Enabled)
        DoUpdate(e);
    }

    public void Draw(FrameEventArgs e)
    {
      if (Visible)
        DoDraw(e);
    }

    protected abstract void DoLoad();

    protected abstract void DoUpdate(FrameEventArgs e);

    protected abstract void DoDraw(FrameEventArgs e);
  }
}