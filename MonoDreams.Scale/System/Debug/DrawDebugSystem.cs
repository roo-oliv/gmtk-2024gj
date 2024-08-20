// using DefaultEcs;
// using DefaultEcs.System;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using MonoDreams.Renderer;
// using MonoDreams.Scale.Component;
// using MonoDreams.State;
//
// namespace MonoDreams.Scale.System.Debug;
//
// public class DrawDebugSystem(World world, SpriteBatch batch, ResolutionIndependentRenderer renderer) : AComponentSystem<GameState, DebugUI>(world)
// {
//     protected override void Update(GameState state, ref DebugUI debugUI)
//     {
//         batch.Begin(
//             SpriteSortMode.Deferred,
//             BlendState.AlphaBlend,
//             SamplerState.PointClamp,
//             DepthStencilState.None,
//             RasterizerState.CullNone);
//         batch.Draw(debugUI.DebugRenderTarget, Vector2.Zero, new Rectangle(0, 0, renderer.ScreenWidth, renderer.ScreenHeight), Color.White);
//         batch.End();
//     }
// }