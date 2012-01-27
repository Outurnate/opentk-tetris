using OpenTK;

namespace Tetris
{
  interface IGameComponent
  {
    void Update(FrameEventArgs e);
    void Draw(FrameEventArgs e);
  }
}