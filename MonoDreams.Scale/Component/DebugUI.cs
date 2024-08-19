using Microsoft.Xna.Framework.Graphics;

namespace MonoDreams.Scale.Component;

public class DebugUI(RenderTarget2D debugRenderTarget)
{
    public RenderTarget2D DebugRenderTarget = debugRenderTarget;
}