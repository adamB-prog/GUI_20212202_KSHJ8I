namespace GravityDash.Renderer
{
    public interface IViewPort
    {
        int ViewPortHeightZoomed { get; }
        int ViewPortWidthZoomed { get; }
        int X { get; }
        int Y { get; }
        double Zoom { get; }

        void Follow();
        void ResizeViewPort(int width, int height);

        
    }
}