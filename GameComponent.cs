using OpenTK;

namespace Tetris
{
  abstract class GameComponent
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

    protected abstract void DoUpdate(FrameEventArgs e);

    protected abstract void DoDraw(FrameEventArgs e);
  }
}